using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class Control : MonoBehaviour
{
    [SerializeField] private ArticulationBody[] robotJoints = new ArticulationBody[10];
    private Library robot;
    private ReadCSV degree;

    public List<float> joint1L = new List<float>();
    public List<float> joint2L = new List<float>();
    public List<float> joint3L = new List<float>();
    public List<float> joint4L = new List<float>();
    public List<float> joint1U = new List<float>();
    public List<float> joint2U = new List<float>();
    public List<float> joint3U = new List<float>();
    public List<float> joint4U = new List<float>();
    private int current_index = 0;
    void Start()
    {
        robot = FindObjectOfType<Library>();
        degree = FindObjectOfType<ReadCSV>();

        ReadCSVFile();
    }

    void Update()
    {
        if (current_index == joint1L.Count)
        {
            current_index = 0;
        }
        robot.set_upper_joint_target(joint1U[current_index], joint2U[current_index], joint3U[current_index], joint4U[current_index]);
        robot.set_lower_joint_target(joint1L[current_index], joint2L[current_index], joint3L[current_index], joint4L[current_index]);
        current_index++;
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
                var line = strReader.ReadLine();
                DecodeLine(line);
            }
        }
    }

    private void DecodeLine(string line)
    {
        string[] values = line.Split(',');
        float.TryParse(values[12], out float J1L);
        float.TryParse(values[13], out float J2L);
        float.TryParse(values[14], out float J3L);
        float.TryParse(values[15], out float J4L);
        float.TryParse(values[16], out float J1U);
        float.TryParse(values[17], out float J2U);
        float.TryParse(values[18], out float J3U);
        float.TryParse(values[19], out float J4U);
        joint1L.Add(J1L);
        joint2L.Add(J2L);
        joint3L.Add(J3L);
        joint4L.Add(J4L);
        joint1U.Add(J1U);
        joint2U.Add(J2U);
        joint3U.Add(J3U);
        joint4U.Add(J4U);
    }
}
