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
        robot.set_lower_joint_target(45, -45, 0, 0);
        robot.set_upper_joint_target(-45, 45, 0, 0);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
    }
}
