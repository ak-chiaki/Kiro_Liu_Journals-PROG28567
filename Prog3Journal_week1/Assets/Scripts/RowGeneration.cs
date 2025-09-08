using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RowGeneration : MonoBehaviour
{
    public float size = 1f;
    Vector2 origin = new Vector2(0, 0);  // the origin spawn point
    public Button generateButton;
    public TMP_InputField squareNumberInput;

    // Start is called before the first frame update
    void Start()
    {
        generateButton.onClick.AddListener(GenerateRow);  // a listener to check if UI botton is triggered
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateRow()
    {
        string txt = squareNumberInput.text;  
        int SqNumber = int.Parse(txt);  // transfer the text to int
        if (SqNumber > 0) //check the avialibility of the sqnumber
        {

            for (int i = 0; i < SqNumber; i++)
            {
                float sqCenX = origin.x + i * size;
                float SqCenY = origin.y;
                float s = size / 2f;  //the half of the size of squares, I use it for calculate the four points of the squares

                Vector2 quad1 = new Vector2(sqCenX + s, SqCenY + s);
                Vector2 quad2 = new Vector2(sqCenX - s, SqCenY + s);
                Vector2 quad3 = new Vector2(sqCenX - s, SqCenY - s);
                Vector2 quad4 = new Vector2(sqCenX + s, SqCenY - s);//get each of the 4 points of each of the squares

                Debug.DrawLine(quad1, quad2, Color.white, 100f);
                Debug.DrawLine(quad2, quad3, Color.white, 100f);
                Debug.DrawLine(quad3, quad4, Color.white, 100f);
                Debug.DrawLine(quad4, quad1, Color.white, 100f);
            }
        }

    }
}
