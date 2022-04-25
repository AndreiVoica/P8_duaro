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


public class DuaroAgentComplexCollision : Agent
{
  
    public Transform black;
    public Transform green;
    public Transform Rectangle;
    public Transform yellow;
    public Transform blue;
    public Transform red;

    private Control control;

    public int action;

    // variables to check collision
    private bool pickup_blue;
    private bool pickup_red;
    private bool pickup_Rectangle;
    private bool pickup_green;
    private bool pickup_yellow;
    private bool pickup_black;

    // variables to count collision
    int count_collision_blue;
    int count_collision_red;        
    int count_collision_Rectangle;
    int count_collision_green;
    int count_collision_yellow;        
    int count_collision_black;

    int count_collision_arm_link;

    /*
    //****************
    // Cube Positions:
    //****************

    
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
    */
    
    // Check which arm has to move
    private bool moveLowerOrUpper;

    //Max Number of Steps to be performed before the environment restarts
    [Tooltip("Max Environment Steps")] public int MaxEnvironmentSteps = 100000;
    private int m_resetTimer;
    //Max Number of Skills to be performed before the environment restarts
    [Tooltip("Max Number of Skills")] public int MaxSkills = 15;
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
        black.transform.localPosition = new Vector3(1.22f,0.789f,-1.3663f);
        black.transform.rotation = Quaternion.Euler(rotationVector);
        green.transform.localPosition = new Vector3(1.219f,0.898f,-1.32f);
        green.transform.rotation = Quaternion.Euler(rotationVector);
        Rectangle.transform.localPosition = new Vector3(1.22f,0.763f,-1.184f);
        Rectangle.transform.rotation = Quaternion.Euler(rotationVector);
        blue.transform.localPosition = new Vector3(1.219f,0.8231f,-1.267f);
        blue.transform.rotation = Quaternion.Euler(rotationVector);
        yellow.transform.localPosition = new Vector3(1.2213f,0.8255f,-1.0598f);
        yellow.transform.rotation = Quaternion.Euler(rotationVector);
        red.transform.localPosition = new Vector3(1.221f,0.823f,-1.1674f);
        red.transform.rotation = Quaternion.Euler(rotationVector);

        m_resetTimer = 0;
        m_resetSkill = 0;
        
        //shapeBackup.CopyTo(taskArray, 0);
        //taskArray = (int[,]) shapeBackup.Clone(); // make a copy

        SetReward(0.0f);
        //checkAllDone = 0;

