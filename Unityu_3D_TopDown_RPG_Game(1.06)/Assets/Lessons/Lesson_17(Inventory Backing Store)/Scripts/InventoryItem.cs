using GameDevTV.Inventories;
using Lesson_29;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesson_17
{
    /// <summary>
    /// �κ��丮�� ���� �� �ִ� ��� �������� ��Ÿ���� ScriptableObject�Դϴ�.
    /// </summary>
    /// <remarks>
    /// �����δ� 'ActionItem' �Ǵ� 'EquipableItem'�� ���� ���� Ŭ������ ����ϴ� ���� �Ϲ����Դϴ�.
    /// </remarks>
    public abstract class InventoryItem : ScriptableObject, ISerializationCallbackReceiver
    {
        // ���� ������
        [Tooltip("���� �� �ҷ����⸦ ���� �ڵ� ���� UUID. �� UUID�� �����Ϸ��� �� �ʵ带 ����ϴ�.")]
        [SerializeField] string itemID = null;
        [Tooltip("UI�� ǥ�õ� ������ �̸�.")]
        [SerializeField] string displayName = null;
        [Tooltip("UI�� ǥ�õ� ������ ����.")]
        [SerializeField][TextArea] string description = null;
        [Tooltip("�κ��丮���� �� �������� ��Ÿ���� UI ������.")]
        [SerializeField] Sprite icon = null;
        [Tooltip("�� �������� ��ӵ� �� ������ ������.")]
        [SerializeField] Pickup pickup = null;
        [Tooltip("â�� ���Կ� ���� �������� ���� �� �ִ��� ����.")]
        [SerializeField] bool stackable = false;
        [SerializeField] float price;
        [SerializeField] ItemCategory category = ItemCategory.None;
        // ����
        static Dictionary<string, InventoryItem> itemLookupCache;

        // ���� �޼���

        /// <summary>
        /// UUID�κ��� �κ��丮 ������ �ν��Ͻ��� �����ɴϴ�.
        /// </summary>
        /// <param name="itemID">
        /// ���� �ν��Ͻ� ���� ���ӵǴ� ���ڿ� UUID�Դϴ�.
        /// </param>
        /// <returns>
        /// �ش� ID�� �ش��ϴ� �κ��丮 ������ �ν��Ͻ�.
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
                        Debug.LogError(string.Format("�κ��丮 �ý��ۿ� ���� �ߺ� ID�� �ִ� �� �����ϴ�: {0} �� {1}", itemLookupCache[item.itemID], item));
                        continue;
                    }

                    itemLookupCache[item.itemID] = item;
                }
            }

            if (itemID == null || !itemLookupCache.ContainsKey(itemID)) return null;
            return itemLookupCache[itemID];
        }

        /// <summary>
        /// �Ⱦ� ���� ������Ʈ�� ���忡 �����մϴ�.
        /// </summary>
        /// <param name="position">�Ⱦ��� ������ ��ġ.</param>
        /// <param name="number">�Ⱦ��� ��Ÿ���� ������ �ν��Ͻ� ��.</param>
        /// <returns>������ �Ⱦ� ������Ʈ�� ���� ����.</returns>
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

        // ���� �޼���

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            // �� �ʵ尡 ��� ������ �� UUID�� �����ϰ� �����մϴ�.
            if (string.IsNullOrWhiteSpace(itemID))
            {
                itemID = System.Guid.NewGuid().ToString();
            }
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            // ISerializationCallbackReceiver�� �ʿ������� �ƹ� �۾��� ������ �ʿ䰡 �����ϴ�.
        }
    }



}
