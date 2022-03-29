using System;
using Unity.Robotics;
using UnityEngine;

namespace Unity.Robotics.UrdfImporter.Control
{
    public enum RotationDirection { None = 0, Positive = 1, Negative = -1 };
    public enum ControlType { PositionControl };
    public class Controller : MonoBehaviour
    {
        private ArticulationBody[] articulationChain;
        // Stores original colors of the part being highlighted
        private Color[] prevColor;
        private int previousIndex;

        [InspectorReadOnly(hideInEditMode: true)]
        public string selectedJoint;
        [HideInInspector]
        public int selectedIndex;
        public ControlType control = ControlType.PositionControl;
        public float stiffness;
        public float damping;
        public float forceLimit;
        public float speed = 5f; // Units: degree/s
        public float torque = 100f; // Units: Nm or N
        public float acceleration = 5f;// Units: m/s^2 / degree/s^2

        [Tooltip("Color to highlight the currently selected join")]
        public Color highLightColor = new Color(1.0f, 0, 0, 1.0f);
        
        void Start()
        {
            previousIndex = selectedIndex = 1;
            this.gameObject.AddComponent<FKRobot>();
            articulationChain = this.GetComponentsInChildren<ArticulationBody>();
            int defDyanmicVal = 10;
            foreach (ArticulationBody joint in articulationChain)
            {
                joint.gameObject.AddComponent<JointControl>();
                joint.jointFriction = defDyanmicVal;
                joint.angularDamping = defDyanmicVal;
                ArticulationDrive currentDrive = joint.xDrive;
                currentDrive.forceLimit = forceLimit;
                joint.xDrive = currentDrive;
            }
        }

        void SetSelectedJointIndex(int index)
        {
            if (articulationChain.Length > 0) 
            {
                selectedIndex = (index + articulationChain.Length) % articulationChain.Length;
            }
        }

        async void Update()
        {
            bool SelectionInput1 = Input.GetKeyDown("right");
            bool SelectionInput2 = Input.GetKeyDown("left");
            for(int i = 0; i < articulationChain.Length; i++)
            {
                //Debug.Log("position" + i + "=" + articulationChain[i]);
            }
            SetSelectedJointIndex(selectedIndex); // to make sure it is in the valid range
            UpdateDirection(selectedIndex);

            if (SelectionInput2)
            {
                SetSelectedJointIndex(selectedIndex - 1);
            }
            else if (SelectionInput1)
            {
                SetSelectedJointIndex(selectedIndex + 1);
            }

            UpdateDirection(selectedIndex);
        }
        private void UpdateDirection(int jointIndex)
        {
            if (jointIndex < 0 || jointIndex >= articulationChain.Length) 
            {
                return;
            }

            float moveDirection = Input.GetAxis("Vertical");
            JointControl current = articulationChain[jointIndex].GetComponent<JointControl>();
            if (previousIndex != jointIndex)
            {
                JointControl previous = articulationChain[previousIndex].GetComponent<JointControl>();
                previous.direction = RotationDirection.None;
                previousIndex = jointIndex;
            }

            if (current.controltype != control) 
            {
                UpdateControlType(current);
            }

            if (moveDirection > 0)
            {
                current.direction = RotationDirection.Positive;
            }
            else if (moveDirection < 0)
            {
                current.direction = RotationDirection.Negative;
            }
            else
            {
                current.direction = RotationDirection.None;
            }
        }
        public void UpdateControlType(JointControl joint)
        {
            joint.controltype = control;
            if (control == ControlType.PositionControl)
            {
                ArticulationDrive drive = joint.joint.xDrive;
                drive.stiffness = stiffness;
                drive.damping = damping;
                joint.joint.xDrive = drive;
            }
        }

        public void OnGUI()
        {
            GUIStyle centeredStyle = GUI.skin.GetStyle("Label");
            centeredStyle.alignment = TextAnchor.UpperCenter;
            GUI.Label(new Rect(Screen.width / 2 - 200, 10, 400, 20), "Press left/right arrow keys to select a robot joint.", centeredStyle);
            GUI.Label(new Rect(Screen.width / 2 - 200, 30, 400, 20), "Press up/down arrow keys to move " + selectedJoint + ".", centeredStyle);
        }
    }
}
