//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;

namespace RosMessageTypes.DuaroMoveitConfig
{
    [Serializable]
    public class DuaroMoveitJointsMsg : Message
    {
        public const string k_RosMessageName = "duaro_moveit_config/DuaroMoveitJoints";
        public override string RosMessageName => k_RosMessageName;

        public double[] joints;
        public Geometry.PoseMsg pick_pose;
        public Geometry.PoseMsg place_pose;

        public DuaroMoveitJointsMsg()
        {
            this.joints = new double[14];
            this.pick_pose = new Geometry.PoseMsg();
            this.place_pose = new Geometry.PoseMsg();
        }

        public DuaroMoveitJointsMsg(double[] joints, Geometry.PoseMsg pick_pose, Geometry.PoseMsg place_pose)
        {
            this.joints = joints;
            this.pick_pose = pick_pose;
            this.place_pose = place_pose;
        }

        public static DuaroMoveitJointsMsg Deserialize(MessageDeserializer deserializer) => new DuaroMoveitJointsMsg(deserializer);

        private DuaroMoveitJointsMsg(MessageDeserializer deserializer)
        {
            deserializer.Read(out this.joints, sizeof(double), 14);
            this.pick_pose = Geometry.PoseMsg.Deserialize(deserializer);
            this.place_pose = Geometry.PoseMsg.Deserialize(deserializer);
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.joints);
            serializer.Write(this.pick_pose);
            serializer.Write(this.place_pose);
        }

        public override string ToString()
        {
            return "DuaroMoveitJointsMsg: " +
            "\njoints: " + System.String.Join(", ", joints.ToList()) +
            "\npick_pose: " + pick_pose.ToString() +
            "\nplace_pose: " + place_pose.ToString();
        }

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
#else
        [UnityEngine.RuntimeInitializeOnLoadMethod]
#endif
        public static void Register()
        {
            MessageRegistry.Register(k_RosMessageName, Deserialize);
        }
    }
}
