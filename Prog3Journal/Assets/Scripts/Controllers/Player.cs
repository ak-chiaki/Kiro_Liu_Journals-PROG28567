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


    public float maxSpeed = 5.0f;
    public float accelerationTime = 5.0f;
    public float deTime = 2.0f;

    public float explosionRadius = 2.0f;      
    public float intensity = 10.0f;      
    public float velocityDecrease = 2.0f;
    public float explodeCD = 0.3f;
    private Vector3 knockbackVelocity;
    private float lastExplodeTime = 10f;

    private Vector3 velocity;


    void Update()
    {
        PlayerMovement();


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
            velocity += Vector3.down * acceleration * Time.deltaTime;

        if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow))
        {
            if (velocity.x > 0 && velocity.y > 0)
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

    private void CheckExplosion()
    {
        if (asteroidTransforms == null || asteroidTransforms.Count == 0) return; // check if it is not null

        if (Time.deltaTime - lastExplodeTime < explodeCD) return; //check the CD to avoid trigger many times in a same time

        Vector2 myPos = transform.position;

        for (int i = 0; i < asteroidTransforms.Count; i++)
        {
            Transform ast = asteroidTransforms[i];
            if (ast == null)
            {
                Vector2 astPos = ast.position;

                if ((astPos - myPos).sqrMagnitude <= explosionRadius) //check if the distance of player and ast smaller than the required rad
                {
                 
                    lastExplodeTime = Time.time;
                    break;
                }
            }

           
        }
    }



}