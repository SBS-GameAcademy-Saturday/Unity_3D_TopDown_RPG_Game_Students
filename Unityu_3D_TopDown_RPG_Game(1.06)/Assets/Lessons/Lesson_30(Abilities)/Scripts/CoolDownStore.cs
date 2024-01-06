using Lesson_17;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;

namespace Lesson_30
{
    public class CoolDownStore : MonoBehaviour
    {
        Dictionary<InventoryItem, float> cooldownTimers = new Dictionary<InventoryItem,float>();
        Dictionary<InventoryItem, float> initialcooldownTimers = new Dictionary<InventoryItem, float>();

        private void Update()
        {
            var keys = new List<InventoryItem>(cooldownTimers.Keys);
            foreach(InventoryItem ability in keys)
            {
                cooldownTimers[ability] -= Time.deltaTime;
                if (cooldownTimers[ability] < 0)
                {
                    cooldownTimers.Remove(ability);
                    initialcooldownTimers.Remove(ability);
                }
            }
        }


        public void StartCoolDown(InventoryItem ability , float cooldownTime)
        {
            cooldownTimers[ability] = cooldownTime;
            initialcooldownTimers[ability] = cooldownTime;
        }

        public float GetTimeRemaining(InventoryItem ability)
        {
            if (!cooldownTimers.ContainsKey(ability))
            {
                return 0;
            }
            return cooldownTimers[ability];
        }

        public float GetFractionRemaining(InventoryItem ability)
        {
            if (ability == null)
                return 0;
            if (!cooldownTimers.ContainsKey(ability))
            {
                return 0;
            }
            return cooldownTimers[ability] / initialcooldownTimers[ability];
        }
    }
}

