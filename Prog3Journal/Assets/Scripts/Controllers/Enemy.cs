using UnityEngine;
using System.Collections;
using System.ComponentModel;

public class Enemy : MonoBehaviour
{
    public Transform playerTrans;
    public float maxSpeed = 5f;
    public float slowRad = 10f;
    public float stopRad = 1.0f;
    private Vector3 velocity;
    private void Update()
    {

        EnemyMovement();
    }

    public void EnemyMovement()
    {
        Vector3 playerdis = playerTrans.position - transform.position;
        float distance = playerdis.magnitude;// calculate the distance

        if ( distance > stopRad )
        {
            if (playerdis.x>0)
            {
                velocity += Vector3.right * Time.deltaTime;
            }
            if (playerdis.y > 0)
            {
                velocity += Vector3.up * Time.deltaTime;
            }
            if (playerdis.x < 0)
            {
                velocity += Vector3.left * Time.deltaTime;
            }
            if (playerdis.y < 0)
            {
                velocity += Vector3.down * Time.deltaTime;
            }

           


        }
        if(distance < slowRad)
            {

            if (playerdis.x > 0)
            {
                velocity -= Vector3.right * Time.deltaTime;
            }
            if (playerdis.y > 0)
            {
                velocity -= Vector3.up * Time.deltaTime;
            }
            if (playerdis.x < 0)
            {
                velocity -= Vector3.left * Time.deltaTime;
            }
            if (playerdis.y < 0)
            {
                velocity -= Vector3.down * Time.deltaTime;
            }
        }
        transform.position += velocity * Time.deltaTime;


    }

}
