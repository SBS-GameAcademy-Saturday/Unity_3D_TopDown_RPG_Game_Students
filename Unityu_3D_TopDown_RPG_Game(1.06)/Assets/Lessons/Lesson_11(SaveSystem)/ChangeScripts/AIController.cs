using Lesson.Utils;
using Lesson_7;
using Lesson_8;
using Lesson_Common;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace Lesson_11
{
    // 적 캐릭터의 행동 및 상태를 제어하는 AIController 클래스입니다.
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f; // 플레이어를 추격하기 시작하는 거리
        [SerializeField] float suspicionTime = 3f; // 플레이어를 의심하기 시작하는 시간
        [SerializeField] float agroCooldownTime = 5f; // 평소 상태로 돌아가는 쿨다운 시간
        [SerializeField] PatrolPath patrolPath; // 순찰 경로
        [SerializeField] float waypointTolerance = 1f; // 순찰 지점 도달 허용 거리
        [SerializeField] float waypointDwellTime = 3f; // 순찰 지점 도착 후 대기 시간
        [Range(0, 1)]
        [SerializeField] float patrolSpeedFraction = 0.2f; // 순찰 속도 비율
        [SerializeField] float shoutDistance = 5f; // 주변의 적에게 알려주는 범위

        Fighter fighter; // 공격을 담당하는 Fighter 클래스
        Health health; // 캐릭터의 체력을 관리하는 Health 클래스
        Mover mover; // 이동을 담당하는 Mover 클래스
        GameObject player; // 플레이어 객체

        LazyValue<Vector3> guardPosition; // AI의 경계 위치
        float timeSinceLastSawPlayer = Mathf.Infinity; // 마지막으로 플레이어를 본 시간
        float timeSinceArrivedAtWaypoint = Mathf.Infinity; // 순찰 지점에 도착한 시간
        float timeSinceAggrevated = Mathf.Infinity; // 공격 상태로 전환한 후 경과한 시간
        int currentWaypointIndex = 0; // 현재 순찰 지점 인덱스

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
            guardPosition.ForceInit(); // 경계 위치 초기화
        }

        private void Update()
        {
            if (health.IsDead()) return; // AI가 사망한 경우 아래 코드를 실행하지 않습니다.

            if (IsAggrevated() && fighter.CanAttack(player))
            {
                AttackBehaviour(); // 플레이어를 공격하는 동작
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehaviour(); // 플레이어를 의심하는 동작
            }
            else
            {
                PatrolBehaviour(); // 순찰 동작
            }

            UpdateTimers(); // 타이머 업데이트
        }

        // AI를 공격 상태로 전환합니다.
        public void Aggrevate()
        {
            timeSinceAggrevated = 0;
        }

        // 시간 경과 타이머 업데이트
        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
            timeSinceAggrevated += Time.deltaTime;
        }

        // 순찰 동작
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

        // 순찰 지점에 도달했는지 확인
        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
        }

        // 다음 순찰 지점으로 순환
        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        // 현재 순찰 지점 가져오기
        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        // 의심 동작
        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancleCurrentAction(); // 현재 동작 취소
        }

        // 공격 동작
        private void AttackBehaviour()
        {
            timeSinceLastSawPlayer = 0;
            fighter.Attack(player);

            AggrevateNearbyEnemies(); // 주변의 다른 AI를 공격 상태로 전환
        }

        // 주변의 다른 AI를 공격 상태로 전환
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

        // 플레이어에게 공격 상태인지 확인
        private bool IsAggrevated()
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return distanceToPlayer < chaseDistance || timeSinceAggrevated < agroCooldownTime;
        }

        // Unity에서 호출되는 함수로, 디버깅을 위해 적의 추격 범위를 시각화합니다.
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }

    }

}