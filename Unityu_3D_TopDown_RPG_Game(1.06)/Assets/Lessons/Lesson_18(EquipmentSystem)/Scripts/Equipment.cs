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
    /// 플레이어가 장착한 아이템을 보관하는 역할을 합니다. 아이템은 장착 위치에 따라 저장됩니다.
    /// 
    /// 이 컴포넌트는 "Player"로 태그가 지정된 게임 오브젝트에 배치되어야 합니다.
    /// </summary>
    public class Equipment : MonoBehaviour, ISaveable, IPredicateEvaluator
    {
        Dictionary<EquipLocation, EquipableItem> equippedItems = new Dictionary<EquipLocation, EquipableItem>();

        // 공개 메서드

        /// <summary>
        /// 슬롯에 있는 아이템이 추가/제거될 때 브로드캐스트됩니다.
        /// </summary>
        public event Action equipmentUpdated;

        /// <summary>
        /// 주어진 장착 위치의 아이템을 반환합니다.
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
        /// 아이템을 주어진 장착 위치에 추가합니다. 호환되지 않는 슬롯에 장착하려 시도하지 마십시오.
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
        /// 주어진 슬롯의 아이템을 제거합니다.
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
        /// 현재 아이템이 있는 모든 슬롯을 열거합니다.
        /// </summary>
        public IEnumerable<EquipLocation> GetAllPopulatedSlots()
        {
            return equippedItems.Keys;
        }

        // 개인 메서드

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
