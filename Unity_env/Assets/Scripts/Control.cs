using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using SensorUnity = RosMessageTypes.Sensor.JointStateMsg;
using System.Collections;

public class Control : MonoBehaviour
{
    [SerializeField] private ArticulationBody[] robotJoints = new ArticulationBody[10];
    private Library robot;
    void Start()
    {
        robot = FindObjectOfType<Library>();
    }

    void Update()
    {
        robot.set_upper_joint_target(45f, -45f, 0.09f, 0.0f);
        robot.set_lower_joint_target(-45f, 45f, 0.09f, 0f);
    }
}