using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class GameObjectExtention
{
    public static T GetComponentAroundOrAdd<T>(this GameObject obj) where T : Component
	{
		//현재 오브젝트의 자식 계층에 있는 컴포넌트를 가져온다.
		T component = obj.GetComponentInChildren<T>(true);
		if(component != null) return component;

		//현재 오브젝트에서 부모 계층에 있는 컴포넌트를 가져온다.
		component = obj.GetComponentInParent<T>();
		if(component != null) return component;

		//현재 오브젝트에 컴포넌트를 가져온다.
		component = obj.GetComponent<T>();
		if(component != null) return component;

		//없으면 추가해서 반환한다
		return obj.AddComponent<T>(); ;
	}
}
