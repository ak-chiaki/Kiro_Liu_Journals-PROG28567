using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flare : MonoBehaviour
{
    private Vector3 dir;
    private float speed;


    // Update is called once per frame
    void Update()
    {
        transform.position += dir * speed * Time.deltaTime;

    }
}
