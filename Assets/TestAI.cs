using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAI : MonoBehaviour
{
    public float movementSpeed;
   // Rigidbody rigidbody;
    CharacterController character;
    // Start is called before the first frame update
    void Start()
    {
        //rigidbody = GetComponent<Rigidbody>();
        character = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(GameObject.Find("Player").transform.position);
        Vector3 heading = GameObject.Find("Player").transform.position - transform.position;
        var distance = heading.magnitude;
        var direction = heading / distance;
        character.Move(direction * movementSpeed * Time.deltaTime);
    }
}
