using Lesson_11;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lesson_32
{
    public class SaveLoadUI : MonoBehaviour
    {
        [SerializeField] Transform contentRoot;
        [SerializeField] GameObject buttonPrefab;

        private void OnEnable()
        {
            foreach(Transform child in contentRoot)
            {
                Destroy(child.gameObject);
            }
            SavingWrapper savingWrapper = FindAnyObjectByType<SavingWrapper>();
            if (savingWrapper == null)
                return;

            foreach(string save in savingWrapper.ListSaves())
            {
                GameObject buttonInstance = Instantiate(buttonPrefab, contentRoot);
                Text textComp = buttonInstance.GetComponentInChildren<Text>();
                textComp.text = save;
                Button button = buttonInstance.GetComponentInChildren<Button>();
                button.onClick.AddListener(() =>
                {
                    savingWrapper.LoadGame(save);
                });
            }
        }
    }
}

