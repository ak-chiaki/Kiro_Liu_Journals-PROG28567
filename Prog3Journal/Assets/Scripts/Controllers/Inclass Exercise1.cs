using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InclassExercise1 : MonoBehaviour
{

    public List<float> angles = new List<float>();
   
    public float radius = 1f;
    public Vector3 center = Vector3.zero;

    [Space(10)]
    public float lineDuration = 1f;
    private float elapsedTime = 0f;
    private int currentIndex = 0;


    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            angles.Add(Random.value * 360f);

        }
    }

   
    void Update()
    {
        DrawCircleLine();
    }

    public void DrawCircleLine()
    {

        elapsedTime += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) || elapsedTime > lineDuration)
        {
            elapsedTime = 0f;
            currentIndex = (currentIndex + 1) % angles.Count;
        }

        Vector3 point = GetPointOnUnitCircle(angles[currentIndex]);
        Debug.DrawLine(center, center + point, Color.green);

    }
    private Vector2 GetPointOnUnitCircle(float angle)
    {
        float radians = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(radians), Mathf.Sin(radians))* radius ;

    }
}
