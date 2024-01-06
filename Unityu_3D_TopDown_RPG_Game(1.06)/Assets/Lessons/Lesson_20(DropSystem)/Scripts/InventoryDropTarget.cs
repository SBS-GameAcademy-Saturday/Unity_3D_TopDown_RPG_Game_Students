using Lesson_17;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesson_20
{
    /// <summary>
    /// �������� ����� ������ �� �Ⱦ� �������� �����ϴ� ����� �ٷ�ϴ�.
    /// �������� �巡���� �� �ִ� ��Ʈ ĵ������ ��ġ�ؾ� �մϴ�. �� ���� ���� ����߸� ��� ȣ��˴ϴ�.
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