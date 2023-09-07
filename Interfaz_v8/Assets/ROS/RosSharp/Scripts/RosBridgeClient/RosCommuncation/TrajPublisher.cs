/*******************
Autores:    Juan David Ruiz (juandavidrf@unicauca.edu.co)
            Sebastian Montenegro (exlogam@unicauca.edu.co)
*******************/

/********************* Librerias ********************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

namespace RosSharp.RosBridgeClient 
{
    public class TrajPublisher : UnityPublisher<MessageTypes.CartesianControl.CartesianTrajectory>
    {
        /**************** Variables ******************/
        //public Vector3 angle = new Vector3(0,180,0);  // Hacemos pruebas en el Inspector ingresando ángulos manualmente
        private Vector3 angle_posini = new Vector3(-161.63f,-16.13f,180); // Ángulos Euler de la posición inicial
        private MessageTypes.CartesianControl.CartesianTrajectory message;  //Mensaje ROS de tipo CartesianTrajectory

        /**************** Funciones ******************/
        protected override void Start()                             //Inicializa el publicador
        {
            base.Start();
            InitializeMessage();
        }
        public void Reactivar()                                     //Reactiva el cliente ROS para publicar
        {
            base.Start();
        }
        private void InitializeMessage()                            //Inicializa el mensaje ROS
        {
            message = new MessageTypes.CartesianControl.CartesianTrajectory();
        }
        private void UpdateMessage()                                //Publica el mensaje en el nodo de ROS
        {
            Publish(message);
        }

        /* Posición inicial (me asegura que la pinza no se choque con el cráneo en su 
        trayecto hacia la punta de la nariz, por ejemplo)*/
        public void Pos_Inicial(float velocidad /*float[,] Coordenadas*/)
        {
            message.points = new MessageTypes.CartesianControl.CartesianTrajectoryPoint[1];
            message.points[0] = new MessageTypes.CartesianControl.CartesianTrajectoryPoint();

            message.points[0].pose.position.x = -0.22797;  // -0.17 (Medida en metros)
            message.points[0].pose.position.y = -0.21678;  // -0.17
            message.points[0].pose.position.z = 0.35115;   // 0.28
            Quaternion cuaternions = ToQuaternion(angle_posini);  // Ángulos cuaterniones de la posición incial 
            message.points[0].pose.orientation.x = cuaternions.x;      // 1/Mathf.Sqrt(2)
            message.points[0].pose.orientation.y = cuaternions.y;      // 1/Mathf.Sqrt(2)
            message.points[0].pose.orientation.z = cuaternions.z;  
            message.points[0].pose.orientation.w = cuaternions.w;  

            message.points[0].twist.linear.x = velocidad;
            UpdateMessage();

            /* El Debug.Log me ayuda a ver la posición exacta de la primera esfera (Entry-marker):
            message.points[0].pose.position.x = Coordenadas[0,0];
            Debug.Log("Coordenada en x-> " + Coordenadas[0,0]);
            message.points[0].pose.position.y = Coordenadas[0,1];
            Debug.Log("Coordenada en y-> " + Coordenadas[0,1]);
            message.points[0].pose.position.z = Coordenadas[0,2];
            Debug.Log("Coordenada en z-> " + Coordenadas[0,2]); */
        }

        //Función que me permite ejecutar una trayectoria libre con orientación fija
        public void Trayectoria(float[,] Coordenadas, int NumMuestras, float velocidad)   //Organiza y publica la trayectoria
        {         
            //Quaternion cuaternions = ToQuaternion(angle);  // Ángulos de prueba del inspector se convierten a cuaterniones      

            message.points = new MessageTypes.CartesianControl.CartesianTrajectoryPoint[NumMuestras];
   
                for(int j = 0; j < NumMuestras; j++)
                {
                    message.points[j] = new MessageTypes.CartesianControl.CartesianTrajectoryPoint();

                    message.points[j].pose.position.x = Coordenadas[j,0];
                    message.points[j].pose.position.y = Coordenadas[j,1];
                    message.points[j].pose.position.z = Coordenadas[j,2];
                    message.points[j].pose.orientation.x = 0;  
                    message.points[j].pose.orientation.y = 1;  
                    message.points[j].pose.orientation.z = 0;  
                    message.points[j].pose.orientation.w = 0;
                }
                message.points[0].twist.linear.x = velocidad;
                UpdateMessage();
        }

        //Función que me permite ejecutar una trayectoria libre con orientación dinámica, en sentido Entry-Target
        public void Trayectoria_Entry_target(float[,] Coordenadas, int NumMuestras, float velocidad)   //Organiza y publica la trayectoria
        {         
            // Definimos el punto de entrada y el punto objetivo como Vector3
            Vector3 entry = new Vector3(Coordenadas[0,0],Coordenadas[0,1],Coordenadas[0,2]);
            Vector3 target = new Vector3(Coordenadas[NumMuestras-1,0],Coordenadas[NumMuestras-1,1],Coordenadas[NumMuestras-1,2]);
            Vector3 direccion = target - entry;
            Quaternion rotation = Quaternion.LookRotation(direccion, Vector3.up);       

            message.points = new MessageTypes.CartesianControl.CartesianTrajectoryPoint[2];

            message.points[0] = new MessageTypes.CartesianControl.CartesianTrajectoryPoint();

            message.points[0].pose.position.x = Coordenadas[0,0];
            message.points[0].pose.position.y = Coordenadas[0,1];
            message.points[0].pose.position.z = Coordenadas[0,2];
            message.points[0].pose.orientation.x = rotation.x;  
            message.points[0].pose.orientation.y = rotation.y;  
            message.points[0].pose.orientation.z = rotation.z;  
            message.points[0].pose.orientation.w = rotation.w;

            message.points[1] = new MessageTypes.CartesianControl.CartesianTrajectoryPoint();

            message.points[1].pose.position.x = Coordenadas[NumMuestras-1,0];
            message.points[1].pose.position.y = Coordenadas[NumMuestras-1,1];
            message.points[1].pose.position.z = Coordenadas[NumMuestras-1,2];
            message.points[1].pose.orientation.x = rotation.x;  
            message.points[1].pose.orientation.y = rotation.y;  
            message.points[1].pose.orientation.z = rotation.z;  
            message.points[1].pose.orientation.w = rotation.w;  
                
            message.points[0].twist.linear.x = velocidad;
            UpdateMessage();
        }
        
        //Función que me permite ejecutar una trayectoria libre con orientación dinámica, en sentido Target-Entry
        public void Trayectoria_target_Entry(float[,] Coordenadas_past, int NumMuestras, float velocidad)   //Organiza y publica la trayectoria
        {                     
            // Definimos el punto de entrada y el punto objetivo de la trayectoria anterior como Vector3
            Vector3 entry_past = new Vector3(Coordenadas_past[0,0],Coordenadas_past[0,1],Coordenadas_past[0,2]);
            Vector3 target_past = new Vector3(Coordenadas_past[NumMuestras-1,0],Coordenadas_past[NumMuestras-1,1],Coordenadas_past[NumMuestras-1,2]);
            Vector3 direccion_past = target_past - entry_past;
            Quaternion rotation_past = Quaternion.LookRotation(direccion_past, Vector3.up);

            message.points = new MessageTypes.CartesianControl.CartesianTrajectoryPoint[2];

            message.points[0] = new MessageTypes.CartesianControl.CartesianTrajectoryPoint();

            message.points[0].pose.position.x = Coordenadas_past[NumMuestras-1,0];
            message.points[0].pose.position.y = Coordenadas_past[NumMuestras-1,1];
            message.points[0].pose.position.z = Coordenadas_past[NumMuestras-1,2];
            message.points[0].pose.orientation.x = rotation_past.x;  
            message.points[0].pose.orientation.y = rotation_past.y;  
            message.points[0].pose.orientation.z = rotation_past.z;  
            message.points[0].pose.orientation.w = rotation_past.w; 

            message.points[1] = new MessageTypes.CartesianControl.CartesianTrajectoryPoint();

            message.points[1].pose.position.x = Coordenadas_past[0,0];
            message.points[1].pose.position.y = Coordenadas_past[0,1];
            message.points[1].pose.position.z = Coordenadas_past[0,2];
            message.points[1].pose.orientation.x = rotation_past.x;  
            message.points[1].pose.orientation.y = rotation_past.y;  
            message.points[1].pose.orientation.z = rotation_past.z;  
            message.points[1].pose.orientation.w = rotation_past.w;
                
            message.points[0].twist.linear.x = velocidad;
            UpdateMessage();
        }

        //Función que me permite ejecutar un movimiento en péndulo con el Marker-Entry como pivote
        public void Trayectoria_Pivote(float[,] Coordenadas, int NumMuestras, float velocidad)   //Organiza y publica la trayectoria
        {           
            // Definimos el punto de entrada y el punto objetivo de la trayectoria actual como Vector3
            Vector3 entry = new Vector3(Coordenadas[0,0],Coordenadas[0,1],Coordenadas[0,2]);
            Vector3 target = new Vector3(Coordenadas[NumMuestras-1,0],Coordenadas[NumMuestras-1,1],Coordenadas[NumMuestras-1,2]);
            Vector3 direccion = target - entry;
            Quaternion rotation = Quaternion.LookRotation(direccion, Vector3.up);      

            message.points = new MessageTypes.CartesianControl.CartesianTrajectoryPoint[1];

                    message.points[0] = new MessageTypes.CartesianControl.CartesianTrajectoryPoint();

                    message.points[0].pose.position.x = Coordenadas[NumMuestras-1,0];
                    message.points[0].pose.position.y = Coordenadas[NumMuestras-1,1];
                    message.points[0].pose.position.z = Coordenadas[NumMuestras-1,2];
                    message.points[0].pose.orientation.x = rotation.x;  
                    message.points[0].pose.orientation.y = rotation.y;  
                    message.points[0].pose.orientation.z = rotation.z;  
                    message.points[0].pose.orientation.w = rotation.w;  
                    
                message.points[0].twist.linear.x = velocidad;
                UpdateMessage();
            
        }

        public static Quaternion ToQuaternion(Vector3 v)      //Convierte la rotacion de grados(RPY) a cuaternions
        {
            v.x = Mathf.Deg2Rad*v.x;
            v.y = Mathf.Deg2Rad*v.y;
            v.z = Mathf.Deg2Rad*v.z;

            float cy = (float)Math.Cos(v.z * 0.5);
            float sy = (float)Math.Sin(v.z * 0.5);
            float cp = (float)Math.Cos(v.y * 0.5);
            float sp = (float)Math.Sin(v.y * 0.5);
            float cr = (float)Math.Cos(v.x * 0.5);
            float sr = (float)Math.Sin(v.x * 0.5);

            return new Quaternion
            {
                w = (cr * cp * cy + sr * sp * sy),
                x = (sr * cp * cy - cr * sp * sy),
                y = (cr * sp * cy + sr * cp * sy),
                z = (cr * cp * sy - sr * sp * cy)
            };
        }      
    }
}