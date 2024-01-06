using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Lesson_17;

namespace Lesson_21
{
    /// <summary>
    /// 인벤토리 아이템을 나타내는 아이콘에 배치되며, 슬롯이 아이콘과 수량을 업데이트할 수 있게 합니다.
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class InventoryItemIcon : MonoBehaviour
    {
        // 구성 데이터
        [SerializeField] GameObject textContainer = null; // 텍스트 컨테이너
        [SerializeField] TextMeshProUGUI itemNumber = null; // 아이템 수량을 표시하는 텍스트

        // 공개 메서드

        // 아이템을 설정합니다.
        public void SetItem(InventoryItem item)
        {
            SetItem(item, 0);
        }

        // 아이템과 수량을 설정합니다.
        public void SetItem(InventoryItem item, int number)
        {
            var iconImage = GetComponent<Image>();
            if (item == null) // 아이템이 없는 경우
            {
                iconImage.enabled = false; // 아이콘 이미지를 비활성화
            }
            else
            {
                iconImage.enabled = true; // 아이콘 이미지를 활성화
                iconImage.sprite = item.GetIcon(); // 아이콘 이미지를 아이템의 아이콘으로 설정
            }

            if (itemNumber)
            {
                if (number <= 1) // 수량이 1 이하인 경우
                {
                    textContainer.SetActive(false); // 텍스트 컨테이너를 비활성화
                }
                else
                {
                    textContainer.SetActive(true); // 텍스트 컨테이너를 활성화
                    itemNumber.text = number.ToString(); // 아이템 수량을 텍스트로 표시
                }
            }
        }
    }

}