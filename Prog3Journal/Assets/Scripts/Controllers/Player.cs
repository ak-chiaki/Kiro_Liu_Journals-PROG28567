using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public List<Transform> asteroidTransforms;
    public Transform enemyTransform;
    public GameObject bombPrefab;
    public Transform bombsTransform;

    public float maxSpeed =5.0f;
    public float accelerationTime = 5.0f;
    public float deTime = 2.0f;

    private Vector3 velocity;

    [Header("Radar Properties")]
    public float radarRadius = 1f;
    public int numberOfPoints = 1;


    void Update()
    {
        PlayerMovement();
        RaderScan(radarRadius, numberOfPoints);

    }

    public void PlayerMovement()
    {
        float acceleration = maxSpeed / accelerationTime;

        if (Input.GetKey(KeyCode.LeftArrow))
            velocity += Vector3.left * acceleration * Time.deltaTime;
        if (Input.GetKey(KeyCode.RightArrow))
            velocity += Vector3.right * acceleration * Time.deltaTime;
        if (Input.GetKey(KeyCode.UpArrow))
            velocity += Vector3.up * acceleration * Time.deltaTime;
        if (Input.GetKey(KeyCode.DownArrow))
            velocity += Vector3.down * acceleration* Time.deltaTime;

        if(!Input.GetKey(KeyCode.LeftArrow)&& !Input.GetKey(KeyCode.RightArrow)&& !Input.GetKey(KeyCode.UpArrow)&& !Input.GetKey(KeyCode.DownArrow))
        {
            if (velocity.x>0 && velocity.y > 0)
            {
                velocity.x -= deTime * Time.deltaTime;
                velocity.y -= deTime * Time.deltaTime;
            }
            if (velocity.x < 0 && velocity.y > 0)
            {
                velocity.x += deTime * Time.deltaTime;
                velocity.y -= deTime * Time.deltaTime;
            }
            if (velocity.x > 0 && velocity.y < 0)
            {
                velocity.x -= deTime * Time.deltaTime;
                velocity.y += deTime * Time.deltaTime;
            }
            if (velocity.x < 0 && velocity.y < 0)
            {
                velocity.x += deTime * Time.deltaTime;
                velocity.y += deTime * Time.deltaTime;
            }
        }


        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        transform.position += velocity * Time.deltaTime;

        Debug.Log(velocity);

    }

    private void RaderScan(float radius, int numberOfPoints)
    {
        float angleStep = 360f / numberOfPoints;
        float radians = angleStep * Mathf.Deg2Rad;

        List<Vector3> points = new List<Vector3>();
        for (int i = 0; i < numberOfPoints; i++)
        {
            float adjustment = radians * i;
            Vector3 point = new Vector3(Mathf.Cos(radians + adjustment), Mathf.Sin(radians + adjustment));
            points.Add(point);

        }

        Vector3 center = transform.position;
        for (int i = 0; i < points.Count - 1; i++)
        {
            Debug.DrawLine(center + points[i], center + points[i + 1], Color.green);

        }
        Debug.DrawLine(center + points[points.Count -1] ,center + points[0], Color.green);




    }
}