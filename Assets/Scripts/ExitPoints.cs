using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExitPoints
{
    
    public static List<ExitPoint> points = new List<ExitPoint>()
        {
            new ExitPoint(new Vector3(1.35f,1f,11.3f), new Vector3(1.75f,1.193f,13.6f),"InitialScene","Town0"),
            new ExitPoint(new Vector3(1.75f,1.193f,13.6f), new Vector3(1.35f,1f,11.3f),"Town0","InitialScene")
        };
}

public class ExitPoint {
    public Vector3 posFrom;
    public Vector3 posTo;
    public string sceneFrom;
    public string sceneTo;

    public ExitPoint(Vector3 posFrom, Vector3 posTo, string sceneFrom, string sceneTo) {
        this.posFrom = posFrom;
        this.posTo = posTo;
        this.sceneFrom = sceneFrom;
        this.sceneTo = sceneTo;
    }
    }