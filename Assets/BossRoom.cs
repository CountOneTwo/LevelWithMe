using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : MonoBehaviour
{
    [Header("Doors")]
    public GameObject doors;
    public Vector3 doorPosition;
    public Vector3 originalDoorPositon;
    public float doorSpeed;

    [Header("Waves")]
    public int waves;

    [Header("General")]
    public Wave[] enemyWaves;

    [Header("Enemies")]
    public int minEnemies;
    public GameObject shotgunEnemy;
    public GameObject basicEnemy;
    int currentEnemies;
    public Vector3[] spawnPositions;
    int lastSpawn;

    public enum enemyType
    {
       Shotgun, Basic
    }

    bool activated;
    bool doorsClosed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!doorsClosed && activated)
        {
            CloseDoors();
        }

        if (activated)
        {
            CheckForMinEnemies();
        }
    }

    void CheckForMinEnemies()
    {
        if (currentEnemies < minEnemies)
        {
            SpawnEnemy((enemyType)Random.Range(0, 2));
        }
    }

    void SpawnEnemy(enemyType e)
    {
        int nextSpawn = lastSpawn;

        while (nextSpawn == lastSpawn)
        {
            nextSpawn = Random.Range(0, spawnPositions.Length);
        }
        

        if (e == enemyType.Shotgun)
        {
           // Instantiate(shotgunEnemy, spawnPositions[nextSpawn],shotgunEnemy.transform.LookAt(spawnPositions[nextSpawn]));
        }
        else
        {

        }
    }

    void CloseDoors()
    {
        if (Vector3.Distance(doorPosition, doors.transform.position) < 0.5)
        {
            doors.transform.position -= (-transform.up * doorSpeed * Time.deltaTime);
            doorsClosed = true;
        }
       
    }

    void OnTriggerEnter(Collider collision)
    {
        if (!activated)
        {
            if (collision.gameObject.name.Equals("Player"))
            {
                activated = true;
            }
        }

    }

}
