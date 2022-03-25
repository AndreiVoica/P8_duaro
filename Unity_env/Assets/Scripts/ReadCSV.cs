using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ReadCSV : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ReadCSVFile();
    }

    // Update is called once per frame
    void ReadCSVFile()
    {
        StreamReader strReader = new StreamReader("/home/andrei/Unity_env/bagfiles/test.csv");
        bool endOfFile = false;
        while(!endOfFile)
        {
            string data_String = strReader.ReadLine ();
            if(data_String == null)
            {
                endOfFile = true;
                break;
            }
            var data_values = data_String.Split(',');

            Debug.Log(data_values[4].ToString() + ": " + data_values[12].ToString() + " ");
        }
    }
}
