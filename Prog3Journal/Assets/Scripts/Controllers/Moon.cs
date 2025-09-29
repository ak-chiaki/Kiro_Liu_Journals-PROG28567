using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Moon : MonoBehaviour
{
    public Transform planetTransform;
    private float angleDeg;
    public float radius = 1f;
    public float speedDegPerSec = 10f; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        OrbitalMotion(radius, speedDegPerSec, planetTransform);
    }

    public void OrbitalMotion(float radius, float speed, Transform target)
    {

        angleDeg += speed * Time.deltaTime;
        float rad = angleDeg * Mathf.Deg2Rad;
        Vector3 center = target.position;
        Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f) * radius;
        Vector3 pos = center + offset;
        transform.position = pos;

    }
}
