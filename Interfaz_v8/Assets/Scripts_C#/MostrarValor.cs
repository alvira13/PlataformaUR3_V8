/*******************
Autores:    Juan David Ruiz (juandavidrf@unicauca.edu.co)
            Sebastian Montenegro (exlogam@unicauca.edu.co)
*******************/

/********************* Librerias ********************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MostrarValor : MonoBehaviour
{
    /**************** Variables ******************/
    public Slider Deslizador;
    public Text ValorTexto;


    /**************** Funciones ******************/
    public void AsignarValor()
    {
        ValorTexto.text =  Math.Round((float)(Deslizador.value),1) + "°";
    }

    public void ValorArticulacion()             //Funcion para slider articular
    {
        ValorTexto.text =  Math.Round((float)(Deslizador.value),2) + "°";
    }

    public void Limpiar()                       //Funcion para slider articular
    {
        ValorTexto.text = " ";
    }

    public void ValorVelocidad()                //Funcion para slider de velocidad
    {
        ValorTexto.text =  Math.Round((Deslizador.value*0.1f),2)+ " cm/s";
    }
}
