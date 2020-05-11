using Slots;
using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace Bets
{
    public class BetManager : MonoBehaviour
    {
        public static event Action RollSignal;

        public InputField BetField;
        public Text BalanceText;
        public Button RollButton;

        private float Balance
        {
            get
            {
                return Convert.ToSingle(BalanceText.text);
            }
            set
            {
                BalanceText.text = value.ToString();
            }
        }
        private float Bet
        {
            get
            {
                if (BetField.text == "")
                {
                    return 0;
                }

                return Convert.ToSingle(BetField.text);
            }
        }
        private float? LastBet;

        private Regex FloatSort = new Regex(@"^[0-9]*(?:\,[0-9]*)?$");

        private void Start()
        {
            BetField.onValidateInput += delegate (string input, int charIndex, char addedChar) { return InputValidation(addedChar); };
            RollButton.onClick.AddListener(delegate { LetItRoll(); });
            ResultMaker.RollEnded += ResultMaker_RollEnded; 

            Balance = 1000;
        }


        private char InputValidation(char newSymbol)
        {
            if (FloatSort.IsMatch(newSymbol.ToString()))
            {
                return newSymbol;
            }
            else
            {
                return '\0';
            }
        }

        private void LetItRoll()
        {
            if (LastBet == null)
            {
                LastBet = Bet;
                Balance -= Bet;
                RollSignal.Invoke();
            }
        }

        private void ResultMaker_RollEnded(bool win)
        {
            if (win)
            {
                Balance += LastBet.Value * 1.3f;
            }
            else
            {
                Balance -= LastBet.Value;
            }
            LastBet = null;
        }
    }
}