using RPG.Control;
using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Lesson_1
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] float maxNavMeshProjectionDistance = 1;
        [SerializeField] private Mover mover;
        private Vector3 _inputVec;

        // Start is called before the first frame update
        void Awake()
        {
            mover = GetComponent<Mover>();
        }

        // Update is called once per frame
        void Update()
        {
            InteractWithMovement();
        }

        //private void GetInput()
        //{
        //    float z = Input.GetAxis("Vertical");
        //    float x = Input.GetAxis("Horizontal");

        //    _inputVec = new Vector3(x, 0, z);
        //}

        //private void UpdateMove()
        //{
        //    if (_inputVec == Vector3.zero)
        //        return;
        //    mover.Move(_inputVec);
        //}
        //private void GetPoint()
        //{
        //    Vector3 target;

        //    RaycastHit hit;
        //    bool hasHit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);
        //    if (!hasHit) return;

        //    //target = hit.point;
        //    //Debug.Log(target);

        //    NavMeshHit navMeshHit;
        //    bool hasCastToNavMesh = NavMesh.SamplePosition(
        //        hit.point, out navMeshHit, maxNavMeshProjectionDistance, NavMesh.AllAreas);
        //    if (!hasCastToNavMesh) return;

        //    target = navMeshHit.position;
        //}

        private void InteractWithMovement()
        {
            Vector3 target;
            bool hasHit = RaycastNavMesh(out target);
            if (hasHit)
            {
                if (!mover.CanMoveTo(target))
                    return;
                if (Input.GetMouseButton(0))
                {
                    mover.StartMoveAction(target, 1f);
                }
            }
        }

        private bool RaycastNavMesh(out Vector3 target)
        {
            target = new Vector3();

            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (!hasHit) return false;

            NavMeshHit navMeshHit;
            bool hasCastToNavMesh = NavMesh.SamplePosition(
                hit.point, out navMeshHit, maxNavMeshProjectionDistance, NavMesh.AllAreas);
            if (!hasCastToNavMesh) return false;

            target = navMeshHit.position;

            return true;
        }
        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
