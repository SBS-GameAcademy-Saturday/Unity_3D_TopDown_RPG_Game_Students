using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesson_30
{
    [CreateAssetMenu(fileName = "DemoTargeting", menuName = "Lesson/Abilities/Targeting/DemoTargeting", order = 0)]
    public class DemoTargeting : TargetingStrategy
    {
        public override void StartTargeting(AbilityData data, Action finished)
        {
            Debug.Log("Stating Target");
            finished?.Invoke();
        }
    }

}
