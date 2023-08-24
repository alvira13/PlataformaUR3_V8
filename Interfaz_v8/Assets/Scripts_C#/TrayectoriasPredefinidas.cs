/*******************
Autores:    Juan David Ruiz (juandavidrf@unicauca.edu.co)
            Sebastian Montenegro (exlogam@unicauca.edu.co)
*******************/

/********************* Librerias ********************/
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TrayectoriasPredefinidas : MonoBehaviour
{
    /**************** Variables ******************/
    [HideInInspector] public float[,] figura = new float[500,3];        //Matriz de coordenadas con 500 muestras
    public RosSharp.RosBridgeClient.TrajPublisher publicador;           //Cliente ROS para enviar la figura
    public TMP_InputField[] CirculoCoor = new TMP_InputField[3];        //Origen del circulo
    public TMP_InputField CirculoRadio;                                 //Radio del circulo
    public TMP_InputField[] EspiralCoor = new TMP_InputField[3];        //Origen de la espiral
    public TMP_InputField EspiralRadio;                                 //Radio de la espiral


    /**************** Funciones ******************/
    public void Circulo()                               //Dibuja un circulo en el plano XY
    {
        int Tfinal = 50;
        int NumMuestras = 500;
        double Tem = 1;
        int nbench = (int)(Tfinal/Tem);
        float Radio = float.Parse(CirculoRadio.text)*0.01f;
        float desviacionX = float.Parse(CirculoCoor[0].text)*0.01f;
        float desviacionY = float.Parse(CirculoCoor[1].text)*0.01f;
        float altura = float.Parse(CirculoCoor[2].text)*0.01f;
        float[] angulo;

        angulo = linspace(0, 2*Mathf.PI, 500);
        
        for (int i = 0; i < NumMuestras; i++)
        {
            figura[i,0] = ((Radio * Mathf.Cos(angulo[i]))+desviacionX);
            figura[i,1] = ((Radio * Mathf.Sin(angulo[i]))+desviacionY);
            figura[i,2] = altura;
        }
        publicador.Trayectoria(figura,NumMuestras,0.05f);
    }

    public void EspiralConica()                         //Dibuja una espiral en el plano XY
    {
        int Tfinal = 30;
        int NumMuestras = 500;
        double Tem = 0.05;
        int nbech = (int)(Tfinal/Tem)+1;
        double t=0;
        float Radio = float.Parse(EspiralRadio.text)*0.01f;
        float desviacionX = float.Parse(EspiralCoor[0].text)*0.01f;
        float desviacionY = float.Parse(EspiralCoor[1].text)*0.01f;
        float altura = float.Parse(EspiralCoor[2].text)*0.01f;

        for (int i = 0; i < NumMuestras; i++)
        {
            t=t+Tem;
            figura[i,0] = (float)((Radio*t*Mathf.Sin((float)t))+desviacionX);
            figura[i,1] = (float)((Radio*t*Mathf.Cos((float)t))+desviacionY);
            figura[i,2] = (float)((0.008*t)+altura);
        }
        publicador.Trayectoria(figura,NumMuestras,0.05f);
    }

    public static float[] linspace(float startval, float endval, int steps)
    {
        float interval = (endval / Mathf.Abs(endval)) * Mathf.Abs(endval - startval) / (steps - 1);
        return (from val in Enumerable.Range(0,steps)
                select startval + (val * interval)).ToArray(); 
    }
}
