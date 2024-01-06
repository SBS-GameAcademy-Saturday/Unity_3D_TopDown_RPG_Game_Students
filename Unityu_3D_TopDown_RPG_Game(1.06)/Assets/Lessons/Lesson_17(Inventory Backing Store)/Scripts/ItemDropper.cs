using Lesson_11;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesson_17
{
    /// <summary>
    /// 아이템을 세계에 드롭하려는 모든 것에 배치할 수 있습니다. 드롭을 저장 및 복원하기 위해 드롭을 추적합니다.
    /// </summary>
    public class ItemDropper : MonoBehaviour, ISaveable
    {
        // 상태
        private List<Pickup> droppedItems = new List<Pickup>();

        // 공개 메서드

        /// <summary>
        /// 현재 위치에 픽업을 만듭니다.
        /// </summary>
        /// <param name="item">픽업의 아이템 유형입니다.</param>
        /// <param name="number">
        /// 픽업에 포함된 아이템 개수입니다. 아이템이 쌓일 수 있는 경우에만 사용됩니다.
        /// </param>
        public void DropItem(InventoryItem item, int number)
        {
            SpawnPickup(item, GetDropLocation(), number);
        }

        /// <summary>
        /// 현재 위치에 픽업을 만듭니다.
        /// </summary>
        /// <param name="item">픽업의 아이템 유형입니다.</param>
        public void DropItem(InventoryItem item)
        {
            SpawnPickup(item, GetDropLocation(), 1);
        }

        // 보호된 메서드

        /// <summary>
        /// 드롭 위치를 지정하는 사용자 정의 방법을 설정하려면 재정의합니다.
        /// </summary>
        /// <returns>드롭이 생성될 위치입니다.</returns>
        protected virtual Vector3 GetDropLocation()
        {
            return transform.position;
        }

        // 개인 메서드

        public void SpawnPickup(InventoryItem item, Vector3 spawnLocation, int number)
        {
            var pickup = item.SpawnPickup(spawnLocation, number);
            droppedItems.Add(pickup);
        }

        [System.Serializable]
        private struct DropRecord
        {
            public string itemID;
            public SerializableVector3 position;
            public int number;
        }

        object ISaveable.CaptureState()
        {
            RemoveDestroyedDrops();
            var droppedItemsList = new DropRecord[droppedItems.Count];
            for (int i = 0; i < droppedItemsList.Length; i++)
            {
                droppedItemsList[i].itemID = droppedItems[i].GetItem().GetItemID();
                droppedItemsList[i].position = new SerializableVector3(droppedItems[i].transform.position);
                droppedItemsList[i].number = droppedItems[i].GetNumber();
            }
            return droppedItemsList;
        }

        void ISaveable.RestoreState(object state)
        {
            var droppedItemsList = (DropRecord[])state;
            foreach (var item in droppedItemsList)
            {
                var pickupItem = InventoryItem.GetFromID(item.itemID);
                Vector3 position = item.position.ToVector();
                int number = item.number;
                SpawnPickup(pickupItem, position, number);
            }
        }

        /// <summary>
        /// 이후에 픽업된 드롭을 제거합니다.
        /// </summary>
        private void RemoveDestroyedDrops()
        {
            var newList = new List<Pickup>();
            foreach (var item in droppedItems)
            {
                if (item != null)
                {
                    newList.Add(item);
                }
            }
            droppedItems = newList;
        }
    }

}

