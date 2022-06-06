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


public class DuaroAgentSimple : Agent
{

    public int action;

    List<int> sequence = new List<int>(6);
    //List<List<int>> allSequences = new List<List<int>>(); //Creates new nested List
    List<int> allSequences = new List<int>();

    //**************
    //Add all the cube Positions:
    //**************
    
    //Array to Store the items Position (Initial Row, Initial Column, Row Length, Column Length)
    int[,] itemsPosition = new int[6,4]{ 
                                        {2,0,2,1}, // Black Cube
                                        {2,2,1,1}, // Blue Cube
                                        {1,0,1,3}, // Green Cube
                                        {2,4,1,1}, // Red Cube
                                        {3,1,1,6}, // White Cube
                                        {2,6,1,1}, // Yellow Cube
                                    };

    // Array with the shape of the blocks
    int[,] taskArray = new int[4,7]{ 
                                    {0,0,0,0,0,0,0},
                                    {1,1,1,0,0,0,0},
                                    {1,0,1,0,1,0,1}, 
                                    {1,1,1,1,1,1,1},  
                                };

    int[,] shapeBackup = new int[4,7]{ 
                                    {0,0,0,0,0,0,0},
                                    {1,1,1,0,0,0,0},
                                    {1,0,1,0,1,0,1}, 
                                    {1,1,1,1,1,1,1}, 
                                };

    // Boolean to check if there are items above in the rewards function                                
    private bool itemsAbove;

    private int checkAllDone;
    
    // Check which arm has to move
    private bool moveLowerOrUpper;

    private int arm_collision = 0;

    //Max Number of Steps to be performed before the environment restarts
    [Tooltip("Max Environment Steps")] public int MaxEnvironmentSteps = 100000;
    private int m_resetTimer;
    //Max Number of Skills to be performed before the environment restarts
    [Tooltip("Max Number of Skills")] public int MaxSkills = 10;
    private int m_resetSkill;

    // Variable to store the cumulative Reward
    // float reward;

    public override void Initialize()
    {
        //control = FindObjectOfType<Control>();
    }

    public override void OnEpisodeBegin() //set-up the environment for a new episode
    {
        Debug.Log("OnEpisodeBegin");


        arm_collision = 0;
        m_resetTimer = 0;
        m_resetSkill = 0;
        
        taskArray = (int[,]) shapeBackup.Clone(); // make a copy

        sequence.Clear();

    }

    /// <summary>
    /// Add relevant information on each body part to observations.
    /// </summary>
    public override void CollectObservations(VectorSensor sensor) //collect info needed to make decision
    {
        for (int i = 0; i < 7; i++)
        {
            for (int j = 1; j < 4; j++)
            {
                sensor.AddObservation(taskArray[j,i]);
            }
            sensor.AddObservation(arm_collision);
        }
    }

    public override void OnActionReceived(ActionBuffers actionBuffers) //receives actions and assigns the reward
    {      
        Debug.Log("OnActionReceived");
        // Move the agent using the action.
        //MoveAgent(actionBuffers.DiscreteActions);
        AgentRewards(actionBuffers.DiscreteActions);

        // CHECK WHY USE THIS EVEN IN HEURISTIC MODE
    }


    /// <summary>
    /// Function to call the discrete action selected 
    /// (while training or in heuristics mode)
    /// </summary>
    public void MoveAgent(ActionSegment<int> act)
    {
        
    }

    public void AgentRewards (ActionSegment<int> act)
    {
        action = act[0];
        var decision = act[0];
        
        if (action > 4)
        {
            action = act[0] - 5;
        }

        m_resetSkill +=1; // Add +1 to Skills Counter
        Debug.Log("Skill Number: " + m_resetSkill);
        // Debug.Log("Agent Rewards");

        // Rewards
        Debug.Log("Action Lower = " + action);

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
            AddReward(2.0f);
            sequence.Add(decision);
            
            // Update items Matrix Space
            for (int i = itemsPosition[action,1]; i < (itemsPosition[action,1] + itemsPosition[action,3]); i++)
            {
                for (int j = itemsPosition[action,0]; j < (itemsPosition[action,0] + itemsPosition[action,2]); j++)
                {
                    taskArray[j,i] = 0;
                    //Debug.Log("Cleaning");
                }
            }
            Debug.Log("Add Reward (+2)");

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
            AddReward(-2.0f);
            Debug.Log("Add Negative Reward - There is something above (-2)");
        } 
        //*********************************************************
        // TO BE DONE: (It is not scalable, just for training test)
        //********************************************************* 
        checkAllDone = 0;
        for (int i = 0; i < 7; i++)
        {
            checkAllDone = checkAllDone + taskArray[3,i];
        }

        if (checkAllDone == 0)
        {
            AddReward(5.0f);
            Debug.Log("TASK COMPLETED (+5)! -- Restarting the Environment");
            foreach(int item in sequence)
            {
                Debug.Log("item = " + item);            
            }

            allSequences.AddRange(sequence);

            EndEpisode();
        }

        
        // foreach (int a in taskArray) 
        //   {
        //       Debug.Log(a);
        //   }
        
        // Update and show Cumulative Reward
        //reward = GetCumulativeReward();        
        //Debug.Log("CumulativeReward: " + reward);
    }

        


    void FixedUpdate()
    {
        // Update Cumulative Reward
        //reward = GetCumulativeReward();

        //Debug.Log("Fixed Update");
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



