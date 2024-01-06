using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesson_27
{
    public class QuestCompletion : MonoBehaviour
    {
        [SerializeField] Quest quest;
        // 완료될 Quest 오브젝트를 직렬화 필드로 선언

        [SerializeField] string objective;
        // 완료될 목표(quest의 일부)의 이름을 직렬화 필드로 선언

        public void CompleteObjective()
        {
            QuestList questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
            // 'Player' 태그가 지정된 게임 오브젝트를 찾아 QuestList 컴포넌트를 가져옴
            // QuestList는 플레이어가 가지고 있는 퀘스트 목록을 관리하는 스크립트
            if (questList == null)
                return;
            questList.CompleteObjective(quest, objective);
            // QuestList에게 특정 Quest의 목표를 완료하도록 요청
        }
    }
}
