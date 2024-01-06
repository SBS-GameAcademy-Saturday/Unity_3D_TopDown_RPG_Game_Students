using Lesson.Utils;
using Lesson_7;
using Lesson_8;
using Lesson_Common;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace Lesson_11
{
    // �� ĳ������ �ൿ �� ���¸� �����ϴ� AIController Ŭ�����Դϴ�.
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f; // �÷��̾ �߰��ϱ� �����ϴ� �Ÿ�
        [SerializeField] float suspicionTime = 3f; // �÷��̾ �ǽ��ϱ� �����ϴ� �ð�
        [SerializeField] float agroCooldownTime = 5f; // ��� ���·� ���ư��� ��ٿ� �ð�
        [SerializeField] PatrolPath patrolPath; // ���� ���
        [SerializeField] float waypointTolerance = 1f; // ���� ���� ���� ��� �Ÿ�
        [SerializeField] float waypointDwellTime = 3f; // ���� ���� ���� �� ��� �ð�
        [Range(0, 1)]
        [SerializeField] float patrolSpeedFraction = 0.2f; // ���� �ӵ� ����
        [SerializeField] float shoutDistance = 5f; // �ֺ��� ������ �˷��ִ� ����

        Fighter fighter; // ������ ����ϴ� Fighter Ŭ����
        Health health; // ĳ������ ü���� �����ϴ� Health Ŭ����
        Mover mover; // �̵��� ����ϴ� Mover Ŭ����
        GameObject player; // �÷��̾� ��ü

        LazyValue<Vector3> guardPosition; // AI�� ��� ��ġ
        float timeSinceLastSawPlayer = Mathf.Infinity; // ���������� �÷��̾ �� �ð�
        float timeSinceArrivedAtWaypoint = Mathf.Infinity; // ���� ������ ������ �ð�
        float timeSinceAggrevated = Mathf.Infinity; // ���� ���·� ��ȯ�� �� ����� �ð�
        int currentWaypointIndex = 0; // ���� ���� ���� �ε���

        private void Awake()
        {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            player = GameObject.FindWithTag("Player");

            guardPosition = new LazyValue<Vector3>(GetGuardPosition);
        }

        private Vector3 GetGuardPosition()
        {
            return transform.position;
        }

        private void Start()
        {
            guardPosition.ForceInit(); // ��� ��ġ �ʱ�ȭ
        }

        private void Update()
        {
            if (health.IsDead()) return; // AI�� ����� ��� �Ʒ� �ڵ带 �������� �ʽ��ϴ�.

            if (IsAggrevated() && fighter.CanAttack(player))
            {
                AttackBehaviour(); // �÷��̾ �����ϴ� ����
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehaviour(); // �÷��̾ �ǽ��ϴ� ����
            }
            else
            {
                PatrolBehaviour(); // ���� ����
            }

            UpdateTimers(); // Ÿ�̸� ������Ʈ
        }

        // AI�� ���� ���·� ��ȯ�մϴ�.
        public void Aggrevate()
        {
            timeSinceAggrevated = 0;
        }

        // �ð� ��� Ÿ�̸� ������Ʈ
        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
            timeSinceAggrevated += Time.deltaTime;
        }

        // ���� ����
        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition.value;

            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    timeSinceArrivedAtWaypoint = 0;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }

            if (timeSinceArrivedAtWaypoint > waypointDwellTime)
            {
                mover.StartMoveAction(nextPosition, patrolSpeedFraction);
            }
        }

        // ���� ������ �����ߴ��� Ȯ��
        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
        }

        // ���� ���� �������� ��ȯ
        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        // ���� ���� ���� ��������
        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        // �ǽ� ����
        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancleCurrentAction(); // ���� ���� ���
        }

        // ���� ����
        private void AttackBehaviour()
        {
            timeSinceLastSawPlayer = 0;
            fighter.Attack(player);

            AggrevateNearbyEnemies(); // �ֺ��� �ٸ� AI�� ���� ���·� ��ȯ
        }

        // �ֺ��� �ٸ� AI�� ���� ���·� ��ȯ
        private void AggrevateNearbyEnemies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);
            foreach (RaycastHit hit in hits)
            {
                AIController ai = hit.collider.GetComponent<AIController>();
                if (ai == null) continue;

                ai.Aggrevate();
            }
        }

        // �÷��̾�� ���� �������� Ȯ��
        private bool IsAggrevated()
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return distanceToPlayer < chaseDistance || timeSinceAggrevated < agroCooldownTime;
        }

        // Unity���� ȣ��Ǵ� �Լ���, ������� ���� ���� �߰� ������ �ð�ȭ�մϴ�.
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }

    }

}