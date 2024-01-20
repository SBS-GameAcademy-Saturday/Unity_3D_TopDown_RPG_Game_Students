using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SaveSystem))]
public class SavingWrapper : MonoBehaviour
{
	private const string currentSaveKey = "currentSaveName";
	[SerializeField] private float fadeInTime = 0.2f;
	[SerializeField] private float fadeOutTime = 0.2f;

	private void Start()
	{
		ContinueGame();
	}

	public void ContinueGame()
	{
		StartCoroutine(LoadLastScene());
	}

	// 마지막에 저장되어 있는 데이터를 기반으로 게임을 시작한다.
	private IEnumerator LoadLastScene()
	{
		Fader fader = FindObjectOfType<Fader>();
		yield return fader.FadeOut(fadeOutTime);
		yield return GetComponent<SaveSystem>().LoadLastScene(currentSaveKey);
		yield return fader.FadeIn(fadeInTime);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.S))
		{
			Save();
		}
		if (Input.GetKeyDown(KeyCode.L))
		{
			Load();
		}
	}

	public void Save()
	{
		GetComponent<SaveSystem>().Save(currentSaveKey);
	}

	public void Load()
	{
		GetComponent<SaveSystem>().Load(currentSaveKey);
	}
}
