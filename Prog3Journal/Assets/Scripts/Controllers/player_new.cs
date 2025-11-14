using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_new : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rigidbody;
    private Vector2 input;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float x = 0f;
        float y = 0f;

        if (Input.GetKey(KeyCode.LeftArrow))
            x = -1f;
        if (Input.GetKey(KeyCode.RightArrow))
            x = 1f;
        if (Input.GetKey(KeyCode.UpArrow))
            y = 1f;
        if (Input.GetKey(KeyCode.DownArrow))
            y = -1f;

        input = new Vector2(x, y).normalized;
    }

    void FixedUpdate()
    {
        rigidbody.velocity = input * moveSpeed;
    }


}
