using Lesson_11;
using Lesson_17;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesson_24
{
    /// <summary>
    /// ���� ������ ����Ҹ� �����մϴ�. �� ���뿡�� ä�� �� �ִ� ������ �������̸� ���� ���� ������ "���"�� �� �ֽ��ϴ�.
    /// 
    /// �� ���� ��Ҵ� "Player"�� �±װ� ������ GameObject�� ��ġ�Ǿ�� �մϴ�.
    /// </summary>
    public class ActionStore : MonoBehaviour, ISaveable
    {
        // ����
        Dictionary<int, DockedItemSlot> dockedItems = new Dictionary<int, DockedItemSlot>();
        private class DockedItemSlot
        {
            public ActionItem item; // ���� �׸�
            public int number;      // �׸� ��
        }

        // ���� ���

        /// <summary>
        /// ���� ���� �׸��� �߰�/���ŵ� �� ��ε�ĳ��Ʈ�˴ϴ�.
        /// </summary>
        public event Action storeUpdated;

        /// <summary>
        /// �־��� �ε������� ������ �����ɴϴ�.
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
        /// �־��� �ε����� ���� �׸� ���� �����ɴϴ�.
        /// </summary>
        /// <returns>
        /// �ش� �ε����� �׸��� ���ų� �׸��� ������ �Һ�� ��� 0�� ��ȯ�մϴ�.
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
        /// �־��� �ε����� �׸��� �߰��մϴ�.
        /// </summary>
        /// <param name="item">�߰��� �׸�.</param>
        /// <param name="index">�׸��� �߰��� ��ġ.</param>
        /// <param name="number">�߰��� �׸� ��.</param>
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
        /// �־��� ���Կ��� �׸��� ����մϴ�. �׸��� �Һ� ������ ��� �׸��� ������ ���ŵ� ������ �ϳ��� �ν��Ͻ��� �Ҹ�˴ϴ�.
        /// </summary>
        /// <param name="user">�� ������ ����Ϸ��� ĳ����.</param>
        /// <returns>������ ������ �� ���� ��� false�� ��ȯ�մϴ�.</returns>
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
        /// �־��� ���Կ��� ������ ���� �׸��� �����մϴ�.
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
        /// �� ���Կ� ���Ǵ� �׸��� �ִ� ���� ���ΰ���.
        /// 
        /// ������ �̹� �׸��� �����ϰ� ������ ������ ��� �Ǵ� �׸��� �Һ� ������ ��쿡�� ���� ���� ����մϴ�.
        /// </summary>
        /// <returns>�������� ������ ���� ��� int.MaxValue�� ��ȯ�մϴ�.</returns>
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

        // ���� ���

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
