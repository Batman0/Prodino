using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Mathfx
{

    public static float Hermite(float start, float end, float time, float easingValue)
    {
        return Mathf.Lerp(start, end, time * (time * easingValue) * (3.0f - 2.0f * time));
    }

    public static Vector2 Hermite(Vector2 start, Vector2 end, float time, float easingValue)
    {
        return new Vector2(Hermite(start.x, end.x, time, easingValue), Hermite(start.y, end.y, time, easingValue));
    }

    public static Vector3 Hermite(Vector3 start, Vector3 end, float time, float easingValue)
    {
        return new Vector3(Hermite(start.x, end.x, time, easingValue), Hermite(start.y, end.y, time, easingValue), Hermite(start.z, end.z, time, easingValue));
    }

}
