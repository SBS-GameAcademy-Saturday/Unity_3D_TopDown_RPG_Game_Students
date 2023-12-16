using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
public class Progression : ScriptableObject
{
	//characterClasses 배열은 직업 클래스와 그에 따른 스탯 정보를 저장합니다
	[SerializeField] ProgressionCharacterClass[] characterClasses = null;

	//lookupTable은 스탯 및 직업 클래스에 대한 정보를 미리 계산하여 저장하는 Dictionary입니다.
	Dictionary<CharacterClass, Dictionary<Stats, float[]>> lookupTable = null;

	//특정 스탯의 특정 직업 클래스 및 레벨에 해다하는 스탯 값을 반환을 합니다.
	public float GetStat(Stats stats, CharacterClass characterClass, int level)
	{
		BuildLookup();

		//lookupTable에 해당 characterClass의 스텟 값이 설정되어 있지 않으면 0 반환
		if (!lookupTable[characterClass].ContainsKey(stats))
			return 0;

		float[] levels = lookupTable[characterClass][stats];

		//levels 배열이 0 보다 작을 경우 0 반환
		if (levels.Length <= 0)
			return 0;

		//현재 캐릭터의 레벨이 설정한 디자인의 스탯 값보다 클 경우 최대 스탯 값으로 반환
		if (levels.Length < level)
			return levels[levels.Length - 1];

		//현재 레벨의 스탯 값으로 반환
		return levels[level - 1];
	}

	//lookupTable을 빌드하고 계산된 정보를 저장합니다.
	private void BuildLookup()
	{
		// 이미 lookupTable이 생성되었으면 함수를 종료합니다.
		if (lookupTable != null) return;

		//lookupTable를 초기화합니다.
		lookupTable = new Dictionary<CharacterClass, Dictionary<Stats, float[]>>();

		//characterClasses 배열에 있는 직업 클래스 및 스탯 정보를 루프를 통해 처리합니다.
		foreach (ProgressionCharacterClass progressionClass in characterClasses)
		{
			//statLookupTable를 선언합니다.
			var statLookupTable = new Dictionary<Stats, float[]>();

			//직업 클래스에 대한 스탯 정보를 루프를 통해 처리합니다.
			foreach(ProgressionStat progressionStat in progressionClass.stats)
			{
				statLookupTable[progressionStat.stats] = progressionStat.levels;
			}

			//직업 클래스와 스탯 정보를 lookupTable에 저장합니다.
			lookupTable[progressionClass.characterClass] = statLookupTable;
		}
	}

	//직업 클래스 정보를 저장하는 내부 클래스
	[Serializable]
	class ProgressionCharacterClass
	{
		public CharacterClass characterClass;//직업 클래스
		public ProgressionStat[] stats;//해당 직업 클래스의 스탯 정보 배열
	}

	//스텟 정보를 저장하는 내부 클래스
    [Serializable]
    class ProgressionStat
    {
		public Stats stats;//스탯
		public float[] levels;//레벨별 스탯 값 배열
    }
}
