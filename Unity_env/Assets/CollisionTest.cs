using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTest : MonoBehaviour
{
    void OnEnable()
    {
        //Register to OnTriggered event
        CollisionCallback.OnCollision += CollisionDetected;
    }

    void OnDisable()
    {
        //Un-Register to OnTriggered event
        CollisionCallback.OnCollision -= CollisionDetected;
    }


    //This is invoked when trigger happens
    void CollisionDetected(Collision collision)
    {
        //Debug.Log("Collision happened with: " + collision.gameObject.name);
    }
}
