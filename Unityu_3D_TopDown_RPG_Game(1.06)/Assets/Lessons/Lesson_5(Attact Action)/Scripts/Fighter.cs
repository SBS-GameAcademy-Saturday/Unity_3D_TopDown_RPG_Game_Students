using Lesson_4;
using Lesson_Common;
using System;
using UnityEngine;

namespace Lesson_5
{
    public class Fighter : MonoBehaviour,IAction
    {
        [SerializeField] float timeBetweenAttacks = 1f;

        Animator _animator;
        ActionScheduler actionScheduler;
        Mover mover;
        float timeSinceLastAttack = 0;
        GameObject target = null;

        // Start is called before the first frame update
        void Awake()
        {
            _animator = GetComponent<Animator>();
            actionScheduler = GetComponent<ActionScheduler>();
            mover = GetComponent<Mover>();
        }

        // Update is called once per frame
        void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (!target) return;

            if (!GetIsInRange(target.transform))
            {
                mover.MoveTo(target.transform.position, 1f);
            }
            else
            {
                mover.Cancle();
                AttackBehaviour();
            }
        }



        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                // This will trigger the Hit() event.
                TriggerAttack();
                timeSinceLastAttack = 0;
            }
        }

        public void Attack(GameObject combatTarget)
        {
            actionScheduler.StartAction(this);
            target = combatTarget;
        }

        public void Hit()
        {

        }

        private void TriggerAttack()
        {
            _animator.ResetTrigger("StopAttack");
            _animator.SetTrigger("Attack");
        }

        private void StopAttack()
        {
            _animator.ResetTrigger("Attack");
            _animator.SetTrigger("StopAttack");
        }

        public void Cancle()
        {
            StopAttack();
            target = null;
            mover.Cancle();
        }
        private bool GetIsInRange(Transform targetTransform)
        {
            return Vector3.Distance(transform.position, targetTransform.position) < 2;
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) { return false; }
            if (!mover.CanMoveTo(combatTarget.transform.position))
            {
                return false;
            }
            return true;
        }
    }
}
