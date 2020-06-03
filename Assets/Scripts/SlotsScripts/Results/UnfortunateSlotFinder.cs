using Slots;

namespace SlotsScripts.Results
{
    public static class UnfortunateSlotFinder
    {
        public static int Find(int bigAmmountElemIndex, SlotElement[] elements)
        {
            int way = UnityEngine.Random.Range(1, 3);

            switch (way)
            {
                case 1:
                    {
                        int index = bigAmmountElemIndex - 1;
                        if (index < 0)
                        {
                            index = elements.Length - 1;
                        }
                        return index;
                    }
                case 2:
                    {
                        return (bigAmmountElemIndex + 1) % elements.Length;
                    }
                case 3:
                    {
                        int lastElemIndex = elements.Length - 1;
                        int index = UnityEngine.Random.Range(0, lastElemIndex);
                        while (index == bigAmmountElemIndex)
                        {
                            index = UnityEngine.Random.Range(0, lastElemIndex);
                        }
                        return index;
                    }
                default:
                    {
                        return bigAmmountElemIndex + 1;
                    }
            }
        }
    }
}