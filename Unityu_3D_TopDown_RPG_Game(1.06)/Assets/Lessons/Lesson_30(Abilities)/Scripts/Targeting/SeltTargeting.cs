using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesson_30
{
    [CreateAssetMenu(fileName = "Selt Targeting", menuName = "Lesson/Abilities/Targeting/SeltTargeting", order = 0)]
    public class SeltTargeting : TargetingStrategy
    {
        public override void StartTargeting(AbilityData data, Action finished)
        {
            data.SetTargets(new GameObject[] { data.GetUser()});
            data.SetTragetedPoint(data.GetUser().transform.position);
            finished?.Invoke();
        }
    }
}