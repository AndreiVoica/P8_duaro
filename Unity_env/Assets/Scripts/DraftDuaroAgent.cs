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

    int count_collision_blue;
    int count_collision_red;        
    int count_collision_rectangle;
    private Control control;

    // Position of the BLUE cube in the Array
    int blueRow = 1;
    int blueCol = 0;
    // Position of the RED cube in the Array
    int redRow = 1;
    int redCol = 3;
    // Position of the RECTANGLE cube in the Array
    int rectangleRow = 2;
    int[,] rectangleCol = new int[1,4];
    private bool rectanglePos;

    // Array with the shape of the blocks
    int[,] taskArray = new int[3,4]{ 
                                    {0,0,0,0},
                                    {1,0,0,1}, 
                                    {1,1,1,1}, 
                                };

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
        blue.transform.localPosition = new Vector3(1.22300005f,0.823099971f,-1.32130003f);
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

    public override void OnActionReceived(ActionBuffers actionBuffers) //receives actions and assigns the reward
    {      
        Debug.Log("OnActionReceived");
        // Move the agent using the action.
        //MoveAgent(actionBuffers.DiscreteActions);
        // CHECK WHY USE THIS EVEN IN HEURISTIC MODE
    }


    /// <summary>
    /// Function to call the discrete action selected 
    /// (while training or in heuristics mode)
    /// </summary>
    public void MoveAgent(ActionSegment<int> act)
    {
        var decision = act[0];
       // decision = 0;
        var skill = 4;
        Debug.Log("test decision: " + decision);
        // tbd:
        count_collision_blue = 0;
        count_collision_red = 0;        
        count_collision_rectangle = 0;
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
              Debug.Log("Skill = " + m_resetSkill);

    }


    /// <summary>
    /// Assign rewards when choosing a correct action
    /// </summary>
    public void AgentRewards (ActionSegment<int> act)
    {
        // Debug.Log("Agent Rewards");

        var action = act[0];

        // Rewards
        if (action == 0); // Picking BLUE cube
        {
            if (pickup_blue == true && taskArray[blueRow,blueCol] == 1 && taskArray[blueRow - 1,blueCol] == 0)
            {
                AddReward(1.0f);
                taskArray[blueRow,blueCol] = 0;
                Debug.Log("Add Reward BLUE");
            }
            // Penalize choosing same action again
            else if (pickup_blue == true && taskArray[blueRow,blueCol] == 0)
            { 
                AddReward(-1.0f);
                Debug.Log("Add Negative Reward BLUE");
            }
        }
        if (action == 1); // Picking RED cube
        {
            if (pickup_red == true && taskArray[redRow,redCol] == 1 && taskArray[redRow - 1,redCol] == 0)
            {
                AddReward(1.0f);
                taskArray[redRow,redCol] = 0;
                Debug.Log("Add Reward RED");
            }
            // Penalize choosing same action again
            else if (pickup_red == true && taskArray[redRow,redCol] == 0)
            { 
                AddReward(-1.0f);
                Debug.Log("Add Negative Reward RED");
            }
        }   
        if (action == 2); // Picking RECTANGLE
        {
            rectanglePos = false;
            for (int i = 0; i < rectangleCol.Length; i++)
            {
                if (taskArray[rectangleRow,rectangleCol[0,i]] == 1 && taskArray[rectangleRow - 1,rectangleCol[0,i]] == 1)
                {
                    rectanglePos = true; // There are some objects above this part
                }
                
            }

            if (pickup_rectangle == true && rectanglePos == false)
            {
                AddReward(1.0f);

                for (int i = 0; i < rectangleCol.Length; i++)
                {
                    taskArray[rectangleRow,rectangleCol[0,i]] = 0;
                }

                Debug.Log("Add Reward RECTANGLE");
            }
            // Penalize choosing same action again
            else if (pickup_rectangle == true && taskArray[rectangleRow,rectangleCol[0,0]] == 0)
            { 
                AddReward(-1.0f);
                Debug.Log("Add Negative Reward RECTANGLE - Choosing the same action");
            }
            // Penalize picking up the rectangle while there is something above
            else if (pickup_rectangle == true && rectanglePos == true)
            { 
                AddReward(-1.0f);
                Debug.Log("Add Negative Reward RECTANGLE - There is something above");
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



    // // Collision detection:
    // protected override void OnEnablev()
    // {
    //     // Register to OnCollision event
    //     CollisionCallback.OnCollision += CollisionDetected;
    // }

    // protected override void OnDisable()
    // {
    //     // Un-Register to OnCollision eent
    //     CollisionCallback.OnCollision -= CollisionDetected;
    // }


    // // When collision happens:
    // void CollisionDetected(Collision collision)
    // {
    //     // Debug.Log("Collision happened with: " + collision.gameObject.name);

       
    //     if (collision.gameObject.name == "blue")
    //     {
    //         count_collision_blue += 1;          
            
    //         if (count_collision_blue == 1)
    //         {
    //             if (pickup_blue == true)
    //             {
    //                 SetReward(2.0f);
    //                 Debug.Log("Good Reward for blue");
    //                 EndEpisode();
    //             }
    //             else
    //             {
    //                 SetReward(-1.0f);
    //                 Debug.Log("Bad Reward for blue");
    //                 EndEpisode();                
    //             }    
    //         }
    //     }

    //     if (collision.gameObject.name == "red")
    //     {
    //         count_collision_red += 1;          
            
    //         if (count_collision_red == 1)
    //         {
    //             if (pickup_red == true)
    //             {
    //                 SetReward(2.0f);
    //                 Debug.Log("Good Reward for red");
    //                 EndEpisode();
    //             }
    //             else
    //             {
    //                 SetReward(-1.0f);
    //                 Debug.Log("Bad Reward for red");
    //                 EndEpisode();                
    //             }    
    //         }
    //     }

    //     if (collision.gameObject.name == "Rectangle")
    //     {
    //         count_collision_rectangle += 1;          
            
    //         if (count_collision_rectangle == 1)
    //         {
    //             if (pickup_red == true && pickup_blue == true)
    //             {
    //                 SetReward(2.0f);
    //                 Debug.Log("Good Reward for rectangle");
    //                 EndEpisode();
    //             }
    //             else
    //             {
    //                 SetReward(-1.0f);
    //                 Debug.Log("Bad Reward for rectangle");
    //                 EndEpisode();                
    //             }    
    //         }
    //     }
    //
    //   }

}
