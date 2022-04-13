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
    private int[] skills_array; // order: blue, red, rectangle 

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
        int[] skills_array = {1,1,1}; // order: blue, red, rectangle 

    }

    /// <summary>
    /// Add relevant information on each body part to observations.
    /// </summary>

    public override void CollectObservations(VectorSensor sensor) //collect info needed to make decision
    {

    if (skills_array[0] == 1 && skills_array[1] == 1 && skills_array[2] == 1)
    {
        Random rnd = new Random();
        int random_index = rnd.Next(skills_array.Length);
        skills_array[random_index] = 0;    
    }
    else if (skills_array[0] == 0 && skills_array[1] == 0 && skills_array[2] == 0)    
    {
        // end episode after this? 
        // EndEpisode();

    }
    else if (skills_array[0] == 1 && skills_array[1] == 1 && skills_array[2] == 1)
    {

    }

    
    if (skills_array[0] == 0)
    {
        pickup_blue = true;
    }
    else if (skills_array[1] == 0)
    {
        pickup_red = true;
    }
    }
  

    public override void OnActionReceived(ActionBuffers actionBuffers) //receives actions and assigns the reward
    {

    // tbd: add, which script to start
    // ControlAgent.Start();

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
        Debug.Log("Collision happened with: " + collision.gameObject.name);

        // Reward depending on collision
        if (collision.gameObject.name == "red")
        {
            if (pickup_red == true)
            {
            AddReward(2.0f);
            Debug.Log("Good Reward");
            EndEpisode();
            }
            else
            {
            SetReward(-1.0f);
            Debug.Log("Bad Reward");
            EndEpisode();                
            }

        }

        if (collision.gameObject.name == "blue")
        {
            if (pickup_blue == true)
            {
            SetReward(2.0f);
            Debug.Log("Good Reward");
            EndEpisode();
            }
            else
            {
            SetReward(-1.0f);
            Debug.Log("Bad Reward");
            EndEpisode();                
            }

        }
    }




}
