using Lesson_17;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesson_20
{
    /// <summary>
    /// 아이템이 세계로 떨어질 때 픽업 아이템을 생성하는 기능을 다룹니다.
    /// 아이템을 드래그할 수 있는 루트 캔버스에 배치해야 합니다. 빈 공간 위에 떨어뜨릴 경우 호출됩니다.
    /// </summary>
    public class InventoryDropTarget : MonoBehaviour
    {
        public void AddItems(InventoryItem item, int number)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<ItemDropper>().DropItem(item, number);
        }

        public int MaxAcceptable(InventoryItem item)
        {
            return int.MaxValue;
        }
    }
}