using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour,IAction
{
	[SerializeField] private float timeBetweenAttacks = 1;

	private Animator _animator;
	private ActionScheduler _actionScheduler;
	private Mover mover;

	private Health target = null;

	private float timeSinceLastAttack = 0;

	private void Awake()
	{
		mover = GetComponent<Mover>();
		_actionScheduler = GetComponent<ActionScheduler>();
		_animator = GetComponentInChildren<Animator>();
	}

	// Update is called once per frame
	void Update()
    {
		timeSinceLastAttack += Time.deltaTime;

		if (!target) return;
		if (target.IsDead()) return;

		if (!GetIsInRange(target.transform))
		{
			mover.MoveTo(target.transform.position, 1);
		}
        else
        {
			mover.Cancle();
			AttackBehaviour();
        }
    }

	public void StartAttackEvent()
	{
		Debug.Log("StartAttackEvent");
	}

	private void AttackBehaviour()
	{

		transform.LookAt(target.transform);
		if(timeSinceLastAttack > timeBetweenAttacks)
		{
			TriggerAttack();
			timeSinceLastAttack = 0;
		}
	}

	private void TriggerAttack()
	{
		_animator.ResetTrigger("StopAttack");
		_animator.SetTrigger("Attack");
	}

	private void StopAttack()
	{
		_animator.SetTrigger("StopAttack");
		_animator.ResetTrigger("Attack");
	}

	public bool CanAttack(GameObject combatTarget)
	{
		if (combatTarget == null) return false;
		if (!mover.CanMoveTo(combatTarget.transform.position))
		{
			return false;
		}
		Health targetHealth = combatTarget.GetComponent<Health>();
		return (targetHealth != null && !targetHealth.IsDead());
	}

	public void Attack(GameObject gameObject)
	{
		_actionScheduler.StartAction(this);
		target = gameObject.GetComponent<Health>();
	}

	public void Hit()
	{
		if (target == null) return;

		float damage = GetComponent<BaseStats>().GetStat(Stats.Damage);

		target.TakeDamage(this.gameObject, damage);
	}

	private bool GetIsInRange(Transform targetTransform)
	{
		return Vector3.Distance(transform.position, targetTransform.position) < 2;
	}

	public void Cancle()
	{
		StopAttack();
		target = null;
		mover.Cancle();
		timeSinceLastAttack = 0;
	}
}
