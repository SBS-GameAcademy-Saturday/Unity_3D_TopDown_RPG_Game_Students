using Lesson_11;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesson_32
{
    public class PauseMenuUI : MonoBehaviour
    {
        PlayerController playerController;

        private void Start()
        {
            playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }

        private void OnEnable()
        {
            Time.timeScale = 0;
            if (playerController)
            {
                playerController.enabled = false;
            }
            else
            {
                playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
                playerController.enabled = false;
            }
        }

        private void OnDisable()
        {
            Time.timeScale = 1;
            if (playerController)
            {

                playerController.enabled = true;
            }
            else
            {
                playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
                playerController.enabled = true;
            }

        }

        public void Save()
        {
            SavingWrapper savingWrapper = FindAnyObjectByType<SavingWrapper>();
            savingWrapper.Save();
        }

        public void SaveAndQuit()
        {
            SavingWrapper savingWrapper = FindAnyObjectByType<SavingWrapper>();
            savingWrapper.Save();
            savingWrapper.LoadMenu();
        }
    }

}
