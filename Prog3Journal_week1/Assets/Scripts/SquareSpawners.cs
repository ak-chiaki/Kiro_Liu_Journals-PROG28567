using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Rendering;
using Color = UnityEngine.Color;

public class SquareSpawners : MonoBehaviour
{
    public float size = 1f;

    void Update()
    {
        float a = Input.mouseScrollDelta.y; //get the mousescroll value
        size += a * 0.2f; //change the senstivity
        transform.localScale = new Vector3(size, size, 0);

        Vector3 mousePosV3 = Camera.main.ScreenToWorldPoint(Input.mousePosition); //get the mouse position
        transform.position = mousePosV3;

        Vector2 mousePosV2 = mousePosV3;//transfer Vector3 to Vector 2

        Vector2 quad1 = mousePosV2 + new Vector2(mousePosV2.x + size, mousePosV2.y + size);
        Vector2 quad2 = mousePosV2 + new Vector2(mousePosV2.x - size, mousePosV2.y + size);
        Vector2 quad3 = mousePosV2 + new Vector2(mousePosV2.x - size, mousePosV2.y - size);
        Vector2 quad4 = mousePosV2 + new Vector2(mousePosV2.x + size, mousePosV2.y - size);//get the four points of square
        

        if (Input.GetMouseButtonDown(0))
        {

            Debug.DrawLine(quad1, quad2, Color.white,100f);
            Debug.DrawLine(quad2, quad3, Color.white, 100f);
            Debug.DrawLine(quad3, quad4, Color.white, 100f);
            Debug.DrawLine(quad4, quad1, Color.white, 100f);
        }



    }
}
