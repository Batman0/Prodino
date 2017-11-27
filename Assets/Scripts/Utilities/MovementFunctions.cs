using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MovementFunctions
{
    /// <summary>
    /// Makes the object fluctuate with a specified amplituted starting from its position
    /// </summary>
    /// <param name="fluctuationSpeed"></param>
    /// <param name="fluctuationAmplitudes">Defines the two points between which the object will fluctuate</param>
    /// <param name="direction">States wether the object is moving forward or backwards</param>
    /// <param name="transform">The object's transform</param>
    /// <param name="fracJourney">The current step of the fluctuation</param>
    /// <param name="targetPosition">The next point the object is going to reach</param>
    /// <param name="startPosition">The object's initial position</param>
    public static Vector3 Fluctuate(float fluctuationSpeed, Vector3 fluctuationAmplitudes, Vector3 initialPosition, ref bool direction, Transform transform, ref float fracJourney, ref Vector3 targetPosition, ref Vector3 startPosition)
    {
        float stepLength = Time.fixedDeltaTime * fluctuationSpeed / Vector3.Distance(startPosition, targetPosition);
        Vector3 previousPosition = transform.localPosition;
        fracJourney += stepLength;
        transform.localPosition = Vector3.Lerp(startPosition, targetPosition, fracJourney);
        if (transform.localPosition == targetPosition)
        {
            fracJourney = 0;
            Vector3 lastPosition = targetPosition;
            if (direction)
            {
                direction = false;
                targetPosition = new Vector3(initialPosition.x + Mathf.Abs(fluctuationAmplitudes.x), initialPosition.y + Mathf.Abs(fluctuationAmplitudes.y), initialPosition.z + Mathf.Abs(fluctuationAmplitudes.z));
            }
            else
            {
                direction = true;
                targetPosition = new Vector3(initialPosition.x - Mathf.Abs(fluctuationAmplitudes.x), initialPosition.y - Mathf.Abs(fluctuationAmplitudes.y), initialPosition.z - Mathf.Abs(fluctuationAmplitudes.z));
            }
            startPosition = lastPosition;
        }
        return transform.localPosition;
    }
}
