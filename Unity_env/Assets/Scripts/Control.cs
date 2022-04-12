using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class Control : MonoBehaviour
{
    private Library robot;

    private List<JointAngles> jointAngles = new List<JointAngles>();
    private int currentIndex = 0;

    void Start()
    {
        robot = FindObjectOfType<Library>();
        ReadCSVFile();

        var rate = 10f;
        var waitTime = 1f / rate;

        InvokeRepeating("MoveJoints", 0, waitTime);
    }

    void Update()
    {
        // do nothing
    }

    void MoveJoints()
    {
        robot.set_lower_joint_target(jointAngles[currentIndex].Joint1L, jointAngles[currentIndex].Joint2L, jointAngles[currentIndex].Joint3L, jointAngles[currentIndex].Joint4L, jointAngles[currentIndex].Lrgripper, jointAngles[currentIndex].Llgripper);
        robot.set_upper_joint_target(jointAngles[currentIndex].Joint1U, jointAngles[currentIndex].Joint2U, jointAngles[currentIndex].Joint3U, jointAngles[currentIndex].Joint4U, jointAngles[currentIndex].Urgripper, jointAngles[currentIndex].Ulgripper);
        currentIndex++;
        if (currentIndex >= jointAngles.Count)
        {
            CancelInvoke();
        }
    }

    void ReadCSVFile()
    {
        var path = Directory.GetCurrentDirectory();
        var filePath = Path.Combine(path, "bagfiles/gripper.csv"); 

        using (var strReader = new StreamReader(filePath))
        {
            strReader.ReadLine();
            while (!strReader.EndOfStream)
            {
                var line = strReader.ReadLine();
                var angles = DecodeLine(line);
                jointAngles.Add(angles);
            }
        }
    }

    private JointAngles DecodeLine(string line)
    {
        string[] values = line.Split(',');
        float.TryParse(values[16], out float joint1L);
        float.TryParse(values[17], out float joint2L);
        float.TryParse(values[18], out float joint3L);
        float.TryParse(values[19], out float joint4L);
        float.TryParse(values[20], out float joint1U);
        float.TryParse(values[21], out float joint2U);
        float.TryParse(values[22], out float joint3U);
        float.TryParse(values[23], out float joint4U);
        float.TryParse(values[24], out float llgripper);
        float.TryParse(values[25], out float lrgripper);
        float.TryParse(values[26], out float ulgripper);
        float.TryParse(values[27], out float urgripper);

        return new JointAngles(joint1L, joint2L, joint3L, joint4L, joint1U, joint2U, joint3U, joint4U, llgripper, lrgripper, ulgripper, urgripper);
    }
}
