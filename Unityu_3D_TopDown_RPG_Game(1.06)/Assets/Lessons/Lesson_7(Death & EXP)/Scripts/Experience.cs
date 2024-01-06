using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesson_7
{
    public class Experience : MonoBehaviour
    {
        [SerializeField] float experiencePoints = 0;

        public event Action onExperienceGained;
        public void GainExperience(float experience)
        {
            experiencePoints += experience;
            Debug.Log("EXP : " + experiencePoints);
            onExperienceGained();
        }

        public float GetPoints()
        {
            return experiencePoints;
        }
    }
}

