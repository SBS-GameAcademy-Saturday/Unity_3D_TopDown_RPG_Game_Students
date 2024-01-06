using Lesson.Utils;
using Lesson_Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesson_6
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;
        [SerializeField] bool shouldUseModifiers = false;

        LazyValue<int> currentLevel;

        private void Awake()
        {
            currentLevel = new LazyValue<int>(CalculateLevel);
        }

        // Start is called before the first frame update
        void Start()
        {
            currentLevel.ForceInit();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public float GetStat(Stat stat)
        {
            return (GetBaseStat(stat));
        }

        private float GetBaseStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        public int GetLevel()
        {
            return currentLevel.value;
        }
        private int CalculateLevel()
        {
            return startingLevel;
        }
    }

}
