/*******************
Autores:    Juan David Ruiz (juandavidrf@unicauca.edu.co)
            Sebastian Montenegro (exlogam@unicauca.edu.co)
*******************/

/********************* Librerias ********************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace RosSharp.RosBridgeClient
{
    public class ArtPublisher : UnityPublisher<MessageTypes.Trajectory.JointTrajectory>
    {
        /**************** Variables ******************/
        public Control_Articular posiciones;                        //Script del control articular
        private int numeroPosiciones;                               //Numero de posiciones en la lista del panel posiciones
        private MessageTypes.Trajectory.JointTrajectory message;    //Mensaje ROS de tipo JointTrajectory
        private double[] ValorArt;                                  //Vector con los valores articulares
        private double[] Velocidad;                                 //Velocidad de movimiento
        

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
            message = new MessageTypes.Trajectory.JointTrajectory();
        }

        public void joints()                                        //Organiza y publica el mensaje si se mueve un slider
        {
            
            message.points = new MessageTypes.Trajectory.JointTrajectoryPoint[1];
            message.points[0] = new MessageTypes.Trajectory.JointTrajectoryPoint();
            ValorArt = new double[6];

            for (int j = 0; j < 6; j++)
            {
                ValorArt[j] = (GameObject.Find("Art"+(j+1)+"/Slider").GetComponent<Slider>().value)*(Mathf.PI/180);
            }
            message.points[0].positions = ValorArt;
            Velocidad = new double[] {3,0,0,0,0,0};

            message.points[0].velocities = Velocidad;

            UpdateMessage();
        }
        public void Botones()                                       //Organiza y publica el mensaje si se oprimen los botones de lso sliders
        {
            
            message.points = new MessageTypes.Trajectory.JointTrajectoryPoint[1];
            message.points[0] = new MessageTypes.Trajectory.JointTrajectoryPoint();
            ValorArt = new double[6];

            for (int j = 0; j < 6; j++)
            {
                ValorArt[j] = (GameObject.Find("Art"+(j+1)+"/Slider").GetComponent<Slider>().value)*(Mathf.PI/180);
            }
            message.points[0].positions = ValorArt;
            Velocidad = new double[] {0.1,0,0,0,0,0};

            message.points[0].velocities = Velocidad;

            UpdateMessage();
        }

        public void ListaPosiciones()                               //Organiza y publica la lista de posiciones del panel posiciones
        {
            numeroPosiciones = posiciones.NumPos;   
            message.points = new MessageTypes.Trajectory.JointTrajectoryPoint[numeroPosiciones]; 

            for (int j = 0; j < numeroPosiciones; j++)
            {
                message.points[j] = new MessageTypes.Trajectory.JointTrajectoryPoint();
                ValorArt = new double[6];

                for (int i = 0; i < 6; i++)
                {
                    ValorArt[i] = (float.Parse(GameObject.Find("Posi"+(j)+"/VJoint"+(i+1)).GetComponent<TMP_Text>().text))*(Mathf.PI/180);
                }
                message.points[j].positions = ValorArt;
                
                Velocidad = new double[] {40,0,0,0,0,0};
                message.points[j].velocities = Velocidad;
            }
            UpdateMessage();
        }

        private void UpdateMessage()                                    //Publica el mensaje en el nodo de ROS
        {
            Publish(message);
        }
    }
}