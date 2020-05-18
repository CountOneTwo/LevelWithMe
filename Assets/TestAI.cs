using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAI : MonoBehaviour
{
    public float movementSpeed;
    CharacterController character;
    void Start()
    {
        character = GetComponent<CharacterController>();
    }


    void Update()
    {
        //Looking at the player and moving towards him via the character controller
        transform.LookAt(GameObject.Find("Player").transform.position);
        Vector3 heading = GameObject.Find("Player").transform.position - transform.position;
        var distance = heading.magnitude;
        var direction = heading / distance;
        character.Move(direction * movementSpeed * Time.deltaTime);
    }
}
