using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotProductExercise : MonoBehaviour
{
    public float redAngle = 60f;
    public float blueAngle = 30f;
    private Vector2 redVector = Vector2.zero;
    private Vector2 blueVector = Vector2.zero;


  

    // Update is called once per frame
    void Update()
    {
        redVector = CalculateVector2FromAngle(redAngle);
        blueVector = CalculateVector2FromAngle(blueAngle);

        Debug.DrawLine(Vector3.zero, redVector, Color.red);
        Debug.DrawLine(Vector3.zero, blueVector, Color.blue);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            float dot = calculateDotProduct(redVector, blueVector);
            Debug.Log($"<color=orange><size=16>{dot}</size></color>");

        }
    }

    private Vector2 CalculateVector2FromAngle(float angle)
    {
        float angleInRads = Mathf.Deg2Rad * angle;
        return new Vector2(Mathf.Cos(angleInRads), Mathf.Sin(angleInRads));
    }

    private float calculateDotProduct(Vector2 a, Vector2 b)
    {
        return a.x * b.x + a.y * b.y;
    }

}
