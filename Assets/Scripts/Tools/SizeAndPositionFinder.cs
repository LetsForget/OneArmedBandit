using System;
using System.Linq;
using UnityEngine;

namespace Tools
{
    public static class SizeAndPositionFinder
    {
        public static Vector3[] GetSpriteBorderPoints(SpriteRenderer spriteRenderer)
        {
            Bounds bounds = spriteRenderer.sprite.bounds;

            Vector3[] points = new Vector3[4];

            points[0] = spriteRenderer.transform.TransformPoint(bounds.min);
            points[3] = spriteRenderer.transform.TransformPoint(bounds.max);

            Vector3 localFirst = new Vector3(bounds.min.x, bounds.max.y, bounds.max.z);
            Vector3 localSecond = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z);

            points[1] = spriteRenderer.transform.TransformPoint(localFirst);
            points[2] = spriteRenderer.transform.TransformPoint(localSecond);

            return points;
        }

        public static void CalculateSpriteSizes(Vector3[] points, out float xDist, out float yDist, out Vector3 start)
        {
            Vector3 leftMiddle = (points[0] + points[1]) / 2;
            Vector3 rightMiddle = (points[2] + points[3]) / 2;
            xDist = Vector3.Distance(leftMiddle, rightMiddle);

            Vector3 downMiddle = (points[0] + points[2]) / 2;
            Vector3 upMiddle = (points[1] + points[3]) / 2;
            yDist = Vector3.Distance(downMiddle, upMiddle);

            start = leftMiddle;
        }   
    }
}