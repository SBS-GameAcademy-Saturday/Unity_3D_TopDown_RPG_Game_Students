using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Lesson_22
{
    /// <summary>
    /// 커서와 관련된 화면에서 툴팁 프리팹을 올바른 위치에 생성하는 추상 기본 클래스입니다.
    /// 
    /// 추상 함수를 재정의하여 고유한 데이터에 대한 툴팁 생성기를 만듭니다.
    /// </summary>
    public abstract class TooltipSpawner : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        // 설정 데이터
        [Tooltip("생성할 툴팁의 프리팹.")]
        [SerializeField] GameObject tooltipPrefab = null;

        // 비공개 상태
        GameObject tooltip = null;

        /// <summary>
        /// 툴팁 프리팹을 업데이트해야 할 때 호출됩니다.
        /// </summary>
        /// <param name="tooltip">
        /// 업데이트할 생성된 툴팁 프리팹입니다.
        /// </param>
        public abstract void UpdateTooltip(GameObject tooltip);

        /// <summary>
        /// 툴팁을 생성할 수 있는 경우 true를 반환합니다.
        /// </summary>
        public abstract bool CanCreateTooltip();

        // 비공개

        private void OnDestroy()
        {
            ClearTooltip();
        }

        private void OnDisable()
        {
            ClearTooltip();
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            var parentCanvas = GetComponentInParent<Canvas>();

            if (tooltip && !CanCreateTooltip())
            {
                ClearTooltip();
            }

            if (!tooltip && CanCreateTooltip())
            {
                tooltip = Instantiate(tooltipPrefab, parentCanvas.transform);
            }

            if (tooltip)
            {
                UpdateTooltip(tooltip);
                PositionTooltip();
            }
        }

        private void PositionTooltip()
        {
            // 구석 위치를 업데이트하려면 필요합니다.
            Canvas.ForceUpdateCanvases();

            var tooltipCorners = new Vector3[4];
            tooltip.GetComponent<RectTransform>().GetWorldCorners(tooltipCorners);
            var slotCorners = new Vector3[4];
            GetComponent<RectTransform>().GetWorldCorners(slotCorners);

            bool below = transform.position.y > Screen.height / 2;
            bool right = transform.position.x < Screen.width / 2;

            int slotCorner = GetCornerIndex(below, right);
            int tooltipCorner = GetCornerIndex(!below, !right);

            tooltip.transform.position = slotCorners[slotCorner] - tooltipCorners[tooltipCorner] + tooltip.transform.position;
        }

        private int GetCornerIndex(bool below, bool right)
        {
            if (below && !right) return 0;
            else if (!below && !right) return 1;
            else if (!below && right) return 2;
            else return 3;
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            var slotCorners = new Vector3[4];
            GetComponent<RectTransform>().GetWorldCorners(slotCorners);
            Rect rect = new Rect(slotCorners[0], slotCorners[2] - slotCorners[0]);
            if (rect.Contains(eventData.position)) return;
            ClearTooltip();
        }

        private void ClearTooltip()
        {
            if (tooltip)
            {
                Destroy(tooltip.gameObject);
            }
        }
    }

}
