using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class FlareControl : MonoBehaviour
{

    private Vector2 velocity;
    public float hitRadius = 0.3f;    
    public float outdis = 0.1f;     
    public float flareSpeed = 3f;
    private float spawnTime;

    private List<Transform> asteroids;

    public void Init(Vector2 v, List<Transform> asteroidList)
    {
        velocity = v;
        spawnTime = Time.time;
        asteroids = asteroidList;

        float deg = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, deg);
    }

    void Update()
    {
      
        transform.position += (Vector3)(velocity * Time.deltaTime * flareSpeed);

        Camera cam = Camera.main;
        if (cam != null)
        {
            Vector3 viewpoint = cam.WorldToViewportPoint(transform.position);
            if (viewpoint.x < -outdis || viewpoint.x > 1f + outdis ||
                viewpoint.y < -outdis || viewpoint.y > 1f + outdis)
            {
                Destroy(gameObject);
                return;
            }
        }
        if (asteroids == null || asteroids.Count == 0) return;

        float r2 = hitRadius * hitRadius;
        Vector2 flarePos = transform.position;

        for (int i = 0; i < asteroids.Count; i++)
        {
            Transform ast = asteroids[i];

            float dist2 = ((Vector2)ast.position - flarePos).sqrMagnitude;
            if (dist2 <= r2)
            {

                Ring marker = ast.GetComponent<Ring>();
                if (marker != null) marker.Mark();
                Destroy(gameObject);
                break;
             
            }
        }




    } 
}
