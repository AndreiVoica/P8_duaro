using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCallback : MonoBehaviour
{
    public delegate void CollisionAction(Collision collision);
    public static event CollisionAction OnCollision;

    void OnCollisionEnter(Collision collision)
    {
        if (OnCollision != null)
            OnCollision(collision);
    }
}

