using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lesson_29
{
    public class RowUI : MonoBehaviour
    {
        [SerializeField]
        Image iconField;
        [SerializeField]
        Text nameField;
        [SerializeField]
        Text availabilityField;
        [SerializeField]
        Text priceField;
        [SerializeField]
        Text qurantityInTransactionField;

        private Shop currentShop = null;
        private ShopItem item = null;
        public void Setup(Shop currentShyop,ShopItem item)
        {
            this.currentShop = currentShyop;
            this.item = item;

            iconField.sprite = item.GetIcon();
            nameField.text = item.GetName();
            availabilityField.text = $"{item.GetAvailability()}";
            priceField.text = $"${item.GetPrice():N2}";
            qurantityInTransactionField.text = $"{item.GetQuantityInTransaction()}";
        }

        public void Add()
        {
            currentShop.AddToTracnsaction(item.GetInventoryItem(), 1);
        }

        public void Remove()
        {
            currentShop.AddToTracnsaction(item.GetInventoryItem(), -1);
        }
    }

}
