using UnityEngine;

namespace Tools
{
    public static class CodeGenerator
    {
        public static string Generate(Sprite sprite)
        {
            string hash = sprite.GetHashCode().ToString();
            string time = Time.realtimeSinceStartup.ToString();
            string random = Random.Range(0, 1000).ToString();

            return hash + time + random;
        }
    }
}