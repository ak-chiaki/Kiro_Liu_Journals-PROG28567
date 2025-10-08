using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flare : MonoBehaviour
{
    private Vector3 dir;
    private float speed;
    private float life;
    private float spawnTime;

    // Update is called once per frame
    void Update()
    {
        transform.position += dir * speed * Time.deltaTime;

    
    }
}
