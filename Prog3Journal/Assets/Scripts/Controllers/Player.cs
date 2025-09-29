using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI.Table;

public class Player : MonoBehaviour
{
    public List<Transform> asteroidTransforms;
    public Transform enemyTransform;
    public GameObject bombPrefab;
    public Transform bombsTransform;

    public float maxSpeed =5.0f;
    public float accelerationTime = 5.0f;
    public float deTime = 2.0f;

    public GameObject powerupPrefab;
    public float powerUpRadius = 1;
    public int powerUpNumbers = 1;

    private Vector3 velocity;

    [Header("Radar Properties")]
    public float radarRadius = 1f;
    public int numberOfPoints = 1;


    void Update()
    {
        PlayerMovement();
        RaderScan(radarRadius, numberOfPoints);
        SpawnPowerups(powerUpRadius,powerUpNumbers);

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
        Color raderColor = Color.green;

        List<Vector3> points = new List<Vector3>();
        for (int i = 0; i < numberOfPoints; i++)
        {
            float adjustment = radians * i;
            Vector3 point = new Vector3(Mathf.Cos(radians + adjustment)* radius, Mathf.Sin(radians + adjustment)* radius);
            points.Add(point);

        }


        float dist = Vector3.Distance(transform.position, enemyTransform.position);

        if (dist <= radius)
        {
            raderColor = Color.red;
        }


        Vector3 center = transform.position;
        for (int i = 0; i < points.Count - 1; i++)
        {
            Debug.DrawLine(center + points[i], center + points[i + 1], raderColor);

        }


        Debug.DrawLine(center + points[points.Count - 1], center + points[0], raderColor);

        if (dist == radius)
        {
           
            Debug.Log("match");
           
        }
        if (enemyTransform == null)
        {

            Debug.Log("enemy not assgined");

        }



    }

    public void SpawnPowerups(float radius, int numberOfPowerups)
    {

        Vector3 center = transform.position;
        float angleStep = 360f / numberOfPowerups;

        for (int i = 0; i < numberOfPowerups; i++)
        {
            float angleDeg = i * angleStep;
            float angleRad = angleDeg * Mathf.Deg2Rad;

            Vector3 change = new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad), 0f) * radius;
            Vector3 spawnPos = center + change;

            Instantiate(powerupPrefab, spawnPos, Quaternion.identity);
        }
        if (powerupPrefab == null)
        {
            Debug.Log("powerup prefab not assgined");
        }
        if (numberOfPowerups <= 0)
        {
            Debug.Log(" numberOfPowerups should > 0");
        }
    }
}