using Lesson_17;
using Lesson_Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesson_30
{
    [CreateAssetMenu(fileName ="Ability",menuName = "Lesson/Abilities/Ability",order = 0)]
    public class Ability : ActionItem
    {
        [SerializeField] TargetingStrategy targetingStrategy;
        [SerializeField] FilterStrategy[] filterStrategies;
        [SerializeField] EffectStrategy[] effectStrategies;
        [SerializeField] private float cooldownTime;
        [SerializeField] float manaCost = 0;

        CoolDownStore coolDownStore;
        Mana mana;
        public override bool Use(GameObject user)
        {
            base.Use(user);

            if (!mana)
                mana = user.GetComponent<Mana>();
            if(mana.GetMana() < manaCost)
                return false;

            if (!coolDownStore)
                coolDownStore =  user.GetComponent<CoolDownStore>();
            if (coolDownStore.GetTimeRemaining(this) > 0)
                return false;

            AbilityData data = new AbilityData(user);

            ActionScheduler actionScheduler= user.GetComponent<ActionScheduler>();
            actionScheduler.StartAction(data);

            if (targetingStrategy != null)
                targetingStrategy.StartTargeting(data, () =>
                {
                    TargetAquired(data);
                });
            return true;
        }

        private void TargetAquired(AbilityData data)
        {
            if (data.IsCancled()) return;

            if(!mana.UseMana(manaCost))
                return;

            coolDownStore.StartCoolDown(this,cooldownTime);



            foreach (var filterStrategy in filterStrategies)
            {
                data.SetTargets ( filterStrategy.Filter(data.GetTargets()));
            }

            foreach(var effect in effectStrategies)
            {
                effect.StartEffect(data, EffectFinished);
            }
        }

        private void EffectFinished()
        {

        }
    }
}
