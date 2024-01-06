using Lesson_17;
using Lesson_25;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Lesson_27
{
    [CreateAssetMenu(fileName = "Quest", menuName = "Lesson/Quest", order = 0)]
    // 새로운 Quest 오브젝트를 Unity 에셋 메뉴에서 생성할 수 있도록 설정
    // 오브젝트 이름과 메뉴 이름을 정의하며, 순서를 지정합니다.

    public class Quest : ScriptableObject
    {
        [SerializeField] List<Objective> objectives = new List<Objective>();
        [SerializeField] List<Reward> rewards = new List<Reward>();
        // Quest에 속한 Objectives(목표)와 Rewards(보상)을 저장하는 리스트

        [System.Serializable]
        public class Reward
        {
            [Min(1)]
            public int number;
            public InventoryItem item;
        }
        // 보상 클래스 정의
        // 'number' 필드는 최소값이 1인 정수로 정의됨
        // 'item' 필드는 InventoryItem 타입의 보상 아이템을 나타냅니다.

        [System.Serializable]
        public class Objective
        {
            public string reference;
            public string description;
            public bool usesCondition = false;
            public Condition completionCondition;
        }
        // 목표 클래스 정의
        // 'reference' 필드는 목표를 고유하게 식별하는 문자열
        // 'description' 필드는 목표의 설명을 저장합니다.

        public string GetTitle()
        {
            return name;
        }
        // Quest의 타이틀을 반환하는 메서드

        public int GetObjectiveCount()
        {
            return objectives.Count;
        }
        // Quest에 속한 목표 개수를 반환하는 메서드

        public IEnumerable<Objective> GetObjectives()
        {
            return objectives;
        }
        // Quest에 속한 목표들을 열거형으로 반환하는 메서드

        public IEnumerable<Reward> GetRewards()
        {
            return rewards;
        }
        // Quest에 속한 보상들을 열거형으로 반환하는 메서드

        public bool HasObjective(string objectiveRef)
        {
            foreach (var objective in objectives)
            {
                if (objective.reference == objectiveRef)
                {
                    return true;
                }
            }
            return false;
        }
        // 특정 목표가 Quest에 존재하는지 확인하는 메서드

        public static Quest GetByName(string questName)
        {
            foreach (Quest quest in Resources.LoadAll<Quest>(""))
            {
                if (quest.name == questName)
                {
                    return quest;
                }
            }
            return null;
        }
        // 이름을 기반으로 Quest를 반환하는 정적 메서드
    }
}
