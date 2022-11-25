using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventConsumer : MonoBehaviour
{
    public void Step(string Footname)
    {
        Debug.Log("Step with foot " + Footname);
    }
    public void PunchSound1(AnimationEvent AnimEvent)
    {
        string l_StringParameter = AnimEvent.stringParameter;
        float l_FloatParameter = AnimEvent.floatParameter;
        int l_IntParameter = AnimEvent.intParameter;
        Object l_Object = AnimEvent.objectReferenceParameter;
    }
}
