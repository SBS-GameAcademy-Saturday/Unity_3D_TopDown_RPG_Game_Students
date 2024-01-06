using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
namespace Lesson_25
{
    public class DialogueEditor : EditorWindow
    {
        Dialogue selectedDialogue = null; // 선택된 대화 객체를 저장하는 변수
        [NonSerialized]
        GUIStyle nodeStyle; // 대화 노드의 스타일을 저장하는 변수
        [NonSerialized]
        GUIStyle playerNodeStyle; // 플레이어 대화 노드의 스타일을 저장하는 변수
        [NonSerialized]
        DialogueNode draggingNode = null; // 현재 드래깅 중인 대화 노드를 저장하는 변수
        [NonSerialized]
        Vector2 draggingOffset; // 드래깅 중인 대화 노드의 오프셋을 저장하는 변수
        [NonSerialized]
        DialogueNode creatingNode = null; // 생성 중인 대화 노드를 저장하는 변수
        [NonSerialized]
        DialogueNode deletingNode = null; // 삭제 중인 대화 노드를 저장하는 변수
        [NonSerialized]
        DialogueNode linkingParentNode = null; // 링크 설정 중인 대화 노드의 부모를 저장하는 변수
        Vector2 scrollPosition; // 스크롤 뷰의 위치를 저장하는 변수
        [NonSerialized]
        bool draggingCanvas = false; // 캔버스 드래깅 여부를 저장하는 변수
        [NonSerialized]
        Vector2 draggingCanvasOffset; // 캔버스 드래깅 중의 오프셋을 저장하는 변수

        const float canvasSize = 4000; // 캔버스의 크기
        const float backgroundSize = 50; // 배경 이미지의 크기

        [MenuItem("Window/Lesson/Dialogue Editor")]
        public static void ShowEditorWindow()
        {
            GetWindow(typeof(DialogueEditor), false, "Dialogue Editor"); // 에디터 윈도우를 열어줄 메뉴 아이템 정의
        }

        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            Dialogue dialogue = EditorUtility.InstanceIDToObject(instanceID) as Dialogue; // 선택된 에셋을 Dialogue 객체로 변환
            if (dialogue != null)
            {
                ShowEditorWindow(); // 에디터 윈도우를 열어줌
                return true;
            }
            return false;
        }

        private void OnEnable()
        {
            Selection.selectionChanged += OnSelectionChanged; // 선택이 변경될 때 호출할 이벤트 핸들러 등록

            nodeStyle = new GUIStyle(); // 일반 대화 노드 스타일 초기화
            nodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D; // 노드의 배경 이미지 설정
            nodeStyle.normal.textColor = Color.white; // 텍스트 색상 설정
            nodeStyle.padding = new RectOffset(20, 20, 20, 20); // 내부 패딩 설정
            nodeStyle.border = new RectOffset(12, 12, 12, 12); // 테두리 설정

            playerNodeStyle = new GUIStyle(); // 플레이어 대화 노드 스타일 초기화
            playerNodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D; // 노드의 배경 이미지 설정
            playerNodeStyle.normal.textColor = Color.white; // 텍스트 색상 설정
            playerNodeStyle.padding = new RectOffset(20, 20, 20, 20); // 내부 패딩 설정
            playerNodeStyle.border = new RectOffset(12, 12, 12, 12); // 테두리 설정
        }

        private void OnSelectionChanged()
        {
            Dialogue newDialogue = Selection.activeObject as Dialogue; // 선택된 객체를 Dialogue 객체로 변환
            if (newDialogue != null)
            {
                selectedDialogue = newDialogue; // 선택된 대화 객체 설정
                Repaint(); // 에디터 윈도우 갱신
            }
        }

