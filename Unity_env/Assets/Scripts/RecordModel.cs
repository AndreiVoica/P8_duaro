using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordModel : MonoBehaviour
{
    public GameObject duarolower_link_j1;
    public GameObject duarolower_link_j2;
    public GameObject duarolower_link_j3;
    //public GameObject duarolower_link_j3_obst;
    public GameObject duarolower_link_j4;
    public GameObject duaroupper_link_j1;
    public GameObject duaroupper_link_j2;
    public GameObject duaroupper_link_j3;
    //public GameObject duaroupper_link_j3_obst;
    public GameObject duaroupper_link_j4;
    //public GameObject upper_gripper_gripper_right;
    //public GameObject upper_gripper_gripper_left;
    public GameObject lower_gripper_gripper_right;
    public GameObject lower_gripper_gripper_left;
    public GameObject upper_gripper_gripper_right;
    public GameObject upper_gripper_gripper_left;
    //public GameObject lower_gripper_gripper_right;
    //public GameObject lower_gripper_gripper_left;
    List<float> lowerLink1 = new List<float>();
    List<float> lowerLink2 = new List<float>();
    List<float> lowerLink3 = new List<float>();
    List<float> lowerLink4 = new List<float>();
    List<float> timestampList = new List<float>();
    List<float> upperLink1 = new List<float>();
    List<float> upperLink2 = new List<float>();
    List<float> upperLink3 = new List<float>();
    List<float> upperLink4 = new List<float>();
    List<float> GripLowerR = new List<float>();
    List<float> GripLowerL = new List<float>();
    List<float> GripUpperR = new List<float>();
    List<float> GripUpperL = new List<float>();
    private float unixTimestamp = 0.0f;
    public float angleU1;
    public float angleU2;
    public float angleU3;
    public float angleU4;
    public float angleL1;
    public float angleL2;
    public float angleL3;
    public float angleL4;
    public float upperR;
    public float upperL;
    public float lowerR;
    public float lowerL;

    Dictionary<string, List<float>> store = new Dictionary<string, List<float>>();
    void FixedUpdate()
    {
        angleU1 = UnityEditor.TransformUtils.GetInspectorRotation(duaroupper_link_j1.transform).y;
        angleU2 = UnityEditor.TransformUtils.GetInspectorRotation(duaroupper_link_j2.transform).y;
        angleU3 = duaroupper_link_j3.transform.localPosition.y;
        angleU4 = UnityEditor.TransformUtils.GetInspectorRotation(duaroupper_link_j4.transform).y;
        angleL1 = UnityEditor.TransformUtils.GetInspectorRotation(duarolower_link_j1.transform).y;
        angleL2 = UnityEditor.TransformUtils.GetInspectorRotation(duarolower_link_j2.transform).y;
        angleL3 = duarolower_link_j3.transform.localPosition.y;
        angleL4 = UnityEditor.TransformUtils.GetInspectorRotation(duarolower_link_j4.transform).y;
        upperR = upper_gripper_gripper_right.transform.localPosition.z;
        upperL = upper_gripper_gripper_left.transform.localPosition.z;
        lowerR = lower_gripper_gripper_right.transform.localPosition.z;
        lowerL = lower_gripper_gripper_left.transform.localPosition.z;

        Time.fixedDeltaTime = 0.02f;
        unixTimestamp += Time.fixedDeltaTime*2.5f;
        timestampList.Add(unixTimestamp);
        lowerLink1.Add(angleL1 * Mathf.Deg2Rad * Mathf.Sign(-1));
        lowerLink2.Add(angleL2 * Mathf.Deg2Rad * Mathf.Sign(-1));
        lowerLink3.Add(angleL3);
        lowerLink4.Add(angleL4 * Mathf.Deg2Rad * Mathf.Sign(-1));
        upperLink1.Add(angleU1 * Mathf.Deg2Rad * Mathf.Sign(-1));
        upperLink2.Add(angleU2 * Mathf.Deg2Rad * Mathf.Sign(-1));
        upperLink3.Add(angleU3);
        upperLink4.Add(angleU4 * Mathf.Deg2Rad * Mathf.Sign(-1));
        GripLowerR.Add(lowerR * Mathf.Sign(-1));
        GripLowerL.Add(lowerL * Mathf.Sign(-1));
        GripUpperR.Add(upperR * Mathf.Sign(-1));
        GripUpperL.Add(upperL * Mathf.Sign(-1));
    }

    private void OnApplicationQuit()
    {
        using (System.IO.StreamWriter file =
           new System.IO.StreamWriter("goodfinalseq20hz.csv"))
           {
               store.Add("Lower Link 1", lowerLink1);
               store.Add("Lower Link 2", lowerLink2);
               store.Add("Lower Link 3", lowerLink3);
               store.Add("Lower Link 4", lowerLink4);
               store.Add("Upper Link 1", upperLink1);
               store.Add("Upper Link 2", upperLink2);
               store.Add("Upper Link 3", upperLink3);
               store.Add("Upper Link 4", upperLink4);
               store.Add("Grip Lower R", GripLowerR);
               store.Add("Grip Lower L", GripLowerL);
               store.Add("Grip Upper R", GripUpperR);
               store.Add("Grip Upper L", GripUpperL);
               string[] joints = new[]{"Lower Link 1","Lower Link 2","Lower Link 3","Lower Link 4","Upper Link 1","Upper Link 2","Upper Link 3","Upper Link 4","Grip Lower R","Grip Lower L","Grip Upper R","Grip Upper L"};
               for (int i = 0; i < store["Lower Link 1"].Capacity; i++)
               {
                    if(i == 0)
                    {
                        //write column headings for .csv file
                        string colHeadings = "Timestamp, lowerLink1, lowerLink2, lowerLink3, lowerLink4, upperLink1, upperLink2, upperLink3, upperLink4, gripLowerR, gripLowerL, gripUpperR, gripUpperL";
    
                        file.WriteLine(colHeadings);
                    }
                    string toAppend = null;
                    foreach (string theJoint in joints)
                    {
                        List<float> entries = store[theJoint];
                        float entryValue = entries[i];
    
                        float entryValueY = entryValue;
    
                        toAppend += entryValueY + ", ";
                    }
                    toAppend.Remove(toAppend.Length-2);
                    //write line to file with timestamp
                    file.WriteLine(timestampList[i] + ", " + toAppend);
               }
           
           
    }    
    }
}

// + ", " + upperLink1[i] + ", " + upperLink2[i] + ", " + upperLink3[i] + ", " + upperLink4[i]+ ", " + lowerLink1[i] + ", " + lowerLink2[i]+ ", " + lowerLink3[i] + ", " + lowerLink4[i] + ", " + GripUpperR[i] + ", " + GripUpperL[i]+ ", " + GripLowerR[i] + ", " + GripLowerL[i]+ ", "