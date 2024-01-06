using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesson_27
{
    public class QuestGiver : MonoBehaviour
    {
        [SerializeField] Quest quest;

        public void GiveQuest()
        {
            QuestList questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
            if (questList == null)
                return;
            questList.AddQuest(quest);
        }

    }

}