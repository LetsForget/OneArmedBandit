using Slots;

namespace SlotsScripts.Results
{
    public static class Balanced
    {
        public static SlotElement[] Result(SlotHandler[] slots, SlotElement[] elements)
        {
            int point = UnityEngine.Random.Range(0, 101);

            if (point > 50)
            {
                return Win.Result(slots, elements);
            }
            else
            {
                return FullLose.Result(slots, elements);
            }
        }
    }
}