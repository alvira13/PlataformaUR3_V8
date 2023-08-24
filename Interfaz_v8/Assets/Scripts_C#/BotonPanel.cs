/*******************
Autores:    Juan David Ruiz (juandavidrf@unicauca.edu.co)
            Sebastian Montenegro (exlogam@unicauca.edu.co)
*******************/

/********************* Librerias ********************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotonPanel : MonoBehaviour
{
    /**************** Variables ******************/
    public GameObject Panel;


    /**************** Funciones ******************/
    public void  OpenPanel()        //Activa y desactiva el panel principal
    {
        if(Panel != null)
        {
            bool isActive = Panel.activeSelf;
            Panel.SetActive(!isActive);
        }
    }
}
