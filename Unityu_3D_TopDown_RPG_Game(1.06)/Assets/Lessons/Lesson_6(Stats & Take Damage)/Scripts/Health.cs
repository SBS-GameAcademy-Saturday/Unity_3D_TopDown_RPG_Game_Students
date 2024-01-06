using Lesson.Utils;
using Lesson_Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Lesson_6
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float regenerationPercentage = 70;
        [SerializeField] TakeDamageEvent takeDamage;
        [SerializeField] UnityEvent onDie;

        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {
        }
        ActionScheduler actionScheduler;
        LazyValue<float> healthPoints;
        bool isDead = false;

        private void Awake()
        {
            healthPoints = new LazyValue<float>(GetInitialHealth);
            actionScheduler = GetComponent<ActionScheduler>();
        }

        private float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        // Start is called before the first frame update
        void Start()
        {
            healthPoints.ForceInit();
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            healthPoints.value = Mathf.Max(healthPoints.value - damage, 0);
            Debug.Log("Health : " + healthPoints.value);
            if (healthPoints.value == 0)
            {
                onDie?.Invoke();
                Die();
            }
            else
            {
                takeDamage.Invoke(damage);
            }
        }

        private void Die()
        {
            if (isDead) return;
            Debug.Log("Die");
            isDead = true;
            actionScheduler?.CancleCurrentAction();
        }
    }
}
