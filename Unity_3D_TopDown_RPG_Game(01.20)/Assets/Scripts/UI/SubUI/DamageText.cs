using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
	[SerializeField] private Text damageText;
    
	public void DestoryText()
	{
		Destroy(gameObject);
	}

	public void SetValue(float amount)
	{
		damageText.text = String.Format("{0:0}", amount);
		//damageText.text = $"{amount}";
	}

}
