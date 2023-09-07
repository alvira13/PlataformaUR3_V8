/*******************
Autores:    Juan David Ruiz (juandavidrf@unicauca.edu.co)
            Sebastian Montenegro (exlogam@unicauca.edu.co)
*******************/

/*******************
Este script ha sido ligeramente modificado por:
           Jan Carlos Alvira Meneses(janalvira@unicauca.edu.co)
           Leonardo Alberto Paz Paz (leopaz@unicauca.edu.co)
*******************/

/********************* Librerias ********************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class TrayectoriaLibre : MonoBehaviour
{
    /**************** Variables ******************/
    [SerializeField] private Control_Panel CPanel;              //Booleana del panel libre
    public Slider DeslizadorVel;                                //Deslizador de velocidad
    public Text ValorTexto;                                     //Valor velocidad
    private float velocidad;                                    //Velocidad

    // Variables para crear los puntos
    private List<GameObject> puntos = new List<GameObject>();   //Vector de objetos punto
    private int NumPuntos = 0;                                  //Numeero de objetos puntos
    int segmento;                                               //NUmero de segmento de la linea
    public GameObject PrefabPuntoInical;                        //Prefab del punto inicial
    public GameObject PrefabPunto;                              //Prefab de los puntos
    public GameObject ContenedorPuntos;                         //COntenedor de los objetos punto

    // Variable para crea la linea
    public LineRenderer Linea;                                  //Linea
    public bool CerrarTrayectoria = false;                      //Booleana para hacer trayectoria cerrada
    private int numberOfPoints = 50;                            //Numero de puntos por segmento de linea

    // Variables para Colicionar
    RaycastHit hit;
    Ray ray;

    // Variables para publicar mediante ROS
    public RosSharp.RosBridgeClient.TrajPublisher publicador;   //Cliente ROS
    [HideInInspector] public float[,] figura;                   //Matriz con coordenadas de la linea
    [HideInInspector] public float[,] figura_past;              //Matriz con coordenadas de la linea pasada

    // Variables para exportar e importar
    public TMP_InputField Ubicacion;                            //Ubicacion del almacenamiento interno
    public TMP_InputField Nombre;                               //Nombre del archivo
    private DatosTrayectoria Traj;                              //Objeto con los datos de la trayectoria
    private Variables datos;                                    
    private bool Editable = true;

    /*Variable para intercambiar entre modos de trayectoria:
    La tecla 1 del teclado alfanumérico entra al modo por defecto (Dibujo libre, orientación fija)
    La tecla 2 del teclado alfanumérico entra al modo adicional (Dibujo libre, orientación dinámica)*/
    private byte opcion = 1;
    private bool salida = false;  // En el modo 2, indica regresar a la posición inicial

    /**************** Funciones ******************/
    void Start()                                    //Funcion inicial
    {
        Inicializar_linea();
        ValorTexto.text =  Math.Round((DeslizadorVel.value*0.1f),2)+ " cm/s";

        /* Para el modo de trayectorias 2, creamos un arreglo que va a guardar las posiciones del
        line renderer anterior.*/
        figura_past = new float[50,3];
        for (int i = 0; i < 50; i++)
        {
            // Eje x
            figura_past[i,0] = 0;
            // Eje y
            figura_past[i,1] = 0;
            // Eje z
            figura_past[i,2] = 0;
        }
    }

    void Update()                                   //Mantine la linea actaulizada
    {
        if (CPanel.P_Libre)
        {
            if (Editable)
            {
                DibujarLinea(puntos);
                AgregarPuntoFinal();              //Funicon en proceso
                AgregarPuntoIntermedio();
            }
        }
        /* Se había escogido como entradas de teclado el Keypad, pero al ingresar la dirección IP
        en la interfaz, el valor "opcion" también se modificaba. No se 
        recomienda usar el teclado alfanumérico para ingresar la dirección IP en la interfaz.
        Los métodos Input (así como el Switch-case) tienen que ir dentro del Update (dentro de una clase no funcionan).*/
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            opcion = 1;
            Debug.Log("Opción " + opcion + ": Trayectoria libre, orientación fija (Modo por defecto)");
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            opcion = 2;
            Debug.Log("Opción " + opcion + ": Trayectoria libre, orientación dinámica");
        }
        /* Antes se tenía KeyCode.Space para salir, pero el juego a veces tomaba la tecla como un click
        cuando se había dado click sobre un objeto por última vez (e.g., Menú hamburguesa, botón cargar...).*/
        if(Input.GetKeyDown(KeyCode.Q))
        {
            salida = true;
            // Vuelve a la posición inicial
        }
    }

    void Inicializar_linea()                        //Dibuja la linea inicial
    {
        NumPuntos = 2;

        puntos.Add(Instantiate(PrefabPuntoInical, ContenedorPuntos.transform, false));
        puntos[0].transform.name = "Punto_1";
        puntos[0].transform.position = new Vector3 (-0.1f,0.244f,0.342f);

        puntos.Add(Instantiate(PrefabPunto, ContenedorPuntos.transform, false));
        puntos[1].transform.name = "Punto_2";
        puntos[1].transform.position = new Vector3 (-0.342f,0.244f,0.1f);
    }

    void AgregarPuntoFinal()                        //Agrega un punto al final de la linea
    {
        if (Input.GetMouseButtonDown(1))
        {
            NumPuntos += 1;

            puntos.Add(Instantiate(PrefabPunto, ContenedorPuntos.transform, false));
            puntos[NumPuntos-1].transform.position = puntos[NumPuntos-2].transform.position + new Vector3(0.05f, 0.05f, 0f);
            puntos[NumPuntos-1].transform.name = "Punto_"+NumPuntos;
        }
    }

    void AgregarPuntoIntermedio()                   //Agrega un punto en donde se clickee sobre la linea
    {
        if(Input.GetMouseButtonDown(0))
        {
            Debug.DrawRay(Camera.main.ScreenToWorldPoint(Input.mousePosition),PosicionMouse(), Color.red);

            if (Physics.Raycast(ray, out hit) && (hit.collider.gameObject.layer == 6)) //6 = layer "Linea"
            {
                double[] diferencia = new double[Linea.positionCount];
                double menor=10;

                for (int i = 0; i < Linea.positionCount; i++)
                {
                    diferencia[i] = Math.Sqrt(Math.Pow(hit.point.x - Linea.GetPosition(i).x, 2)
                                            +Math.Pow(hit.point.y - Linea.GetPosition(i).y, 2)
                                            +Math.Pow(hit.point.z - Linea.GetPosition(i).z, 2));
                    if (diferencia[i] < menor)
                    {
                        menor = diferencia[i];
                        segmento = (i/numberOfPoints)+1;
                    }
                }
                menor=10;
                NumPuntos += 1;
                puntos.Insert(segmento,Instantiate(PrefabPunto, ContenedorPuntos.transform, false));
                puntos[segmento].transform.position = hit.point;

                for (int i = 0; i < puntos.Count; i++)
                {
                    puntos[i].transform.name = "Punto_"+(i+1);
                }
            }
        }
    }

    void DibujarLinea(List<GameObject> points)      //Dibuja una linea curva entre los objetos punto
    {
        Linea.positionCount = numberOfPoints*(puntos.Count-1);
        
        Vector3 p0, p1, m0, m1;

        for(int j = 0; j < puntos.Count - 1; j++)
		{
			// determine control points of segment
			p0 = puntos[j].transform.position;
			p1 = puntos[j+1].transform.position;
			
			if (j > 0) 
			{
				m0 = 0.5f * (puntos[j+1].transform.position - puntos[j-1].transform.position);
			}
			else
			{
				m0 = puntos[j+1].transform.position - puntos[j].transform.position;
			}
			if (j < puntos.Count - 2)
			{
				m1 = 0.5f * (puntos[j+2].transform.position - puntos[j].transform.position);
			}
			else
			{
				m1 = puntos[j+1].transform.position - puntos[j].transform.position;
			}

			// set points of Hermite curve
			Vector3 position;
			float t;
			float pointStep = 1.0f / numberOfPoints;

			if (j == puntos.Count - 2)
			{
				pointStep = 1.0f / (numberOfPoints - 1.0f);
				// last point of last segment should reach p1
			}  
			for(int i = 0; i < numberOfPoints; i++) 
			{
				t = i * pointStep;
				position = (2.0f * t * t * t - 3.0f * t * t + 1.0f) * p0 
					        + (t * t * t - 2.0f * t * t + t) * m0 
					        + (-2.0f * t * t * t + 3.0f * t * t) * p1 
					        + (t * t * t - t * t) * m1;
				Linea.SetPosition(i + j * numberOfPoints, position);
			}
		}
    }

    Vector3 PosicionMouse()                         //Devuelve la posicion del mouse en el espacio tridimensional
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return ray.direction* 0.5f;
    }

    public void PublicarTrayectoria()               //Enviar trayectoria al cliente ROS
    {
        velocidad = DeslizadorVel.value*0.001f;

        figura = new float[Linea.positionCount,3];
        // Debug.Log("Linea.position Count es: " + Linea.positionCount);
        // Cada que aparece un nuevo segmento, Linea.positionCount aumenta 50 puntos.
        for (int i = 0; i < Linea.positionCount; i++)
        {
            /*
            NOTA: Aqui se aplica el cambio del sistema de refencia de Unity al sistema de referencia de ROS
            -------------------
            |  ROS  |  Unity  |
            |-------|---------|
            | Eje x |  Eje z  |
            | Eje y |  Eje x  |
            | Eje z |  Eje y  |
            -------------------
            */

            // Eje x
            figura[i,0] = -Linea.GetPosition(i).z;
            // Eje y
            figura[i,1] = Linea.GetPosition(i).x;
            // Eje z
            figura[i,2] = Linea.GetPosition(i).y;
        }
        //Opción 1 (La orientación de la pinza está fija)
        if(opcion == 1)
        {
            publicador.Trayectoria(figura, Linea.positionCount, velocidad);
            /* Reiniciamos en ceros el arreglo que guarda trayectorias pasadas (para evitar recorrer alguna trayectoria guardada,
            en caso de que se haya estado utilizando la opción 2 previamente y se quiera volver a esta nuevamente.*/
            for (int i = 0; i < 50; i++)
            {
                // Eje x
                figura_past[i,0] = 0;
                // Eje y
                figura_past[i,1] = 0;
                // Eje z
                figura_past[i,2] = 0;
            }
        }
        //Opción 2 (La orientación de la pinza está dada por la inclinación del "line renderer")
        if(opcion == 2)
        {
            if (salida == true) 
            // No puedo volver a la posición inicial si la Q no se ha presionado (condiciones de seguridad)
            {
                if(figura_past[0,0] != 0 && figura_past[0,1] != 0 && figura_past[0,2] != 0)
                {
                    /*Antes de ir a la posición inicial, el robot recorre la trayectoria anterior en sentido target-entry,
                    sólo si el arreglo que guarda trayectorias pasadas no está en ceros (condiciones de seguridad)*/
                    publicador.Trayectoria_target_Entry(figura_past, Linea.positionCount, velocidad);
                }
                // Invoco la posición inicial
                publicador.Pos_Inicial(velocidad);
                Debug.Log("Posición inicial: " + salida);
                // Reiniciamos en ceros el arreglo que guarda trayectorias pasadas (para evitar ir de la posición inicial al Marker-Target)
                for (int i = 0; i < 50; i++)
                {
                    // Eje x
                    figura_past[i,0] = 0;
                    // Eje y
                    figura_past[i,1] = 0;
                    // Eje z
                    figura_past[i,2] = 0;
                }
            }
            else
            {
                //La primera trayectoria se ejecuta en sentido entry-target si el arreglo que guarda trayectorias pasadas está en ceros
                if(figura_past[0,0] == 0 && figura_past[0,1] == 0 && figura_past[0,2] == 0)
                {
                    publicador.Trayectoria_Entry_target(figura, Linea.positionCount, velocidad);
                }
                else
                {
                    /* Accedemos al modo Entry-Target:
                    Si no se ha movido la segunda esfera ó si se ha movido la primera esfera, de la trayectoria actual.*/
                    if((figura[49,0] == figura_past[49,0] && figura[49,1] == figura_past[49,1] && figura[49,2] == figura_past[49,2]) ||
                    (figura[0,0] != figura_past[0,0] && figura[0,1] != figura_past[0,1] && figura[0,2] != figura_past[0,2]))
                    {
                        // Antes de ejecutar la nueva trayectoria, el robot recorre la trayectoria anterior en sentido target-entry (condiciones de seguridad)
                        publicador.Trayectoria_target_Entry(figura_past, Linea.positionCount, velocidad);
                        publicador.Trayectoria_Entry_target(figura, Linea.positionCount, velocidad);
                    }
                    /* Accedemos al modo Pivote:
                    Si se ha movido la segunda esfera y si no se ha movido la primera esfera, de la trayectoria actual.*/
                    else
                    {
                        publicador.Trayectoria_Pivote(figura, Linea.positionCount, velocidad);
                    }
                    /* La primera impresión de este Debug debe ser 0,0,0 y 0,0,0. Luego empieza a guardar las posiciones de la trayectoria anterior.*/
                    //Debug.Log("Posición anterior en x del Marker-Entry: " + figura_past[0,0]);
                    //Debug.Log("Posición anterior en y del Marker-Entry: " + figura_past[0,1]);
                    //Debug.Log("Posición anterior en z del Marker-Entry: " + figura_past[0,2]);
                    //Debug.Log("Posición anterior en x del Marker-Target: " + figura_past[49,0]);
                    //Debug.Log("Posición anterior en y del Marker-Target: " + figura_past[49,1]);
                    //Debug.Log("Posición anterior en z del Marker-Target: " + figura_past[49,2]);
                }
                figura_past = figura;  // Actualizamos el valor de la trayectoria pasada
            }
            salida = false;  //salida se resetea a false
        }
    }

    public void ExportarTrayectoria()               //Guardar linea en el almacenamiento interno
    {
        Traj = new DatosTrayectoria();
        Traj.MyArray = new List<Variables>();
        Traj.Muestreo = new int();
        Traj.Muestreo = numberOfPoints;
        for (int i = 0; i < Linea.positionCount; i++)
        {
            datos = new Variables();
            datos.ejex = new float();
            datos.ejey = new float();
            datos.ejez = new float();
                
            datos.ejex = (Linea.GetPosition(i).x);
            datos.ejey = (Linea.GetPosition(i).y);
            datos.ejez = (Linea.GetPosition(i).z);

            Traj.MyArray.Add(datos);
        }
        Exportar_importar.SaveData(Traj,Ubicacion.text,Nombre.text);
        Debug.Log("Exportado");
    }

    public void ImportarTrajEditable()              //Extraer datos de linea del almacenamiento interno (Linea editable = menos precision)
    {
        Traj = Exportar_importar.LoadData<DatosTrayectoria>(Ubicacion.text,Nombre.text);
        VaciarPuntos();
        NumPuntos =(Traj.MyArray.Count/Traj.Muestreo)+1;
        for (int i = 0; i < NumPuntos; i++)
        {
            int j = (i*Traj.Muestreo)-1;
            if (j<0)
                j=0;
                
            /******************
            NOTA:   El sistema de referencia usado es el que corresponde a unity
                    X: De izquierda a derecha
                    Y: De arriba a abajo
                    Z: De adelante hacia atras
            *******************/
            Vector3 posicion;
            // Eje x
            posicion.x = Traj.MyArray[j].ejex;
            // Eje y
            posicion.y = Traj.MyArray[j].ejey;
            // Eje z
            posicion.z = Traj.MyArray[j].ejez;
                
            if (i==0)
                puntos.Add(Instantiate(PrefabPuntoInical, ContenedorPuntos.transform, false));
            else
                puntos.Add(Instantiate(PrefabPunto, ContenedorPuntos.transform, false));
            puntos[i].transform.position = posicion;
            puntos[i].transform.name = "Punto_"+(i+1);
        }
        Editable = true;
        Debug.Log("importado");
    }

    public void ImportarTrajNoEditable()            //Extraer datos de linea del almacenamiento interno (Linea no editable = precision exacta)
    {
        Editable = false;

        VaciarPuntos();
        Traj = Exportar_importar.LoadData<DatosTrayectoria>(Ubicacion.text,Nombre.text);

        Linea.positionCount = Traj.MyArray.Count;
        for (int i = 0; i < Traj.MyArray.Count; i++)
        {
            /******************
            NOTA:   El sistema de referencia usado es el que corresponde a unity
                    X: De izquierda a derecha
                    Y: De arriba a abajo
                    Z: De adelante hacia atras
            *******************/
            Vector3 posicion;
            // Eje x
            posicion.x = Traj.MyArray[i].ejex;
            // Eje y
            posicion.y = Traj.MyArray[i].ejey;
            // Eje z
            posicion.z = Traj.MyArray[i].ejez;
            Linea.SetPosition(i,posicion);
        }
        Debug.Log("importado");
    }

    public void Reiniciar()                         //Elimina la linea actual y dibuja la linea inicial
    {
        VaciarPuntos();
        Inicializar_linea();
        Editable = true;
    }

    private void VaciarPuntos()                     //Elimina y resetea todos los objetos punto
    {
        if (puntos.Count>0)
        {
            for (int i = 0; i < NumPuntos; i++)
            {
                Destroy(puntos[i]);
            }
            puntos.Clear();
        }
    }
}
