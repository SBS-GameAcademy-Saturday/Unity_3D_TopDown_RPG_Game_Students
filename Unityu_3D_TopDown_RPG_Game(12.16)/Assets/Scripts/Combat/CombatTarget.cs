using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTarget : MonoBehaviour , IRaycastable
{
	public CursorType GetCursorType()
	{
		return CursorType.Combat;
	}

	public bool HandleRayCast(PlayerController callingController)
	{
		if (!enabled) return false;
		if (!CatchComponent(callingController)) return false;

        if (Input.GetMouseButtonDown(0))
        {
			CatchComponentToAttack(callingController);
        }

        return true;
	}

	public bool CatchComponent(PlayerController callingController)
	{
		var fighter = callingController.GetComponent<Fighter>();
        if (fighter)
        {
			return fighter.CanAttack(this.gameObject);
        }
		return false;
    }

	private void CatchComponentToAttack(PlayerController callingController)
	{
		var fighter = callingController.GetComponent<Fighter>();
		if (fighter)
		{
			fighter.Attack(this.gameObject);
			return;
		}
	}

}
