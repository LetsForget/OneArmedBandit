using Slots;

namespace SlotsScripts.Results
{
    public static class Win
    {
        public static SlotElement[] Result(SlotHandler[] slots, SlotElement[] elements)
        {
            SlotElement[] result = new SlotElement[slots.Length];

            int winIndex = UnityEngine.Random.Range(0, elements.Length - 1);
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = elements[winIndex];
            }

            return result;
        }
    }
}