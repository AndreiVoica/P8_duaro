using System.Collections;

public class JointAngles
{
    public float Joint1L {get; private set;}
    public float Joint2L {get; private set;}
    public float Joint3L {get; private set;}
    public float Joint4L {get; private set;}
    public float Joint1U {get; private set;}
    public float Joint2U {get; private set;}
    public float Joint3U {get; private set;}
    public float Joint4U {get; private set;}

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
        return string.Format("Joint1L: {0}, Joint2L: {1}, Joint3L: {2}, Joint4L: {3}, Joint1U: {4}, Joint2U: {5}, Joint3U: {6}, Joint4U: {7}",
            Joint1L, Joint2L, Joint3L, Joint4L, Joint1U, Joint2U, Joint3U, Joint4U);
    }
}
