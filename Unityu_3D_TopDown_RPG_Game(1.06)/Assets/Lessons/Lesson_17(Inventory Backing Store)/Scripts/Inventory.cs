using Lesson_11;
using Lesson_33;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Lesson_17
{
    /// <summary>
    /// 플레이어 인벤토리에 대한 저장 공간을 제공합니다. 구성 가능한 개수의 슬롯이 사용 가능합니다.
    ///
    /// 이 컴포넌트는 "Player"로 태그가 지정된 게임 오브젝트에 배치되어야 합니다.
    /// </summary>
    public class Inventory : MonoBehaviour,ISaveable
    {
        // 설정 데이터
        [Tooltip("허용된 크기")]
        [SerializeField] int inventorySize = 16;

        // 상태
        [SerializeField] InventorySlot[] slots;

        [Serializable]
        public struct InventorySlot
        {
            public InventoryItem item;
            public int number;
        }

        /// <summary>
        /// 슬롯에 아이템이 추가/제거될 때 브로드캐스트됩니다.
        /// </summary>
        public event Action inventoryUpdated;

        /// <summary>
        /// Convenience for getting the player's inventory.
        /// </summary>
        public static Inventory GetPlayerInventory()
        {
            var player = GameObject.FindWithTag("Player");
            return player.GetComponent<Inventory>();
        }

        /// <summary>
        /// 이 아이템을 인벤토리에 어디에든지 넣을 수 있을까요?
        /// </summary>
        public bool HasSpaceFor(InventoryItem item)
        {
            return FindSlot(item) >= 0;
        }


        /// <summary>
        /// Lesson_29에서 사용할 내용
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public bool HasSpaceFor(IEnumerable<InventoryItem> items)
        {
            int freeslots  = FreeSlots();
            List<InventoryItem> stackedItems = new List<InventoryItem>();
            foreach(var item in items)
            {
                if (item.IsStackable())
                {
                    if (HasItem(item)) continue;
                    if (stackedItems.Contains(item)) continue;
                    stackedItems.Add(item);
                }
                if (freeslots <= 0) return false;
                freeslots--;
            }
            return true;
        }

        public int FreeSlots()
        {
            int count = 0;
            foreach(InventorySlot slot in slots)
            {
                if(slot.number == 0)
                {
                    count++;
                }
            }
            return count;
        }
             


        /// <summary>
        /// How many slots are in the inventory?
        /// </summary>
        public int GetSize()
        {
            return slots.Length;
        }


        /// <summary>
        /// 아이템을 첫 번째 사용 가능한 슬롯에 추가를 시도합니다.
        /// </summary>
        /// <param name="item">추가할 아이템.</param>
        /// <param name="number">추가할 개수.</param>
        /// <returns>아이템을 추가할 수 있는지 여부.</returns>
        public bool AddToFirstEmptySlot(InventoryItem item, int number)
        {
            //Lesson_33
            foreach(var store in GetComponents<IItemStore>())
            {
                number -= store.AddItem(item, number);
            }
            if (number <= 0) return true;

            int i = FindSlot(item);

            if (i < 0)
            {
                return false;
            }

            slots[i].item = item;
            slots[i].number += number;
            if (inventoryUpdated != null)
            {
                inventoryUpdated();
            }
            return true;
        }

        /// <summary>
        /// 이미 있는 스택에 아이템을 추가하려면 주어진 슬롯에 아이템을 추가합니다. 
        /// 이 아이템 유형의 스택이 이미 있는 경우 기존 스택에 추가됩니다.
        /// 그렇지 않으면 첫 번째 비어있는 슬롯에 추가됩니다.
        /// </summary>
        /// <param name="slot">추가할 슬롯을 시도합니다.</param>
        /// <param name="item">추가할 아이템 유형.</param>
        /// <param name="number">추가할 아이템 개수.</param>
        /// <returns>아이템이 인벤토리의 어디든지 추가되었는지 여부.</returns>
        public bool AddItemToSlot(int slot, InventoryItem item, int number)
        {
            if (slots[slot].item != null)
            {
                return AddToFirstEmptySlot(item, number);
            }

            var i = FindStack(item);
            if (i >= 0)
            {
                slot = i;
            }

            slots[slot].item = item;
            slots[slot].number += number;
            if (inventoryUpdated != null)
            {
                inventoryUpdated();
            }
            return true;
        }

        private void Awake()
        {
            slots = new InventorySlot[inventorySize];
        }

        /// <summary>
        /// 주어진 아이템을 수용할 수 있는 슬롯을 찾습니다.
        /// </summary>
        /// <returns>슬롯을 찾지 못하면 -1입니다.</returns>
        private int FindSlot(InventoryItem item)
        {
            int i = FindStack(item);
            if (i < 0)
            {
                i = FindEmptySlot();
            }
            return i;
        }

        /// <summary>
        /// 빈 슬롯을 찾습니다.
        /// </summary>
        /// <returns>모든 슬롯이 가득 차 있으면 -1입니다.</returns>
        private int FindEmptySlot()
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item == null)
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// 이 아이템 유형의 기존 스택을 찾습니다.
        /// </summary>
        /// <returns>스택이 없거나 아이템이 쌓일 수 없는 경우 -1입니다.</returns>
        private int FindStack(InventoryItem item)
        {
            if (!item.IsStackable())
            {
                return -1;
            }

            for (int i = 0; i < slots.Length; i++)
            {
                if (object.ReferenceEquals(slots[i].item, item))
                {
                    return i;
                }
            }
            return -1;
        }

        // Leson_21 Inventory UI
        /// <summary>
        /// Is there an instance of the item in the inventory?
        /// </summary>
        public bool HasItem(InventoryItem item)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (object.ReferenceEquals(slots[i].item, item))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Return the item type in the given slot.
        /// </summary>
        public InventoryItem GetItemInSlot(int slot)
        {
            return slots[slot].item;
        }

        /// <summary>
        /// Get the number of items in the given slot.
        /// </summary>
        public int GetNumberInSlot(int slot)
        {
            return slots[slot].number;
        }

        /// <summary>
        /// Remove a number of items from the given slot. Will never remove more
        /// that there are.
        /// </summary>
        public void RemoveFromSlot(int slot, int number)
        {
            slots[slot].number -= number;
            if (slots[slot].number <= 0)
            {
                slots[slot].number = 0;
                slots[slot].item = null;
            }
            if (inventoryUpdated != null)
            {
                inventoryUpdated();
            }
        }
        [System.Serializable]
        private struct InventorySlotRecord
        {
            public string itemID;
            public int number;
        }

        object ISaveable.CaptureState()
        {
            var slotStrings = new InventorySlotRecord[inventorySize];
            for (int i = 0; i < inventorySize; i++)
            {
                if (slots[i].item != null)
                {
                    slotStrings[i].itemID = slots[i].item.GetItemID();
                    slotStrings[i].number = slots[i].number;
                }
            }
            return slotStrings;
        }

        void ISaveable.RestoreState(object state)
        {
            var slotStrings = (InventorySlotRecord[])state;
            for (int i = 0; i < inventorySize; i++)
            {
                slots[i].item = InventoryItem.GetFromID(slotStrings[i].itemID);
                slots[i].number = slotStrings[i].number;
            }
            if (inventoryUpdated != null)
            {
                inventoryUpdated();
            }
        }

    }

}
