/*******************
Autores:    Juan David Ruiz (juandavidrf@unicauca.edu.co)
            Sebastian Montenegro (exlogam@unicauca.edu.co)
*******************/

/********************* Librerias ********************/
using UnityEngine;
using UnityEngine.EventSystems;

public class Observador : MonoBehaviour
{
    /**************** Variables ******************/
    public float VelMov;                                    //Velocidad de traslacion
    public float VelRot;                                    //Velocidad de rotacion
    Vector3 Movimiento;                                     //Traslacion de la camara
    Vector2 Rotacion;                                       //Rotacion de la camara
    Vector3 PosicionInicial;                                //Posicion inicial
    Quaternion RotacionInicial;                             //Rotacion inical
    bool CambioVista1=true;                                 //Booleano para vista frontal y trasera
    bool CambioVista2=true;                                 //Booleano para vistas laterales


    /**************** Funciones ******************/
    void Awake()                                            //Asigna la posicion inicial
    {
        PosicionInicial = transform.position;
        RotacionInicial = transform.rotation;
    }

    void Update()                                           //Mantine actualizada la vista de la camara
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Movimiento.x = Input.GetAxis("Horizontal")* VelMov * Time.deltaTime;
            Movimiento.y = Input.GetAxis("Vertical")* VelMov * Time.deltaTime;
            Movimiento.z = Input.GetAxis("Zoom")* VelMov * Time.deltaTime;
            Rotacion.x = Input.GetAxis("RotHorizontal")* VelRot * Time.deltaTime;
            Rotacion.y = Input.GetAxis("RotVertical")* VelRot * Time.deltaTime;

            MoverVista();

            // Retorna a la vista inicial
            if(Input.GetKeyDown(KeyCode.R))
            {
                ReiniciarVista();
            }

            // Ubica la vista en los planos frontal y trasero
            if(Input.GetKeyDown(KeyCode.T))
            {
                if(CambioVista1)
                {
                    VistaFrontal();
                } else
                {
                    VistaTrasera();
                }
                CambioVista1 = !CambioVista1;
            }

            // Ubica la vista en los planos laterales
            if(Input.GetKeyDown(KeyCode.Y))
            {
                if(CambioVista2)
                {
                    VistaLateraleDerecha();
                } else
                {
                    VistaLateraleIzquierda();
                }
                CambioVista2 = !CambioVista2;
            }

            // Ubica la vista en el plano superior
            if(Input.GetKeyDown(KeyCode.U))
            {
                VistaSuperior();
            }
        }

    }

    void MoverVista()                           //Mover la camara por el espacio
    {
        transform.Translate(Movimiento.x, Movimiento.y, Movimiento.z);
        transform.Rotate(Rotacion.y, Rotacion.x, 0);
    }

    void ReiniciarVista()                       //Mover la camara a la vista inicial
    {
        transform.position = PosicionInicial;
        transform.rotation = RotacionInicial;
    }

    void VistaFrontal()                         //Camara con vista frontal al robot
    {
        transform.position = new Vector3 (-0.727f, 0.299f, 0.779f);
        transform.rotation = Quaternion.Euler(new Vector3 (0, 135, 0));
    }
    void VistaTrasera()                         //Camara con vista trasera al robot
    {
        transform.position = new Vector3 (0.7f, 0.299f, -0.725f);
        transform.rotation = Quaternion.Euler(new Vector3 (0, 315, 0));
    }

    void VistaLateraleDerecha()                 //Camara con vista lateral derecha al robot
    {
        transform.position = new Vector3 (-0.736f, 0.299f, -0.394f);
        transform.rotation = Quaternion.Euler(new Vector3 (0, 45, 0));
    }
    void VistaLateraleIzquierda()               //Camara con vista lateral izquierda al robot
    {
        transform.position = new Vector3 (0.359f, 0.299f, 0.957f);
        transform.rotation = Quaternion.Euler(new Vector3 (0, 225, 0));
    }

    void VistaSuperior()                        //Camara con vista superior al robot
    {
        transform.position = new Vector3 (-0.246f, 1.065f, 0.267f);
        transform.rotation = Quaternion.Euler(new Vector3 (90, 135, 0));
    }
}