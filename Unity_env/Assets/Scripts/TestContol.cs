using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestContol : MonoBehaviour
{
    private Control control;
    private Library robot;
    bool isDone = false;
    // Start is called before the first frame update
    void Start()
    {
        control = FindObjectOfType<Control>();
        robot = FindObjectOfType<Library>();
        robot.set_lower_joint_target(-45f, 45f, 0f, 0f, 0.055f, -0.055f);
        robot.set_upper_joint_target(45f, -45f, 0f, 0f, 0.055f, -0.055f);
        var rate = 10f;
        var waitTime = 1f / rate;
        
        InvokeRepeating("MoveJoints", 0, waitTime);
    }

    // Update is called once per frame
    void Update()
    {
        if(!isDone)
        {
            ChooseSkill();
        }
    }

    public void ChooseSkill()
    {
        if(Input.GetKey(KeyCode.A))
        {
            control.PickBlue();
            isDone = true;
        }
        if(Input.GetKey(KeyCode.S))
        {
            control.PickRed();
        }
        if(Input.GetKey(KeyCode.D))
        {
            control.PicKWhite();
        }
    }
}
