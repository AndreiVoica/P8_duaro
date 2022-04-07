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

        robot.set_upper_joint_target(-45f, 45f, 0f, 0f, -0.055f, 0.055f);
        robot.set_lower_joint_target(45f, -45f, 0f, 0f);

        ReadCSVFile();

        var rate = 10f;
        var waitTime = 1f / rate;

        InvokeRepeating("MoveJoints", 0, waitTime);
    }

    void Update()
    {
        // do nothing
    }

    public void MoveJoints()
    {
        robot.set_lower_joint_target(jointAngles[currentIndex].Joint1L, jointAngles[currentIndex].Joint2L, jointAngles[currentIndex].Joint3L, jointAngles[currentIndex].Joint4L);
        robot.set_upper_joint_target(jointAngles[currentIndex].Joint1U, jointAngles[currentIndex].Joint2U, jointAngles[currentIndex].Joint3U, jointAngles[currentIndex].Joint4U, -0.055f, 0.055f);
        currentIndex++;
        if (currentIndex >= jointAngles.Count)
        {
            CancelInvoke();
        }
    }

    void ReadCSVFile()
    {
        var path = Directory.GetCurrentDirectory();
        var filePath = Path.Combine(path, "bagfiles/test_1.csv"); 

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
        float.TryParse(values[12], out float joint1L);
        float.TryParse(values[13], out float joint2L);
        float.TryParse(values[14], out float joint3L);
        float.TryParse(values[15], out float joint4L);
        float.TryParse(values[16], out float joint1U);
        float.TryParse(values[17], out float joint2U);
        float.TryParse(values[18], out float joint3U);
        float.TryParse(values[19], out float joint4U);

        return new JointAngles(joint1L, joint2L, joint3L, joint4L, joint1U, joint2U, joint3U, joint4U);
    }
}
