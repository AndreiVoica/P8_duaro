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

    int[,] shapeBackup = new int[3,4]{ 
                                    {0,0,0,0},
                                    {1,0,0,1}, 
                                    {1,1,1,1}, 
                                };


    // Boolean to check if there are items above in the rewards function                                
    private bool itemsAbove;

    public bool actionEndLower = true;
    public bool actionEndUpper = true;


    private int checkAllDone;

    //Max Number of Steps to be performed before the environment restarts
    [Tooltip("Max Environment Steps")] public int MaxEnvironmentSteps = 50000;
    private int m_resetTimer;
    //Max Number of Skills to be performed before the environment restarts
    [Tooltip("Max Number of Skills")] public int MaxSkills = 6;
    private int m_resetSkill;

    // Variable to store the cumulative Reward
    float reward;

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

        m_resetTimer = 0;
        m_resetSkill = 0;

        //shapeBackup.CopyTo(taskArray, 0);
        taskArray = (int[,]) shapeBackup.Clone(); // make a copy

        //SetReward(0.0f);
        
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
    //     MoveAgent(actionBuffers.DiscreteActions);
    //     AgentRewards(actionBuffers.DiscreteActions);
    //     // CHECK WHY APPLIES THIS EVEN IN HEURISTIC MODE
    // }


    /// <summary>
    /// Function to call the discrete action selected 
    /// (while training or in heuristics mode)
    /// </summary>
    public void MoveAgent(ActionSegment<int> act)
    {
        var decision = act[0];
       // decision = 0;
        //Debug.Log("Decision: " + decision);

        switch (decision)
        {        
        case 0:
            actionEndLower = false;
            control.PickBlueUpper();
            break;
        case 1:
            actionEndUpper = false;
            control.PickRedUpper();
            break;
        case 2:
            actionEndUpper = false;
            control.PickBlackLower();
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
        Debug.Log("Action = " + action);

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
            for (int i = itemsPosition[action,1]; i < (itemsPosition[action,1] + itemsPosition[action,3]); i++)
            {
                for (int j = itemsPosition[action,0]; j < (itemsPosition[action,0] + itemsPosition[action,2]); j++)
                {
                    taskArray[j,i] = 0;
                    //Debug.Log("Cleaning");
                }
            }
            Debug.Log("Add Reward (+1)");
        }
        // Penalize choosing same action again
        else if(taskArray[itemsPosition[action,0], itemsPosition[action,1]] == 0)
        { 
            AddReward(-1.0f);
            Debug.Log("Add Negative Reward - Choosing the same action (-1)");
        }
        // Penalize picking up the rectangle while there is something above
        if(itemsAbove == true)
        { 
            AddReward(-1.0f);
            Debug.Log("Add Negative Reward - There is something above (-1)");
        } 

        //*********************************************************
        // TO BE DONE: (It is not scalable, just for training test)
        checkAllDone = 0;
        for (int i = 0; i < 4; i++)
        {
            checkAllDone = checkAllDone + taskArray[2,i];
        }

        if (checkAllDone == 0)
        {
            AddReward(5.0f);
            Debug.Log("TASK COMPLETED +5!");
            EndEpisode();
        }
        //********************************************************* 

        // CHECK IF taskArray ONLY CONTAINS ZEROS, GIVE A HIGH REWARD AND RESET 
        // TO DO BETTER:

        // var allElementsAreZero = taskArray.All(o => o == 0);
        // if (allElementsAreZero == true)
        // {
        //     AddReward(10.0f);
        //     Debug.Log("TASK COMPLETED +10!");
        //     EndEpisode();
        // }

        // foreach (int a in taskArray) 
        //   {
        //       Debug.Log(a);
        //   }
        
        // Update and show Cumulative Reward
        reward = GetCumulativeReward();        
        Debug.Log("CumulativeReward: " + reward);
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
        // Debug.Log("discreteActionsOut = " + discreteActionsOut[0]);
    }	 


    void FixedUpdate()
    {
        if(control.currentIndexL >= control.jointAnglesL.Count)
        {
            actionEndLower = true;
            RequestDecision();
        }
        if(control.currentIndexU >= control.jointAnglesU.Count)
        {
            actionEndUpper = true;
            RequestDecision();
        }


        // // Request a decision when last action has finished
        // if (actionEndLower == true)
        // {
        //     RequestDecision();
        // }
        // if (actionEndUpper == true)
        // {
        //     RequestDecision();
        // }
        // Update Cumulative Reward
        reward = GetCumulativeReward();

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



