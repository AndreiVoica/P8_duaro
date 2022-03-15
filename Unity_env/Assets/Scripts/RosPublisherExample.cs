using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.UnityRoboticsDemo;

/// <summary>
/// 
/// </summary>
public class khi_duaro : MonoBehaviour
{
    ROSConnection ros;
    public string topicName = "pos_rot";

    // The game object 
    public GameObject khiduaro;
    // Publish the cube's position and rotation every N seconds
    public float publishMessageFrequency = 5f;

    // Used to determine how much time has elapsed since the last message was published
    private float timeElapsed;

    void Start()
    {
        // start the ROS connection
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<PosRotMsg>(topicName);
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed > publishMessageFrequency)
        {
            khiduaro.transform.rotation = Random.rotation;

            PosRotMsg khiduaroPos = new PosRotMsg(
                khiduaro.transform.position.x,
                khiduaro.transform.position.y,
                khiduaro.transform.position.z,
                khiduaro.transform.rotation.x,
                khiduaro.transform.rotation.y,
                khiduaro.transform.rotation.z,
                khiduaro.transform.rotation.w
            );

            // Finally send the message to server_endpoint.py running in ROS
            ros.Publish(topicName, khiduaroPos);

            timeElapsed = 0;
        }
    }
}
