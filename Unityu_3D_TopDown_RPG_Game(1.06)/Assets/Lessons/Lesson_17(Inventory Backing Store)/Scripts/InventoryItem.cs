using GameDevTV.Inventories;
using Lesson_29;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesson_17
{
    /// <summary>
    /// 인벤토리에 넣을 수 있는 모든 아이템을 나타내는 ScriptableObject입니다.
    /// </summary>
    /// <remarks>
    /// 실제로는 'ActionItem' 또는 'EquipableItem'과 같은 하위 클래스를 사용하는 것이 일반적입니다.
    /// </remarks>
    public abstract class InventoryItem : ScriptableObject, ISerializationCallbackReceiver
    {
        // 설정 데이터
        [Tooltip("저장 및 불러오기를 위한 자동 생성 UUID. 새 UUID를 생성하려면 이 필드를 지웁니다.")]
        [SerializeField] string itemID = null;
        [Tooltip("UI에 표시될 아이템 이름.")]
        [SerializeField] string displayName = null;
        [Tooltip("UI에 표시될 아이템 설명.")]
        [SerializeField][TextArea] string description = null;
        [Tooltip("인벤토리에서 이 아이템을 나타내는 UI 아이콘.")]
        [SerializeField] Sprite icon = null;
        [Tooltip("이 아이템이 드롭될 때 생성될 프리팹.")]
        [SerializeField] Pickup pickup = null;
        [Tooltip("창고 슬롯에 여러 아이템을 쌓을 수 있는지 여부.")]
        [SerializeField] bool stackable = false;
        [SerializeField] float price;
        [SerializeField] ItemCategory category = ItemCategory.None;
        // 상태
        static Dictionary<string, InventoryItem> itemLookupCache;

        // 공개 메서드

        /// <summary>
        /// UUID로부터 인벤토리 아이템 인스턴스를 가져옵니다.
        /// </summary>
        /// <param name="itemID">
        /// 게임 인스턴스 간에 지속되는 문자열 UUID입니다.
        /// </param>
        /// <returns>
        /// 해당 ID에 해당하는 인벤토리 아이템 인스턴스.
        /// </returns>
        public static InventoryItem GetFromID(string itemID)
        {
            if (itemLookupCache == null)
            {
                itemLookupCache = new Dictionary<string, InventoryItem>();
                var itemList = Resources.LoadAll<InventoryItem>("");
                foreach (var item in itemList)
                {
                    if (itemLookupCache.ContainsKey(item.itemID))
                    {
                        Debug.LogError(string.Format("인벤토리 시스템에 대한 중복 ID가 있는 것 같습니다: {0} 및 {1}", itemLookupCache[item.itemID], item));
                        continue;
                    }

                    itemLookupCache[item.itemID] = item;
                }
            }

            if (itemID == null || !itemLookupCache.ContainsKey(itemID)) return null;
            return itemLookupCache[itemID];
        }

        /// <summary>
        /// 픽업 게임 오브젝트를 월드에 스폰합니다.
        /// </summary>
        /// <param name="position">픽업을 스폰할 위치.</param>
        /// <param name="number">픽업이 나타내는 아이템 인스턴스 수.</param>
        /// <returns>스폰된 픽업 오브젝트에 대한 참조.</returns>
        public Pickup SpawnPickup(Vector3 position, int number)
        {
            var pickup = Instantiate(this.pickup);
            pickup.transform.position = position;
            pickup.Setup(this, number);
            return pickup;
        }

        public Sprite GetIcon()
        {
            return icon;
        }

        public string GetItemID()
        {
            return itemID;
        }

        public bool IsStackable()
        {
            return stackable;
        }

        public string GetDisplayName()
        {
            return displayName;
        }

        public string GetDescription()
        {
            return description;
        }

        public float GetPrice()
        {
            return price;
        }

        public ItemCategory GetCategory()
        {
            return category;
        }

        // 개인 메서드

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            // 이 필드가 비어 있으면 새 UUID를 생성하고 저장합니다.
            if (string.IsNullOrWhiteSpace(itemID))
            {
                itemID = System.Guid.NewGuid().ToString();
            }
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            // ISerializationCallbackReceiver에 필요하지만 아무 작업도 수행할 필요가 없습니다.
        }
    }



}
