using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestContol : MonoBehaviour
{
    private Control control;
    private Library robot;
    // Start is called before the first frame update
    void Start()
    {
        control = FindObjectOfType<Control>();
        robot = FindObjectOfType<Library>();
        HomePos();
        var rate = 10f;
        var waitTime = 1f / rate;
        
        //InvokeRepeating("MoveJoints", 0, waitTime);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.A))
        {
            StartCoroutine(Blue());
        }
        else if(Input.GetKey(KeyCode.S))
        {
            StartCoroutine(Red());
        }
        else if(Input.GetKey(KeyCode.D))
        {
            control.PickBlackLower();
        }
    }


    IEnumerator Blue()
    {
        control.PickGreenUpper();
        yield return new WaitForEndOfFrame();
    }
    IEnumerator Red()
    {
        control.PickGreenLower();
        yield return new WaitForEndOfFrame();
    }
    IEnumerator White()
    {
        control.PickBlackLower();
        yield return null;
    }

/*
    public void ChooseSkill()
    {
        if(Input.GetKey(KeyCode.A))
        {
            control.PickBlue();
        }
        if(Input.GetKey(KeyCode.S))
        {
            control.PickRed();
        }
        if(Input.GetKey(KeyCode.D))
        {
            control.PickWhite();
        }
    }
*/
    public void HomePos()
    {
        robot.set_lower_joint_target(-45f, 45f, 0f, 0f, 0.055f, -0.055f);
        robot.set_upper_joint_target(45f, -45f, 0f, 0f, 0.055f, -0.055f);
    }
}
