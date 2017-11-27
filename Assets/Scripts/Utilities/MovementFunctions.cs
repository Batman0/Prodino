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
    /// <returns>The position of the fluctuating object</returns>
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

    /// <summary>
    /// Makes the object fluctuate with a specified amplituted starting from its position, following an easing function
    /// </summary>
    /// <param name="fluctuationSpeed"></param>
    /// <param name="fluctuationAmplitudes">Defines the two points between which the object will fluctuate</param>
    /// <param name="direction">States wether the object is moving forward or backwards</param>
    /// <param name="transform">The object's transform</param>
    /// <param name="fracJourney">The current step of the fluctuation</param>
    /// <param name="targetPosition">The next point the object is going to reach</param>
    /// <param name="startPosition">The object's initial position</param>
    /// <param name="easingValue">The easing value defines how speed will change around the limits of the fluctuation</param>
    /// <returns>The position of the fluctuating object</returns>
    public static Vector3 Fluctuate(float fluctuationSpeed, Vector3 fluctuationAmplitudes, Vector3 initialPosition, ref bool direction, Transform transform, ref float fracJourney, ref Vector3 targetPosition, ref Vector3 startPosition, float easingValue)
    {
        float stepLength = Time.fixedDeltaTime * fluctuationSpeed / Vector3.Distance(startPosition, targetPosition) / (1 + easingValue * Mathf.Pow((fracJourney - .5f), 4));
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

    public static Vector3 Lerp(float fluctuationSpeed, Transform transform, ref float fracJourney, Vector3 targetPosition, Vector3 startPosition, out bool targetReached)
    {
        if (transform.localPosition == targetPosition)
        {
            targetReached = true;
            return transform.localPosition;
        }
        targetReached = false;
        float stepLength = Time.fixedDeltaTime * fluctuationSpeed;
        fracJourney += stepLength;
        transform.localPosition = Vector3.Lerp(startPosition, targetPosition, fracJourney);
        if (transform.localPosition == targetPosition)
        {
            targetReached = true;
        }
        return transform.localPosition;
    }
    public static Quaternion Rotate(Transform transform, ref float fracRotation, float rotationDuration, int spins, bool xAxis, bool yAxis, bool zAxis, out bool rotationComplete)
    {
        rotationComplete = false;
        float stepRotation = 0;
        if (Mathf.Abs(fracRotation) >= spins * 360)
        {
            transform.localRotation = Quaternion.identity;
            rotationComplete = true;
            return transform.localRotation;
        }
        stepRotation = spins * 360 / rotationDuration * Time.fixedDeltaTime;
        fracRotation += stepRotation;
        if (Mathf.Abs(fracRotation) >= spins * 360)
        {
            transform.localRotation = Quaternion.identity;
            rotationComplete = true;
        }
        else
        {
            transform.Rotate(xAxis ? stepRotation : 0, yAxis ? stepRotation : 0, zAxis ? stepRotation : 0);
        }
        return transform.localRotation;
    }
}
