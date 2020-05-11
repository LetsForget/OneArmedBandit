using UnityEngine;
using Tools;
using System;
using System.Collections;
using System.Linq;

namespace Slots
{
    public class SlotHandler : MonoBehaviour
    {
        public static event Action SlotStopped;
        public GameObject ElementPrefab => Resources.Load<GameObject>("Prefabs\\SlotElem");

        public SlotElement[] Elements;

        public Vector2 Size;
        public Vector3 VerticalStep;
        public Vector3 LocalVerStep;

        public Vector3 LocalUpBorder;
        public Vector3 LocalDownBorder;

        public void SetPictures(Sprite[] sprites, Vector2 size)
        {
            Size = size;
            VerticalStep = new Vector3(0, size.y, 0);
            LocalVerStep = transform.InverseTransformVector(VerticalStep);

            int elemQuan = sprites.Length;
            Elements = new SlotElement[elemQuan];

            for (int i = 0; i < elemQuan; i++)
            {
                GameObject slotElem = GameObject.Instantiate(ElementPrefab, transform);
                SlotElement se = slotElem.GetComponent<SlotElement>();
                se.SR.sprite = sprites[i];
                se.OriginCode = CodeGenerator.Generate(sprites[i]);

                Vector3[] spritePoints = SizeAndPositionFinder.GetSpriteBorderPoints(se.SR);
                SizeAndPositionFinder.CalculateSpriteSizes(spritePoints, out float xDist, out float yDist, out Vector3 s);

                Vector2 pictureSize = new Vector2(xDist, yDist);
                Vector3 needSize = size / pictureSize;
                se.transform.localScale = needSize;

                se.transform.position += new Vector3(0, i * size.y, 0);

                Elements[i] = se;
            }

            LocalUpBorder = Vector3.zero + LocalVerStep;
            LocalDownBorder = Vector3.zero - LocalVerStep;
        }

        #region HighLevelMove
        public void SpinToElement(SlotElement se, int circlesAmmount)
        {
            SlotElement needElem = null;

            foreach (SlotElement elem in Elements)
            {
                if (elem.OriginCode == se.OriginCode)
                {
                    needElem = elem;
                    break;
                }
            }

            Vector3 pos = needElem.transform.position;
            pos = transform.InverseTransformPoint(pos);
            float needDistance = -pos.y;

            Vector3 needOffset = new Vector3(0, needDistance, 0);
            needOffset = transform.TransformVector(needOffset);
            needDistance = needOffset.y;

            needDistance += circlesAmmount * Elements.Length * Size.y;
            StartCoroutine(SpinToElementCor(0.005f, 0.0005f, Size.y / 4, needDistance, needElem));
        }
        private IEnumerator SpinToElementCor(float accel, float decel, float speedLimit, float distance, SlotElement se)
        {
            float speed = 0;
            int sign = distance > 0 ? 1 : -1;

            while (Mathf.Abs(distance) > 0.001f)
            {
                AcceleratedMoveTools.HandleSpeed(distance * sign, accel, decel, speedLimit, ref speed);
                Spin(speed * Time.deltaTime * 100);
                distance -= speed * Time.deltaTime * 100 * sign;

                yield return null;
            }
            float y = se.transform.localPosition.y;
            Spin(-y, true);

            if (SlotStopped != null)
            {
                SlotStopped.Invoke();
            }
        }
        #endregion

        #region LowLevelMove
        private void Spin(float step, bool local = false)
        {
            MoveElements(step, local);
            CheckMove(step < 0);
        }
        private void MoveElements(float step, bool local)
        {
            Vector3 stepV = new Vector3(0, step, 0);
            if (!local)
            {
                stepV = transform.InverseTransformVector(stepV);
            }
            
            foreach(SlotElement se in Elements)
            {
                se.transform.localPosition += stepV;
            }
        }
        private void CheckMove(bool movingDown)
        {
            SlotElement[] orderedByY = Elements.OrderBy(elem => elem.transform.localPosition.y).ToArray();

            if (!movingDown)
            {
                for (int i = orderedByY.Length - 1; i >= 0; i--)
                {
                    Vector3 localPos = orderedByY[i].transform.localPosition;
                    Vector3 downBorderPos = localPos - LocalVerStep / 2;
                    if (downBorderPos.y > LocalUpBorder.y)
                    {
                        Vector3 needPos = orderedByY[0].transform.localPosition - LocalVerStep;
                        orderedByY[i].transform.localPosition = needPos;
                        orderedByY = Elements.OrderBy(elem => elem.transform.localPosition.y).ToArray();
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < orderedByY.Length; i++)
                {
                    Vector3 localPos = orderedByY[i].transform.localPosition;
                    Vector3 upBorderPos = localPos + LocalVerStep / 2;
                    if (upBorderPos.y < LocalDownBorder.y)
                    {
                        Vector3 needPos = orderedByY[0].transform.localPosition + LocalVerStep;
                        orderedByY[i].transform.localPosition = needPos;
                        orderedByY = Elements.OrderBy(elem => elem.transform.localPosition.y).ToArray();
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        #endregion
    }
}