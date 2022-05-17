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
using System.Threading.Tasks;


public class DuaroAgentComplexWT : Agent
{
    public Transform black;
    public Transform green;
    public Transform Rectangle;
    public Transform yellow;
    public Transform blue;
    public Transform red;
    public Transform DuaroAgent;

    private Control control;
    private Library robot;

    public int action;

    //*************************
    // For Collision of Arms:
    //*************************
    int count_collision_arm_link;
    int arm_collision;

    //*************************
    // Cube Positions:
    //*************************
    
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

    private int checkAllDone = 0;
    
    // Check which arm has to move
    private bool moveLowerOrUpper = true;

    // Max Number of Steps to be performed before the environment restarts
    [Tooltip("Max Environment Steps")] public int MaxEnvironmentSteps = 10000;
    public int m_resetTimer;
    // Max Number of Skills to be performed before the environment restarts
    [Tooltip("Max Number of Skills")] public int MaxSkills = 15;
    private int m_resetSkill;

    // Timer variable for the waiting action
    float waitingTimerLower;
    float waitingTimerUpper;
    float waitingTimer;
    int waitTime = 250; // 50 steps = 1s

    // Variable to store the cumulative Reward
    float reward;

    // Variable to Update the taskArray when an action has been completed
    public int skillLower = -1;
    public int skillUpper = -1;
    public bool lowerSkillReward;
    public bool upperSkillReward;

    public override void Initialize()
    {
        control = FindObjectOfType<Control>();
        robot = FindObjectOfType<Library>();

        robot.set_lower_joint_target(-45f, 45f, 0f, 0f, 0.055f, -0.055f); //Set the lower joint values to a home position
        robot.set_upper_joint_target(45f, -45f, 0f, 0f, 0.055f, -0.055f); //Set the upper joint values to a home position
    }

    public override void OnEpisodeBegin() //set-up the environment for a new episode
    {
        // Reset cube positions:
        Vector3 rotationVector = new Vector3(0, 0, 0);
        Vector3 rotationVectorDuaro = new Vector3(0, 180, 0);
        Debug.Log("OnEpisodeBegin");

        robot.set_lower_joint_target(-45f, 45f, 0.15f, 0f, 0.055f, -0.055f); //Set the lower joint values to a home position
        robot.set_upper_joint_target(45f, -45f, 0.15f, 0f, 0.055f, -0.055f); //Set the upper joint values to a home position

        DuaroAgent.position = new Vector3(-0.6622652f, 0.0f, 0.0f);
        DuaroAgent.rotation = Quaternion.Euler(rotationVectorDuaro);
        transform.Find("world/duarobase_link").GetComponent<ArticulationBody>().TeleportRoot(DuaroAgent.position, DuaroAgent.rotation);

        black.transform.localPosition = new Vector3(1.22f,0.774f,-1.3663f);
        black.transform.rotation = Quaternion.Euler(rotationVector);
        green.transform.localPosition = new Vector3(1.219f,0.856f,-1.316f);
        green.transform.rotation = Quaternion.Euler(rotationVector);
        Rectangle.transform.localPosition = new Vector3(1.22f,0.748f,-1.184f);
        Rectangle.transform.rotation = Quaternion.Euler(rotationVector);
        blue.transform.localPosition = new Vector3(1.219f,0.804f,-1.267f);
        blue.transform.rotation = Quaternion.Euler(rotationVector);
        yellow.transform.localPosition = new Vector3(1.2213f,0.804f,-1.0598f);
        yellow.transform.rotation = Quaternion.Euler(rotationVector);
        red.transform.localPosition = new Vector3(1.221f,0.823f,-1.1674f);
        red.transform.rotation = Quaternion.Euler(rotationVector);

        m_resetTimer = 0;
        m_resetSkill = 0;

        waitingTimerLower = -waitTime;
        waitingTimerUpper = -waitTime;
        waitingTimer = 0.0f;
        
        //shapeBackup.CopyTo(taskArray, 0);
        taskArray = (int[,]) shapeBackup.Clone(); // make a copy

        //SetReward(0.0f);
        checkAllDone = 0;
        
        // Arm Collision observation
        arm_collision = 0;
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
        sensor.AddObservation(arm_collision);
    }

