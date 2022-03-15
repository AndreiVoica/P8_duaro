using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using SensorUnity = RosMessageTypes.Sensor.JointStateMsg;
using System.Collections;

public class JointStateSub : MonoBehaviour
{
    [SerializeField] private string rosTopic = "joint_states";
    [SerializeField] private ROSConnection ROS;
    [SerializeField] private ArticulationBody[] robotJoints = new ArticulationBody[10];
    // Start is called before the first frame update
    void Start()
    {
        ROS = ROSConnection.GetOrCreateInstance();
        ROS.Subscribe<SensorUnity>(rosTopic, GetJointPositions);
    }
 
    private void GetJointPositions(SensorUnity sensorMsg)
    {
        StartCoroutine(SetJointValues(sensorMsg));
    }
    IEnumerator SetJointValues(SensorUnity message)
    {
        for (int i = 0; i < message.name.Length; i++)
        {
            if (message.name[i].Contains("lower_joint1"))
            {
                var joint1LoXDrive = robotJoints[0].xDrive;
                joint1LoXDrive.target = ((float)(message.position[i]) * Mathf.Rad2Deg);
                robotJoints[0].xDrive = joint1LoXDrive;
            }
            else if (message.name[i].Contains("lower_joint2"))
            {
                var joint2LoXDrive = robotJoints[1].xDrive;
                joint2LoXDrive.target = ((float)(message.position[i]) * Mathf.Rad2Deg);
                robotJoints[1].xDrive = joint2LoXDrive;
            }
            else if (message.name[i].Contains("lower_joint3"))
            {
                var joint3LoXDrive = robotJoints[2].xDrive;
                joint3LoXDrive.target = ((float)(message.position[i]));
                robotJoints[2].xDrive = joint3LoXDrive;
                var joint3_5LoXDrive = robotJoints[3].xDrive;
                joint3_5LoXDrive.target = ((float)(message.position[i]));
                robotJoints[3].xDrive = joint3_5LoXDrive;
            }
            else if (message.name[i].Contains("lower_joint4"))
            {
                var joint4LoXDrive = robotJoints[4].xDrive;
                joint4LoXDrive.target = ((float)(message.position[i]) * Mathf.Rad2Deg);
                robotJoints[4].xDrive = joint4LoXDrive;
            }
            else if (message.name[i].Contains("upper_joint1"))
            {
                var joint1UpXDrive = robotJoints[5].xDrive;
                joint1UpXDrive.target = ((float)(message.position[i]) * Mathf.Rad2Deg);
                robotJoints[5].xDrive = joint1UpXDrive;
            }
            else if (message.name[i].Contains("upper_joint2"))
            {
                var joint2UpXDrive = robotJoints[6].xDrive;
                joint2UpXDrive.target = ((float)(message.position[i]) * Mathf.Rad2Deg);
                robotJoints[6].xDrive = joint2UpXDrive;
            }
            else if (message.name[i].Contains("upper_joint3"))
            {
                var joint3UpXDrive = robotJoints[7].xDrive;
                joint3UpXDrive.target = ((float)(message.position[i]));
                robotJoints[7].xDrive = joint3UpXDrive;
                var joint3_5UpXDrive = robotJoints[8].xDrive;
                joint3_5UpXDrive.target = ((float)(message.position[i]));
                robotJoints[8].xDrive = joint3_5UpXDrive;
            }
            else if (message.name[i].Contains("upper_joint4"))
            {
                var joint4UpXDrive = robotJoints[9].xDrive;
                joint4UpXDrive.target = ((float)(message.position[i]) * Mathf.Rad2Deg);
                robotJoints[9].xDrive = joint4UpXDrive;
            }
        }
        
        /*
        for (int i = 0; i < message.name.Length; i++)
        {
            var joint1XDrive = robotJoints[i].xDrive;
            joint1XDrive.target = (float)(message.position[i]) * Mathf.Rad2Deg;
            robotJoints[i].xDrive = joint1XDrive;
            //Debug.Log(joint1XDrive.target);
        }
        */
 
        yield return new WaitForSeconds(1f);
    }
 
    public void UnSub()
    {
        ROS.Unsubscribe(rosTopic);
    }
}
