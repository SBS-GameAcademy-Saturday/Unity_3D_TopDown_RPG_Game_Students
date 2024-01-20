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

	// ���� �������� �����ϴ� �ڵ�
	public Weapon Spawn(Transform rightHand, Transform leftHand, Animator animator)
	{
		//������ �����ϰ� �ִ� ���⸦ �ı��Ѵ�.
		DestroyOldWeapon(rightHand, leftHand);

		//���ο� ���⸦ �����Ѵ�.
		Weapon weapon = null;
		//������ ���� ���� �ִٸ�
		if(equippedPrefab != null)
		{
			Transform handTransform = GetTransform(rightHand, leftHand);
			weapon = Instantiate(equippedPrefab,handTransform);
			weapon.gameObject.name = weaponName;
			Debug.Log("Instantiate Weapon");
		}

		var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
		//�����س��� ������� �ִϸ����� ��Ʈ�ѷ��� �ִ��� 
		if(animatorOverride != null)
		{
			animator.runtimeAnimatorController = animatorOverride;
		}
		////������ �����ϰ� �ִ�  ������� �ִϸ����� ��Ʈ�ѷ� �ٽ� �����ϴ� �ڵ�
		//else if (overrideController != null)
		//{
		//	animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
		//}

		return weapon;
	}

	/// <summary>
	/// �������� ��ȯ���� ������ ��ȯ���� üũ�ϴ� �Լ�
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
	/// ������ ���� ���̳� ������ �տ� �ִ� ���� ���� �ı��Ѵ�.
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

		//�̸� �ٲ㼭 �ߺ� ȣ���� �Ǵ� ���� �����Ѵ�.
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
