using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossRoom : MonoBehaviour
{
    [Header("Doors")]
    public GameObject doors;
    public Vector3 doorPosition;
    public Vector3 originalDoorPosition;
    public float doorSpeed;

    [Header("General")]
    public Wave[] enemyWaves;
    public float completeDuration;
    public Text countdownText;
    [HideInInspector]
    public float durationTimer;
    int nextWave;

    [Header("Enemies")]
    public int minEnemies;
    public GameObject shotgunEnemy;
    public GameObject basicEnemy;
    [HideInInspector]
     public int currentEnemies;
    public GameObject[] spawnPositions;
    int lastSpawn;

    public enum enemyType
    {
       Shotgun, Basic
    }

    bool activated;
    bool doorsClosed = false;
    bool completed;

    public musicManagerScript musicmanagerscript;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!completed)
        {
            if (!doorsClosed && activated)
            {
                CloseDoors();
            }

            if (activated)
            {
                durationTimer += Time.deltaTime;
                CheckForMinEnemies();
                countdownText.text = "Survive for " + (int)(completeDuration - durationTimer) + "s";
                CheckForWin();
                CheckForWaves();
            }           
        }

        if(currentEnemies > 0)
        {
            musicmanagerscript.NrOfDetections = 99;
        }
        else musicmanagerscript.NrOfDetections = 0;

    }

    public void Reset()
    {
        countdownText.enabled = false;
        durationTimer = 0;
        nextWave = 0;
        activated = false;
        OpenDoors();
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    void CheckForWin()
    {
        if (durationTimer > completeDuration)
        {
            completed = true;
            OpenDoors();
        }
    }

    void CheckForWaves()
    {
        if (nextWave < enemyWaves.Length)
        {
            if (durationTimer > enemyWaves[nextWave].time)
            {
                for (int i = 0; i < enemyWaves[nextWave].shotgunEnemies; i++)
                {
                    //print(1);
                    SpawnEnemy(enemyType.Shotgun);
                }
                for (int i = 0; i < enemyWaves[nextWave].basicEnemies; i++)
                {
                    SpawnEnemy(enemyType.Basic);
                }



                nextWave++;
            }
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
            Instantiate(shotgunEnemy, spawnPositions[nextSpawn].transform.position, transform.rotation, transform);
           // Instantiate(shotgunEnemy, spawnPositions[nextSpawn],shotgunEnemy.transform.LookAt(spawnPositions[nextSpawn]));
        }
        else
        {
            Instantiate(basicEnemy, spawnPositions[nextSpawn].transform.position, transform.rotation, transform);
        }

        currentEnemies++;
    }

    void CloseDoors()
    {
        doors.transform.localPosition = Vector3.MoveTowards(doors.transform.localPosition, doorPosition, doorSpeed * Time.deltaTime);
        if (Vector3.Distance(doorPosition, doors.transform.localPosition) < 0.1)
        {
            
            doorsClosed = true;
        }
       
    }



    IEnumerator OpenDoorsEnumerator()
    {
        while (Vector3.Distance(originalDoorPosition, doors.transform.localPosition) > 0.1)
        {
            doors.transform.localPosition = Vector3.MoveTowards(doors.transform.localPosition, originalDoorPosition, doorSpeed * Time.deltaTime);
            yield return null;
        }
        doorsClosed = false;
    }


    void OpenDoors()
    {

        StartCoroutine("OpenDoorsEnumerator");

    }

    void OnTriggerEnter(Collider collision)
    {
        if (!activated)
        {
            if (collision.gameObject.name.Equals("Player"))
            {
                activated = true;
                countdownText.enabled = true;
            }
        }

    }

}
