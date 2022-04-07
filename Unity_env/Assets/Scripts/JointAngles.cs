public class JointAngles
{
    public float Joint1L {get; set;}
    public float Joint2L {get; set;}
    public float Joint3L {get; set;}
    public float Joint4L {get; set;}
    public float Joint1U {get; set;}
    public float Joint2U {get; set;}
    public float Joint3U {get; set;}
    public float Joint4U {get; set;}

    public JointAngles()
    {
    }

    public JointAngles(float joint1L, float joint2L, float joint3L, float joint4L, float joint1U, float joint2U, float joint3U, float joint4U)
    {
        Joint1L = joint1L;
        Joint2L = joint2L;
        Joint3L = joint3L;
        Joint4L = joint4L;
        Joint1U = joint1U;
        Joint2U = joint2U;
        Joint3U = joint3U;
        Joint4U = joint4U;
    }

    public override string ToString()
    {
        return $"{Joint1L},{Joint2L},{Joint3L},{Joint4L},{Joint1U},{Joint2U},{Joint3U},{Joint4U}";
    }
}
