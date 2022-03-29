using System;
using Unity.Robotics;
using UnityEngine;

namespace Unity.Robotics.UrdfImporter.Control
{
    public enum RotationDirection { None = 0, Positive = 1, Negative = -1 };
    public enum ControlType { PositionControl };

    public class Control : MonoBehaviour
    {
        private Vector3 rotation;
        [SerializeField] private float _speed;

        void Update()
        {
            if(Input.GetKeyDown(KeyCode.A)) rotation = Vector3.up;
            else if (Input.GetKeyDown(KeyCode.D)) rotation = Vector3.down;
            else rotation = Vector3.zero;

            transform.Rotate(rotation * _speed * Time.deltaTime);
        }
    }
}