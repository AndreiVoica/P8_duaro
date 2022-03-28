using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ReadCSV : MonoBehaviour
{
    public string data_String;
    public string[] data_values;
    private GameObject joint_1;
    public List<string> angles = new List<string>();
    float change = 50f;
    void Start()
    {
        ReadCSVFile();
        joint_1 = GameObject.Find("khi_duaro/world/base_link/duaro_body/duaro_j0/upper_link_j1");
    }

    void update()
    {
        
    }
    async void ReadCSVFile()
    {
        StreamReader strReader = new StreamReader("/home/andrei/Unity_env/bagfiles/test_1.csv");
        bool endOfFile = false;
        while(!endOfFile)
        {
            data_String = strReader.ReadLine ();
            if(data_String == null)
            {
                endOfFile = true;
                break;
            }
            var data_values = data_String.Split(',');
            //Debug.Log("test: " + data_values[12].ToString());
            //Debug.Log(data_values[4].ToString() + ": " + data_values[12].ToString() + " ");
            //Debug.Log(data_values[12].ToString());
            //var Str = data_values[12].ToString();
            //float deg = float.Parse(data_values[12], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
            //Debug.Log("test:" + deg);
        }
    }
}
