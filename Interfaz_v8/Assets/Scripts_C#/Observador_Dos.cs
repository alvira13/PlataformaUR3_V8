/*******************
Autores:    Jan Carlos Alvira Meneses(janalvira@unicauca.edu.co)
            Leonardo Alberto Paz Paz (leopaz@unicauca.edu.co)
*******************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;



public class Observador_Dos : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera virtualcamera;
    [SerializeField] CinemachineVirtualCamera virtualcamera1;
    [SerializeField] CinemachineVirtualCamera virtualcamera2;
    [SerializeField] CinemachineVirtualCamera virtualcamera3;
    [SerializeField] CinemachineVirtualCamera virtualcamera4;
    CinemachineComponentBase ComponentBase;
    CinemachineComponentBase ComponentBase1;
    CinemachineComponentBase ComponentBase2;
    CinemachineComponentBase ComponentBase3;
    CinemachineComponentBase ComponentBase4;
    float cameraDistance;
    [SerializeField] float sensitivity = 10f;
    [SerializeField] float sensitivity1 = 5f;
    [SerializeField] float sensitivity2 = 8f;
    [SerializeField] float sensitivity3 = 5f;
    [SerializeField] float sensitivity4 = 5f;


    // Update is called once per frame
    void Update()
    {
        if (ComponentBase == null)
        {
            ComponentBase =virtualcamera.GetCinemachineComponent(CinemachineCore.Stage.Body);
        }
        if (Input.GetAxis("Mouse ScrollWheel") !=0)
        {
            cameraDistance = Input.GetAxis("Mouse ScrollWheel")*sensitivity;
            if (ComponentBase is CinemachineFramingTransposer)
            {
            (ComponentBase as CinemachineFramingTransposer).m_CameraDistance -=cameraDistance;
            }

        }

        if (ComponentBase1 == null)
        {
            ComponentBase1 =virtualcamera1.GetCinemachineComponent(CinemachineCore.Stage.Body);
        }
        if (Input.GetAxis("Mouse ScrollWheel") !=0)
        {
            cameraDistance = Input.GetAxis("Mouse ScrollWheel")*sensitivity;
            if (ComponentBase1 is CinemachineFramingTransposer)
            {
            (ComponentBase1 as CinemachineFramingTransposer).m_CameraDistance -=cameraDistance;
            }

        }

        if (ComponentBase2 == null)
        {
            ComponentBase2 =virtualcamera2.GetCinemachineComponent(CinemachineCore.Stage.Body);
        }
        if (Input.GetAxis("Mouse ScrollWheel") !=0)
        {
            cameraDistance = Input.GetAxis("Mouse ScrollWheel")*sensitivity;
            if (ComponentBase2 is CinemachineFramingTransposer)
            {
            (ComponentBase2 as CinemachineFramingTransposer).m_CameraDistance -=cameraDistance;
            }

        }

         if (ComponentBase3 == null)
        {
            ComponentBase3 =virtualcamera3.GetCinemachineComponent(CinemachineCore.Stage.Body);
        }
        if (Input.GetAxis("Mouse ScrollWheel") !=0)
        {
            cameraDistance = Input.GetAxis("Mouse ScrollWheel")*sensitivity;
            if (ComponentBase3 is CinemachineFramingTransposer)
            {
            (ComponentBase3 as CinemachineFramingTransposer).m_CameraDistance -=cameraDistance;
            }

        }
         if (ComponentBase4 == null)
        {
            ComponentBase4 =virtualcamera4.GetCinemachineComponent(CinemachineCore.Stage.Body);
        }
        if (Input.GetAxis("Mouse ScrollWheel") !=0)
        {
            cameraDistance = Input.GetAxis("Mouse ScrollWheel")*sensitivity;
            if (ComponentBase4 is CinemachineFramingTransposer)
            {
            (ComponentBase4 as CinemachineFramingTransposer).m_CameraDistance -=cameraDistance;
            }

        }
    }
}
