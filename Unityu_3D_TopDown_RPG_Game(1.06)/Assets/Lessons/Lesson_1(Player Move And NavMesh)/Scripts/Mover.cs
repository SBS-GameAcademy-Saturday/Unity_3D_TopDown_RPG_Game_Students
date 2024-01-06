using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Lesson_1
{
    public class Mover : MonoBehaviour
    {
        //public enum MoveType
        //{
        //    KeyBoard_Input,
        //    Mouse_Input,
        //}
        //[SerializeField] private MoveType moveType;
        [SerializeField] private Transform target;
        //[SerializeField] private float moveSpeed = 3f;
        [SerializeField] private float maxMoveSpeed = 6f;
        //[SerializeField] private float rotateSpeed = 10f;
        [SerializeField] private float maxNavPathLength = 40f;

        NavMeshAgent navMeshAgent;
        CharacterController _controller;

        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            _controller = GetComponent<CharacterController>();
        }

        //public void Move(Vector3 playerInput)
        //{
        //    switch (moveType)
        //    {
        //        case MoveType.KeyBoard_Input:
        //            MoveTo(playerInput);
        //            RotateTo(playerInput);
        //            break;
        //        case MoveType.Mouse_Input:
        //            MoveTo(playerInput,1);
        //            break;
        //    }
        //}

        //public void MoveTo(Vector3 moveDirection)
        //{
        //    _controller.Move(moveDirection * moveSpeed * Time.deltaTime);
        //}

        //public void RotateTo(Vector3 lookDirection)
        //{
        //    //lookDirection의 위치로 바라본다.
        //    //transform.LookAt(lookDirection);
        //    Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
        //    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        //}

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            MoveTo(destination, speedFraction);
        }

        public bool CanMoveTo(Vector3 destination)
        {
            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);
            if (!hasPath) return false;
            if (path.status != NavMeshPathStatus.PathComplete) return false;
            if (GetPathLength(path) > maxNavPathLength) return false;

            return true;

        }
        private float GetPathLength(NavMeshPath path)
        {
            float total = 0;
            if (path.corners.Length < 2) return total;
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                total += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }

            return total;
        }
        public void MoveTo(Vector3 destination, float speedFaction)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.speed = maxMoveSpeed * Mathf.Clamp01(speedFaction);
            navMeshAgent.isStopped = false;
        }

    }
}

