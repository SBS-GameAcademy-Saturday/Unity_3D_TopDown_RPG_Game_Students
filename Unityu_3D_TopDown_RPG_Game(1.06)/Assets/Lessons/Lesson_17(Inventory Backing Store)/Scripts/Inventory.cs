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
    /// �÷��̾� �κ��丮�� ���� ���� ������ �����մϴ�. ���� ������ ������ ������ ��� �����մϴ�.
    ///
    /// �� ������Ʈ�� "Player"�� �±װ� ������ ���� ������Ʈ�� ��ġ�Ǿ�� �մϴ�.
    /// </summary>
    public class Inventory : MonoBehaviour,ISaveable
    {
        // ���� ������
        [Tooltip("���� ũ��")]
        [SerializeField] int inventorySize = 16;

        // ����
        [SerializeField] InventorySlot[] slots;

        [Serializable]
        public struct InventorySlot
        {
            public InventoryItem item;
            public int number;
        }

        /// <summary>
        /// ���Կ� �������� �߰�/���ŵ� �� ��ε�ĳ��Ʈ�˴ϴ�.
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
        /// �� �������� �κ��丮�� ��𿡵��� ���� �� �������?
        /// </summary>
        public bool HasSpaceFor(InventoryItem item)
        {
            return FindSlot(item) >= 0;
        }


        /// <summary>
        /// Lesson_29���� ����� ����
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
        /// �������� ù ��° ��� ������ ���Կ� �߰��� �õ��մϴ�.
        /// </summary>
        /// <param name="item">�߰��� ������.</param>
        /// <param name="number">�߰��� ����.</param>
        /// <returns>�������� �߰��� �� �ִ��� ����.</returns>
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
        /// �̹� �ִ� ���ÿ� �������� �߰��Ϸ��� �־��� ���Կ� �������� �߰��մϴ�. 
        /// �� ������ ������ ������ �̹� �ִ� ��� ���� ���ÿ� �߰��˴ϴ�.
        /// �׷��� ������ ù ��° ����ִ� ���Կ� �߰��˴ϴ�.
        /// </summary>
        /// <param name="slot">�߰��� ������ �õ��մϴ�.</param>
        /// <param name="item">�߰��� ������ ����.</param>
        /// <param name="number">�߰��� ������ ����.</param>
        /// <returns>�������� �κ��丮�� ������ �߰��Ǿ����� ����.</returns>
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
        /// �־��� �������� ������ �� �ִ� ������ ã���ϴ�.
        /// </summary>
        /// <returns>������ ã�� ���ϸ� -1�Դϴ�.</returns>
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
        /// �� ������ ã���ϴ�.
        /// </summary>
        /// <returns>��� ������ ���� �� ������ -1�Դϴ�.</returns>
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
        /// �� ������ ������ ���� ������ ã���ϴ�.
        /// </summary>
        /// <returns>������ ���ų� �������� ���� �� ���� ��� -1�Դϴ�.</returns>
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
