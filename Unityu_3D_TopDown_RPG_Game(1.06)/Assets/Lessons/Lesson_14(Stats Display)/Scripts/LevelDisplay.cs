using Lesson_11;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lesson_14
{
    public class LevelDisplay : MonoBehaviour
    {
        [SerializeField] BaseStats baseStats;
        [SerializeField] private Text levelText;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            levelText.text = String.Format("{0:0}", baseStats.GetLevel());
        }
    }
}