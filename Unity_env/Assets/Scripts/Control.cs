using UnityEngine;
//using Unity.Robotics.ROSTCPConnector;
//using SensorUnity = RosMessageTypes.Sensor.JointStateMsg;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class Control : MonoBehaviour
{
    [SerializeField] private ArticulationBody[] robotJoints = new ArticulationBody[10];
    private Library robot;
    private ReadCSV degree;

    public string data_String;
    public string[] data_values;
    string userName;
    public float jointAngle;
    public float degJoint1L;
    public List<float> listOfFloats = new List<float>();
    void Start()
    {
        robot = FindObjectOfType<Library>();
        degree = FindObjectOfType<ReadCSV>();

        userName = System.Environment.GetEnvironmentVariable("USER");
        ReadCSVFile();
    }

    void Update()
    {
        //for(int i = 0; i < listOfFloats.Count; i++)
        //{
            int i =+ 1;
            robot.set_upper_joint_target(90, -45f, 0.09f, 0.0f);
            robot.set_lower_joint_target(listOfFloats[i], 45f, 0.09f, 0f);
        //}
    }

    public void ReadCSVFile()
    {
        using (var strReader = new StreamReader("/home/"+ userName + "/P8_duaro/Unity_env/bagfiles/test_1.csv"))
        {
            bool endOfFile = false;
            while(!endOfFile)
            {
                data_String = strReader.ReadLine ();
                if(data_String == null)
                {
                    endOfFile = true;
                    break;
                }
                var data_values = data_String.Split(',');
                float.TryParse(data_values[12], out float degJoint1L);
                listOfFloats.Add(degJoint1L);
            }
        }
    }
}