        private void OnGUI()
        {
            if (selectedDialogue == null)
            {
                EditorGUILayout.LabelField("No Dialogue Selected."); // 대화가 선택되지 않았을 때 메시지 출력
            }
            else
            {
                ProcessEvents(); // 마우스 이벤트 처리

                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition); // 스크롤 뷰 시작

                Rect canvas = GUILayoutUtility.GetRect(canvasSize, canvasSize); // 캔버스 영역을 설정
                Texture2D backgroundTex = Resources.Load("background") as Texture2D; // 배경 이미지 로드
                Rect texCoords = new Rect(0, 0, canvasSize / backgroundSize, canvasSize / backgroundSize); // 배경 이미지의 텍스쳐 좌표 설정
                GUI.DrawTextureWithTexCoords(canvas, backgroundTex, texCoords); // 배경 이미지를 그림

                foreach (DialogueNode node in selectedDialogue.GetAllNodes())
                {
                    DrawConnections(node); // 대화 노드 사이의 연결 그리기
                }
                foreach (DialogueNode node in selectedDialogue.GetAllNodes())
                {
                    DrawNode(node); // 대화 노드 그리기
                }

                EditorGUILayout.EndScrollView(); // 스크롤 뷰 종료

                if (creatingNode != null)
                {
                    selectedDialogue.CreateNode(creatingNode); // 새 대화 노드 생성
                    creatingNode = null;
                }
                if (deletingNode != null)
                {
                    selectedDialogue.DeleteNode(deletingNode); // 대화 노드 삭제
                    deletingNode = null;
                }
            }
        }

        private void ProcessEvents()
        {
            if (Event.current.type == EventType.MouseDown && draggingNode == null)
            {
                draggingNode = GetNodeAtPoint(Event.current.mousePosition + scrollPosition); // 마우스 클릭한 위치의 대화 노드를 가져옴
                if (draggingNode != null)
                {
                    draggingOffset = draggingNode.GetRect().position - Event.current.mousePosition;
                    Selection.activeObject = draggingNode; // 선택된 객체를 드래깅 중인 노드로 설정
                }
                else
                {
                    draggingCanvas = true;
                    draggingCanvasOffset = Event.current.mousePosition + scrollPosition;
                    Selection.activeObject = selectedDialogue; // 선택된 객체를 대화로 설정
                }
            }
            else if (Event.current.type == EventType.MouseDrag && draggingNode != null)
            {
                draggingNode.SetPosition(Event.current.mousePosition + draggingOffset); // 드래깅 중인 노드의 위치 설정

                GUI.changed = true;
            }
            else if (Event.current.type == EventType.MouseDrag && draggingCanvas)
            {
                scrollPosition = draggingCanvasOffset - Event.current.mousePosition; // 캔버스 드래깅 중인 경우 스크롤 뷰 위치 업데이트

                GUI.changed = true;
            }
            else if (Event.current.type == EventType.MouseUp && draggingNode != null)
            {
                draggingNode = null; // 드래깅 종료
            }
            else if (Event.current.type == EventType.MouseUp && draggingCanvas)
            {
                draggingCanvas = false; // 캔버스 드래깅 종료
            }

        }

        private void DrawNode(DialogueNode node)
        {
            GUIStyle style = nodeStyle;
            if (node.IsPlayerSpeaking())
            {
                style = playerNodeStyle;
            }
            GUILayout.BeginArea(node.GetRect(), style); // 대화 노드 영역 시작
            EditorGUI.BeginChangeCheck();

            node.SetText(EditorGUILayout.TextField(node.GetText())); // 대화 내용을 편집할 수 있는 텍스트 필드

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("x"))
            {
                deletingNode = node; // 대화 노드 삭제 버튼 클릭 시
            }
            DrawLinkButtons(node); // 대화 노드와의 연결 버튼 그리기
            if (GUILayout.Button("+"))
            {
                creatingNode = node; // 대화 노드 생성 버튼 클릭 시
            }

            GUILayout.EndHorizontal();

            GUILayout.EndArea(); // 대화 노드 영역 종료
        }

        private void DrawLinkButtons(DialogueNode node)
        {
            if (linkingParentNode == null)
            {
                if (GUILayout.Button("link"))
                {
                    linkingParentNode = node; // 링크 설정 버튼 클릭 시
                }
            }
            else if (linkingParentNode == node)
            {
                if (GUILayout.Button("cancel"))
                {
                    linkingParentNode = null; // 링크 설정 취소 버튼 클릭 시
                }
            }
            else if (linkingParentNode.GetChildren().Contains(node.name))
            {
                if (GUILayout.Button("unlink"))
                {
                    linkingParentNode.RemoveChild(node.name); // 링크 해제 버튼 클릭 시
                    linkingParentNode = null;
                }
            }
            else
            {
                if (GUILayout.Button("child"))
                {
                    Undo.RecordObject(selectedDialogue, "Add Dialogue Link");
                    linkingParentNode.AddChild(node.name); // 자식 링크 설정 버튼 클릭 시
                    linkingParentNode = null;
                }
            }
        }

        private void DrawConnections(DialogueNode node)
        {
            Vector3 startPosition = new Vector2(node.GetRect().xMax, node.GetRect().center.y); // 시작 위치 설정
            foreach (DialogueNode childNode in selectedDialogue.GetAllChildren(node))
            {
                Vector3 endPosition = new Vector2(childNode.GetRect().xMin, childNode.GetRect().center.y); // 끝 위치 설정
                Vector3 controlPointOffset = endPosition - startPosition;
                controlPointOffset.y = 0;
                controlPointOffset.x *= 0.8f;
                Handles.DrawBezier(
                    startPosition, endPosition,
                    startPosition + controlPointOffset,
                    endPosition - controlPointOffset,
                    Color.white, null, 4f); // 베지어 곡선을 그려 연결 표시
            }
        }

        private DialogueNode GetNodeAtPoint(Vector2 point)
        {
            DialogueNode foundNode = null;
            foreach (DialogueNode node in selectedDialogue.GetAllNodes())
            {
                if (node.GetRect().Contains(point))
                {
                    foundNode = node; // 지정된 포인트에서 해당 대화 노드를 찾음
                }
            }
            return foundNode;
        }
    }
}
