using System;
using System.Collections.Generic;
using UnityEngine;
using UI;
using Bets;

namespace Slots
{
    public class ResultMaker : MonoBehaviour
    {
        public static event Action<bool> RollEnded;
        public int SpiningSlots = 0;

        public SlotHandler[] Slots;
        public SlotElement[] Elements;

        private Func<SlotElement[]> ResultSetter;
        private int LastElemIndex;
        private int LastSlotIndex;
        private bool LastWin;

        private void Start()
        {
            Elements = Slots[0].Elements;
            LastElemIndex = Elements.Length - 1;
            LastSlotIndex = Slots.Length - 1;

            ResultSetter = RandomResult;
            SlotHandler.SlotStopped += SlotStopped;
            DropdownHandler.PatternChanged += WorkPatternChange;
            BetManager.RollSignal += DoASpin;
        }

        private void DoASpin()
        {
            if (SpiningSlots != 0)
            {
                return;
            }

            SlotElement[] settedResult = ResultSetter();
            string firstCode = settedResult[0].OriginCode;
            LastWin = true;

            for (int i = 0; i < Slots.Length; i++)
            {
                Slots[i].SpinToElement(settedResult[i], (i + 1) * 10);
                SpiningSlots++;

                if (firstCode != settedResult[i].OriginCode)
                {
                    LastWin = false;
                }
            }
        }
        private void SlotStopped()
        {
            SpiningSlots--;

            if (SpiningSlots == 0)
            {
                RollEnded.Invoke(LastWin);
            }
        }

        #region Work patterns
        public void WorkPatternChange(int pattern)
        {
            switch (pattern)
            {
                case 0:
                    {
                        ResultSetter = RandomResult;
                        return;
                    }
                case 1:
                    {
                        ResultSetter = WinResult;
                        return;
                    }
                case 2:
                    {
                        ResultSetter = FullLoseResult;
                        return;
                    }
                case 3:
                    {
                        ResultSetter = AlmostWinResult;
                        return;
                    }
                case 4:
                    {
                        ResultSetter = BalancedResult;
                        return;
                    }
            }
        }
        private SlotElement[] AlmostWinResult()
        {
            SlotElement[] result = new SlotElement[Slots.Length];

            int bigAmmountElemIndex = UnityEngine.Random.Range(0, LastElemIndex);       
            int unfortuneSlotIndex = UnityEngine.Random.Range(0, LastSlotIndex);

            for (int i = 0; i < result.Length; i++)
            {
                if (i != unfortuneSlotIndex)
                {
                    result[i] = Elements[bigAmmountElemIndex];
                }
                else
                {
                    int unfortuneElemIndex = UnfortunateSlotFind(bigAmmountElemIndex);
                    result[i] = Elements[unfortuneElemIndex];
                }
            }

            return result;
        }
        private int UnfortunateSlotFind(int bigAmmountElemIndex)
        {
            int way = UnityEngine.Random.Range(1, 3);

            switch (way)
            {
                case 1:
                    {
                        int index = bigAmmountElemIndex - 1;
                        if (index < 0)
                        {
                            index = Elements.Length - 1;
                        }
                        return index;
                    }
                case 2:
                    {
                        return (bigAmmountElemIndex + 1) % Elements.Length;
                    }
                case 3:
                    {
                        int index = UnityEngine.Random.Range(0, LastElemIndex);
                        while(index == bigAmmountElemIndex)
                        {
                            index = UnityEngine.Random.Range(0, LastElemIndex);
                        }
                        return index;
                    }
                default:
                    {
                        return bigAmmountElemIndex + 1;
                    }
            }

        }
        private SlotElement[] FullLoseResult()
        {
            SlotElement[] result = new SlotElement[Slots.Length];

            List<int> usedIndexes = new List<int>();

            int allowedRepeats = 0;
            if (Slots.Length > Elements.Length)
            {
                allowedRepeats = Slots.Length - Elements.Length;
            }

            for (int i = 0; i < result.Length; i++)
            {
                int index = UnityEngine.Random.Range(0, LastElemIndex);
                if (allowedRepeats < 1)
                {
                    while (usedIndexes.Contains(index))
                    {
                        index = UnityEngine.Random.Range(0, LastElemIndex + 1);
                    }
                }
                else if (usedIndexes.Contains(index))
                {
                    allowedRepeats -= 1;
                }

                usedIndexes.Add(index);
                result[i] = Elements[index];
            }

            return result;
        }
        private SlotElement[] RandomResult()
        {
            SlotElement[] result = new SlotElement[Slots.Length];

            for (int i = 0; i < result.Length; i++)
            {
                int index = UnityEngine.Random.Range(0, LastElemIndex);
                result[i] = Elements[index];
            }

            return result;
        } 
        private SlotElement[] WinResult() 
        {
            SlotElement[] result = new SlotElement[Slots.Length];

            int winIndex = UnityEngine.Random.Range(0, LastElemIndex);
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Elements[winIndex];
            }

            return result;
        }
        private SlotElement[] BalancedResult()
        {
            int point = UnityEngine.Random.Range(0, 101);

            if (point > 50)
            {
                return WinResult();
            }
            else
            {
                return FullLoseResult();
            }
        }
        #endregion
    }
}