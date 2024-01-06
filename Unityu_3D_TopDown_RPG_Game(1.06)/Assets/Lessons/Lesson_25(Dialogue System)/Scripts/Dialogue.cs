using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Lesson_25
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Lesson/Dialogue", order = 0)]
    public class Dialogue : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField]
        List<DialogueNode> nodes = new List<DialogueNode>(); // 대화 노드 목록을 저장하는 리스트
        [SerializeField]
        Vector2 newNodeOffset = new Vector2(250, 0); // 새로운 대화 노드의 초기 위치 오프셋

        Dictionary<string, DialogueNode> nodeLookup = new Dictionary<string, DialogueNode>(); // 대화 노드를 이름으로 조회하는 딕셔너리

        private void OnValidate()
        {
            nodeLookup.Clear(); // 노드 조회 딕셔너리 초기화
            foreach (DialogueNode node in GetAllNodes())
            {
                if (node == null) continue;
                if (nodeLookup.ContainsKey(node.name)) continue;
                nodeLookup.Add(node.name,node); // 각 노드의 이름을 키로 사용하여 딕셔너리에 추가
            }
        }

        public IEnumerable<DialogueNode> GetAllNodes()
        {
            return nodes; // 모든 대화 노드를 반환
        }

        public DialogueNode GetRootNode()
        {
            return nodes[0]; // 루트 대화 노드를 반환
        }

        public IEnumerable<DialogueNode> GetAllChildren(DialogueNode parentNode)
        {
            foreach (string childID in parentNode.GetChildren())
            {
                if (nodeLookup.ContainsKey(childID))
                {
                    yield return nodeLookup[childID]; // 주어진 부모 노드의 자식 노드들을 반환
                }
            }
        }

        public IEnumerable<DialogueNode> GetPlayerChildren(DialogueNode currentNode)
        {
            foreach (DialogueNode node in GetAllChildren(currentNode))
            {
                if (node.IsPlayerSpeaking())
                {
                    yield return node; // 현재 노드와 연결된 플레이어 대화 노드들을 반환
                }
            }
        }

        public IEnumerable<DialogueNode> GetAIChildren(DialogueNode currentNode)
        {
            foreach (DialogueNode node in GetAllChildren(currentNode))
            {
                if (!node.IsPlayerSpeaking())
                {
                    yield return node; // 현재 노드와 연결된 AI 대화 노드들을 반환
                }
            }
        }

#if UNITY_EDITOR
        public void CreateNode(DialogueNode parent)
        {
            DialogueNode newNode = MakeNode(parent); // 새 대화 노드 생성
            Undo.RegisterCreatedObjectUndo(newNode, "Created Dialogue Node"); // 에디터에서 생성된 객체 등록
            Undo.RecordObject(this, "Added Dialogue Node"); // 에디터에서 변경된 객체 등록
            AddNode(newNode); // 대화 노드를 추가
        }

        public void DeleteNode(DialogueNode nodeToDelete)
        {
            Undo.RecordObject(this, "Deleted Dialogue Node"); // 에디터에서 삭제된 객체 등록
            nodes.Remove(nodeToDelete); // 대화 노드 삭제
            OnValidate(); // 대화 노드 관련 정보 갱신
            CleanDanglingChildren(nodeToDelete); // 연결된 자식 노드 정리
            Undo.DestroyObjectImmediate(nodeToDelete); // 에디터에서 객체 삭제
        }

        private DialogueNode MakeNode(DialogueNode parent)
        {
            DialogueNode newNode = CreateInstance<DialogueNode>(); // 새 대화 노드 생성
            newNode.name = Guid.NewGuid().ToString(); // 노드의 고유 이름 설정
            if (parent != null)
            {
                parent.AddChild(newNode.name); // 부모 노드와 새로운 노드 연결
                newNode.SetPlayerSpeaking(!parent.IsPlayerSpeaking()); // 새로운 노드의 스피커 설정
                newNode.SetPosition(parent.GetRect().position + newNodeOffset); // 새로운 노드의 위치 설정
            }

            return newNode;
        }

        private void AddNode(DialogueNode newNode)
        {
            nodes.Add(newNode); // 대화 노드를 리스트에 추가
            OnValidate(); // 대화 노드 관련 정보 갱신
        }

        private void CleanDanglingChildren(DialogueNode nodeToDelete)
        {
            foreach (DialogueNode node in GetAllNodes())
            {
                node.RemoveChild(nodeToDelete.name); // 삭제된 노드와 연결된 자식 노드 정리
            }
        }
#endif

        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            if (nodes.Count == 0)
            {
                DialogueNode newNode = MakeNode(null); // 대화 노드가 없으면 루트 노드 생성
                AddNode(newNode);
            }

            if (AssetDatabase.GetAssetPath(this) != "")
            {
                foreach (DialogueNode node in GetAllNodes())
                {
                    if (AssetDatabase.GetAssetPath(node) == "")
                    {
                        AssetDatabase.AddObjectToAsset(node, this); // 대화 노드를 대화 객체에 추가
                    }
                }
            }
#endif
        }

        public void OnAfterDeserialize()
        {
        }
    }
}
