/*******************
Autores:    Juan David Ruiz (juandavidrf@unicauca.edu.co)
            Sebastian Montenegro (exlogam@unicauca.edu.co)
*******************/

/********************* Librerias ********************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatosTrayectoria 
{
    /**************** Variables ******************/
    public int Muestreo;                        //Numero de muestras por segmento
    public List<Variables> MyArray;             //Lista de coordenadas tridimensionales
}

public class Variables
{
    /**************** Variables ******************/
    public float ejex;                          //Coordenada x
    public float ejey;                          //Coordenada y
    public float ejez;                          //Coordenada z
}
