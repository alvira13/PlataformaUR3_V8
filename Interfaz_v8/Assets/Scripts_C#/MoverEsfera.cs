/*******************
    Fuente: https://answers.unity.com/questions/1761558/how-can-i-drag-3d-object-x-axis-only-when-mouse-dr.html
*******************/

/********************* Librerias ********************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverEsfera : MonoBehaviour
{
    /**************** Variables ******************/
    private Vector3 mOffset;
    private float mZCoord;


    /**************** Funciones ******************/
    void OnMouseDown()
    {
        mZCoord = Camera.main.WorldToScreenPoint(
        gameObject.transform.position).z;

        // Store offset = gameobject world pos - mouse world pos
        mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
    }

    private Vector3 GetMouseAsWorldPoint()
    {
        // Pixel coordinates of mouse (x,y)
        Vector3 mousePoint = Input.mousePosition;

        // z coordinate of game object on screen
        mousePoint.z = mZCoord;

        // Convert it to world points
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    void OnMouseDrag()
    {
        transform.position = GetMouseAsWorldPoint() + mOffset;
    }
}
