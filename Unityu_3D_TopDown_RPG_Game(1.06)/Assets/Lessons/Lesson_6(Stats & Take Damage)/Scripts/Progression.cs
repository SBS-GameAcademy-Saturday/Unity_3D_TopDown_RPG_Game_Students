using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesson_Common
{
    // CreateAssetMenu �Ӽ��� ��ũ��Ʈ�� �������� �����ϱ� ���� ������ �����մϴ�.
    // ���� �̸�, �޴� �̸� �� ������ �����մϴ�.
    [CreateAssetMenu(fileName = "Progression", menuName = "Lesson/Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        // characterClasses �迭�� ���� Ŭ������ �׿� ���� ���� ������ �����մϴ�.
        [SerializeField] ProgressionCharacterClass[] characterClasses = null;

        // lookupTable�� ���� �� ���� Ŭ������ ���� ������ �̸� ����Ͽ� �����ϴ� ����(Dictionary)�Դϴ�.
        Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookupTable = null;

        // Ư�� ������ Ư�� ���� Ŭ���� �� ������ �ش��ϴ� ���� ���� ��ȯ�մϴ�.
        public float GetStat(Stat stat, CharacterClass characterClass, int level)
        {
            BuildLookup(); // lookupTable�� �����մϴ�.

            if (!lookupTable[characterClass].ContainsKey(stat))
                return 0;

            float[] levels = lookupTable[characterClass][stat]; // �̸� ���� ���� ���� �����ɴϴ�.

            if(levels.Length == 0)
            {
                return 0;
            }

            if (levels.Length < level) 
            {
                return levels[levels.Length - 1];
            }

            return levels[level - 1]; // �ش� ������ ���� ���� ��ȯ�մϴ�.
        }

        // Ư�� ������ Ư�� ���� Ŭ������ �ִ� ������ ��ȯ�մϴ�.
        public int GetLevels(Stat stat, CharacterClass characterClass)
        {
            BuildLookup(); // lookupTable�� �����մϴ�.

            float[] levels = lookupTable[characterClass][stat]; // �̸� ���� ���� ���� �����ɴϴ�.
            return levels.Length; // ���� �迭�� ����, �� �ִ� ������ ��ȯ�մϴ�.
        }

        // lookupTable�� �����ϰ� ���� ������ �����մϴ�.
        private void BuildLookup()
        {
            if (lookupTable != null) return; // �̹� lookupTable�� �����Ǿ����� �Լ��� �����մϴ�.

            lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();

            // characterClasses �迭�� �ִ� ���� Ŭ���� �� ���� ������ ������ ���� ó���մϴ�.
            foreach (ProgressionCharacterClass progressionClass in characterClasses)
            {
                var statLookupTable = new Dictionary<Stat, float[]>();

                // ���� Ŭ������ ���� ���� ������ ������ ���� ó���մϴ�.
                foreach (ProgressionStat progressionStat in progressionClass.stats)
                {
                    statLookupTable[progressionStat.stat] = progressionStat.levels;
                }

                // ���� Ŭ������ ���� ������ lookupTable�� �����մϴ�.
                lookupTable[progressionClass.characterClass] = statLookupTable;
            }
        }

        // ���� Ŭ���� ������ �����ϴ� ���� Ŭ����
        [System.Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass characterClass; // ���� Ŭ����
            public ProgressionStat[] stats; // �ش� ���� Ŭ������ ���� ���� �迭
        }

        // ���� ������ �����ϴ� ���� Ŭ����
        [System.Serializable]
        class ProgressionStat
        {
            public Stat stat; // ����
            public float[] levels; // ������ ���� �� �迭
        }
    }

}