using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelDisplay : MonoBehaviour
{
	[SerializeField] private BaseStats baseStats;
	[SerializeField] private Text levelText;

    void Update()
    {
		levelText.text = string.Format("{0:0}", baseStats.GetLevel());
    }
}