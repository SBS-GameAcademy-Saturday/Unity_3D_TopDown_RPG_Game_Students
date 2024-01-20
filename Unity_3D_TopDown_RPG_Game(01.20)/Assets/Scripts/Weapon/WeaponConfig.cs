using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon",order = 1)]
public class WeaponConfig : InventoryItem
{
	[SerializeField] AnimatorOverrideController animatorOverride = null;
	[SerializeField] Weapon equippedPrefab = null;
	[SerializeField] float weaponDamage = 5f;
	[SerializeField] float percentageBonus = 0;
	[SerializeField] float weaponRange = 2f;
	[SerializeField] bool isRightHanded = true;
	[SerializeField] Projectile projectile = null;

	const string weaponName = "Weapon";

	// 무기 프리팹을 생성하는 코드
	public Weapon Spawn(Transform rightHand, Transform leftHand, Animator animator)
	{
		//기존에 장차하고 있던 무기를 파괴한다.
		DestroyOldWeapon(rightHand, leftHand);

		//새로운 무기를 생성한다.
		Weapon weapon = null;
		//생성할 무기 모델이 있다면
		if(equippedPrefab != null)
		{
			Transform handTransform = GetTransform(rightHand, leftHand);
			weapon = Instantiate(equippedPrefab,handTransform);
			weapon.gameObject.name = weaponName;
			Debug.Log("Instantiate Weapon");
		}

		var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
		//설정해놓은 오버라드 애니메이터 컨트롤러가 있는지 
		if(animatorOverride != null)
		{
			animator.runtimeAnimatorController = animatorOverride;
		}
		////기존에 동작하고 있던  오버라드 애니메이터 컨트롤러 다시 설정하는 코드
		//else if (overrideController != null)
		//{
		//	animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
		//}

		return weapon;
	}

	/// <summary>
	/// 오른쪽을 반환할지 왼쪽을 반환할지 체크하는 함수
	/// </summary>
	/// <param name="rightHand"></param>
	/// <param name="leftHand"></param>
	/// <returns></returns>
	private Transform GetTransform(Transform rightHand, Transform leftHand)
	{
		Transform handTransform;
		if (isRightHanded) handTransform = rightHand;
		else handTransform = leftHand;
		return handTransform;
	}

	/// <summary>
	/// 기존에 왼쪽 손이나 오른쪽 손에 있는 무기 모델을 파괴한다.
	/// </summary>
	/// <param name="rightHand"></param>
	/// <param name="leftHand"></param>
	private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
	{
		Transform oldWeapon = rightHand.Find(weaponName);
		if (oldWeapon == null)
		{
			oldWeapon = leftHand.Find(weaponName);
		}
		if (oldWeapon == null) return;

		//이름 바꿔서 중복 호출이 되는 것을 방지한다.
		oldWeapon.name = "DESTROYING";
		Destroy(oldWeapon.gameObject);
	}

	public bool HasProjectile()
	{
		return projectile != null;
	}

	public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target
		, GameObject instigator, float calculatedDamage) 
	{
		Projectile projectileInstance = Instantiate(projectile 
			,GetTransform(rightHand,leftHand).position,Quaternion.identity);
		projectileInstance.SetTarget(target, instigator, calculatedDamage);
	}

	public float GetRange()
	{
		return weaponRange;
	}
}
