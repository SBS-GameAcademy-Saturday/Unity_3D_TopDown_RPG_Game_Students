using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 인벤토리 아이템을 나타내는 아이콘을 설정한다.
/// </summary>
[RequireComponent(typeof(Image))]
public class InventoryItemIcon : MonoBehaviour
{
	//구성 데이터
	[SerializeField] GameObject textContainer = null;
	[SerializeField] TextMeshProUGUI itemNumber = null;

	/// <summary>
	/// 아이템을 설정합니다.
	/// </summary>
	/// <param name="item"></param>
	public void SetItem(InventoryItem item)
	{
		SetItem(item, 0);
	}

	/// <summary>
	/// 아이템과 아이템의 개수를 설정합니다.
	/// </summary>
	/// <param name="item"></param>
	/// <param name="number"></param>
	public void SetItem(InventoryItem item, int number)
	{
		var iconImage = GetComponent<Image>();
		if(item == null)
		{
			iconImage.enabled = false;
		}
		else
		{
			iconImage.enabled = true;
			iconImage.sprite = item.GetIcon();
		}

		if (itemNumber)
		{
			if(number <= 1)
			{
				textContainer.SetActive(false);
			}
			else
			{
				textContainer.SetActive(true);
				itemNumber.text = number.ToString();
			}
		}

	}

}
