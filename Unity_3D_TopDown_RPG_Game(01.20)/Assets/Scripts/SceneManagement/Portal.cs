using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
	enum DestinationIdentifier
	{
		A,
		B,
		C,
		D,
		E,
	}

	[SerializeField] private int sceneToLoad = -1;
	[SerializeField] private Transform spawnPoint;
	[SerializeField] private DestinationIdentifier destination;
	[SerializeField] private float fadeOutTime = 1f;
	[SerializeField] private float fadeInTime = 2f;
	[SerializeField] private float fadeWaitTime = 0.5f;

	private void Awake()
	{
		Debug.Log("Awake");
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			StartCoroutine(Transition());
		}
	}

	private IEnumerator Transition()
	{
		if (sceneToLoad < 0)
		{
			Debug.LogError("Scene to load not set.");
			yield break;
		}

		DontDestroyOnLoad(this.gameObject);

		Fader fader = FindObjectOfType<Fader>();
		SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
		PlayerController playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
		playerController.enabled = false;

		//FadeOut ȿ���� �� �ɶ����� ���
		yield return fader.FadeOut(fadeOutTime);

		savingWrapper.Save();

		//Scene�� �� �ε尡 �ɶ����� ���
		yield return SceneManager.LoadSceneAsync(sceneToLoad);

		Debug.Log("LoadSceneAsync Is Done");
		if (playerController == null)
		{
			playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
			playerController.enabled = false;
			Debug.Log("PlayerController was null");
		}

		savingWrapper.Load();

		//�ٸ� �������� ��Ż�� �����ͼ� ĳ���� ��ġ�� �ʱ�ȭ ��Ų�� 
		Portal otherPortal = GetOtherPortal();
		UpdatePlayer(otherPortal);

		savingWrapper.Save();

		yield return new WaitForSeconds(fadeWaitTime);
		//Fader�� ������ ���� �������� FadeIn ȿ���� ����Ѵ�.
		if(fader == null)
		{
			fader = FindObjectOfType<Fader>();
			Debug.Log("fader was null");
		}
		yield return fader.FadeIn(fadeInTime);

		//PlayerController�� �ٽ� ����ϰ� �Ѵ�.
		playerController.enabled = true;

		////���� ��Ż�� �ı��Ѵ�.
		Destroy(gameObject);
	}

	private void UpdatePlayer(Portal otherPortal)
	{
		GameObject player = GameObject.FindWithTag("Player");
		player.GetComponent<NavMeshAgent>().enabled = false;
		player.transform.position = otherPortal.spawnPoint.position;
		player.transform.rotation = otherPortal.spawnPoint.rotation;
		player.GetComponent<NavMeshAgent>().enabled = true;
	}

	private Portal GetOtherPortal()
	{
		foreach(Portal portal in FindObjectsOfType<Portal>())
		{
			if (portal == this) continue;
			if (portal.destination != destination) continue;

			return portal;
		}
		return null;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Fader fader = FindObjectOfType<Fader>();
			fader.FadeOut(fadeOutTime);
			Debug.Log("GetKeyDown  ");
		}
	}
}
