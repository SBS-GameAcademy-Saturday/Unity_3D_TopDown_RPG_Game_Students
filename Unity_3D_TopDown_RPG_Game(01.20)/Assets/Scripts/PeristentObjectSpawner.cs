using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeristentObjectSpawner : MonoBehaviour
{
	[SerializeField] GameObject persistentObjectPrefab;

	static bool hasSpawned = false;

	private void Awake()
	{
		if (hasSpawned) return;

		SpawnPeristentObjects();
		hasSpawned = true;
	}

	private void SpawnPeristentObjects()
	{
		GameObject persistentObject = Instantiate(persistentObjectPrefab);
		DontDestroyOnLoad(persistentObject);
	}

}
