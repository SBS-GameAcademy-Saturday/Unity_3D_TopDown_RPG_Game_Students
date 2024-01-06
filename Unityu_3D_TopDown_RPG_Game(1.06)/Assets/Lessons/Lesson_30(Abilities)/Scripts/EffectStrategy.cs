using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesson_30
{
    [CreateAssetMenu(fileName = "EffectStrategy", menuName = "Lesson/Strategies/EffectStrategy", order = 0)]
    public abstract class EffectStrategy : ScriptableObject
    {
        public abstract void StartEffect(AbilityData data, Action finished);
    }
}
