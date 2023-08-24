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
using RosSharp.Urdf;

public class Control_Articular : MonoBehaviour
{
    /**************** Variables ******************/
    [SerializeField] private Control_Panel CPanel;                  //Booleana del panel Articular
    public GameObject PrefabPosiciones;                             //Prefab de posiciones
    public RectTransform content;                                   //Contenedor lista
    private TMP_Text NumeroPosicion;                                //Numero de posicion
    public GameObject DialogoEliminar;                              //Panel de eliminar
    public GameObject DialogoLimpiar;                               //Panel de borrar
    public GameObject Articular;                                    //Panel articular
    private int NumTraj;                                            //Numero de trayectoria a eliminar
    public List<UrdfJoint> urdfJoint = new List<UrdfJoint>(6);      //Articulaciones del gemelo digital
    private float[] position = new float[6];                        //Valores de las articulaciones
    private Slider[] Deslizador = new Slider[6];                    //Objetos de los sliders
    private Text[] TextoRobot = new Text[6];                        //Objetos de los valores articulares
    private double[] ValorRobot = new double[6];                    //Valores articulares redondeados
    private Text[] TextoSlider = new Text[6];                       //Valor futuro de las articulaciones
    [HideInInspector] public int NumPos = 0;                        //Numero de posiciones articulares
    private GameObject[] poses = new GameObject[100];               //Objeto prefab de posiciones
    private float[] valores = new float[6];                         //Valores de las articulaciones


    /**************** Funciones ******************/
    public void Inicializar_Art()                   //Inicializa los sliders y los valores articulares
    {    
        if (CPanel.P_Articular)
        {
            for(int i=0; i<6; i++)
            {
                Deslizador[i] = GameObject.Find("Art"+(i+1)+"/Slider").GetComponent<Slider>();
                TextoRobot[i] = GameObject.Find("Art"+(i+1)+"/ValorRobot").GetComponent<Text>();
                TextoSlider[i] = GameObject.Find("Art"+(i+1)+"/ValorSlider").GetComponent<Text>();
            }

            for (int i = 0; i < urdfJoint.Count; i++)
            {
                position[i] = urdfJoint[i].GetPosition();
                Deslizador[i].value = position[i]*(180/Mathf.PI);
                ValorRobot[i] = Math.Round((position[i]*(180/Mathf.PI)),2);
                TextoRobot[i].text = ValorRobot[i] + "°";
                TextoSlider[i].text = " ";
            }

            DialogoLimpiar.SetActive(false);
            DialogoEliminar.SetActive(false);
        }
    }

    void Update()               //Mantine los sliders actaulizados a los valores articulares
    {
        if (CPanel.P_Articular)
        {
            for (int i = 0; i < urdfJoint.Count; i++)
            {
                position[i] = urdfJoint[i].GetPosition();
                ValorRobot[i] = Math.Round((position[i]*(180/Mathf.PI)),2);
                TextoRobot[i].text =  ValorRobot[i] + "°";
                if (ValorRobot[i] == Math.Round(Deslizador[i].value,2))
                {
                    LimpiarArticulacion(i);
                }
            }
        }
    }

    public void BotonMas(int NumArt)            //Incrementa el valor del deslizador en 0.1 grados
    {
        Deslizador[NumArt-1].value = Deslizador[NumArt-1].value + (float)0.1;
    }

    public void BotonMenos(int NumArt)          //Disminuye el valor del deslizador en 0.1 grados
    {
        Deslizador[NumArt-1].value = Deslizador[NumArt-1].value - (float)0.1;
    }

    public void ValorArticulacion(int NumArt)   //Muestra el valor futuro a mover la articulacion
    {
        TextoSlider[NumArt-1].text =  Math.Round((float)(Deslizador[NumArt-1].value),2) + "°";
    }

    public void LimpiarArticulacion(int NumArt) //Resetea el valor futuro 
    {
        TextoSlider[NumArt].text = " ";
    }

    public void AgregarPosicion()               //Agrega la posicion alctual del robot a la lista de posiciones
    {
        if (NumPos<poses.Length)
        {
            for(int i = 0; i < 6; i++)
            {
                position[i] = urdfJoint[i].GetPosition();
                valores[i] = (position[i]*(180/Mathf.PI));
            }
            poses[NumPos] = Instantiate(PrefabPosiciones, content, false);
            poses[NumPos].transform.name = "Posi"+(NumPos);

            for(int i = 0; i<6; i++)
            {
                TMP_Text texto = GameObject.Find("Posi"+(NumPos)+"/VJoint"+(i+1)).GetComponent<TMP_Text>();
                texto.text = (Math.Round(valores[i],2)).ToString();
            }
            NumeroPosicion = GameObject.Find("Posi"+(NumPos)+"/NumPosicion").GetComponent<TMP_Text>();
            NumeroPosicion.text = (NumPos+1).ToString();

            NumPos ++;
        }else
        {
            //Mensaje de advertencia
            print("Limite de posiciones alcanzado");
        }
    }

    public void EliminarPosicion()              //Despliega el panel de eliminar posicion determinada
    {
        if (NumPos>0)
        {   
            Articular.SetActive(false);
            DialogoEliminar.SetActive(true);
            DialogoLimpiar.SetActive(false);
        }
    }

    public void LimpiarLista()                  //Despliega el panel de borrar todas las posiciones
    {
        if (NumPos>0)
        {   
            Articular.SetActive(false);
            DialogoLimpiar.SetActive(true);
            DialogoEliminar.SetActive(false);
        }
    }

    public void BotonEliminar()                 //Elimina una posicion determinada de la lista de posiciones
    {
        NumTraj = int.Parse((GameObject.Find("DialogoEliminar/DialogoPanel/Dato").GetComponent<TMP_InputField>().text));

        if ((0<NumTraj)&&(NumTraj<=NumPos))
        {
            Destroy(poses[NumTraj-1]);
            for (int i = NumTraj; i < NumPos; i++)
            {
                GameObject.Find("Posi"+(i)+"/NumPosicion").GetComponent<TMP_Text>().text = (i).ToString();
                poses[i-1] = poses[i];
                poses[i-1].transform.name = "Posi"+(i-1);
            }
            Articular.SetActive(true);
            DialogoEliminar.SetActive(false);
            NumPos = NumPos-1;
        }else
        {
            //Mensaje de error
            print("Numero de posición incorrecta");
            Articular.SetActive(true);
            DialogoEliminar.SetActive(false);
        }
    }

    public void BotonCancelar()                 //Desactiva paneles de elimiinar o borrar posiciones
    {
        Articular.SetActive(true);
        DialogoEliminar.SetActive(false);
        DialogoLimpiar.SetActive(false);
    }

    public void EliminarTodo()                  //Elimina todas las posiciones de la lista de posiciones
    {
        if (NumPos>0)
        { 
            for (int i = 0; i < NumPos; i++)
            {
                Destroy(poses[i]);
            }
            NumPos = 0;
        }
        Articular.SetActive(true);
        DialogoLimpiar.SetActive(false);
    }
}
