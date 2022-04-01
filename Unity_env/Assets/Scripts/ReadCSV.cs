using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ReadCSV : MonoBehaviour
{
    public string data_String;
    public string[] data_values;
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
            strReader.ReadLine();
            while (!strReader.EndOfStream)
            {
                data_String = strReader.ReadLine();
                var data_values = data_String.Split(',');
                float.TryParse(data_values[12], out float jointAngle);
            }
        }
    }
}
