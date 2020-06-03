using Slots;
using System.Collections.Generic;

namespace SlotsScripts.Results
{
    public static class FullLose
    {
        public static SlotElement[] Result(SlotHandler[] slots, SlotElement[] elements)
        {
            SlotElement[] result = new SlotElement[slots.Length];

            List<int> usedIndexes = new List<int>();

            int allowedRepeats = 0;
            if (slots.Length > elements.Length)
            {
                allowedRepeats = slots.Length - elements.Length;
            }

            for (int i = 0; i < result.Length; i++)
            {
                int index = UnityEngine.Random.Range(0, elements.Length - 1);
                if (allowedRepeats < 1)
                {
                    while (usedIndexes.Contains(index))
                    {
                        index = UnityEngine.Random.Range(0, elements.Length);
                    }
                }
                else if (usedIndexes.Contains(index))
                {
                    allowedRepeats -= 1;
                }

                usedIndexes.Add(index);
                result[i] = elements[index];
            }

            return result;
        }
    }
}