    //Action Mask
    public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
    {
        if (moveLowerOrUpper == true && control.currentIndexL >= control.jointAnglesL.Count) // Lower Arm (Disabled Upper)
        {
            //Debug.Log("Mask Lower");
            actionMask.SetActionEnabled(0, 5, false);
            actionMask.SetActionEnabled(0, 6, false);
            actionMask.SetActionEnabled(0, 7, false);
            actionMask.SetActionEnabled(0, 8, false);
            actionMask.SetActionEnabled(0, 9, false);
            actionMask.SetActionEnabled(0, 10, false);
            actionMask.SetActionEnabled(0, 12, false);
        }
        else if (moveLowerOrUpper == false && control.currentIndexU >= control.jointAnglesU.Count) // Upper Arm (Disabled lower)
        {
            //Debug.Log("Mask Upper");
            actionMask.SetActionEnabled(0, 0, false);
            actionMask.SetActionEnabled(0, 1, false);
            actionMask.SetActionEnabled(0, 2, false);
            actionMask.SetActionEnabled(0, 3, false);
            actionMask.SetActionEnabled(0, 4, false);
            actionMask.SetActionEnabled(0, 11, false);
        }
    }

    public override void OnActionReceived(ActionBuffers actionBuffers) //receives actions and assigns the reward
    {      
        //Debug.Log("OnActionReceived");
        // Move the agent using the action.
        MoveAgent(actionBuffers.DiscreteActions);
        AgentRewards(actionBuffers.DiscreteActions);

        // CHECK WHY USE THIS EVEN IN HEURISTIC MODE
    } 

    /// <summary>
    /// Function to call the discrete action selected 
    /// (while training or in heuristics mode)
    /// </summary>
    public void MoveAgent(ActionSegment<int> act)
    {
        var decision = act[0];
        //Debug.Log("Decision = " + decision + " // move Lower or Upper = " + moveLowerOrUpper);

        // For Collision of Arms:
        count_collision_arm_link = 0;

        m_resetSkill +=1; // Add +1 to Skills Counter  CHANGE TO WHEN THE ACTION ENDS
        //Debug.Log("Skill Number: " + m_resetSkill);

        if(moveLowerOrUpper == true) // Lower Arm
        {
            control.jointAnglesL.Clear();
            action = act[0];
            skillLower = action;
        }
        else if (moveLowerOrUpper == false) // Upper Arm
        {
            control.jointAnglesU.Clear();
            action = act[0] - 5;
            skillUpper = action;
        }

        switch (decision)
        {        
            case 0:
                control.currentIndexL = 0;
                control.PickBlackLower();
                break;
            case 1:
                control.currentIndexL = 0;
                control.PickBlueLower();
                break;
            case 2:
                control.currentIndexL = 0;
                control.PickGreenLower();
                break;
            case 3: 
                control.currentIndexL = 0;
                control.PickRedLower();
                break;
            case 4:
                control.currentIndexL = 0;
                control.PickWhiteLower();
                break;
            case 5:
                control.currentIndexU = 0;
                control.PickBlackUpper();
                break;
            case 6:
                control.currentIndexU = 0;
                control.PickBlueUpper();
                break;
            case 7:
                control.currentIndexU = 0;
                control.PickGreenUpper();
                break;
            case 8: 
                control.currentIndexU = 0;
                control.PickRedUpper();
                break;
            case 9:
                control.currentIndexU = 0;
                control.PickWhiteUpper();
                break;
            case 10:
                control.currentIndexU = 0;
                control.PickYellowUpper();
                break;
            case 11:
                control.currentIndexL = 0;
                waitingTimerLower = waitingTimer;
                Debug.Log("Waiting " + waitTime/50 + " Seconds - Lower Arm");
                AddReward(-2.5f);
                break;
            case 12:
                control.currentIndexU = 0;
                waitingTimerUpper = waitingTimer;
                Debug.Log("Waiting " + waitTime/50 + " Seconds - Upper Arm");
                AddReward(-2.5f);
                break;
            default:
                break;
        }
    }

