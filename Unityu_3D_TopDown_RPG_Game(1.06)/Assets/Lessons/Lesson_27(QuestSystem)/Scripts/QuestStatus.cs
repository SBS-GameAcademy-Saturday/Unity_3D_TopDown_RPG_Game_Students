using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesson_27
{
    public class QuestStatus
    {
        Quest quest;
        // 연결된 퀘스트 객체

        List<string> completedObjectives = new List<string>();
        // 완료된 목표(quest의 일부)를 추적하는 리스트

        [System.Serializable]
        class QuestStatusRecord
        {
            public string questName;
            public List<string> completedObjectives;
            // 직렬화 가능한 퀘스트 상태 레코드 클래스
        }

        public QuestStatus(Quest quest)
        {
            this.quest = quest;
            // 주어진 퀘스트에 대한 QuestStatus 객체를 생성

        }

        public QuestStatus(object objectState)
        {
            QuestStatusRecord state = objectState as QuestStatusRecord;
            quest = Quest.GetByName(state.questName);
            completedObjectives = state.completedObjectives;
            // 저장된 퀘스트 상태를 사용하여 QuestStatus 객체를 생성
        }

        public Quest GetQuest()
        {
            return quest;
            // 연결된 퀘스트 객체를 반환
        }

        public bool IsComplete()
        {
            foreach (var objective in quest.GetObjectives())
            {
                if (!completedObjectives.Contains(objective.reference))
                {
                    return false;
                }
            }
            return true;
            // 퀘스트의 모든 목표가 완료되었는지 확인
        }

        public int GetCompletedCount()
        {
            return completedObjectives.Count;
            // 완료된 목표 개수를 반환
        }

        public bool IsObjectiveComplete(string objective)
        {
            return completedObjectives.Contains(objective);
            // 특정 목표가 완료되었는지 확인
        }

        public void CompleteObjective(string objective)
        {
            if (quest.HasObjective(objective))
            {
                completedObjectives.Add(objective);
            }
            // 목표를 완료 상태로 표시 (목표가 퀘스트에 있는 경우)
        }

        public object CaptureState()
        {
            QuestStatusRecord state = new QuestStatusRecord();
            state.questName = quest.name;
            state.completedObjectives = completedObjectives;
            return state;
            // 퀘스트 상태를 직렬화 가능한 레코드로 캡처
        }
    }
}
