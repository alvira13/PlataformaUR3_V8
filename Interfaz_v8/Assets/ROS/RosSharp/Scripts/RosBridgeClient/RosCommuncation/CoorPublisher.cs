/*******************
Autores:    Juan David Ruiz (juandavidrf@unicauca.edu.co)
            Sebastian Montenegro (exlogam@unicauca.edu.co)
*******************/

/********************* Librerias ********************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using RosSharp.RosBridgeClient.MessageTypes.Std;
using Unity.Mathematics;


namespace RosSharp.RosBridgeClient
{
    public class CoorPublisher : UnityPublisher<MessageTypes.CartesianControl.CartesianTrajectory>
    {
        /**************** Variables ******************/
        public Control_Cartesiano coordenadas;                              //Script del control cartesiano
        private int numeroCoordenadas;                                      //Numero de coordenadas en la lista del panel coordenadas
        private MessageTypes.CartesianControl.CartesianTrajectory message;  //Mensaje ROS de tipo CartesianTrajectory
        private float velocidad;                                            //Velocidad de movimiento


        /**************** Funciones ******************/
        protected override void Start()                                 //Inicializa el publicador
        {
            base.Start();
            InitializeMessage();
        }

        public void Reactivar()                                         //Reactiva el cliente ROS para publicar
        {
            base.Start();
        }

        private void InitializeMessage()                                //Inicializa el mensaje ROS
        {
            message = new MessageTypes.CartesianControl.CartesianTrajectory();
        }

        public void coordenadaUnica()                                   //Organiza y publica el mensaje si es solo una coordenada
        {
            message.points = new MessageTypes.CartesianControl.CartesianTrajectoryPoint[1];
            Vector3 angulos = new Vector3();

            message.points[0] = new MessageTypes.CartesianControl.CartesianTrajectoryPoint();

            message.points[0].pose.position.x = ((float.Parse(GameObject.Find("Cartesiano/PosX").GetComponent<TMP_InputField>().text))*0.01);
            message.points[0].pose.position.y = ((float.Parse(GameObject.Find("Cartesiano/PosY").GetComponent<TMP_InputField>().text))*0.01);
            message.points[0].pose.position.z = ((float.Parse(GameObject.Find("Cartesiano/PosZ").GetComponent<TMP_InputField>().text))*0.01);
            
            angulos.x = (float.Parse(GameObject.Find("Cartesiano/OriX").GetComponent<TMP_InputField>().text));
            angulos.y = (float.Parse(GameObject.Find("Cartesiano/OriY").GetComponent<TMP_InputField>().text));
            angulos.z = (float.Parse(GameObject.Find("Cartesiano/OriZ").GetComponent<TMP_InputField>().text));
            Quaternion cuaternions = ToQuaternion(angulos);
            message.points[0].pose.orientation.x = cuaternions.x;
            message.points[0].pose.orientation.y = cuaternions.y;
            message.points[0].pose.orientation.z = cuaternions.z;
            message.points[0].pose.orientation.w = cuaternions.w;

            velocidad = GameObject.Find("SliderVel").GetComponent<Slider>().value*0.001f;
            message.points[0].twist.linear.x = velocidad;

            UpdateMessage();
        }

        public void coordinates()                                       //Organiza y publica la lista de coordenadas del panel coordenadas
        {
            numeroCoordenadas = coordenadas.NumCoor;
            message.points = new MessageTypes.CartesianControl.CartesianTrajectoryPoint[numeroCoordenadas];
            Vector3 angulos = new Vector3();

            for (int j = 0; j < numeroCoordenadas; j++)
            {
                message.points[j] = new MessageTypes.CartesianControl.CartesianTrajectoryPoint();

                message.points[j].pose.position.x = ((float.Parse(GameObject.Find("Coor"+(j)+"/VPosX").GetComponent<TMP_Text>().text))*0.01);
                message.points[j].pose.position.y = ((float.Parse(GameObject.Find("Coor"+(j)+"/VPosY").GetComponent<TMP_Text>().text))*0.01);
                message.points[j].pose.position.z = ((float.Parse(GameObject.Find("Coor"+(j)+"/VPosZ").GetComponent<TMP_Text>().text))*0.01);
                
                angulos.x = (float.Parse(GameObject.Find("Coor"+(j)+"/VOriX").GetComponent<TMP_Text>().text));
                angulos.y = (float.Parse(GameObject.Find("Coor"+(j)+"/VOriY").GetComponent<TMP_Text>().text));
                angulos.z = (float.Parse(GameObject.Find("Coor"+(j)+"/VOriZ").GetComponent<TMP_Text>().text));
                Quaternion cuaternions = ToQuaternion(angulos);
                message.points[j].pose.orientation.x = cuaternions.x;
                message.points[j].pose.orientation.y = cuaternions.y;
                message.points[j].pose.orientation.z = cuaternions.z;
                message.points[j].pose.orientation.w = cuaternions.w;

                velocidad = GameObject.Find("SliderVel").GetComponent<Slider>().value*0.001f;
                message.points[j].twist.linear.x = velocidad;
            }
            UpdateMessage();
        }

        public static Quaternion ToQuaternion(Vector3 v)                //Convierte la rotacion de grados(RPY) a cuaernions
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

        private void UpdateMessage()                                    //Publica el mensaje en el nodo de ROS
        {
            Publish(message);
        }
    }
}