    public void AgentRewards (ActionSegment<int> act)
    {
        // Debug.Log("Agent Rewards");

        // Rewards
        Debug.Log("Action = " + action);

        if (act[0] < 11) // Don't assign rewards for waiting actions
        {
            // Check if there are items above
            itemsAbove = false;
            for (int i = itemsPosition[action,1]; i < itemsPosition[action,1] + itemsPosition[action,3]; i++)
            {
                if (taskArray[itemsPosition[action,0] - 1, i] == 1)
                {
                    itemsAbove = true; // There are some objects above this part
                }
            }

            if (taskArray[itemsPosition[action,0], itemsPosition[action,1]] == 1 && itemsAbove == false)
            {
                if(moveLowerOrUpper == true && skillLower != skillUpper)
                {
                    lowerSkillReward = true;
                }
                else if(moveLowerOrUpper == false && skillLower != skillUpper)
                {
                    upperSkillReward = true;
                }
                else if(skillLower == skillUpper)
                {
                    AddReward(-5.0f);
                    Debug.Log("Add Negative Reward - Choosing the same action at the same time (-5)");
                }
                // Update items Matrix Space
            }
            // Penalize choosing same action again
            else if(taskArray[itemsPosition[action,0], itemsPosition[action,1]] == 0)
            { 
                AddReward(-5.0f);
                Debug.Log("Add Negative Reward - Choosing the same action (-5)");
            }
            // Penalize picking up a block with something above
            if(itemsAbove == true)
            { 
                AddReward(-10.0f);
                Debug.Log("Add Negative Reward - There is something above (-10) - Restarting Environment");
                control.currentIndexU = 1000;
                control.currentIndexL = 1000;
                EndEpisode();
            } 
        }
    }


    void FixedUpdate()
    {
        // Check if an action has finished to request another action
        if(checkAllDone != 2 && control.currentIndexL >= control.jointAnglesL.Count && waitingTimer >= waitingTimerLower + waitTime)
        {
            moveLowerOrUpper = true;
            RequestDecision();
        }
        else if(checkAllDone != 2 && control.currentIndexU >= control.jointAnglesU.Count && waitingTimer >= waitingTimerUpper + waitTime)
        {
            moveLowerOrUpper = false;
            RequestDecision();
        }

        // Check if the task has been completed
        if(checkAllDone == 2)
        {
            Debug.Log("RESTARTING ENVIRONMENT -- TASK COMPLETED SUCCESFULLY");
            control.currentIndexU = 1000;
            control.currentIndexL = 1000;
            EndEpisode();
        }

        // Check if an action has finished to assign the reward and Update the space matrix
        if (upperSkillReward == true &&  control.currentIndexU >= control.jointAnglesU.Count)
        {   
            AddReward(10.0f);
            Debug.Log("Add Reward - Upper Arm (+10)");
            upperSkillReward = false;
            UpdateMatrix(skillUpper);
        }
        else if (lowerSkillReward == true && control.currentIndexL >= control.jointAnglesL.Count)
        {
            AddReward(10.0f);
            Debug.Log("Add Reward - Lower Arm (+10)");
            lowerSkillReward = false;
            UpdateMatrix(skillLower);
        }

        // Update Cumulative Reward
        reward = GetCumulativeReward();

        waitingTimer +=1.0f;

        m_resetTimer += 1; // Add +1 to Steps Counter
        if (m_resetTimer >= MaxEnvironmentSteps && MaxEnvironmentSteps > 0)
        {
            Debug.Log("Restarting Scene from Fixed Update (Max Environment Steps)");
            control.currentIndexU = 1000;
            control.currentIndexL = 1000;
            EndEpisode();
        }
        else if(m_resetSkill > MaxSkills)
        {
            Debug.Log("Restarting Scene from Fixed Update (Max Number of Skills)");
            control.currentIndexU = 1000;
            control.currentIndexL = 1000;
            EndEpisode();
            AddReward(-10.0f);
            Debug.Log("Add Negative Reward - MAX SKILLS REACHED (-10)");
        }

        // For Collision of Arms (Comment that out, if running without collision checking)
        CollisionCallback.OnCollision += CollisionDetected;
    }

