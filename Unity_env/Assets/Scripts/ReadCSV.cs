using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ReadCSV : MonoBehaviour
{
    public string data_String;
    public string[] data_values;
    public float jointAngle;
    public float degJoint1L;
    //public float jointAngle;
    void Start()
    {
        ReadCSVFile();
        
    }
    public void ReadCSVFile()
    {
	var path = Directory.GetCurrentDirectory();
	var filePath = Path.Combine(path, "bagfiles/test_1.csv");

        using (var strReader = new StreamReader(filePath))
        {
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
                float.TryParse(data_values[12], out float degJoint1L);
                jointAngle = degJoint1L;
            }
        }
    }
}
