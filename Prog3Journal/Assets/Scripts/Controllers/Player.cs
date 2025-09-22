using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<Transform> asteroidTransforms;
    public Transform enemyTransform;
    public GameObject bombPrefab;
    public Transform bombsTransform;

    [Header("motion properties")]
    public float moveSpeed = 0.5f;
    public float maxSpeed = 1.0f;
    public float accelerationTime = 5.0f;

    private Vector3 velocity = Vector3.down;

    void Update()
    {
        PlayerMovement();


    }

    public void PlayerMovement()
    {

        float acceleration = maxSpeed / accelerationTime;

        if (Input.GetKey(KeyCode.LeftArrow))
            transform.position += acceleration * Time.deltaTime * Vector3.left;
        if (Input.GetKey(KeyCode.RightArrow))
            transform.position += acceleration * Time.deltaTime * Vector3.right;
        if (Input.GetKey(KeyCode.UpArrow))
            transform.position += acceleration * Time.deltaTime * Vector3.up;
        if (Input.GetKey(KeyCode.DownArrow))
            transform.position += acceleration * Time.deltaTime * Vector3.down;

        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        transform.position += velocity * Time.deltaTime;
    }

}