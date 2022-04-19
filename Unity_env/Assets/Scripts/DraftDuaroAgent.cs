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
using Random = System.Random;
//using Random = UnityEngine.Random;


public class DraftDuaroAgent : Agent
{
  
    public Transform blue;
    public Transform red;
    public Transform rectangle;

    private bool pickup_blue;
    private bool pickup_red;
    private bool pickup_rectangle;

    private Control control;

    //Array to Store the items Position (Initial Row, Initial Column, Row Length, Column Length)
    int[,] itemsPosition = new int[3,4]{ 
                                        {1,0,1,1}, // Blue Cube
                                        {1,3,1,1}, // Red Cube
                                        {2,0,1,4}, // Rectangle
                                    };

    // Array with the shape of the blocks
    int[,] taskArray = new int[3,4]{ 
                                    {0,0,0,0},
                                    {1,0,0,1}, 
                                    {1,1,1,1}, 
                                };

    // Boolean to check if there are items above in the rewards function                                
    private bool itemsAbove;

    //Max Number of Steps to be performed before the environment restarts
    [Tooltip("Max Environment Steps")] public int MaxEnvironmentSteps = 50000;
    private int m_resetTimer;
    //Max Number of Skills to be performed before the environment restarts
    [Tooltip("Max Number of Skills")] public int MaxSkills = 10;
    private int m_resetSkill;


    public override void Initialize()
    {
        control = FindObjectOfType<Control>();
    }

    public override void OnEpisodeBegin() //set-up the environment for a new episode
    {
        // Reset cube positions:
        Vector3 rotationVector = new Vector3(0, 0, 0);
        Debug.Log("OnEpisodeBegin");
        blue.transform.localPosition = new Vector3(1.22300005f,0.823099971f,-1.296f);
        blue.transform.rotation = Quaternion.Euler(rotationVector);
        red.transform.localPosition = new Vector3(1.22500002f,0.823000014f,-1.17999995f);
        red.transform.rotation = Quaternion.Euler(rotationVector);
        rectangle.transform.localPosition = new Vector3(1.22000003f,0.740999997f,-1.23800004f);
        rectangle.transform.rotation = Quaternion.Euler(rotationVector);
        
        pickup_blue = false;
        pickup_red = false;

        m_resetTimer = 0;
        m_resetSkill = 0;
        
    }

    /// <summary>
    /// Add relevant information on each body part to observations.
    /// </summary>
    public override void CollectObservations(VectorSensor sensor) //collect info needed to make decision
    {
        sensor.AddObservation(blue.position);
    }

    // public override void OnActionReceived(ActionBuffers actionBuffers) //receives actions and assigns the reward
    // {      
    //     Debug.Log("OnActionReceived");
    //     // Move the agent using the action.
    //     //MoveAgent(actionBuffers.DiscreteActions);
    //     // CHECK WHY USE THIS EVEN IN HEURISTIC MODE
    // }


    /// <summary>
    /// Function to call the discrete action selected 
    /// (while training or in heuristics mode)
    /// </summary>
    public void MoveAgent(ActionSegment<int> act)
    {
        var decision = act[0];
       // decision = 0;
        Debug.Log("test decision: " + decision);

        switch (decision)
        {        
        case 0:
            pickup_blue = true;
            control.PickBlue();
            break;
        case 1:
            pickup_red = true;
            control.PickRed();
            break;
        case 2:
            pickup_rectangle = true;
            control.PickWhite();
            break;
        default:
            break;
        }
        m_resetSkill +=1; // Add +1 to Skills Counter  
        Debug.Log("Skill Number: " + m_resetSkill);
    }

    public void AgentRewards (ActionSegment<int> act)
        {
        // Debug.Log("Agent Rewards");

        var action = act[0];

        // Rewards
        Debug.Log("action = " + action);
        if (action == 2)
        { 
            // Check if there are items above
            itemsAbove = false;
            for (int i = itemsPosition[action,1]; i < itemsPosition[action,3]; i++)
            {
                if (taskArray[itemsPosition[action,0] - 1, i] == 1)
                {
                    itemsAbove = true; // There are some objects above this part
                }
            }

            if (taskArray[itemsPosition[action,0], itemsPosition[action,1]] == 1 && itemsAbove == false)
            {
                AddReward(1.0f);
                
                // Update items Matrix Space
                for (int i = itemsPosition[action,1]; i < itemsPosition[action,3]; i++)
                {
                    for (int j = itemsPosition[action,0]; j < itemsPosition[action,2]; j++)
                    {
                        taskArray[itemsPosition[action,0 + j], itemsPosition[action,1] + i] = 0;
                    }
                }
                Debug.Log("Add Reward");
            }
            // Penalize choosing same action again
            else if(taskArray[itemsPosition[action,0], itemsPosition[action,1]] == 0)
            { 
                AddReward(-1.0f);
                Debug.Log("Add Negative Reward - Choosing the same action");
            }
            // Penalize picking up the rectangle while there is something above
            else if(itemsAbove == true)
            { 
                AddReward(-1.0f);
                Debug.Log("Add Negative Reward - There is something above");
            }
        }  
    }


    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // Debug.Log("Heuristic");
        var discreteActionsOut = actionsOut.DiscreteActions;

        if (Input.GetKey(KeyCode.A)) // Select discrete action 0
        {
            discreteActionsOut[0] = 0;
            Debug.Log("Key A Pressed");
            MoveAgent(actionsOut.DiscreteActions);
            AgentRewards(actionsOut.DiscreteActions);
        }
        else if (Input.GetKey(KeyCode.B)) // Select discrete action 1
        {
            discreteActionsOut[0] = 1;
            Debug.Log("Key B Pressed");
            MoveAgent(actionsOut.DiscreteActions);
            AgentRewards(actionsOut.DiscreteActions);
        }
        else if (Input.GetKey(KeyCode.C)) // Select discrete action 2
        {
            discreteActionsOut[0] = 2;
            Debug.Log("Key C Pressed");
            MoveAgent(actionsOut.DiscreteActions);
            AgentRewards(actionsOut.DiscreteActions);
        }
        Debug.Log("discreteActionsOut = " + discreteActionsOut[0]);
    }	 


    void FixedUpdate()
    {
        Debug.Log("Fixed Update");
        m_resetTimer += 1; // Add +1 to Steps Counter
        if (m_resetTimer >= MaxEnvironmentSteps && MaxEnvironmentSteps > 0)
        {
            Debug.Log("Restarting Scene from Fixed Update (Max Environment Steps)");
            //AddReward(MaxEnvironmentSteps* - 0.000001f);
            EndEpisode();
        }
        else if(m_resetSkill > MaxSkills)
        {
            Debug.Log("Restarting Scene from Fixed Update (Max Number of Skills)");
            EndEpisode();
        }
    }
}



