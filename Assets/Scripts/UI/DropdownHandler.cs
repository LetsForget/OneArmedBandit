using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Dropdown))]
    public class DropdownHandler : MonoBehaviour
    {
        public static Action<int> PatternChanged;
        private Dropdown Dropdown;
        private void Start()
        {
            Dropdown = GetComponent<Dropdown>();
            Dropdown.onValueChanged.AddListener(delegate { PatternChangeInvoke(); });
        }

        private void PatternChangeInvoke()
        {
            PatternChanged.Invoke(Dropdown.value);
        }
    }
}