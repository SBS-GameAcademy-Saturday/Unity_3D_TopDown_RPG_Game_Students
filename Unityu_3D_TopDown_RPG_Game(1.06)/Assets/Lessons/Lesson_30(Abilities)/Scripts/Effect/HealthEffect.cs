using Lesson_11;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesson_30
{
    [CreateAssetMenu(fileName = "HealthEffect", menuName = "Lesson/Effects/HealthEffect", order = 0)]
    public class HealthEffect : EffectStrategy
    {
        [SerializeField] float healthChange;
        public override void StartEffect(AbilityData data, Action finished)
        {
            foreach (var target in data.GetTargets())
            {
                var health = target.GetComponentInChildren<Health>();
                if (health)
                {
                    if(healthChange < 0)
                    {
                        health.TakeDamage(data.GetUser(), -healthChange);
                    }
                    else
                    {
                        health.Heal(healthChange);
                    }
                }
            }
            finished?.Invoke();
        }
    }
}

