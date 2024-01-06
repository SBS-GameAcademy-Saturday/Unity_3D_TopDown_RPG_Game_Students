using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Lesson_30
{
    [CreateAssetMenu(fileName = "OrientToTargetEffect", menuName = "Lesson/Effects/OrientToTargetEffect", order = 0)]

    public class OrientToTargetEffect : EffectStrategy
    {        
        public override void StartEffect(AbilityData data, Action finished)
        {
            data.GetUser().transform.LookAt(data.GetTragetedPoint());
            finished?.Invoke();
        }
    }
}