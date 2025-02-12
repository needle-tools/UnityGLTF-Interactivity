
using UnityEngine;

namespace Unity.VisualScripting
{
    public static class InterpolateHelper
    {
        
        private static Vector2 cubicBezier(float t, Vector2 p0, Vector2 p1)
        {
            return cubicBezier(t,
                new Vector2(0f, 0f),
                new Vector2(p0.x, p0.y),
                new Vector2(p1.x, p1.y),
                new Vector2(1f, 1f));
        }
        
        private static Vector2 cubicBezier (float t, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
            float uuu = uu * u;
            float ttt = tt * t;

            Vector2 p = new Vector2(0, 0);
            p.x = uuu * p0.x + 3 * uu * t * p1.x + 3 * u * tt * p2.x + ttt * p3.x;
            p.y = uuu * p0.y + 3 * uu * t * p1.y + 3 * u * tt * p2.y + ttt * p3.y;

            return p;
        }

        public static object BezierInterpolate(Vector2 pointAValue, Vector2 pointBValue, object currentValue, object targetValue, float f)
        {
            var bezier = cubicBezier(f, pointAValue, pointBValue);
            f = bezier.y;
            if (currentValue is Vector2 currentVector2)
            {
                return Vector2.Lerp(currentVector2, (Vector2)targetValue , f);
            }
            else if (currentValue is Vector3 currentVector3)
            {
                return Vector3.Lerp(currentVector3, (Vector3)targetValue , f);
            }
            else if (currentValue is Vector4 currentVector4)
            {
                return Vector4.Lerp(currentVector4, (Vector4)targetValue , f);
            }
            else if (currentValue is Quaternion currentQuaternion)
            {
                return Quaternion.Slerp(currentQuaternion, (Quaternion)targetValue , f);
            }
            else if (currentValue is Color currentColor)
            {
                return Color.Lerp(currentColor, (Color)targetValue , f);
            }
            else if (currentValue is int currentInt)
            {
                return Mathf.RoundToInt(Mathf.Lerp(currentInt, (int)targetValue , f));
            }
            else if (currentValue is float currentFloat)
            {
                return Mathf.Lerp(currentFloat, (float)targetValue , f);
            }
            return currentValue;
        }
        
    }
}