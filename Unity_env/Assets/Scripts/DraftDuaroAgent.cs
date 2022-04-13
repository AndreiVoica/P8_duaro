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

    // public Control ControlAgent; 

    // public Transform Target; //Target the agent will try to touch during training.

    [Tooltip("Max Environment Steps")] public int MaxEnvironmentSteps = 2000;
    private int m_resetTimer;


    public override void OnEpisodeBegin() //set-up the environment for a new episode
    {
        // reset cube positions:
        Vector3 rotationVector = new Vector3(0, 0, 0);

        blue.transform.position = new Vector3(1.22300005f,0.823099971f,-1.32130003f);
        blue.transform.rotation = Quaternion.Euler(rotationVector);
        red.transform.position = new Vector3(1.22500002f,0.823000014f,-1.17999995f);
        red.transform.rotation = Quaternion.Euler(rotationVector);
        rectangle.transform.position = new Vector3(1.22000003f,0.740999997f,-1.23800004f);
        rectangle.transform.rotation = Quaternion.Euler(rotationVector);
        pickup_blue = false;
        pickup_red = false;

    }

    /// <summary>
    /// Add relevant information on each body part to observations.
    /// </summary>

    public override void Heuristic(in ActionBuffers actionsOut){
		ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
		if (Input.GetKeyDown(KeyCode.Q))
                	discreteActions[0] = 0;
            	if (Input.GetKeyDown(KeyCode.W))
                	discreteActions[0] = 1;
            	if (Input.GetKeyDown(KeyCode.A))
                	discreteActions[0] = 2;
	}

    public override void CollectObservations(VectorSensor sensor) //collect info needed to make decision
    {


    }
  

    public override void OnActionReceived(ActionBuffers actions) //receives actions and assigns the reward
    {      
        int decision = actions.DiscreteActions[0];
        var skill = 4;

        // tbd:
        switch (decision)
        {        
        case 0:
            count_collision_blue = 0;
            count_collision_red = 0;        
            count_collision_rectangle = 0;
            pickup_blue = true;
            skill = 0; // blue
            break;
        case 1:
            count_collision_blue = 0;
            count_collision_red = 0;        
            count_collision_rectangle = 0;
            pickup_red = true;
            skill = 1; // red
            break;
        case 2:
            count_collision_blue = 0;
            count_collision_red = 0;        
            count_collision_rectangle = 0;
            pickup_rectangle = true;
            skill = 2; // rectangle
            break;
        }
        // ControlAgent.Start(); start the skill 0, 1 or 2
        if (pickup_blue && pickup_red && pickup_rectangle)
        {
            AddReward(2.0f);
            Debug.Log("Good Reward for ending the task");
            EndEpisode();
        }
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


    // Collision detection:
    protected override void OnEnable()
    {
        // Register to OnCollision event
        CollisionCallback.OnCollision += CollisionDetected;
    }

    protected override void OnDisable()
    {
        // Un-Register to OnCollision event
        CollisionCallback.OnCollision -= CollisionDetected;
    }


    // When collision happens:
    void CollisionDetected(Collision collision)
    {
        // Debug.Log("Collision happened with: " + collision.gameObject.name);

       
        if (collision.gameObject.name == "blue")
        {
            count_collision_blue += 1;          
            
            if (count_collision_blue == 1)
            {
                if (pickup_blue == true)
                {
                    SetReward(2.0f);
                    Debug.Log("Good Reward for blue");
                    EndEpisode();
                }
                else
                {
                    SetReward(-1.0f);
                    Debug.Log("Bad Reward for blue");
                    EndEpisode();                
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
                    SetReward(2.0f);
                    Debug.Log("Good Reward for red");
                    EndEpisode();
                }
                else
                {
                    SetReward(-1.0f);
                    Debug.Log("Bad Reward for red");
                    EndEpisode();                
                }    
            }
        }

        if (collision.gameObject.name == "Rectangle")
        {
            count_collision_rectangle += 1;          
            
            if (count_collision_rectangle == 1)
            {
                if (pickup_red == true && pickup_blue == true)
                {
                    SetReward(2.0f);
                    Debug.Log("Good Reward for rectangle");
                    EndEpisode();
                }
                else
                {
                    SetReward(-1.0f);
                    Debug.Log("Bad Reward for rectangle");
                    EndEpisode();                
                }    
            }
        }

    }




}
