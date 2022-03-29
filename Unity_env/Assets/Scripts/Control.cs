using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using SensorUnity = RosMessageTypes.Sensor.JointStateMsg;
using System.Collections;

public class Control : MonoBehaviour
{
    [SerializeField] private ArticulationBody[] robotJoints = new ArticulationBody[10];
    // Start is called before the first frame update
    void Start()
    {
        
    }
 
    /*
    LEGEND: robotJoints[i]:
    i = 0 -> lower_link_1;
    i = 1 -> lower_link_2;
    i = 2 -> lower_link_3;
    i = 3 -> lower_link_3_obst;
    i = 4 -> lower_link_4;
    i = 5 -> upper_link_1;
    i = 6 -> upper_link_2;
    i = 7 -> upper_link_3;
    i = 8 -> upper_link_3_obst;
    i = 9 -> upper_link_4;
    */
    void Update()
    {
        var joint1UpXDrive = robotJoints[5].xDrive;
        joint1UpXDrive.target = -50f;
        robotJoints[5].xDrive = joint1UpXDrive;
    }
}