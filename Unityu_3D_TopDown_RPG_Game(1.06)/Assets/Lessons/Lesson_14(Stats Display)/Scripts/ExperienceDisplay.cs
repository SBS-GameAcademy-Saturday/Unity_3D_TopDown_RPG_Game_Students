using Lesson_11;
using Lesson_Common;
using UnityEngine;
using UnityEngine.UI;

namespace Lesson_14
{
    public class ExperienceDisplay : MonoBehaviour
    {
        [SerializeField] private Experience experience;
        [SerializeField] private Progression progression;
        [SerializeField] private BaseStats baseStats;
        [SerializeField] private Image experienceBar;

        // Update is called once per frame
        void Update()
        {
            int penultimateLevel = progression.GetLevels(Stat.ExperienceToLevelUp, CharacterClass.Player);
            float XPToLevelUp = progression.GetStat(Stat.ExperienceToLevelUp, CharacterClass.Player, baseStats.GetLevel());
            experienceBar.fillAmount = experience.GetPoints() / XPToLevelUp;
        }
    }
}