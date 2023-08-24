using UnityEngine;
using rosapi = RosSharp.RosBridgeClient.MessageTypes.Rosapi;

namespace RosSharp.RosBridgeClient
{
    [RequireComponent(typeof(RosConnector))]
    public abstract class UnityServiceCaller<Tin, Tout> : MonoBehaviour where Tin : Message where Tout : Message
    {
        public string ServiceName;

        protected virtual void Start()
        {
            GetComponent<RosConnector>().RosSocket.CallService<rosapi.GetParamRequest, Tout>(ServiceName, ServiceCall, new rosapi.GetParamRequest("/rosdistro", "default"));
        }

        protected abstract void ServiceCall(Tout response);

        protected abstract void valor(Tin request);

    }
}