        pickup_blue = false;
        pickup_red = false;
        pickup_Rectangle = false;
        pickup_black = false;
        pickup_green = false;
        pickup_yellow = false;

    }

    /// <summary>
    /// Add relevant information on each body part to observations.
    /// </summary>
    public override void CollectObservations(VectorSensor sensor) //collect info needed to make decision
    {
        sensor.AddObservation(pickup_blue);
        sensor.AddObservation(pickup_red);
        sensor.AddObservation(pickup_Rectangle);
        sensor.AddObservation(pickup_black);
        sensor.AddObservation(pickup_green);
        sensor.AddObservation(pickup_yellow);
    }


    public override void OnActionReceived(ActionBuffers actionBuffers) //receives actions and assigns the reward
    {      
        Debug.Log("OnActionReceived");
        // Move the agent using the action.
        MoveAgent(actionBuffers.DiscreteActions);
        //AgentRewards(actionBuffers.DiscreteActions);

        // CHECK WHY USE THIS EVEN IN HEURISTIC MODE
    } 


    /// <summary>
    /// Function to call the discrete action selected 
    /// (while training or in heuristics mode)
    /// </summary>
    public void MoveAgent(ActionSegment<int> act)
    {

        count_collision_blue = 0;
        count_collision_red = 0;        
        count_collision_Rectangle = 0;
        count_collision_arm_link = 0;
        count_collision_black = 0;
        count_collision_green = 0;
        count_collision_yellow = 0;

        if(moveLowerOrUpper == true)
        {
            control.jointAnglesL.Clear();

            action = act[0];

            switch (action)
            {        
                case 0:
                    pickup_black = true;
                    control.currentIndexL = 0;
                    control.PickBlackLower();
                    break;
                case 1:
                    pickup_blue = true;
                    control.currentIndexL = 0;
                    control.PickBlueLower();
                    break;
                case 2:
                    pickup_green = true;
                    control.currentIndexL = 0;
                    control.PickGreenLower();
                    break;
                case 3: 
                    pickup_red = true;
                    control.currentIndexL = 0;
                    control.PickRedLower();
                    break;
                case 4:
                    pickup_Rectangle = true;
                    control.currentIndexL = 0;
                    control.PickWhiteLower();
                    break;
                default:
                    break;
            }
        }
        else if (moveLowerOrUpper == false)
        {
            control.jointAnglesU.Clear();

            action = act[1];
            switch (action)
            {        
                case 0:
                    pickup_black = true;
                    control.currentIndexU = 0;
                    control.PickBlackUpper();
                    break;
                case 1:
                    pickup_blue = true;
                    control.currentIndexU = 0;
                    control.PickBlueUpper();
                    break;
                case 2:
                    pickup_green = true;
                    control.currentIndexU = 0;
                    control.PickGreenUpper();
                    break;
                case 3: 
                    pickup_red = true;
                    control.currentIndexU = 0;
                    control.PickRedUpper();
                    break;
                case 4:
                    pickup_Rectangle = true;
                    control.currentIndexU = 0;
                    control.PickWhiteUpper();
                    break;
                case 5:
                    pickup_yellow = true;
                    control.currentIndexU = 0;
                    control.PickYellowUpper();
                    break;
                default:
                    break;
            }
        }
        m_resetSkill +=1; // Add +1 to Skills Counter  
        Debug.Log("Skill Number: " + m_resetSkill);
    }

    /*
    public void AgentRewards (ActionSegment<int> act)
    {
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
                checkAllDone = 2;
            }

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

        // foreach (int a in taskArray) 
        //   {
        //       Debug.Log(a);
        //   }
        
        // Update and show Cumulative Reward
        //reward = GetCumulativeReward();        
        //Debug.Log("CumulativeReward: " + reward);
    }*/

        // moveLowerOrUpper = true; Lower Arm moves
        // moveLowerOrUpper = false; Upper arm moves
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        
        // Debug.Log("Heuristic");
        
        var discreteActionsOut = actionsOut.DiscreteActions;

        if (Input.GetKey(KeyCode.A) && control.currentIndexL >= control.jointAnglesL.Count) // Select discrete action Lower 0 (black)
        {
            discreteActionsOut[0] = 0;
            Debug.Log("Key A Pressed");
            moveLowerOrUpper = true;
            MoveAgent(actionsOut.DiscreteActions);
            //AgentRewards(actionsOut.DiscreteActions);
        }
        else if (Input.GetKey(KeyCode.B) && control.currentIndexL >= control.jointAnglesL.Count) // Select discrete action Lower 1 (blue)
        {
            discreteActionsOut[0] = 1;
            Debug.Log("Key B Pressed");
            moveLowerOrUpper = true;
            MoveAgent(actionsOut.DiscreteActions);
            //AgentRewards(actionsOut.DiscreteActions);
        }
        else if (Input.GetKey(KeyCode.C) && control.currentIndexL >= control.jointAnglesL.Count) // Select discrete action Lower 2 (green)
        {
            discreteActionsOut[0] = 2;
            Debug.Log("Key C Pressed");
            moveLowerOrUpper = true;
            MoveAgent(actionsOut.DiscreteActions);
            //AgentRewards(actionsOut.DiscreteActions);
        }
        else if(Input.GetKey(KeyCode.D) && control.currentIndexL >= control.jointAnglesL.Count) // Select discrete action Lower 3 (red)
        {
            discreteActionsOut[0] = 3;
            Debug.Log("Key D Pressed");
            moveLowerOrUpper = true;
            MoveAgent(actionsOut.DiscreteActions);
            //AgentRewards(actionsOut.DiscreteActions); 
        }
        else if(Input.GetKey(KeyCode.E) && control.currentIndexL >= control.jointAnglesL.Count) // Select discrete action Lower 4 (Rectangle)
        {
            discreteActionsOut[0] = 4;
            Debug.Log("Key E Pressed");
            moveLowerOrUpper = true;
            MoveAgent(actionsOut.DiscreteActions);
            //AgentRewards(actionsOut.DiscreteActions);
        }
        else if(Input.GetKey(KeyCode.F) && control.currentIndexU >= control.jointAnglesU.Count) // Select discrete action Upper 0 (black)
        {
            discreteActionsOut[1] = 0;
            Debug.Log("Key F Pressed");
            moveLowerOrUpper = false;
            MoveAgent(actionsOut.DiscreteActions);
            //AgentRewards(actionsOut.DiscreteActions);
        }
        else if(Input.GetKey(KeyCode.G) && control.currentIndexU >= control.jointAnglesU.Count) // Select discrete action Upper 1 (blue)
        {
            discreteActionsOut[1] = 1;
            Debug.Log("Key G Pressed");
            moveLowerOrUpper = false;
            MoveAgent(actionsOut.DiscreteActions);
            //AgentRewards(actionsOut.DiscreteActions);
        }
        else if(Input.GetKey(KeyCode.H) && control.currentIndexU >= control.jointAnglesU.Count) // Select discrete action Upper 2 (green)
        {
            discreteActionsOut[1] = 2;
            Debug.Log("Key H Pressed");
            moveLowerOrUpper = false;
            MoveAgent(actionsOut.DiscreteActions);
            //AgentRewards(actionsOut.DiscreteActions);
        }
        else if(Input.GetKey(KeyCode.I) && control.currentIndexU >= control.jointAnglesU.Count) // Select discrete action Upper 3 (red)
        {
            discreteActionsOut[1] = 3;
            Debug.Log("Key I Pressed");
            moveLowerOrUpper = false;
            MoveAgent(actionsOut.DiscreteActions);
            //AgentRewards(actionsOut.DiscreteActions);
        }
        else if(Input.GetKey(KeyCode.J) && control.currentIndexU >= control.jointAnglesU.Count) // Select discrete action Upper 4 (Rectangle)
        {
            discreteActionsOut[1] = 4;
            Debug.Log("Key J Pressed");
            moveLowerOrUpper = false;
            MoveAgent(actionsOut.DiscreteActions);
            //AgentRewards(actionsOut.DiscreteActions);
        }
        else if(Input.GetKey(KeyCode.K) && control.currentIndexU >= control.jointAnglesU.Count) // Select discrete action Upper 5 (yellow)
        {
            discreteActionsOut[1] = 5;
            Debug.Log("Key K Pressed");
            moveLowerOrUpper = false;
            MoveAgent(actionsOut.DiscreteActions);
            //AgentRewards(actionsOut.DiscreteActions);
        }
        Debug.Log("discreteActionsOut Lower = " + discreteActionsOut[0]);
        Debug.Log("discreteActionsOut Upper = " + discreteActionsOut[1]);
    }	 


    void FixedUpdate()
    {
        // RESEARCH IF REQUESTING ACTIONS THIS WAY THE AGENT MAY BE LEARNING FROM HALF OF THE ACTIONS THAT ARE NOT DOING ANYTHING 
        // IF THAT IS THE CASE, TRY TO REQUEST ACTION FROM ONLY 1 OF THE DISCRETE ACTION BRANCHES
        
        /*
        if(control.currentIndexL >= control.jointAnglesL.Count && checkAllDone != 2)
        {
            moveLowerOrUpper = true;
            RequestDecision();
        }
        if(control.currentIndexU >= control.jointAnglesU.Count && checkAllDone != 2)
        {
            moveLowerOrUpper = false;
            RequestDecision();
        }
        if(checkAllDone == 2 && control.currentIndexU >= control.jointAnglesU.Count && control.currentIndexL >= control.jointAnglesL.Count)
        {
            EndEpisode();
        } */

        if(control.currentIndexL >= control.jointAnglesL.Count)
        {
            moveLowerOrUpper = true;
            RequestDecision();
        }
        if(control.currentIndexU >= control.jointAnglesU.Count)
        {
            moveLowerOrUpper = false;
            RequestDecision();
        }

        CollisionCallback.OnCollision += CollisionDetected;

        // Update Cumulative Reward
        reward = GetCumulativeReward();

        //SDebug.Log("Fixed Update");
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

        void CollisionDetected(Collision collision)
    {
        Debug.Log("Collision happened with: " + collision.gameObject.name);
        if (collision.gameObject.name == "green")
        {
            count_collision_green += 1;          
            
            if (count_collision_green == 1)
            {
                if (pickup_green == true)
                {
                    AddReward(1.0f);
                    Debug.Log("Good Reward for green");
                    Debug.Log("CumulativeReward " + reward);
                }
                else
                {
                    AddReward(-1.0f);
                    Debug.Log("Bad Reward for green");
                    Debug.Log("CumulativeReward " + reward);

                }    
            }
        }
        if (collision.gameObject.name == "red")
        {
            count_collision_red += 1;          
            
            if (count_collision_red == 1)
            {
                if (pickup_red == true)
                {
                    AddReward(1.0f);
                    Debug.Log("Good Reward for red");
                    Debug.Log("CumulativeReward " + reward);

                }
                else
                {
                    AddReward(-1.0f);
                    Debug.Log("Bad Reward for red");
                    Debug.Log("CumulativeReward " + reward);
                    
                }    
            }
        }

        if (collision.gameObject.name == "black")
        {
            count_collision_black += 1;          
            
            if (count_collision_black == 1)
            {
                if (pickup_black == true)
                {
                    AddReward(1.0f);
                    Debug.Log("Good Reward for black");
                    Debug.Log("CumulativeReward " + reward);

                }
                else
                {
                    AddReward(-1.0f);
                    Debug.Log("Bad Reward for black");
                    Debug.Log("CumulativeReward " + reward);
                    
                }    
            }
        }

        if (collision.gameObject.name == "blue")
        {
            count_collision_blue += 1;          
            
            if (count_collision_blue == 1)
            {
                if (pickup_blue == true)
                {
                    AddReward(1.0f);
                    Debug.Log("Good Reward for blue");
                    Debug.Log("CumulativeReward " + reward);

                }
                else
                {
                    AddReward(-1.0f);
                    Debug.Log("Bad Reward for blue");
                    Debug.Log("CumulativeReward " + reward);
                    
                }    
            }
        }

        if (collision.gameObject.name == "yellow")
        {
            count_collision_yellow += 1;          
            
            if (count_collision_yellow == 1)
            {
                if (pickup_yellow == true)
                {
                    AddReward(1.0f);
                    Debug.Log("Good Reward for yellow");
                    Debug.Log("CumulativeReward " + reward);

                }
                else
                {
                    AddReward(-1.0f);
                    Debug.Log("Bad Reward for yellow");
                    Debug.Log("CumulativeReward " + reward);
                    
                }    
            }
        }

        if (collision.gameObject.name == "Rectangle")
        {
            count_collision_Rectangle += 1;          
            
            if (count_collision_Rectangle == 1)
            {
                if (pickup_red == true && pickup_blue == true && pickup_yellow == true)
                {
                    AddReward(1.0f);
                    Debug.Log("Good Reward for Rectangle");
                    Debug.Log("CumulativeReward " + reward);

                }
                else
                {
                    AddReward(-1.0f);
                    Debug.Log("Bad Reward for Rectangle");
                    Debug.Log("CumulativeReward " + reward);
                    
                }    
            }
        }

        
        if (collision.gameObject.name == "duarolower_link_j3")
        {
            count_collision_arm_link += 1;          
            
            if (count_collision_arm_link == 1)
            {
                AddReward(-5.0f);
                Debug.Log("Bad Reward for collision of arms");
                Debug.Log("CumulativeReward " + reward);
                EndEpisode();

            }
        } 
    
    }
}



