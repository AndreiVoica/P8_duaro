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



    // public Control ControlAgent; 

    // public Transform Target; //Target the agent will try to touch during training.

    [Tooltip("Max Environment Steps")] public int MaxEnvironmentSteps = 50000;
    private int m_resetTimer;


    public override void Initialize()
    {
        control = FindObjectOfType<Control>();
    }

    public override void OnEpisodeBegin() //set-up the environment for a new episode
    {
        // reset cube positions:
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
        
    }

    /// <summary>
    /// Add relevant information on each body part to observations.
    /// </summary>

    public override void CollectObservations(VectorSensor sensor) //collect info needed to make decision
    {
        sensor.AddObservation(blue.position);
    }

    // public void MoveAgent(ActionSegment<int> act)
    // {
    //     //int decision = actionBuffers.DiscreteActions[0];
    //     var decision = act[0];
    //    // decision = 0;
    //     var skill = 4;
    //     Debug.Log("test decision: " + decision);
    //     // tbd:
    //     switch (decision)
    //     {        
    //     case 0:
    //         count_collision_blue = 0;
    //         count_collision_red = 0;        
    //         count_collision_rectangle = 0;
    //         pickup_blue = true;
    //         control.PickBlue();
    //         break;
    //     case 1:
    //         count_collision_blue = 0;
    //         count_collision_red = 0;        
    //         count_collision_rectangle = 0;
    //         pickup_red = true;
    //         control.PickRed();
    //         break;
    //     case 2:
    //         count_collision_blue = 0;
    //         count_collision_red = 0;        
    //         count_collision_rectangle = 0;
    //         pickup_rectangle = true;
    //         control.PickWhite();
    //         break;
    //     default:
    //         break;
    //     }
    //     // ControlAgent.Start(); start the skill 0, 1 or 2
    //     if (pickup_blue && pickup_red && pickup_rectangle)
    //     {
    //         AddReward(10.0f);
    //         Debug.Log("Good Reward for ending the task");
    //         EndEpisode();
    //     }
    // }

    public override void OnActionReceived(ActionBuffers actionBuffers) //receives actions and assigns the reward
    {      
        // Move the agent using the action.
        //MoveAgent(actionBuffers.DiscreteActions);
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

       public override void Heuristic(in ActionBuffers actionsOut)
    {
        Debug.Log("Heuristic");
        var discreteActionsOut = actionsOut.DiscreteActions;
        if (Input.GetKey(KeyCode.A))
        {
            discreteActionsOut[0] = 0;
            Debug.Log("Key A Pressed");
            control.PickBlue();
        }
        else if (Input.GetKey(KeyCode.B))
        {
            discreteActionsOut[0] = 1;
            Debug.Log("Key B Pressed");
            control.PickRed();
        }
        else if (Input.GetKey(KeyCode.C))
        {
            discreteActionsOut[0] = 2;
            Debug.Log("Key C Pressed");
            control.PickWhite();
        }

        Debug.Log("discreteActionsOut = " + discreteActionsOut[0]);
    }	 


    void FixedUpdate()
    {
        m_resetTimer += 1;
        if (m_resetTimer >= MaxEnvironmentSteps && MaxEnvironmentSteps > 0)
        {
            Debug.Log("Restarting Scene");
            //SetReward(MaxEnvironmentSteps* - 0.000001f);
            m_resetTimer = 0;
            EndEpisode();
        }
    }

}
