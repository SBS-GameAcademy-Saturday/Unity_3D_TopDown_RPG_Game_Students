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
        DropConfig[] potentialDrops; // 가능한 드롭 아이템 설정 배열
        [SerializeField] float[] dropChancePercentage; // 드롭 확률(백분율) 배열
        [SerializeField] int[] minDrops; // 최소 드롭 아이템 수 배열
        [SerializeField] int[] maxDrops; // 최대 드롭 아이템 수 배열

        [System.Serializable]
        class DropConfig
        {
            public InventoryItem item; // 인벤토리 아이템 설정
            public float[] relativeChance; // 상대적 드롭 확률 배열
            public int[] minNumber; // 최소 아이템 수 배열
            public int[] maxNumber; // 최대 아이템 수 배열

            // 지정된 레벨에 대한 무작위 아이템 수를 반환합니다
            public int GetRandomNumber(int level)
            {
                if (!item.IsStackable()) // 아이템이 중첩 가능하지 않은 경우
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
            public InventoryItem item; // 인벤토리 아이템
            public int number; // 아이템 수
        }

        // 무작위 드롭을 가져오는 열거 가능한 함수
        public IEnumerable<Dropped> GetRandomDrops(int level)
        {
            if (!ShouldRandomDrop(level)) // 무작위 드롭 여부 확인
            {
                yield break;
            }
            for (int i = 0; i < GetRandomNumberOfDrops(level); i++)
            {
                yield return GetRandomDrop(level); // 무작위 아이템을 반환
            }
        }

        // 무작위 드롭을 해야 하는지 확인하는 함수
        bool ShouldRandomDrop(int level)
        {
            return Random.Range(0, 100) < GetByLevel(dropChancePercentage, level); // 확률을 기반으로 무작위 드롭 여부 결정
        }

        // 무작위 아이템 수를 가져오는 함수
        int GetRandomNumberOfDrops(int level)
        {
            int min = GetByLevel(minDrops, level);
            int max = GetByLevel(maxDrops, level);
            return Random.Range(min, max);
        }

        // 무작위 아이템을 가져오는 함수
        Dropped GetRandomDrop(int level)
        {
            var drop = SelectRandomItem(level); // 무작위 아이템 선택
            var result = new Dropped();
            result.item = drop.item;
            result.number = drop.GetRandomNumber(level);
            return result;
        }

        // 무작위 아이템 선택 함수
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
                    return drop; // 무작위 아이템 선택
                }
            }
            return null;
        }

        // 전체 확률을 계산하는 함수
        float GetTotalChance(int level)
        {
            float total = 0;
            foreach (var drop in potentialDrops)
            {
                total += GetByLevel(drop.relativeChance, level);
            }
            return total;
        }

        // 지정된 레벨에 해당하는 값을 가져오는 제네릭 함수
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
