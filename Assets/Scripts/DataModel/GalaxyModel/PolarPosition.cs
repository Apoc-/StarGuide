using System;
using UnityEditor;
using UnityEngine;

namespace Navigation
{
    public class PolarPosition
    {
        public float Radius { get; }
        public float Angle { get; }

        public PolarPosition(float radius, float angle)
        {
            Radius = radius;
            Angle = angle;
        }

        public static float Distance(PolarPosition origin, PolarPosition target)
        {
            var v1 = origin.AsVector2();
            var v2 = target.AsVector2();

            var v3 = v2 - v1;

            return v3.magnitude;
        }

        public float Distance(PolarPosition target)
        {
            var v1 = AsVector2();
            var v2 = target.AsVector2();
            var v3 = v2 - v1;

            return v3.magnitude;
        }

        private Vector2 AsVector2()
        {
            var x = Radius * Mathf.Cos(Angle);
            var y = Radius * Mathf.Sin(Angle);
            
            return new Vector2(x,y);
        }

        public override string ToString()
        {
            return "[" + Radius + " AU; " + Angle + " RAD]";
        }
    }
}