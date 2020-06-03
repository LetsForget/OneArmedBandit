using Slots;

namespace SlotsScripts.Results
{
    public static class AlmostWin
    {
        public static SlotElement[] Result(SlotHandler[] slots, SlotElement[] elements)
        {
            SlotElement[] result = new SlotElement[slots.Length];

            int bigAmmountElemIndex = UnityEngine.Random.Range(0, elements.Length - 1);
            int unfortuneSlotIndex = UnityEngine.Random.Range(0, slots.Length - 1);

            for (int i = 0; i < result.Length; i++)
            {
                if (i != unfortuneSlotIndex)
                {
                    result[i] = elements[bigAmmountElemIndex];
                }
                else
                {
                    int unfortuneElemIndex = UnfortunateSlotFinder.Find(bigAmmountElemIndex, elements);
                    result[i] = elements[unfortuneElemIndex];
                }
            }
            return result;
        }
    }
}