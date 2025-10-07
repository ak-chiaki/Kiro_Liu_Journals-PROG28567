using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public float angularSpeed = 60f;
    public Transform target;


    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, angularSpeed * Time.deltaTime);
        Debug.DrawLine(transform.position, transform.position + transform.up, Color.green);


        Vector2 directionToTarget = (target.position - transform.forward).normalized;
        float dot = Vector3.Dot(transform.up, directionToTarget);
        if (dot < 0) Debug.Log($"<color=red><size=16>Behind!</size></color>");
        if (dot > 0) Debug.Log($"<color=red><size=16>In Front!</size></color>");
    }
}
