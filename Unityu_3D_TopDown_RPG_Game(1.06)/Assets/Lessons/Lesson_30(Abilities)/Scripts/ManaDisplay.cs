using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lesson_30
{
    public class ManaDisplay : MonoBehaviour
    {
        [SerializeField] private Mana mana;
        [SerializeField] private Image manaBar;
        private void Update()
        {
            manaBar.fillAmount = (mana.GetMana() / mana.GetMaxMana());
        }
    }
}
