using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesson_30
{
    [CreateAssetMenu(fileName = "Delay Composite Effect", menuName = "Lesson/Effects/DelayCompositeEffect", order = 0)]
    public class DelayCompositeEffect : EffectStrategy
    {
        [SerializeField] float delay = 0;
        [SerializeField] EffectStrategy[] delayedEffects;
        [SerializeField] bool abortIfCancled = false;
        public override void StartEffect(AbilityData data, Action finished)
        {
            data.StartCoroutine(DelayedEffects(data, finished));
        }

        private IEnumerator DelayedEffects(AbilityData data, Action finished)
        {
            yield return new WaitForSeconds(delay);

            if (abortIfCancled && data.IsCancled()) yield break;

            foreach (var effect in delayedEffects)
            {
                effect.StartEffect(data, finished);
            }
            finished?.Invoke();
        }
    }
}