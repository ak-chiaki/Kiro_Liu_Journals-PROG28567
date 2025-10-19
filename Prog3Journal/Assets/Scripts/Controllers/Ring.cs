using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    public bool marked = false;

    void Update()
    {
        if (!marked) return;
        RaderScan(4, 10);

    }

    private void RaderScan(float radius, int numberOfPoints)
    {
        float angleStep = 360f / numberOfPoints;
        float radians = angleStep * Mathf.Deg2Rad;
        Color raderColor = Color.green;

        List<Vector3> points = new List<Vector3>();
        for (int i = 0; i < numberOfPoints; i++)
        {
            float adjustment = radians * i;
            Vector3 point = new Vector3(Mathf.Cos(radians + adjustment) * radius, Mathf.Sin(radians + adjustment) * radius);
            points.Add(point);

        }

        Vector3 center = transform.position;
        for (int i = 0; i < points.Count - 1; i++)
        {
            Debug.DrawLine(center + points[i], center + points[i + 1], raderColor,20f);

        }

        Debug.DrawLine(center + points[points.Count - 1], center + points[0], raderColor,20f);

    }

    public void Mark() 
    { 
        marked = true;
    }
}
