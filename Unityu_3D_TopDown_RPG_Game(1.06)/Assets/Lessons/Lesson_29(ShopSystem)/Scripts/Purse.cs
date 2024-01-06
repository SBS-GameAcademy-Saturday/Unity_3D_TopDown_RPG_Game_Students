using Lesson_11;
using Lesson_17;
using Lesson_33;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesson_29
{
    public class Purse : MonoBehaviour, ISaveable,IItemStore
    {
        [SerializeField] float startingBalance = 400.0f;

        float balance = 0;

        public event Action onChange;

        private void Awake()
        {
            balance = startingBalance;
        }

        public float GetBalance()
        {
            return balance;
        }

        public void UpdateBalance(float amount)
        {
            balance += amount;
            onChange?.Invoke();
        }

        public object CaptureState()
        {
            return balance;
        }

        public void RestoreState(object state)
        {
            balance = (float)state;
        }

        public int AddItem(InventoryItem item, int number)
        {
            if(item is CurrencyItem)
            {
                UpdateBalance(item.GetPrice() * number);
                return number;
            }
            return 0;
        }
    }

}
