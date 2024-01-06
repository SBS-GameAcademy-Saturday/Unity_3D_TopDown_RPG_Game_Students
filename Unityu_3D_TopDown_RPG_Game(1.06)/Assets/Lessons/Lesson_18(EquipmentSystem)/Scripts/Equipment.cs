using Lesson_11;
using Lesson_Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lesson_17;

namespace Lesson_18
{
    /// <summary>
    /// �÷��̾ ������ �������� �����ϴ� ������ �մϴ�. �������� ���� ��ġ�� ���� ����˴ϴ�.
    /// 
    /// �� ������Ʈ�� "Player"�� �±װ� ������ ���� ������Ʈ�� ��ġ�Ǿ�� �մϴ�.
    /// </summary>
    public class Equipment : MonoBehaviour, ISaveable, IPredicateEvaluator
    {
        Dictionary<EquipLocation, EquipableItem> equippedItems = new Dictionary<EquipLocation, EquipableItem>();

        // ���� �޼���

        /// <summary>
        /// ���Կ� �ִ� �������� �߰�/���ŵ� �� ��ε�ĳ��Ʈ�˴ϴ�.
        /// </summary>
        public event Action equipmentUpdated;

        /// <summary>
        /// �־��� ���� ��ġ�� �������� ��ȯ�մϴ�.
        /// </summary>
        public EquipableItem GetItemInSlot(EquipLocation equipLocation)
        {
            if (!equippedItems.ContainsKey(equipLocation))
            {
                return null;
            }

            return equippedItems[equipLocation];
        }

        /// <summary>
        /// �������� �־��� ���� ��ġ�� �߰��մϴ�. ȣȯ���� �ʴ� ���Կ� �����Ϸ� �õ����� ���ʽÿ�.
        /// </summary>
        public void AddItem(EquipLocation slot, EquipableItem item)
        {
            Debug.Assert(item.CanEquip(slot,this));

            equippedItems[slot] = item;

            if (equipmentUpdated != null)
            {
                equipmentUpdated();
            }
        }

        /// <summary>
        /// �־��� ������ �������� �����մϴ�.
        /// </summary>
        public void RemoveItem(EquipLocation slot)
        {
            equippedItems.Remove(slot);
            if (equipmentUpdated != null)
            {
                equipmentUpdated();
            }
        }

        /// <summary>
        /// ���� �������� �ִ� ��� ������ �����մϴ�.
        /// </summary>
        public IEnumerable<EquipLocation> GetAllPopulatedSlots()
        {
            return equippedItems.Keys;
        }

        // ���� �޼���

        object ISaveable.CaptureState()
        {
            var equippedItemsForSerialization = new Dictionary<EquipLocation, string>();
            foreach (var pair in equippedItems)
            {
                equippedItemsForSerialization[pair.Key] = pair.Value.GetItemID();
            }
            return equippedItemsForSerialization;
        }

        void ISaveable.RestoreState(object state)
        {
            equippedItems = new Dictionary<EquipLocation, EquipableItem>();

            var equippedItemsForSerialization = (Dictionary<EquipLocation, string>)state;

            foreach (var pair in equippedItemsForSerialization)
            {
                var item = (EquipableItem)InventoryItem.GetFromID(pair.Value);
                if (item != null)
                {
                    equippedItems[pair.Key] = item;
                }
            }

            //Lesson_33
            equipmentUpdated?.Invoke();
        }

        /// <summary>
        /// Lesson_33
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool? Evaluate(string predicate, string[] parameters)
        {
            if(predicate == "HasItemEquiped")
            {
                foreach(var item in equippedItems.Values)
                {
                    if(item.GetItemID() == parameters[0])
                    {
                        return true;
                    }
                }
                return false;
            }
            return null;
        }
    }
}
