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
  
    public Transform black;
    public Transform green;
    public Transform Rectangle;
    public Transform yellow;
    public Transform blue;
    public Transform red;

    //private Control control;

    public int action;
    public int actionU;


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
        // Reset cube positions:
        //Vector3 rotationVector = new Vector3(0, 0, 0);
        Debug.Log("OnEpisodeBegin");
        // black.transform.localPosition = new Vector3(1.22f,0.789f,-1.3663f);
        // black.transform.rotation = Quaternion.Euler(rotationVector);
        // green.transform.localPosition = new Vector3(1.219f,0.898f,-1.32f);
        // green.transform.rotation = Quaternion.Euler(rotationVector);
        // Rectangle.transform.localPosition = new Vector3(1.22f,0.763f,-1.184f);
        // Rectangle.transform.rotation = Quaternion.Euler(rotationVector);
        // blue.transform.localPosition = new Vector3(1.219f,0.8231f,-1.267f);
        // blue.transform.rotation = Quaternion.Euler(rotationVector);
        // yellow.transform.localPosition = new Vector3(1.2213f,0.8255f,-1.0598f);
        // yellow.transform.rotation = Quaternion.Euler(rotationVector);
        // red.transform.localPosition = new Vector3(1.221f,0.823f,-1.1674f);
        // red.transform.rotation = Quaternion.Euler(rotationVector);

        m_resetTimer = 0;
        m_resetSkill = 0;
        
        //shapeBackup.CopyTo(taskArray, 0);
        taskArray = (int[,]) shapeBackup.Clone(); // make a copy

        //SetReward(0.0f);
        //checkAllDone = 0;

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
        }

    }

    public override void OnActionReceived(ActionBuffers actionBuffers) //receives actions and assigns the reward
    {      
        Debug.Log("OnActionReceived");
        // Move the agent using the action.
        //MoveAgent(actionBuffers.DiscreteActions);
        AgentRewardsL(actionBuffers.DiscreteActions);
        AgentRewardsU(actionBuffers.DiscreteActions);

        // CHECK WHY USE THIS EVEN IN HEURISTIC MODE
    }


    /// <summary>
    /// Function to call the discrete action selected 
    /// (while training or in heuristics mode)
    /// </summary>
    public void MoveAgent(ActionSegment<int> act)
    {
        // if(moveLowerOrUpper == true)
        // {
        //     //control.jointAnglesL.Clear();

        //     action = act[0];

        //     // switch (action)
        //     // {        
        //     //     case 0:
        //     //         control.currentIndexL = 0;
        //     //         control.PickBlackLower();
        //     //         break;
        //     //     case 1:
        //     //         control.currentIndexL = 0;
        //     //         control.PickBlueLower();
        //     //         break;
        //     //     case 2:
        //     //         control.currentIndexL = 0;
        //     //         control.PickGreenLower();
        //     //         break;
        //     //     case 3: 
        //     //         control.currentIndexL = 0;
        //     //         control.PickRedLower();
        //     //         break;
        //     //     case 4:
        //     //         control.currentIndexL = 0;
        //     //         control.PickWhiteLower();
        //     //         break;
        //     //     default:
        //     //         break;
        //     // }
        //     moveLowerOrUpper = false;
        // }
        // else if (moveLowerOrUpper == false)
        // {
        //     //control.jointAnglesU.Clear();

        //     action = act[1];

        //     // switch (action)
        //     // {        
        //     //     case 0:
        //     //         control.currentIndexU = 0;
        //     //         control.PickBlackUpper();
        //     //         break;
        //     //     case 1:
        //     //         control.currentIndexU = 0;
        //     //         control.PickBlueUpper();
        //     //         break;
        //     //     case 2:
        //     //         control.currentIndexU = 0;
        //     //         control.PickGreenUpper();
        //     //         break;
        //     //     case 3: 
        //     //         control.currentIndexU = 0;
        //     //         control.PickRedUpper();
        //     //         break;
        //     //     case 4:
        //     //         control.currentIndexU = 0;
        //     //         control.PickWhiteUpper();
        //     //         break;
        //     //     case 5:
        //     //         control.currentIndexU = 0;
        //     //         control.PickYellowUpper();
        //     //         break;
        //     //     default:
        //     //         break;
        //     // }
        //     moveLowerOrUpper = true;
        // }
    }

    public void AgentRewardsL (ActionSegment<int> act)
    {

        action = act[0];
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
        public void AgentRewardsU (ActionSegment<int> act)
    {

        actionU = act[1];
        m_resetSkill +=1; // Add +1 to Skills Counter
        Debug.Log("Skill Number: " + m_resetSkill);
        // Debug.Log("Agent Rewards");

        // Rewards
        Debug.Log("Action Upper = " + actionU);

        // Check if there are items above
        itemsAbove = false;
        for (int i = itemsPosition[actionU,1]; i < itemsPosition[actionU,3]; i++)
        {
            if (taskArray[itemsPosition[actionU,0] - 1, i] == 1)
            {
                itemsAbove = true; // There are some objects above this part
            }
        }

        if (taskArray[itemsPosition[actionU,0], itemsPosition[actionU,1]] == 1 && itemsAbove == false)
        {
            AddReward(2.0f);
            
            // Update items Matrix Space
            for (int i = itemsPosition[actionU,1]; i < (itemsPosition[actionU,1] + itemsPosition[actionU,3]); i++)
            {
                for (int j = itemsPosition[actionU,0]; j < (itemsPosition[actionU,0] + itemsPosition[actionU,2]); j++)
                {
                    taskArray[j,i] = 0;
                    //Debug.Log("Cleaning");
                }
            }
            Debug.Log("Add Reward (+2)");

        }

        // Penalize choosing same action again
        else if(taskArray[itemsPosition[actionU,0], itemsPosition[actionU,1]] == 0)
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

        // moveLowerOrUpper = true; Lower Arm moves
        // moveLowerOrUpper = false; Upper arm moves

    // public override void Heuristic(in ActionBuffers actionsOut)
    // {
        
    //     // Debug.Log("Heuristic");
        
    //     var discreteActionsOut = actionsOut.DiscreteActions;

    //     if (Input.GetKey(KeyCode.A) && control.currentIndexL >= control.jointAnglesL.Count) // Select discrete action Lower 0
    //     {
    //         discreteActionsOut[0] = 0;
    //         Debug.Log("Key A Pressed");
    //         moveLowerOrUpper = true;
    //         MoveAgent(actionsOut.DiscreteActions);
    //         AgentRewards(actionsOut.DiscreteActions);
    //     }
    //     else if (Input.GetKey(KeyCode.B) && control.currentIndexL >= control.jointAnglesL.Count) // Select discrete action Lower 1
    //     {
    //         discreteActionsOut[0] = 1;
    //         Debug.Log("Key B Pressed");
    //         moveLowerOrUpper = true;
    //         MoveAgent(actionsOut.DiscreteActions);
    //         AgentRewards(actionsOut.DiscreteActions);
    //     }
    //     else if (Input.GetKey(KeyCode.C) && control.currentIndexL >= control.jointAnglesL.Count) // Select discrete action Lower 2
    //     {
    //         discreteActionsOut[0] = 2;
    //         Debug.Log("Key C Pressed");
    //         moveLowerOrUpper = true;
    //         MoveAgent(actionsOut.DiscreteActions);
    //         AgentRewards(actionsOut.DiscreteActions);
    //     }
    //     else if(Input.GetKey(KeyCode.D) && control.currentIndexL >= control.jointAnglesL.Count) // Select discrete action Lower 3
    //     {
    //         discreteActionsOut[0] = 3;
    //         Debug.Log("Key D Pressed");
    //         moveLowerOrUpper = true;
    //         MoveAgent(actionsOut.DiscreteActions);
    //         AgentRewards(actionsOut.DiscreteActions); 
    //     }
    //     else if(Input.GetKey(KeyCode.E) && control.currentIndexL >= control.jointAnglesL.Count) // Select discrete action Lower 4
    //     {
    //         discreteActionsOut[0] = 4;
    //         Debug.Log("Key E Pressed");
    //         moveLowerOrUpper = true;
    //         MoveAgent(actionsOut.DiscreteActions);
    //         AgentRewards(actionsOut.DiscreteActions);
    //     }
    //     else if(Input.GetKey(KeyCode.Keypad1) && control.currentIndexU >= control.jointAnglesU.Count) // Select discrete action Upper 0
    //     {
    //         discreteActionsOut[1] = 0;
    //         Debug.Log("Key 1 Pressed");
    //         moveLowerOrUpper = false;
    //         MoveAgent(actionsOut.DiscreteActions);
    //         AgentRewards(actionsOut.DiscreteActions);
    //     }
    //     else if(Input.GetKey(KeyCode.Keypad2) && control.currentIndexU >= control.jointAnglesU.Count) // Select discrete action Upper 1
    //     {
    //         discreteActionsOut[1] = 1;
    //         Debug.Log("Key 2 Pressed");
    //         moveLowerOrUpper = false;
    //         MoveAgent(actionsOut.DiscreteActions);
    //         AgentRewards(actionsOut.DiscreteActions);
    //     }
    //     else if(Input.GetKey(KeyCode.Keypad3) && control.currentIndexU >= control.jointAnglesU.Count) // Select discrete action Upper 2
    //     {
    //         discreteActionsOut[1] = 2;
    //         Debug.Log("Key 3 Pressed");
    //         moveLowerOrUpper = false;
    //         MoveAgent(actionsOut.DiscreteActions);
    //         AgentRewards(actionsOut.DiscreteActions);
    //     }
    //     else if(Input.GetKey(KeyCode.Keypad4) && control.currentIndexU >= control.jointAnglesU.Count) // Select discrete action Upper 3
    //     {
    //         discreteActionsOut[1] = 3;
    //         Debug.Log("Key 4 Pressed");
    //         moveLowerOrUpper = false;
    //         MoveAgent(actionsOut.DiscreteActions);
    //         AgentRewards(actionsOut.DiscreteActions);
    //     }
    //     else if(Input.GetKey(KeyCode.Keypad5) && control.currentIndexU >= control.jointAnglesU.Count) // Select discrete action Upper 4
    //     {
    //         discreteActionsOut[1] = 4;
    //         Debug.Log("Key 5 Pressed");
    //         moveLowerOrUpper = false;
    //         MoveAgent(actionsOut.DiscreteActions);
    //         AgentRewards(actionsOut.DiscreteActions);
    //     }
    //     else if(Input.GetKey(KeyCode.Keypad6) && control.currentIndexU >= control.jointAnglesU.Count) // Select discrete action Upper 5
    //     {
    //         discreteActionsOut[1] = 5;
    //         Debug.Log("Key 6 Pressed");
    //         moveLowerOrUpper = false;
    //         MoveAgent(actionsOut.DiscreteActions);
    //         AgentRewards(actionsOut.DiscreteActions);
    //     }
    //     Debug.Log("discreteActionsOut Lower = " + discreteActionsOut[0]);
    //     Debug.Log("discreteActionsOut Upper = " + discreteActionsOut[1]);
    // }	 


    void FixedUpdate()
    {
        // RESEARCH IF REQUESTING ACTIONS THIS WAY THE AGENT MAY BE LEARNING FROM HALF OF THE ACTIONS THAT ARE NOT DOING ANYTHING 
        // IF THAT IS THE CASE, TRY TO REQUEST ACTION FROM ONLY 1 OF THE DISCRETE ACTION BRANCHES
        // if(control.currentIndexL >= control.jointAnglesL.Count && checkAllDone != 2)
        // {
        //     moveLowerOrUpper = true;
        //     RequestDecision();
        // }
        // if(control.currentIndexU >= control.jointAnglesU.Count && checkAllDone != 2)
        // {
        //     moveLowerOrUpper = false;
        //     RequestDecision();
        // }


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



