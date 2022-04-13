using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class Control : MonoBehaviour
{
    private Library robot;

    private List<JointAngles> jointAngles = new List<JointAngles>();
    public int currentIndex = 0;
    

    public void Start()
    {
        robot = FindObjectOfType<Library>();
        robot.set_lower_joint_target(-45f, 45f, 0f, 0f, 0.055f, -0.055f);
        robot.set_upper_joint_target(45f, -45f, 0f, 0f, 0.055f, -0.055f);
        var rate = 10f;
        var waitTime = 1f / rate;
        
        InvokeRepeating("MoveJoints", 0, waitTime); //Check this function
    }
/*
    public void Update()
    {
        if(Input.GetKey(KeyCode.A))
        {
            PickRed();
        }
        if(Input.GetKey(KeyCode.S))
        {
            PickBlue();
        }
        //Debug.Log("index test: " + currentIndex);
    }
*/
    void MoveJoints()
    {
        robot.set_lower_joint_target(jointAngles[currentIndex].Joint1L * Mathf.Rad2Deg, jointAngles[currentIndex].Joint2L * Mathf.Rad2Deg, jointAngles[currentIndex].Joint3L, jointAngles[currentIndex].Joint4L * Mathf.Rad2Deg, jointAngles[currentIndex].Lrgripper, jointAngles[currentIndex].Llgripper);
        robot.set_upper_joint_target(jointAngles[currentIndex].Joint1U * Mathf.Rad2Deg, jointAngles[currentIndex].Joint2U * Mathf.Rad2Deg, jointAngles[currentIndex].Joint3U, jointAngles[currentIndex].Joint4U * Mathf.Rad2Deg, jointAngles[currentIndex].Ulgripper, jointAngles[currentIndex].Urgripper);
        currentIndex++;
        //Debug.Log("CurrentIndex = " + currentIndex);
        // if (currentIndex >= 10000) //For what is this needed?
        // {

        //     currentIndex = 0;
        //     //CancelInvoke();
        //     Debug.Log("CancelInvoke");
        // }
    }


    public void PickWhite()
    {
        var pathWhite = Directory.GetCurrentDirectory();
        var filePathWhite = Path.Combine(pathWhite, "bagfiles/grip_white.csv"); 

        using (var strReader = new StreamReader(filePathWhite))
        {
            strReader.ReadLine();
            while (!strReader.EndOfStream)
            {
                var lineWhite = strReader.ReadLine();
                var anglesWhite = DecodeLine(lineWhite);
                jointAngles.Add(anglesWhite);
            }
        }
        Debug.Log("Pick White");
    }

    public void PickBlue()
    {
        var pathBlue = Directory.GetCurrentDirectory();
        var filePathBlue = Path.Combine(pathBlue, "bagfiles/grip_blue.csv"); 

        using (var strReader = new StreamReader(filePathBlue))
        {
            strReader.ReadLine();
            while (!strReader.EndOfStream)
            {
                var lineBlue = strReader.ReadLine();
                var anglesBlue = DecodeLine(lineBlue);
                jointAngles.Add(anglesBlue);
            }
        }
        Debug.Log("Pick Blue");
    }


    public void PickRed()
    {
        var pathRed = Directory.GetCurrentDirectory();
        var filePathRed = Path.Combine(pathRed, "bagfiles/grip_red.csv"); 

        using (var strReader = new StreamReader(filePathRed))
        {
            strReader.ReadLine();
            while (!strReader.EndOfStream)
            {
                var lineRed = strReader.ReadLine();
                var anglesRed = DecodeLine(lineRed);
                jointAngles.Add(anglesRed);
            }
        }
        Debug.Log("Pick Red");
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
