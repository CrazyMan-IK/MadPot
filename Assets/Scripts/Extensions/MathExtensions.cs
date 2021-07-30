using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MadPot
{
    public static class MathExtensions
    {
        public static Vector3 GetBezierPosition(Vector3 from, Vector3 to, Vector3 fromDirection, Vector3 toDirection, float t)
        {
            Vector3 p0 = from;
            Vector3 p1 = p0 + fromDirection;
            Vector3 p3 = to;
            Vector3 p2 = p3 + toDirection;

            return Mathf.Pow(1f - t, 3f) * p0 + 3f * Mathf.Pow(1f - t, 2f) * t * p1 + 3f * (1f - t) * Mathf.Pow(t, 2f) * p2 + Mathf.Pow(t, 3f) * p3;
        }
    }
}
