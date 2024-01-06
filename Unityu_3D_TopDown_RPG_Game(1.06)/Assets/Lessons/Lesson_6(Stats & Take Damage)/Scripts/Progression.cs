using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesson_Common
{
    // CreateAssetMenu 속성은 스크립트를 에셋으로 생성하기 위한 설정을 지정합니다.
    // 파일 이름, 메뉴 이름 및 순서를 설정합니다.
    [CreateAssetMenu(fileName = "Progression", menuName = "Lesson/Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        // characterClasses 배열은 직업 클래스와 그에 따른 스탯 정보를 저장합니다.
        [SerializeField] ProgressionCharacterClass[] characterClasses = null;

        // lookupTable은 스탯 및 직업 클래스에 대한 정보를 미리 계산하여 저장하는 사전(Dictionary)입니다.
        Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookupTable = null;

        // 특정 스탯의 특정 직업 클래스 및 레벨에 해당하는 스탯 값을 반환합니다.
        public float GetStat(Stat stat, CharacterClass characterClass, int level)
        {
            BuildLookup(); // lookupTable을 생성합니다.

            if (!lookupTable[characterClass].ContainsKey(stat))
                return 0;

            float[] levels = lookupTable[characterClass][stat]; // 미리 계산된 스탯 값을 가져옵니다.

            if(levels.Length == 0)
            {
                return 0;
            }

            if (levels.Length < level) 
            {
                return levels[levels.Length - 1];
            }

            return levels[level - 1]; // 해당 레벨의 스탯 값을 반환합니다.
        }

        // 특정 스탯의 특정 직업 클래스의 최대 레벨을 반환합니다.
        public int GetLevels(Stat stat, CharacterClass characterClass)
        {
            BuildLookup(); // lookupTable을 생성합니다.

            float[] levels = lookupTable[characterClass][stat]; // 미리 계산된 스탯 값을 가져옵니다.
            return levels.Length; // 스탯 배열의 길이, 즉 최대 레벨을 반환합니다.
        }

        // lookupTable을 빌드하고 계산된 정보를 저장합니다.
        private void BuildLookup()
        {
            if (lookupTable != null) return; // 이미 lookupTable이 생성되었으면 함수를 종료합니다.

            lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();

            // characterClasses 배열에 있는 직업 클래스 및 스탯 정보를 루프를 통해 처리합니다.
            foreach (ProgressionCharacterClass progressionClass in characterClasses)
            {
                var statLookupTable = new Dictionary<Stat, float[]>();

                // 직업 클래스에 대한 스탯 정보를 루프를 통해 처리합니다.
                foreach (ProgressionStat progressionStat in progressionClass.stats)
                {
                    statLookupTable[progressionStat.stat] = progressionStat.levels;
                }

                // 직업 클래스와 스탯 정보를 lookupTable에 저장합니다.
                lookupTable[progressionClass.characterClass] = statLookupTable;
            }
        }

        // 직업 클래스 정보를 저장하는 내부 클래스
        [System.Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass characterClass; // 직업 클래스
            public ProgressionStat[] stats; // 해당 직업 클래스의 스탯 정보 배열
        }

        // 스탯 정보를 저장하는 내부 클래스
        [System.Serializable]
        class ProgressionStat
        {
            public Stat stat; // 스탯
            public float[] levels; // 레벨별 스탯 값 배열
        }
    }

}