    void UpdateMatrix(int skillCompleted)
    {
        for (int i = itemsPosition[skillCompleted,1]; i < (itemsPosition[skillCompleted,1] + itemsPosition[skillCompleted,3]); i++)
        {
            for (int j = itemsPosition[skillCompleted,0]; j < (itemsPosition[skillCompleted,0] + itemsPosition[skillCompleted,2]); j++)
            {
                taskArray[j,i] = 0;
                //Debug.Log("Cleaning");
            }
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
            AddReward(40.0f);
            Debug.Log("TASK COMPLETED (+40)!");
            checkAllDone = 2; 
        }

        // foreach (int a in taskArray) 
        // {
        //     Debug.Log(a);
        // }
    }

    void CollisionDetected(Collision collision)
    {
        if (collision.gameObject.name == "duarolower_link_j3")
        {
            count_collision_arm_link += 1;          
            
            if (count_collision_arm_link == 1)
            {
                arm_collision = 1;
                AddReward(-10.0f);
                Debug.Log("Bad Reward for collision of arms (-10)");
                Debug.Log("CumulativeReward " + reward);

                control.currentIndexU = 1000;
                control.currentIndexL = 1000;

                EndEpisode();
            } 
        }
    }




    //****************************************************************************************************************
    //**********************HEURISTICS********************************************************************************
    //****************************************************************************************************************

    // moveLowerOrUpper = true; Lower Arm moves
    // moveLowerOrUpper = false; Upper arm moves
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // Debug.Log("Heuristic");
        
        var discreteActionsOut = actionsOut.DiscreteActions;

