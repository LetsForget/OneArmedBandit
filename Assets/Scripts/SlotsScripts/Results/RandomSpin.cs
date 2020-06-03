using Slots;

namespace SlotsScripts.Results
{
    public class RandomSpin
    {
        public static SlotElement[] Result(SlotHandler[] slots, SlotElement[] elements)
        {
            SlotElement[] result = new SlotElement[slots.Length];

            for (int i = 0; i < result.Length; i++)
            {
                int index = UnityEngine.Random.Range(0, elements.Length - 1);
                result[i] = elements[index];
            }

            return result;
        }
    }
}