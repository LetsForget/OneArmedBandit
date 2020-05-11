using UnityEngine;
using Tools;

namespace Slots
{
    public class SlotsMaker : MonoBehaviour
    {
        public GameObject SlotPrefab => Resources.Load<GameObject>("Prefabs\\Slot");

        public Transform SlotsHolder;
        public Sprite[] Pictures;
        public int SlotQuan;

        private void Start()
        {
            ResultMaker rm = gameObject.AddComponent<ResultMaker>();
            rm.Slots = SetSlots();
        }

        private SlotHandler[] SetSlots()
        {
            SlotHandler[] slotHandlers = new SlotHandler[SlotQuan];

            SpriteRenderer sr = SlotsHolder.GetComponent<SpriteRenderer>();
            Vector3[] points = SizeAndPositionFinder.GetSpriteBorderPoints(sr);

            SizeAndPositionFinder.CalculateSpriteSizes(points, out float width, out float height, out Vector3 start);
            Vector2 oneSlotSize = new Vector2(width / SlotQuan, height);
            
            GameObject slot = GameObject.Instantiate(SlotPrefab, SlotsHolder);
            slot.name = "Slot " + 1;
            slot.transform.position = start + new Vector3(oneSlotSize.x / 2, 0, 0);

            slotHandlers[0] = slot.GetComponent<SlotHandler>();
            slotHandlers[0].SetPictures(Pictures, oneSlotSize);

            for (int i = 1; i < SlotQuan; i++)
            {
                GameObject slotCopy = GameObject.Instantiate(slot, SlotsHolder);
                slotCopy.name = "Slot " + (i + 1);

                float xOffset = (i + 1) * oneSlotSize.x - (oneSlotSize.x / 2);
                slotCopy.transform.position = start + new Vector3(xOffset, 0, 0);

                slotHandlers[i] = slotCopy.GetComponent<SlotHandler>();
            }

            return slotHandlers;
        }
    }
}