using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesson_11
{
    public class Experience : MonoBehaviour,ISaveable
    {
        [SerializeField] float experiencePoints = 0;

        public event Action onExperienceGained;

        private void Update()
        {
            if (Input.GetKey(KeyCode.E))
            {
                GainExperience(Time.deltaTime * 100);
            }
        }

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
        public object CaptureState()
        {
            return experiencePoints;
        }

        public void RestoreState(object state)
        {
            experiencePoints = (float)state;
        }
    }
}

