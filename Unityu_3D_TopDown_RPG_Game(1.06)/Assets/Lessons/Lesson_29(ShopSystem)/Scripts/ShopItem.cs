using Lesson_17;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Lesson_29
{
    public class ShopItem
    {
        InventoryItem item;
        int availability;
        float price;
        int qurantityInTransaction;

        public ShopItem(InventoryItem item, int availability, float price, int qurantityInTransaction)
        {
            this.item = item;
            this.availability = availability;
            this.price = price;
            this.qurantityInTransaction = qurantityInTransaction;
        }

        public string GetName()
        {
            return item.GetDisplayName();
        }

        public int GetAvailability()
        {
            return availability;
        }

        public Sprite GetIcon()
        {
            return item.GetIcon();
        }

        public float GetPrice()
        {
            return price;
        }

        public int GetQuantityInTransaction()
        {
            return qurantityInTransaction;
        }

        public InventoryItem GetInventoryItem()
        {
            return item;
        }
    }
}