using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class Control : MonoBehaviour
{
    private Library robot; // imports library script which contains the joint and gripper definitions and the functions to control them

    private List<JointAngles> jointAnglesU = new List<JointAngles>(); //List that holds the joint angles values for the upper hand
    private List<JointAngles> jointAnglesL = new List<JointAngles>(); //List that holds the joint angles values for the lower hand
    public int currentIndexU = 0; //index that will iterate through each value of the joint angle (Upper hand)
    public int currentIndexL = 0; //index that will iterate through each value of the joint angle (Lower hand)
    

    public void Start()
    {
        robot = FindObjectOfType<Library>(); //Finds the object containing the library (Maybe unnecessary, check later)
        robot.set_lower_joint_target(-45f, 45f, 0f, 0f, 0.055f, -0.055f); //Sets the lower joint values to a home position
        robot.set_upper_joint_target(45f, -45f, 0f, 0f, 0.055f, -0.055f); //Sets the upper joint values to a home position
        var rate = 10f;
        var waitTime = 1f / rate; //Wait time for each iterations through joint angles (Upper and Lower)
        
        InvokeRepeating("MoveJointsUpper", 0, waitTime); //Allows for repeating the funtion MoveJointUpper() and MoveJointLower()
        InvokeRepeating("MoveJointsLower", 0, waitTime);
    }

    void MoveJointsLower() //Function for moving the lower joints
    {   
        //jointAngleL is the converted float value from the csv file. We increment currentIndex so jointAnglesL is able to use every value from the csv.
        robot.set_lower_joint_target(jointAnglesL[currentIndexL].Joint1L * Mathf.Rad2Deg, jointAnglesL[currentIndexL].Joint2L * Mathf.Rad2Deg, jointAnglesL[currentIndexL].Joint3L, jointAnglesL[currentIndexL].Joint4L * Mathf.Rad2Deg, jointAnglesL[currentIndexL].Lrgripper, jointAnglesL[currentIndexL].Llgripper);
        //robot.set_upper_joint_target(jointAnglesL[currentIndex].Joint1U * Mathf.Rad2Deg, jointAnglesL[currentIndex].Joint2U * Mathf.Rad2Deg, jointAnglesL[currentIndex].Joint3U, jointAnglesL[currentIndex].Joint4U * Mathf.Rad2Deg, jointAnglesL[currentIndex].Ulgripper, jointAnglesL[currentIndex].Urgripper);
        currentIndexL++;
        
        // if (currentIndex >= 10000) //For what is this needed?
        // {

        //     currentIndex = 0;
        //     //CancelInvoke();
        //     Debug.Log("CancelInvoke");
        // }
    }

    void MoveJointsUpper() //Function for moving the upper joints (exactly like lower joints function)
    {
        //robot.set_lower_joint_target(jointAngles[currentIndex].Joint1L * Mathf.Rad2Deg, jointAngles[currentIndex].Joint2L * Mathf.Rad2Deg, jointAngles[currentIndex].Joint3L, jointAngles[currentIndex].Joint4L * Mathf.Rad2Deg, jointAngles[currentIndex].Lrgripper, jointAngles[currentIndex].Llgripper);
        robot.set_upper_joint_target(jointAnglesU[currentIndexU].Joint1U * Mathf.Rad2Deg, jointAnglesU[currentIndexU].Joint2U * Mathf.Rad2Deg, jointAnglesU[currentIndexU].Joint3U, jointAnglesU[currentIndexU].Joint4U * Mathf.Rad2Deg, jointAnglesU[currentIndexU].Ulgripper, jointAnglesU[currentIndexU].Urgripper);
        currentIndexU++;

        // if (currentIndex >= 10000) //For what is this needed?
        // {

        //     currentIndex = 0;
        //     //CancelInvoke();
        //     Debug.Log("CancelInvoke");
        // }
    }

    // PickWhite, PickBlue and PickRed are functions each representing a skill. PickWhite will pick the white cube in the Unity scene, PickRed -> Red cube, PickBlue -> Blue cube
    // All 3 functions work the same. The functions will find the csv file and read through all the rows and collumns.
    // The values will be then added to the jointAnglesU and jointAnglesL lists.

    public void PickGreenLower()
    {
        var pathWhite = Directory.GetCurrentDirectory();
        var filePathWhite = Path.Combine(pathWhite, "bagfiles/green_cube_lower.csv"); 

        using (var strReader = new StreamReader(filePathWhite))
        {
            strReader.ReadLine();
            while (!strReader.EndOfStream)
            {
                var lineWhite = strReader.ReadLine();
                var anglesWhite = DecodeLine(lineWhite);
                jointAnglesL.Add(anglesWhite);
            }
        }
        Debug.Log("Control - Pick White");
    }

    public void PickGreenUpper()
    {
        var pathWhite = Directory.GetCurrentDirectory();
        var filePathWhite = Path.Combine(pathWhite, "bagfiles/green_cube_upper.csv"); 

        using (var strReader = new StreamReader(filePathWhite))
        {
            strReader.ReadLine();
            while (!strReader.EndOfStream)
            {
                var lineWhite = strReader.ReadLine();
                var anglesWhite = DecodeLine(lineWhite);
                jointAnglesU.Add(anglesWhite);
            }
        }
        Debug.Log("Control - Pick White");
    }

    public void PickBlackLower()
    {
        var pathWhite = Directory.GetCurrentDirectory();
        var filePathWhite = Path.Combine(pathWhite, "bagfiles/black_cube_lower.csv"); 

        using (var strReader = new StreamReader(filePathWhite))
        {
            strReader.ReadLine();
            while (!strReader.EndOfStream)
            {
                var lineWhite = strReader.ReadLine();
                var anglesWhite = DecodeLine(lineWhite);
                jointAnglesL.Add(anglesWhite);
            }
        }
        Debug.Log("Control - Pick White");
    }

    public void PickBlackUpper()
    {
        var pathWhite = Directory.GetCurrentDirectory();
        var filePathWhite = Path.Combine(pathWhite, "bagfiles/black_cube_upper.csv"); 

        using (var strReader = new StreamReader(filePathWhite))
        {
            strReader.ReadLine();
            while (!strReader.EndOfStream)
            {
                var lineWhite = strReader.ReadLine();
                var anglesWhite = DecodeLine(lineWhite);
                jointAnglesU.Add(anglesWhite);
            }
        }
        Debug.Log("Control - Pick White");
    }

    public void PickBlueLower()
    {
        var pathWhite = Directory.GetCurrentDirectory();
        var filePathWhite = Path.Combine(pathWhite, "bagfiles/blue_cube_lower.csv"); 

        using (var strReader = new StreamReader(filePathWhite))
        {
            strReader.ReadLine();
            while (!strReader.EndOfStream)
            {
                var lineWhite = strReader.ReadLine();
                var anglesWhite = DecodeLine(lineWhite);
                jointAnglesL.Add(anglesWhite);
            }
        }
        Debug.Log("Control - Pick White");
    }

    public void PickBlueUpper()
    {
        var pathWhite = Directory.GetCurrentDirectory();
        var filePathWhite = Path.Combine(pathWhite, "bagfiles/blue_cube_upper.csv"); 

        using (var strReader = new StreamReader(filePathWhite))
        {
            strReader.ReadLine();
            while (!strReader.EndOfStream)
            {
                var lineWhite = strReader.ReadLine();
                var anglesWhite = DecodeLine(lineWhite);
                jointAnglesU.Add(anglesWhite);
            }
        }
        Debug.Log("Control - Pick White");
    }

    // The data from the csv comes as a string and we have to use floats for controlling the joints.
    // In here we we take the collumns of interest (16 -> 27), because these contain the data that we need (joint/gripper angles/positions)
    // float.TryParse is try to parse the string value from collum 16 into a float that is later added to the JointAngles and used to move the robot
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
