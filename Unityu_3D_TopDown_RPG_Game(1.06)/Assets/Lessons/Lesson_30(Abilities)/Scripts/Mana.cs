using Lesson.Utils;
using Lesson_11;
using Lesson_Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesson_30
{
    public class Mana : MonoBehaviour, ISaveable
    {
        LazyValue<float> mana;

        private void Awake()
        {
            mana = new LazyValue<float>(GetMaxMana);
        }

        private void Update()
        {
            if(mana.value < GetMaxMana())
            {
                mana.value = Mathf.Min(mana.value + (GetManaRegenRate() * Time.deltaTime), GetMaxMana());
            }

        }

        public float GetMana()
        {
            return mana.value;
        }

        public float GetMaxMana()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Mana);
        }

        public float GetManaRegenRate()
        {
            return GetComponent<BaseStats>().GetStat(Stat.ManaRegenRate);
        }

        public bool UseMana(float manaToUse)
        {
            if (manaToUse > mana.value)
            {
                return false;
            }
            mana.value -= manaToUse;
            return true;
        }

        public object CaptureState()
        {
            return mana.value;
        }

        public void RestoreState(object state)
        {
            mana.value = (float)state;
        }
    }
}
