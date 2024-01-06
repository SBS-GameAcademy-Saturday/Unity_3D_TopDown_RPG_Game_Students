using Lesson_11;
using Lesson_17;
using Lesson_Common;
using RPG.Quests;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesson_27
{
    public class QuestList : MonoBehaviour, ISaveable, IPredicateEvaluator
    {
        List<QuestStatus> statuses = new List<QuestStatus>();
        // QuestStatus 객체를 저장하는 리스트, 퀘스트 상태 추적에 사용됩니다.

        public event Action onUpdate;
        // QuestList의 상태가 업데이트될 때 호출할 이벤트를 정의합니다.

        void Update()
        {
            CompleteObjectiveByPredicates();
        }

        public void AddQuest(Quest quest)
        {
            if (HasQuest(quest)) return;
            // 이미 퀘스트를 가지고 있는 경우 아무 작업도 수행하지 않습니다.

            QuestStatus newStatus = new QuestStatus(quest);
            // 새로운 QuestStatus 객체를 생성하여 새 퀘스트 상태를 추적합니다.

            statuses.Add(newStatus);
            // 새로운 퀘스트 상태를 리스트에 추가합니다.

            if (onUpdate != null)
            {
                onUpdate();
            }
            // 업데이트 이벤트를 호출하여 퀘스트 목록이 변경됐음을 알립니다.
        }

        public void CompleteObjective(Quest quest, string objective)
        {
            QuestStatus status = GetQuestStatus(quest);
            // 주어진 퀘스트에 대한 QuestStatus를 가져옵니다.
            if (status == null)
                return;
            status.CompleteObjective(objective);
            // 주어진 목표를 완료 상태로 표시합니다.

            if (status.IsComplete())
            {
                GiveReward(quest);
                // 퀘스트의 모든 목표가 완료되었을 경우 보상을 주는 메서드를 호출합니다.
            }

            if (onUpdate != null)
            {
                onUpdate();
            }
            // 업데이트 이벤트를 호출하여 퀘스트 목록이 변경됐음을 알립니다.
        }

        public bool HasQuest(Quest quest)
        {
            return GetQuestStatus(quest) != null;
            // 주어진 퀘스트가 목록에 있는지 확인합니다.
        }

        public IEnumerable<QuestStatus> GetStatuses()
        {
            return statuses;
            // 모든 QuestStatus 객체를 열거형으로 반환합니다.
        }

        private QuestStatus GetQuestStatus(Quest quest)
        {
            foreach (QuestStatus status in statuses)
            {
                if (status.GetQuest() == quest)
                {
                    return status;
                }
            }
            return null;
            // 주어진 퀘스트에 해당하는 QuestStatus를 반환하거나, 없을 경우 null을 반환합니다.
        }

        private void GiveReward(Quest quest)
        {
            foreach (var reward in quest.GetRewards())
            {
                bool success = GetComponent<Inventory>().AddToFirstEmptySlot(reward.item, reward.number);
                // 퀘스트의 보상 아이템을 플레이어 인벤토리에 추가합니다.
                // 만약 인벤토리에 공간이 부족하면 아이템을 드롭합니다.

                if (!success)
                {
                    GetComponent<ItemDropper>().DropItem(reward.item, reward.number);
                    // 아이템 드롭 컴포넌트를 사용하여 아이템을 드롭합니다.
                }
            }
        }

        private void CompleteObjectiveByPredicates()
        {
            foreach (QuestStatus status in statuses)
            {
                if (status.IsComplete()) continue;
                Quest quest = status.GetQuest();
                foreach(var objective in quest.GetObjectives())
                {
                    if(status.IsObjectiveComplete(objective.reference)) continue;
                    if(!objective.usesCondition) continue;
                    if(objective.completionCondition.Check(GetComponents<IPredicateEvaluator>()))
                    {
                        CompleteObjective(quest, objective.reference);
                    }
                }
            }
        }

        public object CaptureState()
        {
            List<object> state = new List<object>();
            foreach (QuestStatus status in statuses)
            {
                state.Add(status.CaptureState());
                // QuestStatus의 상태를 캡처하여 리스트에 추가합니다.
            }
            return state;
            // 퀘스트 목록의 상태를 캡처한 리스트를 반환합니다.
        }

        public void RestoreState(object state)
        {
            List<object> stateList = state as List<object>;
            if (stateList == null) return;

            statuses.Clear();
            foreach (object objectState in stateList)
            {
                statuses.Add(new QuestStatus(objectState));
                // 저장된 상태 정보를 사용하여 QuestStatus 객체를 복원합니다.
            }
        }

        public bool? Evaluate(string predicate, string[] parameters)
        {
            switch (predicate)
            {
                case "HasQuest":
                    return HasQuest(Quest.GetByName(parameters[0]));
                // 주어진 퀘스트가 목록에 있는지 확인합니다.

                case "CompletedQuest":
                    return GetQuestStatus(Quest.GetByName(parameters[0])).IsComplete();
                    // 주어진 퀘스트가 완료되었는지 확인합니다.
            }

            return null;
            // 다른 경우에는 평가 결과가 없음 (null) 을 반환합니다.
        }
    }
}
