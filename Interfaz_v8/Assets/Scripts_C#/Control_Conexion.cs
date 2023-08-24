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

namespace RosSharp.RosBridgeClient
{
    [RequireComponent(typeof(RosConnector))]            //Esto indica que debe estar asociado con el script RosConnector en el inspector de unity
    public class Control_Conexion : MonoBehaviour
    {
        /**************** Variables ******************/
        public RosConnector ROS;                            //Script de RosConector
        public JointStateSubscriber SubArt;                 //Script del suscriptor articular
        public CoordenadaSubscriber SubCoor;                //Script del suscriptor cartesiano
        public ArtPublisher PubArt;                         //Script del publicador articular
        public CoorPublisher PubCoor;                       //Script del publicador cartesiano
        public TrajPublisher PubTraj;                       //Script del publicador de trayectorias
        public Text Estado;                                 //Texto del estado en el panel de conexion
        

        /**************** Funciones ******************/
        void Update()                                       //Mantiene actualizado el estado de conexion
        {
            //ROS.EstadoConexion devuelve el estado de conexion en 4 casos posibles
            switch (ROS.EstadoConexion)
            {
                case 0:
                    Estado.text = ("Estado: Desconectado");
                    break;
                case 1:
                    Estado.text = ("Estado: Conectado");
                    break;
                case 2:
                    Estado.text = ("Conectando...");
                    break;
                case 3:
                    Estado.text = ("Estado: Conexión fallida");
                    break;
            }
        }

        public void ConectarRos()                           //Envia la IP al script de conexion de ROS y cctiva los suscriptores y publicadores
        {
            ROS.AsignarIP();
            Invoke("ActivarPubSub", 1.0f);  //Espera un segundo para activar los scripts de suscripcion y publicacion, esto para que primero s conecte el nodo a ROS
        }

        public void DesconectarRos()                        //Desactiva los suscriptores y publicadores y cierra el nodo de ROS
        { 
            DesctivarPubSub();  
            ROS.RosSocket.Close();
        }

        public void ActivarPubSub()                         //Activa y reinicializa los scripts publicadores y suscriptores
        {
            SubArt.enabled = true;
            SubArt.Suscribir();
            SubCoor.enabled = true;
            SubCoor.Suscribir();
            PubArt.enabled = true;
            PubArt.Reactivar();
            PubCoor.enabled = true;
            PubCoor.Reactivar();
            PubTraj.enabled = true;
            PubTraj.Reactivar();
        }

        public void DesctivarPubSub()                       //Desactiva los scripst publicadores y suscriptores
        {
            SubArt.enabled = false;
            SubCoor.enabled = false;
            PubArt.enabled = false;
            PubCoor.enabled = false;
            PubTraj.enabled = false;
        }
    }
}
