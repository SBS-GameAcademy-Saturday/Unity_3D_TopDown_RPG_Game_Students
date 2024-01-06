using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AIController : MonoBehaviour
{
	[SerializeField] private float chaseDistance = 5f; // �÷��̾ �߰��ϱ� �����ϴ� �Ÿ�
	[SerializeField] private float suspicionTime = 3f; // �÷��̾ �ǽ��ϱ� �����ϴ� �ð�
	[SerializeField] private float agroCooldownTime = 5f; // ��� ���·� ���ư��� ��ٿ� �ð�
	[SerializeField] private PatrolPath patrolPath; // ���� ���
	[SerializeField] private float wayPointToLerance = 1f; // ���� ���� ���� ��� �Ÿ�
	[SerializeField] private float waypointDwellTime = 3f; // ���� ���� ������ ��� �ð�
	[Range(0f, 1f)]
	[SerializeField] private float patrolSpeedFraction = 0.2f;// ���� �ӵ� ����
	[SerializeField] private float shoutDistance = 5f; // �ֺ��� ������ �˷��ִ� ����

	Fighter fighter; //���� ���
	Health health; // ü�� ���
	Mover mover; // �̵� ���
	GameObject player; // �÷��̾� Ÿ��

	LazyValue<Vector3> guardPosition; // AI�� ��� ��ġ
	float timeSinceLastSawPlayer = Mathf.Infinity; // ���������� �÷��̾ �� �ð�
	float timeSinceArrivedAtWaypoint = Mathf.Infinity; //���� ������ ������ �ð�
	float timeSinceAggrevated = Mathf.Infinity; // ���� ���·� ��ȯ�� �� ����� �ð�
	int currentWaypointIndex = 0;

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
		if (health.IsDead()) return;

		if (IsAggrevated() && fighter.CanAttack(player))
		{
			AttackBehavior(); // ���� ����
		}
		else if(timeSinceLastSawPlayer < suspicionTime)
		{
			SuspicionBehaviour(); //�ǽ� ����
		}
		else
		{
			PatrolBehaviour(); // ���� ����
		}

		UpdateTimers();
	}

	

	//�ð� ��� Ÿ�̸� ������Ʈ
	private void UpdateTimers()
	{
		timeSinceAggrevated += Time.deltaTime;
		timeSinceArrivedAtWaypoint += Time.deltaTime;
		timeSinceLastSawPlayer += Time.deltaTime;
	}

	// ���� ����
	private void PatrolBehaviour()
	{
		Vector3 nextPosition = guardPosition.value;

		if(patrolPath != null)
		{
			// ��ǥ ������ �����ߴ��� Ȯ��
			if (AtWaypoint())
			{
				//timeSinceArrivedAtWaypoint 0���� �ʱ�ȭ
				timeSinceArrivedAtWaypoint = 0;
				CycleWaypoint();
			}
			nextPosition = GetCurrentWaypoint();
		}
		// ��ǥ������ ������ ����� �ð��� ���� �������� �̵��� �ð����� ũ�ٸ�
		// ���� �������� �̵���Ų��.
		if(timeSinceArrivedAtWaypoint > waypointDwellTime)
		{
			mover.StartMoveAction(nextPosition, patrolSpeedFraction);
		}
	}

	/// <summary>
	/// ���� ���� �������� ��ȯ
	/// </summary>
	/// <exception cref="NotImplementedException"></exception>
	private void CycleWaypoint()
	{
		currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
	}

	/// <summary>
	/// ���� ������ �����ߴ��� Ȯ��
	/// </summary>
	/// <returns></returns>
	private bool AtWaypoint()
	{
		float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
		return distanceToWaypoint < wayPointToLerance;
	}

	/// <summary>
	/// ���� ���� ���� ��������
	/// </summary>
	/// <returns></returns>
	private Vector3 GetCurrentWaypoint()
	{
		return patrolPath.GetWaypoint(currentWaypointIndex);
	}

	/// <summary>
	/// �ǽ� ����
	/// </summary>
	private void SuspicionBehaviour()
	{
		GetComponent<ActionScheduler>().CancleCurrentAction();
	}

	/// <summary>
	/// ���� ���� => ����
	/// </summary>
	private void AttackBehavior()
	{
		timeSinceLastSawPlayer = 0;
		fighter.Attack(player);
	}

	// �ֺ��� �ٸ� AI�� ���� ���·� ��ȯ
	private void AggrevateNearbyEnemies()
	{
		RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);
		foreach(RaycastHit hit in hits) 
		{
			AIController ai = hit.collider.GetComponent<AIController>();
			if (ai == null) continue;

			ai.Aggrevate();
		}

	}
	// AI�� ���� ���·� ��ȯ�մϴ�.
	private void Aggrevate()
	{
		timeSinceAggrevated = 0;
	}

	// �÷��̾�� ��׷� ���� �������� Ȯ��
	private bool IsAggrevated()
	{
		float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
		return distanceToPlayer < chaseDistance || timeSinceAggrevated < agroCooldownTime;
	}
}
