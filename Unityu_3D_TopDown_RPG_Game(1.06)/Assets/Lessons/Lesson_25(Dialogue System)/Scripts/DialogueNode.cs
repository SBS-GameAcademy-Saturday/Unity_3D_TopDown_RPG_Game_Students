using Lesson_Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Lesson_25
{
    public class DialogueNode : ScriptableObject
    {
        [SerializeField]
        bool isPlayerSpeaking = false; // 이 노드가 플레이어가 말하는 노드인지 여부를 나타내는 불리언 변수
        [SerializeField]
        string text; // 대화 내용을 저장하는 문자열 변수
        [SerializeField]
        List<string> children = new List<string>(); // 이 노드와 연결된 자식 노드의 ID 목록을 저장하는 리스트
        [SerializeField]
        Rect rect = new Rect(0, 0, 200, 100); // 이 노드의 위치와 크기를 나타내는 사각형
        [SerializeField]
        string onEnterAction; // 노드 진입 시 실행할 액션을 나타내는 문자열
        [SerializeField]
        string onExitAction; // 노드 종료 시 실행할 액션을 나타내는 문자열
        [SerializeField]
        Condition condition; // 조건 객체, 대화 노드의 활성화 조건을 나타냄

        public Rect GetRect()
        {
            return rect; // 이 노드의 사각형 정보 반환
        }

        public string GetText()
        {
            return text; // 이 노드의 대화 내용 반환
        }

        public List<string> GetChildren()
        {
            return children; // 이 노드와 연결된 자식 노드 ID 목록 반환
        }

        public bool IsPlayerSpeaking()
        {
            return isPlayerSpeaking; // 이 노드가 플레이어가 말하는 노드인지 여부 반환
        }

        public string GetOnEnterAction()
        {
            return onEnterAction; // 노드 진입 시 실행할 액션 반환
        }

        public string GetOnExitAction()
        {
            return onExitAction; // 노드 종료 시 실행할 액션 반환
        }

        public bool CheckCondition(IEnumerable<IPredicateEvaluator> evaluators)
        {
            return condition.Check(evaluators); // 주어진 조건을 평가하고 결과를 반환
        }

#if UNITY_EDITOR
        public void SetPosition(Vector2 newPosition)
        {
            Undo.RecordObject(this, "Move Dialogue Node"); // 에디터에서 이동 작업을 기록
            rect.position = newPosition; // 노드의 위치 업데이트
            EditorUtility.SetDirty(this); // 변경 내용을 에디터에 알림
        }

        public void SetText(string newText)
        {
            if (newText != text)
            {
                Undo.RecordObject(this, "Update Dialogue Text"); // 에디터에서 텍스트 업데이트 작업을 기록
                text = newText; // 대화 내용 업데이트
                EditorUtility.SetDirty(this); // 변경 내용을 에디터에 알림
            }
        }

        public void AddChild(string childID)
        {
            Undo.RecordObject(this, "Add Dialogue Link"); // 에디터에서 연결 추가 작업을 기록
            children.Add(childID); // 자식 노드 ID 추가
            EditorUtility.SetDirty(this); // 변경 내용을 에디터에 알림
        }

        public void RemoveChild(string childID)
        {
            Undo.RecordObject(this, "Remove Dialogue Link"); // 에디터에서 연결 제거 작업을 기록
            children.Remove(childID); // 자식 노드 ID 제거
            EditorUtility.SetDirty(this); // 변경 내용을 에디터에 알림
        }

        public void SetPlayerSpeaking(bool newIsPlayerSpeaking)
        {
            Undo.RecordObject(this, "Change Dialogue Speaker"); // 에디터에서 대화 스피커 변경 작업을 기록
            isPlayerSpeaking = newIsPlayerSpeaking; // 플레이어가 말하는 노드 여부 업데이트
            EditorUtility.SetDirty(this); // 변경 내용을 에디터에 알림
        }
#endif
    }
}
