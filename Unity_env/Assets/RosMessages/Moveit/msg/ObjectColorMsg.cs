//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;

namespace RosMessageTypes.Moveit
{
    [Serializable]
    public class ObjectColorMsg : Message
    {
        public const string k_RosMessageName = "moveit_msgs/ObjectColor";
        public override string RosMessageName => k_RosMessageName;

        //  The object id for which we specify color
        public string id;
        //  The value of the color
        public Std.ColorRGBAMsg color;

        public ObjectColorMsg()
        {
            this.id = "";
            this.color = new Std.ColorRGBAMsg();
        }

        public ObjectColorMsg(string id, Std.ColorRGBAMsg color)
        {
            this.id = id;
            this.color = color;
        }

        public static ObjectColorMsg Deserialize(MessageDeserializer deserializer) => new ObjectColorMsg(deserializer);

        private ObjectColorMsg(MessageDeserializer deserializer)
        {
            deserializer.Read(out this.id);
            this.color = Std.ColorRGBAMsg.Deserialize(deserializer);
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.id);
            serializer.Write(this.color);
        }

        public override string ToString()
        {
            return "ObjectColorMsg: " +
            "\nid: " + id.ToString() +
            "\ncolor: " + color.ToString();
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
