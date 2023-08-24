/*******************
Autores:    Juan David Ruiz (juandavidrf@unicauca.edu.co)
            Sebastian Montenegro (exlogam@unicauca.edu.co)
*******************/

/********************* Librerias ********************/
using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Control_Cartesiano : MonoBehaviour
{
    /**************** Variables ******************/
    [SerializeField] private Control_Panel CPanel;                  //Booleana del panel cartesiano
    public GameObject PrefabCoordenadas;                            //Prefab de coordenadas
    public RectTransform content;                                   //Contenedor lista
    public GameObject DialogoEliminar;                              //Panel de borrar
    public GameObject DialogoLimpiar;                               //Panel de eliminar
    public GameObject Cartesiano;                                   //Panel cartesiano
    private int NumTraj;                                            //Numero de trayectoria a eliminar
    private TMP_InputField[] coordenadas = new TMP_InputField[6];   //Vector de casillas de las coordenadas
    private float[] valores = new float[6];                         //Valores de las coordenadas
    private string[] CoorActual = new string[6];                    //Coordenada actual del robot
    [HideInInspector]public int NumCoor = 0;                        //Numero de coordenadas cartesianas
    private GameObject[] puntos = new GameObject[100];              //Objeto prefab de coordenadas
    private string Nombre;                                          //Posicion u orientacion
    public Slider DeslizadorVel;                                    //Deslizador de velocidad
    public Text ValorTexto;                                         //Valor velocidad
    private TMP_Text NumeroTrayectoria;                             //Numero de trayectoria


    /**************** Funciones ******************/
    public void Inicializar()                       //Inicializa los sliders y los valores articulares
    {
        if (CPanel.P_Cartesiano)
        {
            for(int i=0; i<6; i++)
            {
                string NombreCoor = NumToCoor(i);
                coordenadas[i] = GameObject.Find(NombreCoor).GetComponent<TMP_InputField>();
            }

            ValorTexto.text =  Math.Round((DeslizadorVel.value*0.1f),2)+ " cm/s";

            DialogoLimpiar.SetActive(false);
            DialogoEliminar.SetActive(false);
        }
    }

    public void AgregarCoordenada()                 //Agrega la coordenada escrita a la lista de coordenadas
    {
        if (NumCoor<puntos.Length)
        {
            for(int i = 0; i < 6; i++)
            {
                valores[i] = float.Parse(coordenadas[i].text);
            }
            puntos[NumCoor] = Instantiate(PrefabCoordenadas, content, false);
            puntos[NumCoor].transform.name = "Coor"+(NumCoor);

            for(int i = 0; i<6; i++)
            {
                string NombreCoor = NumToCoor(i);
                TMP_Text texto = GameObject.Find("Coor"+(NumCoor)+"/V"+NombreCoor).GetComponent<TMP_Text>();
                texto.text = valores[i].ToString();
            }
            NumeroTrayectoria = GameObject.Find("Coor"+(NumCoor)+"/NumTra").GetComponent<TMP_Text>();
            NumeroTrayectoria.text = (NumCoor+1).ToString();

            NumCoor ++;
        }else
        {
            //Mensaje de advertencia
            print("Limite de coordenadas alcanzado");
        }
    }


    public void CopiarCoordenada()                  //Copia la coordenada actual del robot en las casillas de escritura
    {   
        for(int i=0; i<6; i++)
        {
            string NombreCoor = NumToCoor(i);
            CoorActual[i]= GameObject.Find(NombreCoor+"/ValorRobot").GetComponent<Text>().text;
            coordenadas[i].text = CoorActual[i];
        }
    }

    public void EliminarCoordenada()                //Despliega el panel de eliminar coordenada determinada
    {
        if (NumCoor>0)
        {   
            Cartesiano.SetActive(false);
            DialogoEliminar.SetActive(true);
            DialogoLimpiar.SetActive(false);
        }
    }

    public void LimpiarLista()                      //Despliega el panel de borrar todas las coordenadas
    {
        if (NumCoor>0)
        {   
            Cartesiano.SetActive(false);
            DialogoLimpiar.SetActive(true);
            DialogoEliminar.SetActive(false);
        }
    }

    public void BotonEliminar()                     //Elimina una coordenada determinada de la lista de coordenadas
    {
        NumTraj = int.Parse((GameObject.Find("DialogoEliminar/DialogoPanel/Dato").GetComponent<TMP_InputField>().text));

        if ((0<NumTraj)&&(NumTraj<=NumCoor))
        {
            Destroy(puntos[NumTraj-1]);
            for (int i = NumTraj; i < NumCoor; i++)
            {
                GameObject.Find("Coor"+(i)+"/NumTra").GetComponent<TMP_Text>().text = (i).ToString();
                puntos[i-1] = puntos[i];
                puntos[i-1].transform.name = "Coor"+(i-1);
            }
            Cartesiano.SetActive(true);
            DialogoEliminar.SetActive(false);
            NumCoor = NumCoor-1;
        }else
        {
            //Mensaje de error
            print("Numero de coordenada incorrecta");
            Cartesiano.SetActive(true);
            DialogoEliminar.SetActive(false);
        }
    }

    public void BotonCancelar()                     //Desactiva paneles de elimiinar o borrar coordenadas
    {
        Cartesiano.SetActive(true);
        DialogoEliminar.SetActive(false);
        DialogoLimpiar.SetActive(false);
    }

    public void EliminarTodo()                      //Elimina todas las coordenadas de la lista de coordenadas
    {
        if (NumCoor>0)
        { 
            for (int i = 0; i < NumCoor; i++)
            {
                Destroy(puntos[i]);
            }
            NumCoor = 0;
        }
        Cartesiano.SetActive(true);
        DialogoLimpiar.SetActive(false);
    }

    public string NumToCoor(int numero)             //Asigna el nombre del objeto a buscar
    {
        switch (numero)
        {
            case 0:
            Nombre = "PosX";
                break;
            case 1:
            Nombre = "PosY";
                break;
            case 2:
            Nombre = "PosZ";
                break;
            case 3:
            Nombre = "OriX";
                break;
            case 4:
            Nombre = "OriY";
                break;
            case 5:
            Nombre = "OriZ";
                break;
            case 6:
            Nombre = "OriW";
                break;
        }
        return(Nombre);
    }

}
