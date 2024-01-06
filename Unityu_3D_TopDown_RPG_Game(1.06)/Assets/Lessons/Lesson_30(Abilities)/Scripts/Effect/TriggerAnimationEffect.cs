using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Lesson_30
{
    [CreateAssetMenu(fileName = "Trigger Animation Effect", menuName = "Lesson/Effects/TriggerAnimationEffect", order = 0)]

    public class TriggerAnimationEffect : EffectStrategy
    {
        [SerializeField] private string animationTrigger;
        
        public override void StartEffect(AbilityData data, Action finished)
        {
            Animator animator = data.GetUser().GetComponent<Animator>();
            animator.SetTrigger(animationTrigger);
            finished?.Invoke();
        }
    }
}