using System.Security.Cryptography;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Cambio_Camera : MonoBehaviour
{
    //[SerializeField] CinemachineVirtualCamera virtualcamera0;
    public GameObject virtualcamera0;       // Camara vista lateral izquierda*
    public GameObject virtualcamera1;       // Camara vista superior derecha con Zomm *
    public GameObject virtualcamera2;       // Camara vista frontal *
    public GameObject virtualcamera3;       // Camara vista original *
    public GameObject virtualcamera4;       // Camara vista superior con Zomm *
    public GameObject virtualcamera5;       // Camara vista desde la perspectiva de la base del robot
    public GameObject virtualcamera6;       // Camara vista lateral derecha con zoom *
    public GameObject virtualcamera7;       // Camara vista lateral izq con zoom  *
    public GameObject virtualcamera8; 
    bool CambioVista1=true;
    bool CambioVista2=true;
    bool CambioVista3=true;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.U))
        {
            if(CambioVista1)
                {
                    vm_camera_one();
                } else
                {
                    vm_camera_four();
                }
         CambioVista1 = !CambioVista1;
            
        }
        
        if(Input.GetKeyDown(KeyCode.T))
        {
            if(CambioVista2)
                {
                    vm_camera_two();
                } else
                {
                    vm_camera_five();
                }
         CambioVista2 = !CambioVista2;
            
        }
        
        if(Input.GetKeyDown(KeyCode.Y))
        {
           if(CambioVista3)
                {
                    vm_camera_seven();
                } else
                {
                    vm_camera_six();
                }
         CambioVista3 = !CambioVista3; 
           
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            vm_camera_Three();
        }
       
        if (Input.GetKeyDown(KeyCode.D))
        {
            vm_camera();
        }
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            vm_camera_eigth();
        }
    }
        void vm_camera ()
        {
            virtualcamera0.SetActive(true);
            virtualcamera1.SetActive(false);
            virtualcamera2.SetActive(false);
            virtualcamera3.SetActive(false);
            virtualcamera5.SetActive(false);
            virtualcamera4.SetActive(false);
            virtualcamera6.SetActive(false);
            virtualcamera7.SetActive(false);
            virtualcamera8.SetActive(false);

        }

        void vm_camera_one ()
        {
            virtualcamera0.SetActive(false);
            virtualcamera1.SetActive(true);
            virtualcamera2.SetActive(false);
            virtualcamera3.SetActive(false);
            virtualcamera5.SetActive(false);
            virtualcamera4.SetActive(false);
            virtualcamera6.SetActive(false);
            virtualcamera7.SetActive(false);
            virtualcamera8.SetActive(false);


        }

        void vm_camera_two ()      //*
        {
            virtualcamera0.SetActive(false);
            virtualcamera1.SetActive(false);
            virtualcamera2.SetActive(true);
            virtualcamera3.SetActive(false);
            virtualcamera5.SetActive(false);
            virtualcamera4.SetActive(false);
            virtualcamera6.SetActive(false);
            virtualcamera7.SetActive(false);
            virtualcamera8.SetActive(false);


        }

        void vm_camera_Three()     //*
        {
            virtualcamera0.SetActive(false);
            virtualcamera1.SetActive(false);
            virtualcamera2.SetActive(false);
            virtualcamera3.SetActive(true);
            virtualcamera5.SetActive(false);
            virtualcamera4.SetActive(false);
            virtualcamera6.SetActive(false);
            virtualcamera7.SetActive(false);
            virtualcamera8.SetActive(false);


        }

        void vm_camera_four ()
        {
            virtualcamera0.SetActive(false);
            virtualcamera1.SetActive(false);
            virtualcamera2.SetActive(false);
            virtualcamera3.SetActive(false);
            virtualcamera4.SetActive(true);
            virtualcamera5.SetActive(false);
            virtualcamera6.SetActive(false);
            virtualcamera7.SetActive(false);
            virtualcamera8.SetActive(false);


        }

     void vm_camera_five ()  //*
        {
            virtualcamera0.SetActive(false);
            virtualcamera1.SetActive(false);
            virtualcamera2.SetActive(false);
            virtualcamera3.SetActive(false);
            virtualcamera4.SetActive(false);
            virtualcamera5.SetActive(true);
            virtualcamera6.SetActive(false);
            virtualcamera7.SetActive(false);
            virtualcamera8.SetActive(false);

        }
     void vm_camera_six() //*
        {
            virtualcamera0.SetActive(false);
            virtualcamera1.SetActive(false);
            virtualcamera2.SetActive(false);
            virtualcamera3.SetActive(false);
            virtualcamera5.SetActive(false);
            virtualcamera4.SetActive(false);
            virtualcamera6.SetActive(true);
            virtualcamera7.SetActive(false);
            virtualcamera8.SetActive(false);

        }

        void vm_camera_seven() //*
        {
            virtualcamera0.SetActive(false);
            virtualcamera1.SetActive(false);
            virtualcamera2.SetActive(false);
            virtualcamera3.SetActive(false);
            virtualcamera4.SetActive(false);
            virtualcamera5.SetActive(false);
            virtualcamera6.SetActive(false);
            virtualcamera7.SetActive(true);
            virtualcamera8.SetActive(false);
        }

        void vm_camera_eigth() //*
        {
            virtualcamera0.SetActive(false);
            virtualcamera1.SetActive(false);
            virtualcamera2.SetActive(false);
            virtualcamera3.SetActive(false);
            virtualcamera4.SetActive(false);
            virtualcamera5.SetActive(false);
            virtualcamera6.SetActive(false);
            virtualcamera7.SetActive(false);
            virtualcamera8.SetActive(true);
        }
    
    
}
