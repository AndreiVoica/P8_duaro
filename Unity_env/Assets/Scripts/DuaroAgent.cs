using UnityEngine;
using UnityEngine.UI;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Policies;
using Unity.MLAgents.Actuators;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Unity.MLAgentsExamples;


public class DuaroAgent : Agent
{
    private Library robot;

    // Robot Joints
    public Transform Joint1Upper;
    public Transform Joint2Upper;
    public Transform Joint3Upper;
    public Transform Joint4Upper;
    public Transform Joint1Lower;
    public Transform Joint2Lower;
    public Transform Joint3Lower;
    public Transform Joint4Lower;
    public Transform GripperLowerLeft;
    public Transform GripperLowerRight;
    public Transform GripperUpperLeft;
    public Transform GripperUpperRight;

    public Transform Target; //Target the agent will try to touch during training.

    // Max steps to do before reset de environment
    [Tooltip("Max Environment Steps")] public int MaxEnvironmentSteps = 2000;
    private int m_resetTimer;

    public override void Initialize()
    {
        robot = FindObjectOfType<Library>();

    }


    public override void OnEpisodeBegin() //set-up the environment for a new episode
    {

        // Move the target to a new spot
        Target.localPosition = new Vector3(Random.value * -0.18f + 0.09f,
                                           -0.1f,
                                           Random.value * - 1.2f);

    }

    /// <summary>
    /// Add relevant information on each body part to observations.
    /// </summary>

    public override void CollectObservations(VectorSensor sensor) //collect info needed to make decision
    {

        // Target and Agent positions
        sensor.AddObservation(Target.position);
        sensor.AddObservation(this.transform.position);

        // Observations for each Joint
        sensor.AddObservation(Joint1Lower.position);
        sensor.AddObservation(Joint2Lower.position);
        sensor.AddObservation(Joint3Lower.position);
        sensor.AddObservation(Joint4Lower.position);
        sensor.AddObservation(Joint1Upper.position);
        sensor.AddObservation(Joint2Upper.position);
        sensor.AddObservation(Joint3Upper.position);
        sensor.AddObservation(Joint4Upper.position);

        // Observations for each Finger
        sensor.AddObservation(GripperLowerLeft.position);
        sensor.AddObservation(GripperLowerRight.position);
        sensor.AddObservation(GripperUpperLeft.position);
        sensor.AddObservation(GripperUpperRight.position);

    }

    public override void OnActionReceived(ActionBuffers actionBuffers) //receives actions and assigns the reward
    {

        var continuousActions = actionBuffers.ContinuousActions;
        var i = -1;

        // Actions, size = 4
        robot.set_lower_joint_target(continuousActions[++i]*90,continuousActions[++i]*90,0,0,0,0);
        robot.set_upper_joint_target(continuousActions[++i]*90,continuousActions[++i]*90,0,0,0,0);
        

        // Rewards
        float distanceToTargetOK = Vector3.Distance(Joint3Lower.position, Target.position);
        float distanceToTargetBAD = Vector3.Distance(Joint3Upper.position, Target.position);

        // Reached target
        if (distanceToTargetOK < 0.45f)
        {
            SetReward(2.0f);
            Debug.Log("Good Reward");
            EndEpisode();
        }

        if (distanceToTargetBAD < 0.45f)
        {
            SetReward(-1.0f);
            Debug.Log("Bad Reward");
            EndEpisode();
        }

    }

    /// <summary>
    /// Stop training when time is reached , Go to the next episode, Reset the scene 
    /// </summary>
    void FixedUpdate()
    {
        m_resetTimer += 1;
        if (m_resetTimer >= MaxEnvironmentSteps && MaxEnvironmentSteps > 0)
        {
            Debug.Log("Restarting Scene - Cube not reachable");
            //SetReward(MaxEnvironmentSteps* - 0.000001f);
            m_resetTimer = 0;
            EndEpisode();
        }
    }

    // /// <summary>
    // /// Check collisions between the arms
    // /// </summary>
    // void OnTriggerEnter(Collider other)
    // {
    //     //if (other.TryGetComponent<Goal>(out Goal goal)){
    //     AddReward(-0.2f);
    //     //EndEpisode();
    //     Debug.Log("Collision Found - Small Negative Reward");
    //     //}
	// }


    // public override void Heuristic(in ActionBuffers actionsOut){
    // ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
    // if (Input.GetKeyDown(KeyCode.W))
    //             discreteActions[0] = 0;
    //         if (Input.GetKeyDown(KeyCode.S))
    //             discreteActions[0] = 1;
    //         if (Input.GetKeyDown(KeyCode.A))
    //             discreteActions[0] = 2;
    //         if (Input.GetKeyDown(KeyCode.D))
    //             discreteActions[0] = 3;
    // }

}
