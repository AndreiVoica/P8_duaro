using UnityEngine;
using UnityEngine.UI;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Policies;
using Unity.MLAgents.Actuators;
using System.Collections.Generic;
using System.Linq;
using System.IO;

class DuaroAgent : Agent
{
    private Library robot;

    void Start()
    {
        robot = GetComponent<Library>();
    }

    public override void OnEpisodeBegin() //set-up the environment for a new episode
    {
        robot.set_lower_joint_target(45, -45, 0, 0, -0.055f, 0.055f);
        robot.set_upper_joint_target(-45, 45, 0, 0, -0.055f, 0.055f);
    }

    public override void CollectObservations(VectorSensor sensor) //collect info needed to make decision
    {
    }

    public override void OnActionReceived(ActionBuffers actionBuffers) //receives actions and assigns the reward
    {
    }
}
