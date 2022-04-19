
using UnityEngine;
using System.Collections;

public class MoveJointTest : MonoBehaviour {
    public GameObject player;

    void Start(){

        Debug.Log("I am alive!");
        Rigidbody rb = GetComponent<Rigidbody>();

                // Start the enemy ten units behind the player character.
       // transform.position = player.transform.position - Vector3.forward * 3f;
        //transform.rotation = player.transform.rotation + Vector3.forward * 20f;

    }


    // Update is called once per frame
    void Update () {
        
        // Change the mass of the object's Rigidbody.
        // rb.mass = 10f;
        // Add a force to the Rigidbody.
        //rb.AddForce(Vector3.up * 10f);

    
    }
}