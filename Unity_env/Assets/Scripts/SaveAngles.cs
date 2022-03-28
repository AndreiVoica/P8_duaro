using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveAngles : MonoBehaviour
{
    public string[] angles;
    async void Start()
    {
        GameObject readAngles = GameObject.Find("CSVread");
        ReadCSV csv = readAngles.GetComponent<ReadCSV>();
        angles = csv.data_String.Split(',');
        for(int i = 0; i < csv.data_String.Length; i++)
        {
            //Debug.Log("test:" + angles[12].ToString());
        }
    }

}
