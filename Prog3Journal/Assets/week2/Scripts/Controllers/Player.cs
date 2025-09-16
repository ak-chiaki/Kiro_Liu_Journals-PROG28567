using JetBrains.Annotations;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public float distance = 1f;
    public float movingratio = 0.5f;
    public Transform enemy;

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

        if(Input.GetKeyDown(KeyCode.S))//when press S, bomb randomly spawn at the 4 cornor of player
        {
            SpawnBombOnRandomCornor(distance);

        }

        if(Input.GetKeyDown(KeyCode.D))
        {
            WarpPlayer( enemy, movingratio);
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

    public void SpawnBombOnRandomCornor(float inDistance)//spawn bomb at 4 cornor
    {
        int directionBombIndex = Random.Range(0, 4); //use random to decide which cornor

        if (directionBombIndex == 0)
        {
            Vector3 directionBomb = new Vector3(1, 1, 0); //rightup
            directionBomb = directionBomb.normalized;//normalize the vector
            Vector3 randomSpawnBomb = transform.position + directionBomb * inDistance;//calculate the spawn position
            Instantiate(bombPrefab, randomSpawnBomb, Quaternion.identity);
        }

        if (directionBombIndex == 1)
        {
            Vector3 directionBomb = new Vector3(-1, 1, 0);//left up
            directionBomb = directionBomb.normalized;
            Vector3 randomSpawnBomb = transform.position + directionBomb * inDistance;
            Instantiate(bombPrefab, randomSpawnBomb, Quaternion.identity);
        }

        if (directionBombIndex == 2)
        {
            Vector3 directionBomb = new Vector3(-1, -1, 0);//left down
            directionBomb = directionBomb.normalized;
            Vector3 randomSpawnBomb = transform.position + directionBomb * inDistance;
            Instantiate(bombPrefab, randomSpawnBomb, Quaternion.identity);
        }

        if (directionBombIndex == 3)
        {
            Vector3 directionBomb = new Vector3(1, -1, 0);//right down
            directionBomb = directionBomb.normalized;
            Vector3 randomSpawnBomb = transform.position + directionBomb * inDistance;
            Instantiate(bombPrefab, randomSpawnBomb, Quaternion.identity);
        }


    }
    public void WarpPlayer(Transform target, float ratio)
    {
        Vector3 newpos = Vector3.zero; 

        if (ratio <= 0) 
        {
            newpos = transform.position;
        }

        else if(ratio >=1)
        {
            newpos = target.position;
        }
        else
        {
            newpos = Vector3.Lerp(transform.position, target.position, ratio);

        }

        transform.position = newpos;    

    }

    public void DetectAsteroids(float inMaxRange, List<Transform> inAsteroids)
    {



    }
}
