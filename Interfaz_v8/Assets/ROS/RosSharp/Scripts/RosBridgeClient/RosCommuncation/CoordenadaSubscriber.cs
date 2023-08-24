/*******************
Autores:    Juan David Ruiz (juandavidrf@unicauca.edu.co)
            Sebastian Montenegro (exlogam@unicauca.edu.co)
*******************/

/********************* Librerias ********************/
using UnityEngine;
using UnityEngine.UI;
using Unity.Mathematics;
using System.Collections;
using System.Collections.Generic;
using TMPro;

namespace RosSharp.RosBridgeClient
{
    public class CoordenadaSubscriber : UnitySubscriber<MessageTypes.Tf2.TFMessage>
    {
        /**************** Variables ******************/
        private Vector3 position;                           //Vector de posicion cartesiana
        private Quaternion rotationQ;                       //CVector de rotacion en cuaternions
        private Vector3 rotationD;                          //Vector de rotacion en grados(RPY)
        public List<Text> Cartesiano = new List<Text>();    //Casillas de texto del panel cartesiano
        public List<TMP_Text> Posicion = new List<TMP_Text>();  //Casillas de texto superiores
        private bool Visible;                               //Booleano de visibilidad


        /**************** Funciones ******************/
        protected override void Start()                     //Inicaliza el suscriptor
        {
			base.Start();
            Visible = false;
		}
        
		
        private void Update()                               //Mantine actualizada las coordenadas cartesianas
        {
            if (Visible)
            {
                ProcessMessage();
            }  
            else
            {
                HideMessage();
            }
            if(Input.GetKeyDown(KeyCode.P))
            {
                Visible=!Visible;
            }
            MensajeCartesiano();
        }

        public void Suscribir()                             //Suscribe el cliente ROS al topico
        {
            base.Start();
        }

        protected override void ReceiveMessage(MessageTypes.Tf2.TFMessage message)  //Recibe el mensaje ROS
        {
            position = GetPosition(message);
            rotationQ = GetRotation(message);
        }

        private void ProcessMessage()                               //Asigna las coordenadas a las casillas de texto superiores
        {
            Posicion[0].text = ("Px: ")+position.x.ToString("f3");
            Posicion[1].text = ("Py: ")+position.y.ToString("f3");
            Posicion[2].text = ("Pz: ")+position.z.ToString("f3");

            rotationD = ConvertirAngulos(rotationQ);
            
            Posicion[3].text = ("Rx: ")+rotationD.x.ToString("f2");
            Posicion[4].text = ("Ry: ")+rotationD.y.ToString("f2");
            Posicion[5].text = ("Rz: ")+rotationD.z.ToString("f2");
        }

        private void MensajeCartesiano()                            //Asigna las coordenadas a las casillas de texto del panel cartesiano
        {
            //Cartesiano
            Cartesiano[0].text = position.x.ToString("f3");
            Cartesiano[1].text = position.y.ToString("f3");
            Cartesiano[2].text = position.z.ToString("f3");

            rotationD = ConvertirAngulos(rotationQ);

            Cartesiano[3].text = rotationD.x.ToString("f2");
            Cartesiano[4].text = rotationD.y.ToString("f2");
            Cartesiano[5].text = rotationD.z.ToString("f2");
        }
        private void HideMessage()                                  //Oculta las casillas de texto superiores 
        {
            Posicion[0].text = ("");
            Posicion[1].text = ("");
            Posicion[2].text = ("");
            Posicion[3].text = ("");
            Posicion[4].text = ("");
            Posicion[5].text = ("");
        }

        private Vector3 GetPosition(MessageTypes.Tf2.TFMessage message)             //Obtine la posicion del robot del mensaje ROS
        {        
            Vector3 posi; 
                posi.x = (float)message.transforms[0].transform.translation.x*100f;
                posi.y = (float)message.transforms[0].transform.translation.y*100f;
                posi.z = (float)message.transforms[0].transform.translation.z*100f;
            
            return posi;
        }

        private Quaternion GetRotation(MessageTypes.Tf2.TFMessage message)          //Obtine la rotacion del robot del mensaje ROS
        {
            Quaternion ori;
            ori.x = (float)message.transforms[0].transform.rotation.x;
            ori.y = (float)message.transforms[0].transform.rotation.y;
            ori.z = (float)message.transforms[0].transform.rotation.z;
            ori.w = (float)message.transforms[0].transform.rotation.w;
            return ori;
        }

        public static float XAngulo(Quaternion q)                           //Retorna el angulo del eje X en grados
        {
            float sinr_cosp = 2 * (q.w * q.x + q.y * q.z);
            float cosr_cosp = 1 - 2 * (q.x * q.x + q.y * q.y);
            return math.atan2(sinr_cosp, cosr_cosp);
        }

        public static float YAngulo( Quaternion q)                          //Retorna el angulo del eje Y en grados
        {
            float sinp = 2 * (q.w * q.y - q.z * q.x);
            if (math.abs(sinp) >= 1)
                return math.PI / 2 * math.sign(sinp); // use 90 degrees if out of range
            else
                return math.asin(sinp);
        }

        public static float ZAngulo(Quaternion q)                           //Retorna el angulo del eje Z en grados
        {
            float siny_cosp = 2 * (q.w * q.z + q.x * q.y);
            float cosy_cosp = 1 - 2 * (q.y * q.y + q.z * q.z);
            return math.atan2(siny_cosp, cosy_cosp);
        }

        public static Vector3 ConvertirAngulos(Quaternion q)                //Convierte la rotacion de cuaternoins en grados(RPY)
        {
            Vector3 angulos = new Vector3();
            angulos.x = (float)(XAngulo(q)*(180/math.PI));
            angulos.y = (float)(YAngulo(q)*(180/math.PI));
            angulos.z = (float)(ZAngulo(q)*(180/math.PI));
            return angulos;
        }
    }
}