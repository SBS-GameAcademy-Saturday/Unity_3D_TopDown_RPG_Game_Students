using Lesson_17;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesson_20
{
    [CreateAssetMenu(menuName = ("Lesson/InventorySystem/Drop Library"))]
    public class DropLibrary : ScriptableObject
    {
        [SerializeField]
        DropConfig[] potentialDrops; // ������ ��� ������ ���� �迭
        [SerializeField] float[] dropChancePercentage; // ��� Ȯ��(�����) �迭
        [SerializeField] int[] minDrops; // �ּ� ��� ������ �� �迭
        [SerializeField] int[] maxDrops; // �ִ� ��� ������ �� �迭

        [System.Serializable]
        class DropConfig
        {
            public InventoryItem item; // �κ��丮 ������ ����
            public float[] relativeChance; // ����� ��� Ȯ�� �迭
            public int[] minNumber; // �ּ� ������ �� �迭
            public int[] maxNumber; // �ִ� ������ �� �迭

            // ������ ������ ���� ������ ������ ���� ��ȯ�մϴ�
            public int GetRandomNumber(int level)
            {
                if (!item.IsStackable()) // �������� ��ø �������� ���� ���
                {
                    return 1;
                }
                int min = GetByLevel(minNumber, level);
                int max = GetByLevel(maxNumber, level);
                return UnityEngine.Random.Range(min, max + 1);
            }
        }

        public struct Dropped
        {
            public InventoryItem item; // �κ��丮 ������
            public int number; // ������ ��
        }

        // ������ ����� �������� ���� ������ �Լ�
        public IEnumerable<Dropped> GetRandomDrops(int level)
        {
            if (!ShouldRandomDrop(level)) // ������ ��� ���� Ȯ��
            {
                yield break;
            }
            for (int i = 0; i < GetRandomNumberOfDrops(level); i++)
            {
                yield return GetRandomDrop(level); // ������ �������� ��ȯ
            }
        }

        // ������ ����� �ؾ� �ϴ��� Ȯ���ϴ� �Լ�
        bool ShouldRandomDrop(int level)
        {
            return Random.Range(0, 100) < GetByLevel(dropChancePercentage, level); // Ȯ���� ������� ������ ��� ���� ����
        }

        // ������ ������ ���� �������� �Լ�
        int GetRandomNumberOfDrops(int level)
        {
            int min = GetByLevel(minDrops, level);
            int max = GetByLevel(maxDrops, level);
            return Random.Range(min, max);
        }

        // ������ �������� �������� �Լ�
        Dropped GetRandomDrop(int level)
        {
            var drop = SelectRandomItem(level); // ������ ������ ����
            var result = new Dropped();
            result.item = drop.item;
            result.number = drop.GetRandomNumber(level);
            return result;
        }

        // ������ ������ ���� �Լ�
        DropConfig SelectRandomItem(int level)
        {
            float totalChance = GetTotalChance(level);
            float randomRoll = Random.Range(0, totalChance);
            float chanceTotal = 0;
            foreach (var drop in potentialDrops)
            {
                chanceTotal += GetByLevel(drop.relativeChance, level);
                if (chanceTotal > randomRoll)
                {
                    return drop; // ������ ������ ����
                }
            }
            return null;
        }

        // ��ü Ȯ���� ����ϴ� �Լ�
        float GetTotalChance(int level)
        {
            float total = 0;
            foreach (var drop in potentialDrops)
            {
                total += GetByLevel(drop.relativeChance, level);
            }
            return total;
        }

        // ������ ������ �ش��ϴ� ���� �������� ���׸� �Լ�
        static T GetByLevel<T>(T[] values, int level)
        {
            if (values.Length == 0)
            {
                return default;
            }
            if (level > values.Length)
            {
                return values[values.Length - 1];
            }
            if (level <= 0)
            {
                return default;
            }
            return values[level - 1];
        }
    }


}
