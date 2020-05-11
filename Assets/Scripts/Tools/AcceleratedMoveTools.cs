using UnityEngine;

namespace Tools
{
    public static class AcceleratedMoveTools
    {
        public static void HandleSpeed(float needDistance, float accel, float decel, float speedLim, ref float currentSpeed)
        {
            float maybeSpeed = currentSpeed + accel;
            float maybePath = CalculatePath(maybeSpeed, decel);

            if (maybePath > needDistance)
            {
                float currentPath = CalculatePath(currentSpeed, decel);
                if (currentPath > needDistance)
                {
                    currentSpeed -= decel;
                }
            }
            else
            {
                currentSpeed += accel;
                if (currentSpeed > speedLim)
                {
                    currentSpeed = speedLim;
                }
            }

        }

        public static float CalculatePath(float speed, float accel)
        {
            return (speed * speed) / (2 * accel);
        }
    }
}