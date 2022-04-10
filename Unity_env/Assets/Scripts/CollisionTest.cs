using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTest : MonoBehaviour
{


void OnCollisionEnter(Collision other){

 
Debug.Log("Do something");
Destroy(gameObject);

}
    
}


