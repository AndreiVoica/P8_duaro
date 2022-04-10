using UnityEngine;
using UnityEditor;

class VoidControl : EditorWindow
{
    private Library robot;
    JointAngles jointAngles = new JointAngles(-45f, 45f, 0, 0, 45f, -45f, 0, 0);

    [MenuItem("Robotics/Void Controller")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(VoidControl)).titleContent = new GUIContent("Void Controller");
    }

    void CreateObjectIfNotExists()
    {
        if (robot != null)
        {
            return;
        }
        robot = FindObjectOfType<Library>();
    }

    void OnGUI()
    {
        CreateObjectIfNotExists();
        GUILayout.Label("Robot", EditorStyles.boldLabel);

        GUILayout.Label("Duaro Lower Arm", EditorStyles.boldLabel);
        jointAngles.Joint1L = EditorGUILayout.Slider("Joint 1L", jointAngles.Joint1L, -170, 170);
        jointAngles.Joint2L = EditorGUILayout.Slider("Joint 2L", jointAngles.Joint2L, -140, 140);
        jointAngles.Joint3L = EditorGUILayout.Slider("Joint 3L", jointAngles.Joint3L, 0f, 0.15f);
        jointAngles.Joint4L = EditorGUILayout.Slider("Joint 4L", jointAngles.Joint4L, -360, 360);

        GUILayout.Label("Duaro Upper Arm", EditorStyles.boldLabel);
        jointAngles.Joint1U = EditorGUILayout.Slider("Joint 1U", jointAngles.Joint1U, -140, 500);
        jointAngles.Joint2U = EditorGUILayout.Slider("Joint 2U", jointAngles.Joint2U, -140, 140);
        jointAngles.Joint3U = EditorGUILayout.Slider("Joint 3U", jointAngles.Joint3U, 0f, 0.15f);
        jointAngles.Joint4U = EditorGUILayout.Slider("Joint 4U", jointAngles.Joint4U, -360, 360);
    }

    void OnInspectorUpdate()
    {
        if (robot == null)
        {
            return;
        }
        robot.set_lower_joint_target(jointAngles.Joint1L, jointAngles.Joint2L, jointAngles.Joint3L, jointAngles.Joint4L, -0.055f, 0.055f);
        robot.set_upper_joint_target(jointAngles.Joint1U, jointAngles.Joint2U, jointAngles.Joint3U, jointAngles.Joint4U, -0.055f, 0.055f);
    }
}
