using UnityEngine;

namespace Slots
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SlotElement : MonoBehaviour
    {
        public string OriginCode;    
        public SpriteRenderer SR;
    }
}