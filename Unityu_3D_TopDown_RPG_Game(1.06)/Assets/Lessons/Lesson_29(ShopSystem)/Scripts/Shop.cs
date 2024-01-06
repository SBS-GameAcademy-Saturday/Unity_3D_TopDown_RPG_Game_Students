using Lesson_11;
using Lesson_17;
using Lesson_Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace Lesson_29
{
    public class Shop : MonoBehaviour, IRaycastable,ISaveable
    {
        [SerializeField] string shopName;
        [Range(0, 100)]
        [SerializeField] float sellingDiscount = 80f;
        [SerializeField] float maximumBarterDiscount = 80f;
        // Stock Config
        //Item:
        //InventoryItem
        //Initial Stock
        //BuyingDiscount
        [SerializeField]
        StockItemConfig[] stockConfig;

        [Serializable]
        class StockItemConfig
        {
            public InventoryItem item;
            public int initialStock;
            [Range(0,100)]
            public float buyingDiscountPercentage;
            public int levelToUnlock = 0;
        }

        Dictionary<InventoryItem,int> transaction = new Dictionary<InventoryItem,int>();
        Dictionary<InventoryItem,int> stockSold = new Dictionary<InventoryItem , int>();
        Shopper currentShopper;
        ItemCategory filter = ItemCategory.None;
        bool isBuyingMode = true;

        public event Action onChange;

        //private void Awake()
        //{
        //    foreach(StockItemConfig config in stockConfig)
        //    {
        //        stockSold[config.item] = config.initialStock;
        //    }
        //}

        public void SetShopper(Shopper shopper)
        {
            this.currentShopper = shopper;
        }

        public IEnumerable<ShopItem> GetAllItems()
        {
            //int shopperLevel = GetShopperLevel();

            Dictionary<InventoryItem,float> prices = GetPrices();
            Dictionary<InventoryItem, int> availabilities = GetAvailabilities();

            foreach (InventoryItem item in availabilities.Keys)
            {
                if (availabilities[item] <= 0) continue;

                float price = prices[item];
                int quantityInTransaction = 0;
                transaction.TryGetValue(item, out quantityInTransaction);
                int currentStock = availabilities[item];
                yield return new ShopItem(item, currentStock, price, quantityInTransaction);
            }

            //foreach (StockItemConfig config in stockConfig)
            //{
            //    if (config.levelToUnlock > shopperLevel) continue;

            //    float price = GetPrice(config);
            //    int quantityInTransaction = 0;
            //    transaction.TryGetValue(config.item, out quantityInTransaction);
            //    int currentStock = GetAvailability(config.item);
            //    yield return new ShopItem(config.item, currentStock, price , quantityInTransaction);
            //}
        }

        private Dictionary<InventoryItem, int> GetAvailabilities()
        {
            Dictionary<InventoryItem, int> availibilities = new Dictionary<InventoryItem, int>();

            foreach(var config in GetAvailableConfigs())
            {
                if (isBuyingMode)
                {
                    if (!availibilities.ContainsKey(config.item))
                    {
                        int sold = 0;
                        stockSold.TryGetValue(config.item, out sold);
                        availibilities[config.item] = -sold;
                    }
                    availibilities[config.item] += config.initialStock;
                }
                else
                {
                    availibilities[config.item] = CountItemInInventory(config.item);
                }
            }

            return availibilities;
        }

        private Dictionary<InventoryItem, float> GetPrices()
        {
            Dictionary<InventoryItem, float> prices = new Dictionary<InventoryItem, float>();

            foreach(var config in GetAvailableConfigs())
            {
                if (isBuyingMode)
                {
                    if (!prices.ContainsKey(config.item))
                    {
                        //                                                                  Lesson_31
                        prices[config.item] = config.item.GetPrice() * GetBarterDiscount();
                    }

                    prices[config.item] *= (1 - config.buyingDiscountPercentage / 100);
                }
                else
                {
                    prices[config.item] = config.item.GetPrice() * (sellingDiscount / 100);
                }

            }

            return prices;
        }

        //Lesson_31
        private float GetBarterDiscount()
        {
            BaseStats baseStats = currentShopper.GetComponent<BaseStats>();
            float discount = baseStats.GetStat(Stat.BuyingDiscountPercentage);
            return (1 -  Mathf.Min(discount, maximumBarterDiscount) / 100); 
        }

        private IEnumerable<StockItemConfig> GetAvailableConfigs()
        {
            int shopperLevel = GetShopperLevel();
            foreach(var config in stockConfig)
            {
                if (config.levelToUnlock > shopperLevel) continue;
                yield return config;
            }
        }

        //private float GetPrice(StockItemConfig config)
        //{
        //    if (isBuyingMode)
        //    {
        //        return config.item.GetPrice() * (1 - config.buyingDiscountPercentage / 100);
        //    }
        //    return config.item.GetPrice() * (sellingDiscount / 100);
        //}

        //private int GetAvailability(InventoryItem item)
        //{
        //    if (isBuyingMode)
        //    {
        //        return 0;
        //    }
        //    return CountItemInInventory(item);
        //}

        private int CountItemInInventory(InventoryItem item)
        {
            Inventory inventory = currentShopper.GetComponent<Inventory>();
            if (inventory == null) return 0;

            int total = 0;
            for(int i = 0; i < inventory.GetSize(); i++)
            {
                if(inventory.GetItemInSlot(i) == item)
                {
                    total += inventory.GetNumberInSlot(i);
                }
            }
            return total;
        }

        public IEnumerable<ShopItem> GetFilteredItems()
        {
            foreach(ShopItem shopItem in GetAllItems())
            {
                InventoryItem item = shopItem.GetInventoryItem();
                if (filter == ItemCategory.None || item.GetCategory() == filter)
                {
                    yield return shopItem;
                }
            }
        }

        public void SelectFilter(ItemCategory category) 
        {
            filter = category;


            onChange?.Invoke();
        }

        public ItemCategory GetFilter()
        {
            return filter;
        }

        public void SelectMode(bool isBuying)
        {
            isBuyingMode = isBuying;

            onChange?.Invoke();
        }

        public bool IsBuyingMode()
        {
            return isBuyingMode;
        }

        public bool CanTransact()
        {

            //Empty transaction
            if(IsTransactionEmpty()) return false;

            //Not sufficient funds
            if (!HasSufficientFunds()) return false;

            //Not sufficient inventory space
            if(!HasInventorySpace()) return false;

            return true;
        }

        public void ConfirmTrasaction() 
        {
            Inventory shopperInventory = currentShopper.GetComponent<Inventory>();
            Purse shopperPurse = currentShopper.GetComponent<Purse>();
            if (shopperInventory == null && shopperPurse == null) return;

            //Transfer to or from the inventory
            foreach(ShopItem shopItem in GetAllItems())
            {
                InventoryItem item = shopItem.GetInventoryItem();
                int quantity = shopItem.GetQuantityInTransaction();
                float price = shopItem.GetPrice();
                for(int i = 0; i < quantity; i++)
                {
                    if (isBuyingMode)
                    {
                        BuyItem(shopperInventory, shopperPurse, item, price);
                    }
                    else
                    {
                        SellItem(shopperInventory, shopperPurse, item, price);
                    }
                }
            }

            if (onChange != null)
                onChange();
        }

        private void BuyItem(Inventory shopperInventory, Purse shopperPurse, InventoryItem item, float price)
        {
            if (shopperPurse.GetBalance() < price)
                return;

            bool success = shopperInventory.AddToFirstEmptySlot(item, 1);
            if (success)
            {
                AddToTracnsaction(item, -1);
                if (!stockSold.ContainsKey(item))
                {
                    stockSold[item] = 0;
                }
                stockSold[item]++;
                shopperPurse.UpdateBalance(-price);

            }
        }
        private void SellItem(Inventory shopperInventory, Purse shopperPurse, InventoryItem item, float price)
        {
            int slot = FindFirstItemSlot(shopperInventory, item);
            if (slot == -1) return;

            AddToTracnsaction(item, 1);
            shopperInventory.RemoveFromSlot(slot, 1);
            if (!stockSold.ContainsKey(item))
            {
                stockSold[item] = 0;
            }
            stockSold[item]--;
            shopperPurse.UpdateBalance(price);
        }

        private int FindFirstItemSlot(Inventory shopperInventory, InventoryItem item)
        {
            for(int i = 0; i < shopperInventory.GetSize(); i++)
            {
                if(shopperInventory.GetItemInSlot(i) == item)
                {
                    return i;
                }
            }
            return -1;
        }

        public float TransactionTotal()
        {
            float total = 0;
            foreach(ShopItem item in GetAllItems())
            {
                total += item.GetPrice() * item.GetQuantityInTransaction();
            }
            return total;
        }
        public void AddToTracnsaction(InventoryItem item, int quantity)
        {
            if (transaction.ContainsKey(item) == false)
            {
                transaction[item] = 0;
            }

            var availabilities = GetAvailabilities();
            int availability = availabilities[item];
            if (transaction[item] + quantity > availability)
            {
                transaction[item] = availability;
            }
            else
            {
                transaction[item] += quantity;
            }

            //transaction[item] += quantity;

            if (transaction[item] <= 0)
            {
                transaction.Remove(item);
            }

            if (onChange != null)
                onChange();
        }

        public CursorType GetCursorType()
        {
            return CursorType.Shop;
        }

        public bool HandleRaycast(Lesson_5.PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                callingController.GetComponent<Shopper>().SetActiveShop(this);
            }
            return true;
        }

        public bool HandleRaycast(Lesson_11.PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                callingController.GetComponent<Shopper>().SetActiveShop(this);
            }
            return true;
        }

        public string GetShopName()
        {
            return shopName;
        }


        public bool HasSufficientFunds()
        {
            if (!isBuyingMode) return true;

            Purse purse = currentShopper.GetComponent<Purse>();
            if(!purse) return false;
            return purse.GetBalance() >= TransactionTotal();
        }

        public bool IsTransactionEmpty()
        {
            return transaction.Count == 0;
        }

        private bool HasInventorySpace()
        {
            if (!isBuyingMode) return true;

            Inventory shopperInventory = currentShopper.GetComponent<Inventory>();
            if (shopperInventory == null) return false;

            List<InventoryItem> flatItems = new List<InventoryItem>();
            foreach(ShopItem shopItem in GetAllItems())
            {
                InventoryItem item = shopItem.GetInventoryItem();
                int quantity = shopItem.GetQuantityInTransaction();
                for(int i = 0; i < quantity; i++)
                {
                    flatItems.Add(item);
                }
            }
            return shopperInventory.HasSpaceFor(flatItems);
        }

        private int GetShopperLevel()
        {
            BaseStats stats = currentShopper.GetComponent<BaseStats>();
            if(!stats) return 0;

            return stats.GetLevel();
        }

        public object CaptureState()
        {
            Dictionary<string, int> saveObject = new Dictionary<string, int>();
            foreach(var pair in stockSold)
            {
                saveObject[pair.Key.GetItemID()] = pair.Value;
            }
            return saveObject;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, int> saveObject = (Dictionary<string, int>)state;
            stockSold.Clear();
            foreach(var pair in saveObject)
            {
                stockSold[InventoryItem.GetFromID(pair.Key)] = pair.Value; 
            }
        }
    }
}