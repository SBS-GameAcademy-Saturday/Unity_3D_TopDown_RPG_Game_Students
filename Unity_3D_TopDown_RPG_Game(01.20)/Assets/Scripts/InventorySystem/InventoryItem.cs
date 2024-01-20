using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �Γ��丮�� ���� �� �ִ� ��� �������� ��Ÿ���� ScriptableObject�Դϴ�.
/// </summary>
public class InventoryItem : ScriptableObject, ISerializationCallbackReceiver
{
	[Tooltip("���� �� �ҷ����⸦ ���� �ڵ� ���� UUID, �� UUID�� �����Ϸ��� �� �ʵ带 ����ϴ�.")]
	[SerializeField] private string itemID = null;
	[Tooltip("UI�� ǥ�õ� ������ �̸�")]
	[SerializeField] private string displayName = null;
	[Tooltip("UI�� ǥ�õ� ������ ����")]
	[SerializeField] [TextArea] string description = null;
	[Tooltip("�κ��丮���� �� �������� ��Ÿ���� UI Icon")]
	[SerializeField] private Sprite icon;
	//[Tooltip("�� �������� ��ӵ� �� ������ ������")]
	//[SerializeField] 
	[Tooltip("â�� ���Կ� ���� �������� ���� �� �ִ��� ����")]
	[SerializeField] private bool stackable = false;

	//����
	static Dictionary<string, InventoryItem> itemLookupCache;

    public bool IsStackable() => stackable;

	public string GetID() => itemID;

	public Sprite GetIcon() => icon;

	public string GetDisplayName() => displayName;

	public string GetDescription() => description;

	public void OnBeforeSerialize()
	{
		if (string.IsNullOrWhiteSpace(itemID))
		{
			itemID = System.Guid.NewGuid().ToString();
		}
	}



	public void OnAfterDeserialize()
	{

	}
}