        if (Input.GetKey(KeyCode.A) && control.currentIndexL >= control.jointAnglesL.Count && waitingTimer >= waitingTimerLower + waitTime) // Select discrete action Lower 0
        {
            discreteActionsOut[0] = 0;
            Debug.Log("Key A Pressed");
            moveLowerOrUpper = true;
            MoveAgent(actionsOut.DiscreteActions);
            AgentRewards(actionsOut.DiscreteActions);
        }
        else if (Input.GetKey(KeyCode.B) && control.currentIndexL >= control.jointAnglesL.Count && waitingTimer >= waitingTimerLower + waitTime) // Select discrete action Lower 1
        {
            discreteActionsOut[0] = 1;
            Debug.Log("Key B Pressed");
            moveLowerOrUpper = true;
            MoveAgent(actionsOut.DiscreteActions);
            AgentRewards(actionsOut.DiscreteActions);
        }
        else if (Input.GetKey(KeyCode.C) && control.currentIndexL >= control.jointAnglesL.Count && waitingTimer >= waitingTimerLower + waitTime) // Select discrete action Lower 2
        {
            discreteActionsOut[0] = 2;
            Debug.Log("Key C Pressed");
            moveLowerOrUpper = true;
            MoveAgent(actionsOut.DiscreteActions);
            AgentRewards(actionsOut.DiscreteActions);
        }
        else if(Input.GetKey(KeyCode.D) && control.currentIndexL >= control.jointAnglesL.Count && waitingTimer >= waitingTimerLower + waitTime) // Select discrete action Lower 3
        {
            discreteActionsOut[0] = 3;
            Debug.Log("Key D Pressed");
            moveLowerOrUpper = true;
            MoveAgent(actionsOut.DiscreteActions);
            AgentRewards(actionsOut.DiscreteActions); 
        }
        else if(Input.GetKey(KeyCode.E) && control.currentIndexL >= control.jointAnglesL.Count && waitingTimer >= waitingTimerLower + waitTime) // Select discrete action Lower 4
        {
            discreteActionsOut[0] = 4;
            Debug.Log("Key E Pressed");
            moveLowerOrUpper = true;
            MoveAgent(actionsOut.DiscreteActions);
            AgentRewards(actionsOut.DiscreteActions);
        }
        else if(Input.GetKey(KeyCode.F) && control.currentIndexU >= control.jointAnglesU.Count && waitingTimer >= waitingTimerUpper + waitTime) // Select discrete action Upper 0
        {
            discreteActionsOut[0] = 5;
            Debug.Log("Key F Pressed");
            moveLowerOrUpper = false;
            MoveAgent(actionsOut.DiscreteActions);
            AgentRewards(actionsOut.DiscreteActions);
        }
        else if(Input.GetKey(KeyCode.G) && control.currentIndexU >= control.jointAnglesU.Count && waitingTimer >= waitingTimerUpper + waitTime) // Select discrete action Upper 1
        {
            discreteActionsOut[0] = 6;
            Debug.Log("Key G Pressed");
            moveLowerOrUpper = false;
            MoveAgent(actionsOut.DiscreteActions);
            AgentRewards(actionsOut.DiscreteActions);
        }
        else if(Input.GetKey(KeyCode.H) && control.currentIndexU >= control.jointAnglesU.Count && waitingTimer >= waitingTimerUpper + waitTime) // Select discrete action Upper 2
        {
            discreteActionsOut[0] = 7;
            Debug.Log("Key H Pressed");
            moveLowerOrUpper = false;
            MoveAgent(actionsOut.DiscreteActions);
            AgentRewards(actionsOut.DiscreteActions);
        }
        else if(Input.GetKey(KeyCode.I) && control.currentIndexU >= control.jointAnglesU.Count && waitingTimer >= waitingTimerUpper + waitTime) // Select discrete action Upper 3
        {
            discreteActionsOut[0] = 8;
            Debug.Log("Key I Pressed");
            moveLowerOrUpper = false;
            MoveAgent(actionsOut.DiscreteActions);
            AgentRewards(actionsOut.DiscreteActions);
        }
        else if(Input.GetKey(KeyCode.J) && control.currentIndexU >= control.jointAnglesU.Count && waitingTimer >= waitingTimerUpper + waitTime) // Select discrete action Upper 4
        {
            discreteActionsOut[0] = 9;
            Debug.Log("Key J Pressed");
            moveLowerOrUpper = false;
            MoveAgent(actionsOut.DiscreteActions);
            AgentRewards(actionsOut.DiscreteActions);
        }
        else if(Input.GetKey(KeyCode.K) && control.currentIndexU >= control.jointAnglesU.Count && waitingTimer >= waitingTimerUpper + waitTime) // Select discrete action Upper 5
        {
            discreteActionsOut[0] = 10;
            Debug.Log("Key K Pressed");
            moveLowerOrUpper = false;
            MoveAgent(actionsOut.DiscreteActions);
            AgentRewards(actionsOut.DiscreteActions);
        }
        else if(Input.GetKey(KeyCode.L) && control.currentIndexL >= control.jointAnglesL.Count && waitingTimer >= waitingTimerLower + waitTime) // Select discrete action Lower 5
        {
            discreteActionsOut[0] = 11;
            Debug.Log("Key L Pressed");
            moveLowerOrUpper = true;
            MoveAgent(actionsOut.DiscreteActions);
            //AgentRewards(actionsOut.DiscreteActions);
        }
        else if(Input.GetKey(KeyCode.M) && control.currentIndexU >= control.jointAnglesU.Count && waitingTimer >= waitingTimerUpper + waitTime) // Select discrete action Upper 6
        {
            discreteActionsOut[0] = 12;
            Debug.Log("Key M Pressed");
            moveLowerOrUpper = false;
            MoveAgent(actionsOut.DiscreteActions);
            //AgentRewards(actionsOut.DiscreteActions);
        }
        else if(Input.GetKey(KeyCode.N)) // Restart Episode
        {
            Debug.Log("Key N Pressed -- Restarting Episode");
            control.currentIndexU = 1000;
            control.currentIndexL = 1000;
            EndEpisode();
        }
    }	 
}





