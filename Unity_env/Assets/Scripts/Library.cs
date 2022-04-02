using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Library : MonoBehaviour
{
    [SerializeField] private ArticulationBody[] robotJoints = new ArticulationBody[10];
    public void set_upper_joint_target(float j1_u, float j2_u, float j3_u, float j4_u)
    {
        var joint1UpXDrive = robotJoints[5].xDrive;
        joint1UpXDrive.target = j1_u;
        robotJoints[5].xDrive = joint1UpXDrive;

        var joint2UpXDrive = robotJoints[6].xDrive;
        joint2UpXDrive.target = j2_u;
        robotJoints[6].xDrive = joint2UpXDrive;

        var joint3UpXDrive = robotJoints[7].xDrive;
        joint3UpXDrive.target = j3_u;
        robotJoints[7].xDrive = joint3UpXDrive;
        
        var joint3_oUpXDrive = robotJoints[8].xDrive;
        joint3_oUpXDrive.target = j3_u;
        robotJoints[8].xDrive = joint3_oUpXDrive;

        var joint4UpXDrive = robotJoints[9].xDrive;
        joint4UpXDrive.target = j4_u;
        robotJoints[9].xDrive = joint4UpXDrive;
    }

    public void set_lower_joint_target(float j1_l, float j2_l, float j3_l, float j4_l)
    {
        var joint1LoXDrive = robotJoints[0].xDrive;
        joint1LoXDrive.target = j1_l;
        robotJoints[0].xDrive = joint1LoXDrive;
        //return a;

        var joint2LoXDrive = robotJoints[1].xDrive;
        joint2LoXDrive.target = j2_l;
        robotJoints[1].xDrive = joint2LoXDrive;
        //return b;

        var joint3LoXDrive = robotJoints[2].xDrive;
        joint3LoXDrive.target = j3_l;
        robotJoints[2].xDrive = joint3LoXDrive;
        
        var joint3_oLoXDrive = robotJoints[3].xDrive;
        joint3_oLoXDrive.target = j3_l;
        robotJoints[3].xDrive = joint3_oLoXDrive;
        //return c;

        var joint4LoXDrive = robotJoints[4].xDrive;
        joint4LoXDrive.target = j4_l;
        robotJoints[4].xDrive = joint4LoXDrive;
    }
}
