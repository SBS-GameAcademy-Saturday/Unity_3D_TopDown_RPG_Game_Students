using GameDevTV.Saving;
using Lesson_10;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Lesson_11
{
    public class SavingWrapper : MonoBehaviour
    {
        //const string defaultSaveFile = "save";
        private const string currentSaveKey = "currentSaveName";
        [SerializeField] float fadeInTime = 0.2f;
        [SerializeField] float fadeOutTime = 0.2f;
        [SerializeField] string firstFieldName = string.Empty;
        [SerializeField] string MenuSceneName = string.Empty;
        private void Start()
        {
            //StartCoroutine(LoadLastScene());
        }
        public void ContinueGame()
        {
            if(!PlayerPrefs.HasKey(currentSaveKey)) return;
            if (!GetComponent<SaveSystem>().SaveFileExists(GetCurrentSave())) return;
            StartCoroutine(LoadLastScene());
        }

        //Lesson_32

        public void LoadGame(string saveFile)
        {
            SetCurrentSave(saveFile);
            ContinueGame();
        }
        //Lesson_32

        public void LoadMenu()
        {
            StartCoroutine(LoadMenuScene());
        }

        public void NewGame(string saveFile)
        {
            if (string.IsNullOrEmpty(saveFile)) return;

            SetCurrentSave(saveFile);
            StartCoroutine(LoadFirstScene());
        }
        //Lesson_32
        private void SetCurrentSave(string saveFile)
        {
            PlayerPrefs.SetString(currentSaveKey, saveFile);
        }
        //Lesson_32
        private string GetCurrentSave()
        {
            return PlayerPrefs.GetString(currentSaveKey);
        }

        //private IEnumerator LoadLastScene()
        //{
        //    Fader fader = FindObjectOfType<Fader>();
        //    yield return fader.FadeOut(fadeOutTime);
        //    yield return GetComponent<SaveSystem>().LoadLastScene(defaultSaveFile);
        //    yield return fader.FadeIn(fadeInTime);
        //}

        //Lesson_32
        private IEnumerator LoadLastScene()
        {
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeOutTime);
            yield return GetComponent<SaveSystem>().LoadLastScene(GetCurrentSave());
            yield return fader.FadeIn(fadeInTime);
        }
        //Lesson_32
        private IEnumerator LoadFirstScene()
        {
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeOutTime);
            yield return SceneManager.LoadSceneAsync(firstFieldName);
            yield return fader.FadeIn(fadeInTime);
        }
        //Lesson_32

        private IEnumerator LoadMenuScene()
        {
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeOutTime);
            yield return SceneManager.LoadSceneAsync(MenuSceneName);
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
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                Delete();
            }
        }

        public void Load()
        {
            GetComponent<SaveSystem>().Load(GetCurrentSave());
        }

        public void Save()
        {
            GetComponent<SaveSystem>().Save(GetCurrentSave());
        }

        public void Delete()
        {
            GetComponent<SaveSystem>().Delete(GetCurrentSave());
        }

        //Lesson_32
        public IEnumerable<string> ListSaves()
        {
            return GetComponent<SaveSystem>().ListSaves();
        }
    }

}
