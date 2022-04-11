using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using SensorUnity = RosMessageTypes.Sensor.JointStateMsg;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class JointStateSub : MonoBehaviour
{
    [SerializeField] private string rosTopic = "joint_states";
    [SerializeField] private ROSConnection ROS;
    [SerializeField] private ArticulationBody[] robotJoints = new ArticulationBody[14];
    private List<JointAngles> jointAngles = new List<JointAngles>();
    private bool recording = false;
    // Start is called before the first frame update
    void Start()
    {
        ROS = ROSConnection.GetOrCreateInstance();
        ROS.Subscribe<SensorUnity>(rosTopic, GetJointPositions);
    }

    void StartRecordingIfNotAlready()
    {
        if (!recording)
        {
            recording = true;
            jointAngles.Clear();
            print("Started recording");
        }
    }

    void StopRecordingIfAlready()
    {
        if (recording)
        {
            recording = false;
            print("Stopped recording");

            string fileName = "recording_" + System.DateTime.Now.ToString("yy-MM-dd-HH-mm-ss") + ".csv";
            var path = Directory.GetCurrentDirectory();
            var filePath = Path.Combine(path, $"bagfiles/{fileName}");

            using (var writer = new StreamWriter(filePath))
            {
                writer.WriteLine("Joint1L,Joint2L,Joint3L,Joint4L,Joint1U,Joint2U,Joint3U,Joint4U");
                foreach (var joints in jointAngles)
                {
                    writer.WriteLine(joints.ToString());
                }
            }
            print($"Saved to {filePath}");
        }
    }

    void Update()
    {
        if (Input.GetKey("r"))
        {
            StartRecordingIfNotAlready();
        }
        else if (Input.GetKey("s"))
        {
            StopRecordingIfAlready();
        }
    }
 
    private void GetJointPositions(SensorUnity sensorMsg)
    {
        StartCoroutine(SetJointValues(sensorMsg));
    }

    IEnumerator SetJointValues(SensorUnity message)
    {
        JointAngles joints = new JointAngles();
        bool updated = false;
        for (int i = 0; i < message.name.Length; i++)
        {
            if (message.name[i].Equals("duarolower_joint1"))
            {
                var joint = robotJoints[0].xDrive;
                joint.target = ((float)(message.position[i]) * Mathf.Rad2Deg);
                robotJoints[0].xDrive = joint;
                joints.Joint1L = joint.target;
                updated = true;
            }
            else if (message.name[i].Equals("duarolower_joint2"))
            {
                var joint = robotJoints[1].xDrive;
                joint.target = ((float)(message.position[i]) * Mathf.Rad2Deg);
                robotJoints[1].xDrive = joint;
                joints.Joint2L = joint.target;
                updated = true;
            }
            else if (message.name[i].Equals("duarolower_joint3"))
            {
                var joint = robotJoints[2].xDrive;
                joint.target = ((float)(message.position[i]));
                robotJoints[2].xDrive = joint;
                var joint2 = robotJoints[3].xDrive;
                joint2.target = joint.target;
                robotJoints[3].xDrive = joint2;
                joints.Joint3L = joint.target;
                updated = true;
            }
            else if (message.name[i].Equals("duarolower_joint4"))
            {
                var joint = robotJoints[4].xDrive;
                joint.target = ((float)(message.position[i]) * Mathf.Rad2Deg);
                robotJoints[4].xDrive = joint;
                joints.Joint4L = joint.target;
                updated = true;
            }
            else if (message.name[i].Equals("lower_gripper_finger_left_joint"))
            {
                var joint = robotJoints[10].xDrive;
                joint.target = ((float)(message.position[i]) * Mathf.Rad2Deg);
                robotJoints[5].xDrive = joint;
                joints.Llgripper = joint.target;
                updated = true;
            }
            else if (message.name[i].Equals("lower_gripper_finger_right_joint"))
            {
                var joint = robotJoints[11].xDrive;
                joint.target = ((float)(message.position[i]) * Mathf.Rad2Deg);
                robotJoints[6].xDrive = joint;
                joints.Lrgripper = joint.target;
                updated = true;
            }
            else if (message.name[i].Equals("duaroupper_joint1"))
            {
                var joint = robotJoints[5].xDrive;
                joint.target = ((float)(message.position[i]));
                robotJoints[5].xDrive = joint;
                joints.Joint1U = joint.target;
                updated = true;
            }
            else if (message.name[i].Equals("duaroupper_joint2"))
            {
                var joint = robotJoints[6].xDrive;
                joint.target = ((float)(message.position[i]) * Mathf.Rad2Deg);
                robotJoints[6].xDrive = joint;
                joints.Joint2U = joint.target;
                updated = true;
            }
            else if (message.name[i].Equals("duaroupper_joint3"))
            {
                var joint = robotJoints[7].xDrive;
                joint.target = ((float)(message.position[i]) * Mathf.Rad2Deg);
                robotJoints[7].xDrive = joint;
                var joint2 = robotJoints[8].xDrive;
                joint2.target = joint.target;
                robotJoints[8].xDrive = joint2;
                joints.Joint3U = joint.target;
                updated = true;
            }
            else if (message.name[i].Equals("duaroupper_joint4"))
            {
                var joint = robotJoints[9].xDrive;
                joint.target = ((float)(message.position[i]) * Mathf.Rad2Deg);
                robotJoints[9].xDrive = joint;
                joints.Joint4U = joint.target;
                updated = true;
            }
            else if (message.name[i].Equals("upper_gripper_finger_left_joint"))
            {
                var joint = robotJoints[12].xDrive;
                joint.target = ((float)(message.position[i]) * Mathf.Rad2Deg);
                robotJoints[12].xDrive = joint;
                joints.Ulgripper = joint.target;
                updated = true;
            }
            else if (message.name[i].Equals("upper_gripper_finger_right_joint"))
            {
                var joint = robotJoints[13].xDrive;
                joint.target = ((float)(message.position[i]) * Mathf.Rad2Deg);
                robotJoints[13].xDrive = joint;
                joints.Urgripper = joint.target;
                updated = true;
            }
        }

        if (updated && recording)
        {
            jointAngles.Add(joints);
        }

        yield return new WaitForSeconds(1f);
    }
 
    public void UnSub()
    {
        ROS.Unsubscribe(rosTopic);
    }
}
