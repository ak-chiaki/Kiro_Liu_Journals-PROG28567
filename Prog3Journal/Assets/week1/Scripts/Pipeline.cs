using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

public class Pipeline : MonoBehaviour
{

    Vector3 lastpoint;
    Vector3 point;
    float timer = 0f;
    float totaldis = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))  //get the first point when mouse button press down
        {
            lastpoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            timer = 0f; // initialize the timer
        }

        if(Input.GetMouseButton(0)) 
        {
            timer += Time.deltaTime;

            if(timer > 0.1f)
            {
                point = Camera.main.ScreenToWorldPoint(Input.mousePosition); // get the point of the mouse after mouse down over 0.1sec
                Debug.DrawLine(lastpoint, point, Color.white, 2f);//draw the line

                float dx = point.x - lastpoint.x;
                float dy = point.y - lastpoint.y;
                float distance = Mathf.Sqrt(Mathf.Pow(dx, 2) + Mathf.Pow(dy, 2)); //calculate the distance
                totaldis += distance; // calculate the total distance


                lastpoint = point; // initialize the point

                timer = 0f; //clean the timer
            }
        }

        if (Input.GetMouseButtonUp(0)) //when mouse up
        {
            Debug.Log("total length = " + totaldis); // out put the distance log in console
            totaldis = 0f;//initialize the totaldis
        }

    }
}
