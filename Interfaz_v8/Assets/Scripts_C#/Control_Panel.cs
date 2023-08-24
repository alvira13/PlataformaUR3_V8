/*******************
Autores:    Juan David Ruiz (juandavidrf@unicauca.edu.co)
            Sebastian Montenegro (exlogam@unicauca.edu.co)
*******************/

/********************* Librerias ********************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control_Panel : MonoBehaviour
{
    /**************** Variables ******************/
    //boolenos del estado de visibilidad los panesls
    [HideInInspector] public bool P_Principal;
    [HideInInspector] public bool P_Conexion;
    [HideInInspector] public bool P_Articular;
    [HideInInspector] public bool P_Cartesiano;
    [HideInInspector] public bool P_Predefinidas;
    [HideInInspector] public bool P_Posiciones;
    [HideInInspector] public bool P_Coordenadas;
    [HideInInspector] public bool P_Libre;

    //Objetos Panels
    public GameObject PanelMain;
    public GameObject PanelCon;
    public GameObject PanelArt;
    public GameObject PanelPos;
    public GameObject PanelCart;
    public GameObject PanelCoor;
    public GameObject PanelPred;
    public GameObject PanelLibre;
    public GameObject Trayectoria;


    /**************** Funciones ******************/
    void Start()                            //Inicacializa los panels (Solo el prinicpal visible)
    {
        P_Principal = true;
        P_Conexion = false;
        P_Articular = false;
        P_Cartesiano = false;
        P_Predefinidas = false;
        P_Posiciones = false;
        P_Coordenadas = false;
        P_Libre = false;
    }

    public void Conexion()                  //Activa y desactiva visibilidad del panel conexion
    {
        P_Conexion = !P_Conexion;
        P_Articular = false;
        P_Cartesiano = false;
        P_Predefinidas = false;
        P_Posiciones = false;
        P_Coordenadas = false;
        P_Libre = false;
        CambioPanel();
    }

    public void Articular()                 //Activa y desactiva visibilidad del panel articular
    {
        P_Conexion = false;
        P_Articular = !P_Articular;
        P_Cartesiano = false;
        P_Predefinidas = false;
        P_Posiciones = false;
        P_Coordenadas = false;
        P_Libre = false;
        CambioPanel();
    }

    public void Cartesiano()                //Activa y desactiva visibilidad del panel cartesiano
    {
        P_Conexion = false;
        P_Articular = false;
        P_Cartesiano = !P_Cartesiano;
        P_Predefinidas = false;
        P_Posiciones = false;
        P_Coordenadas = false;
        P_Libre = false;
        CambioPanel();
    }

    public void Predefinidas()              //Activa y desactiva visibilidad del panel de trayectorias predefinidas
    {
        P_Conexion = false;
        P_Articular = false;
        P_Cartesiano = false;
        P_Predefinidas = !P_Predefinidas;
        P_Posiciones = false;
        P_Coordenadas = false;
        P_Libre = false;
        CambioPanel();
    }

    public void Libre()                     //Activa y desactiva visibilidad del panel de trayectorias libres
    {
        P_Conexion = false;
        P_Articular = false;
        P_Cartesiano = false;
        P_Predefinidas = false;
        P_Posiciones = false;
        P_Coordenadas = false;
        P_Libre = !P_Libre;
        CambioPanel();
    }

    public void Posiciones()                //Cambia la visibilidad del panel de la lista de posiciones
    {
        P_Posiciones = !P_Posiciones;
        PanelPos.SetActive(P_Posiciones);
    }

    public void Coordenadas()               //Cambia la visibilidad del panel de la lista de coordenadas
    {
        P_Coordenadas = !P_Coordenadas;
        PanelCoor.SetActive(P_Coordenadas);
    }

    public void CambioPanel()               //Cambia la visibilidad de todos los paneles
    {
        PanelCon.SetActive(P_Conexion);
        PanelArt.SetActive(P_Articular);
        PanelPos.SetActive(P_Posiciones);
        PanelCart.SetActive(P_Cartesiano);
        PanelCoor.SetActive(P_Coordenadas);
        PanelPred.SetActive(P_Predefinidas);
        PanelLibre.SetActive(P_Libre);
        Trayectoria.SetActive(P_Libre);
    }

    public void OcultarPanel()              //Cambia la visbilidad del panel principal
    {
        P_Principal = !P_Principal;
        PanelMain.SetActive(P_Principal);
    }
}
