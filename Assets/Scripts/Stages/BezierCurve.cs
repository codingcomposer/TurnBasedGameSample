using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurve
{
    static Vector3 result = new Vector3();
    static float u, t2, u2, u3, t3;
    public static Vector3 GetPointOnBezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        u = 1f - t;
        t2 = t * t;
        u2 = u * u;
        u3 = u2 * u;
        t3 = t2 * t;

        result =
            (u3) * p0 +
            (3f * u2 * t) * p1 +
            (3f * u * t2) * p2 +
            (t3) * p3;

        return result;
    }

}
