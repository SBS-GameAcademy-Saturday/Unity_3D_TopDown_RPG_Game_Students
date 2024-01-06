using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesson_30
{
    //[CreateAssetMenu(fileName = "TargetingStrategy", menuName = "Lesson/Strategies/TargetingStrategy", order = 0)]
    public abstract class TargetingStrategy : ScriptableObject
    {
        public abstract void StartTargeting(AbilityData data, Action finished);


    }
}
