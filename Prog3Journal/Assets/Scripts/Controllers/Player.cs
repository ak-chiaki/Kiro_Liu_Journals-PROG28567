using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;
public class Player : MonoBehaviour
{
    public List<Transform> asteroidTransforms;

    public Transform enemyTransform;
    public GameObject bombPrefab;
    public Transform bombsTransform;


    public float maxSpeed = 5.0f;
    public float accelerationTime = 5.0f;
    public float deTime = 2.0f;

    public float explosionRadius = 1.0f;      
    public float intensity = 10.0f;      
    public float velocityDecrease = 2.0f;
    public float explodeCD = 5f;
    private Vector3 knockbackVelocity;
    private float lastExplodeTime = 0f;

    private Vector3 velocity;
    public float beta = 3.0f;

    [Header("Rotate")]
    public float angleSpeedDeg = 180f; 

    [Header("Flare")]
    public FlareControl flarePrefab;     

    public float flareSpeed = 5f; 
   


    void Update()
    {
        PlayerMovement();
        CheckExplosion();
        PlayerRotation();

        transform.position += (velocity + knockbackVelocity) * Time.deltaTime;

        if (knockbackVelocity.sqrMagnitude > 0f) 
        {
    
            knockbackVelocity = Vector3.Lerp(knockbackVelocity, Vector3.zero, beta * Time.deltaTime);
        }
        else
        {
            knockbackVelocity = Vector3.zero;
        }


    }

    public void PlayerMovement()
    {
        float acceleration = maxSpeed / accelerationTime;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            velocity += Vector3.left * acceleration * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            velocity += Vector3.right * acceleration * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            velocity += Vector3.up * acceleration * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        { 
        velocity += Vector3.down * acceleration * Time.deltaTime;
        }

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


        Debug.Log(velocity);

    }

    private void CheckExplosion()
    {
        if (asteroidTransforms == null || asteroidTransforms.Count == 0) return; // check if it is not null

        if (Time.time - lastExplodeTime < explodeCD) return; //check the CD to avoid trigger many times in a same time

        Vector2 myPos = transform.position;

        for (int i = 0; i < asteroidTransforms.Count; i++)
        {
            Transform ast = asteroidTransforms[i];
            if (ast != null)
            {
                Vector2 astPos = ast.position;

                if ((astPos - myPos).magnitude <= explosionRadius) //check if the distance of player and ast smaller than the required rad
                {
                    
                    lastExplodeTime = Time.time;

                    Boom(astPos);
                    DestroyAsteroid(ast);

                    break;
                }
            }

           
        }
    }

    private void Boom(Vector2 explosionCenter)
    {
        int boomIndex = UnityEngine.Random.Range(1, 5);
        Vector3 dir = Vector3.zero;

        if (boomIndex == 1) dir = Vector3.left;
        if (boomIndex == 2) dir = Vector3.right;
        if (boomIndex == 3) dir = Vector3.up;
        if (boomIndex == 4) dir = Vector3.down;

       
        knockbackVelocity += dir.normalized * intensity;


    }
    private void DestroyAsteroid(Transform t)
    {
        if (t == null) return;

        asteroidTransforms.Remove(t);

        Destroy(t.gameObject);
    }

    private void PlayerRotation()
    {
        float z = 0f;
        if (Input.GetKey(KeyCode.A)) 
            z += 1f;
        if (Input.GetKey(KeyCode.D))
            z -= 1f;
        transform.Rotate(0, 0, z * angleSpeedDeg * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryShoot();
        }
    }

    private void TryShoot()
    {

        Vector2 dir = (Vector2)transform.up.normalized; 
        FlareControl flare = Instantiate(flarePrefab, transform.position, Quaternion.identity);
        flare.Init(dir, asteroidTransforms);
    }



   

}

