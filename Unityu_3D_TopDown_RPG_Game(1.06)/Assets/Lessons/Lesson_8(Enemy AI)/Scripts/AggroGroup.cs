using Lesson_12;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesson_Common
{
    public class AggroGroup : MonoBehaviour
    {

        [SerializeField] Fighter[] fighters;
        [SerializeField] bool activateOnStart = false;
        private void Start()
        {
            Activate(activateOnStart);
        }

        public void Activate(bool shouldActivate)
        {
            foreach (Fighter fighter in fighters)
            {
                CombatTarget target = fighter.GetComponent<CombatTarget>();
                if (target != null)
                {
                    target.enabled = shouldActivate;
                }
                fighter.enabled = shouldActivate;
            }
        }
    }

}

