using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform enemyTransform;
    public GameObject bombPrefab;
    public List<Transform> asteroidTransforms;
    public Vector2 bombOffset = Vector2.zero;
    public float bombTrailSpacing = 2f;
    public int numberOfTrailBombs = 10;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.B)) // when press B, Bomb spawn at player's offset location
        {
            SpawnBombAtOffset(bombOffset);
        }

        if (Input.GetKeyDown(KeyCode.T))// when press T, Bombs spawn behind player in trail
        {
            SpawnBombTrail(bombTrailSpacing, numberOfTrailBombs);
        }
    }

    public void SpawnBombAtOffset(Vector3 inOffset)
    {
        Vector3 spawnPosition = transform.position + inOffset; //get the spawn location
        Instantiate(bombPrefab, spawnPosition, Quaternion.identity); //spawn the bomb

    }
    public void SpawnBombTrail(float inBombSpacing, int inNumberOfBombs)
    {
        Vector3 playerDirection = new Vector3(0, -1, 0);//get the direction of the trail of bomb

        for (int i=0; i<inNumberOfBombs; i++)
        {
            Vector3 spawnPositionTrail = transform.position + playerDirection * ( i*inBombSpacing); // bomb was spawn depends on the numbers and the space
            Instantiate( bombPrefab, spawnPositionTrail, Quaternion.identity);
        }

    }
}
