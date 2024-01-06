using Lesson_11;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lesson_14
{
    public class HealthDisplay : MonoBehaviour
    {
        [SerializeField] private Health health;
        [SerializeField] private Image healthBar;

        private void Update()
        {
            healthBar.fillAmount = (health.GetHealthPoints() / health.GetMaxHealthPoints());
        }
    }

}
