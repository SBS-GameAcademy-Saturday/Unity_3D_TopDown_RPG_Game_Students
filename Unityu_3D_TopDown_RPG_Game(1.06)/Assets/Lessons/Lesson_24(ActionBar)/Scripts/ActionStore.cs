using Lesson_11;
using Lesson_17;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesson_24
{
    /// <summary>
    /// 동작 막대의 저장소를 제공합니다. 이 막대에는 채울 수 있는 슬롯이 제한적이며 슬롯 내의 동작을 "사용"할 수 있습니다.
    /// 
    /// 이 구성 요소는 "Player"로 태그가 지정된 GameObject에 배치되어야 합니다.
    /// </summary>
    public class ActionStore : MonoBehaviour, ISaveable
    {
        // 상태
        Dictionary<int, DockedItemSlot> dockedItems = new Dictionary<int, DockedItemSlot>();
        private class DockedItemSlot
        {
            public ActionItem item; // 동작 항목
            public int number;      // 항목 수
        }

        // 공개 멤버

        /// <summary>
        /// 슬롯 내의 항목이 추가/제거될 때 브로드캐스트됩니다.
        /// </summary>
        public event Action storeUpdated;

        /// <summary>
        /// 주어진 인덱스에서 동작을 가져옵니다.
        /// </summary>
        public ActionItem GetAction(int index)
        {
            if (dockedItems.ContainsKey(index))
            {
                return dockedItems[index].item;
            }
            return null;
        }

        /// <summary>
        /// 주어진 인덱스의 남은 항목 수를 가져옵니다.
        /// </summary>
        /// <returns>
        /// 해당 인덱스에 항목이 없거나 항목이 완전히 소비된 경우 0을 반환합니다.
        /// </returns>
        public int GetNumber(int index)
        {
            if (dockedItems.ContainsKey(index))
            {
                return dockedItems[index].number;
            }
            return 0;
        }

        /// <summary>
        /// 주어진 인덱스에 항목을 추가합니다.
        /// </summary>
        /// <param name="item">추가할 항목.</param>
        /// <param name="index">항목을 추가할 위치.</param>
        /// <param name="number">추가할 항목 수.</param>
        public void AddAction(InventoryItem item, int index, int number)
        {
            if (dockedItems.ContainsKey(index))
            {
                if (object.ReferenceEquals(item, dockedItems[index].item))
                {
                    dockedItems[index].number += number;
                }
            }
            else
            {
                var slot = new DockedItemSlot();
                slot.item = item as ActionItem;
                slot.number = number;
                dockedItems[index] = slot;
            }
            if (storeUpdated != null)
            {
                storeUpdated();
            }
        }

        /// <summary>
        /// 주어진 슬롯에서 항목을 사용합니다. 항목이 소비 가능한 경우 항목이 완전히 제거될 때까지 하나의 인스턴스가 소멸됩니다.
        /// </summary>
        /// <param name="user">이 동작을 사용하려는 캐릭터.</param>
        /// <returns>동작을 실행할 수 없는 경우 false를 반환합니다.</returns>
        public bool Use(int index, GameObject user)
        {
            if (dockedItems.ContainsKey(index))
            {
                bool wasUse = dockedItems[index].item.Use(user);
                if (wasUse && dockedItems[index].item.isConsumable())
                {
                    RemoveItems(index, 1);
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 주어진 슬롯에서 지정된 수의 항목을 제거합니다.
        /// </summary>
        public void RemoveItems(int index, int number)
        {
            if (dockedItems.ContainsKey(index))
            {
                dockedItems[index].number -= number;
                if (dockedItems[index].number <= 0)
                {
                    dockedItems.Remove(index);
                }
                if (storeUpdated != null)
                {
                    storeUpdated();
                }
            }

        }

        /// <summary>
        /// 이 슬롯에 허용되는 항목의 최대 수는 얼마인가요.
        /// 
        /// 슬롯이 이미 항목을 포함하고 동일한 유형인 경우 또는 항목이 소비 가능한 경우에만 여러 개를 허용합니다.
        /// </summary>
        /// <returns>실질적인 제한이 없는 경우 int.MaxValue를 반환합니다.</returns>
        public int MaxAcceptable(InventoryItem item, int index)
        {
            var actionItem = item as ActionItem;
            if (!actionItem) return 0;

            if (dockedItems.ContainsKey(index) && !object.ReferenceEquals(item, dockedItems[index].item))
            {
                return 0;
            }
            if (actionItem.isConsumable())
            {
                return int.MaxValue;
            }
            if (dockedItems.ContainsKey(index))
            {
                return 0;
            }

            return 1;
        }

        // 개인 멤버

        [System.Serializable]
        private struct DockedItemRecord
        {
            public string itemID;
            public int number;
        }

        object ISaveable.CaptureState()
        {
            var state = new Dictionary<int, DockedItemRecord>();
            foreach (var pair in dockedItems)
            {
                var record = new DockedItemRecord();
                record.itemID = pair.Value.item.GetItemID();
                record.number = pair.Value.number;
                state[pair.Key] = record;
            }
            return state;
        }

        void ISaveable.RestoreState(object state)
        {
            var stateDict = (Dictionary<int, DockedItemRecord>)state;
            foreach (var pair in stateDict)
            {
                AddAction(InventoryItem.GetFromID(pair.Value.itemID), pair.Key, pair.Value.number);
            }
        }
    }

}
