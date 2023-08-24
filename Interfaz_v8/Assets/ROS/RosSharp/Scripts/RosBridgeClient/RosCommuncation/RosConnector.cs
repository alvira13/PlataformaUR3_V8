/*
© Siemens AG, 2017-2019
Author: Dr. Martin Bischoff (martin.bischoff@siemens.com)

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

<http://www.apache.org/licenses/LICENSE-2.0>.

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

/*******************
    Este script ha sido ligeramente modificado
*******************/

using System;
using System.Threading;
using RosSharp.RosBridgeClient.Protocols;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace RosSharp.RosBridgeClient
{
    public class RosConnector : MonoBehaviour
    {
        private string DireccionIP;
        public int SecondsTimeout = 10;

        public RosSocket RosSocket { get; set; }
        public RosSocket.SerializerEnum Serializer;
        public Protocol protocol;
        private string RosBridgeServerUrl;
        [HideInInspector] public int EstadoConexion = 0;       //0=Desconectado  1=Concetado   2=Esperando   3=Fallido

        public Control_Conexion Control;

        public ManualResetEvent IsConnected { get; private set; }

        public virtual void Awake()
        {   
            //AsignarIP();
        }

        public void AsignarIP()
        {
            DireccionIP = (GameObject.Find("IP").GetComponent<TMP_InputField>().text);
            RosBridgeServerUrl = "ws://" + DireccionIP + ":9090";
            IsConnected = new ManualResetEvent(false);
            new Thread(ConnectAndWait).Start();
        }

        protected void ConnectAndWait()
        {
            EstadoConexion = 2;
            RosSocket = ConnectToRos(protocol, RosBridgeServerUrl, OnConnected, OnClosed, Serializer);

            if (!IsConnected.WaitOne(SecondsTimeout * 1000))
                Debug.LogWarning("Failed to connect to RosBridge at: " + RosBridgeServerUrl);
                EstadoConexion = 3;
        }

        public static RosSocket ConnectToRos(Protocol protocolType, string serverUrl, EventHandler onConnected = null, EventHandler onClosed = null, RosSocket.SerializerEnum serializer = RosSocket.SerializerEnum.Microsoft)
        {
            IProtocol protocol = ProtocolInitializer.GetProtocol(protocolType, serverUrl);
            protocol.OnConnected += onConnected;
            protocol.OnClosed += onClosed;

            return new RosSocket(protocol, serializer);
        }

        public void OnApplicationQuit()
        {
            RosSocket.Close();
        }

        private void OnConnected(object sender, EventArgs e)
        {
            IsConnected.Set();
            Debug.Log("Connected to RosBridge: " + RosBridgeServerUrl);
            EstadoConexion = 1;
        }

        private void OnClosed(object sender, EventArgs e)
        {
            IsConnected.Reset();
            Debug.Log("Disconnected from RosBridge: " + RosBridgeServerUrl);
            EstadoConexion = 0;
        }
    }
}