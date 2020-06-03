using System;
using UnityEngine;
using UI;
using Bets;
using SlotsScripts.Results;

namespace Slots
{
    public class ResultMaker : MonoBehaviour
    {
        public static event Action<bool> RollEnded;
        public int SpiningSlots = 0;

        public SlotHandler[] Slots;
        public SlotElement[] Elements;

        private Func<SlotHandler[], SlotElement[], SlotElement[]> ResultSetter;
        private bool LastWin;

        private void Start()
        {
            Elements = Slots[0].Elements;

            ResultSetter = RandomSpin.Result;
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

            SlotElement[] settedResult = ResultSetter(Slots, Elements);
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
        public void WorkPatternChange(int pattern)
        {
            switch (pattern)
            {
                case 0:
                    {
                        ResultSetter = RandomSpin.Result;
                        return;
                    }
                case 1:
                    {
                        ResultSetter = Win.Result;
                        return;
                    }
                case 2:
                    {
                        ResultSetter = FullLose.Result;
                        return;
                    }
                case 3:
                    {
                        ResultSetter = AlmostWin.Result;
                        return;
                    }
                case 4:
                    {
                        ResultSetter = Balanced.Result;
                        return;
                    }
            }
        }
    }
}