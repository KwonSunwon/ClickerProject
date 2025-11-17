using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class StatManager
{


	public float IncreasedMineralYield = 0;
	public void CalcStat()
	{
		InitStat();

		Dictionary<int, SkillNodeData> Skills = Managers.Skill.SkillMap;
		foreach (KeyValuePair<int, SkillNodeData> pair in Skills)
		{
			int id = pair.Key;
			SkillNodeData data = pair.Value;

			if (string.IsNullOrWhiteSpace(data.Function))
				continue;

			AddStat(data.Function, data.StatPerLevel);
		}
	}

	public void InitStat()
	{
		IncreasedMineralYield = 0;
	}

	public void AddStat(string statName, float statInc)
	{
		// 인스턴스의 public/private 필드 검색
		FieldInfo field = GetType().GetField(statName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

		if (field == null)
		{
			Debug.LogWarning($"[AddStat] 필드 '{statName}' 를 찾을 수 없습니다.");
			return;
		}

		// 값 가져오기 및 증가
		float current = (float)field.GetValue(this);
		float newValue = current + statInc;

		// 값 다시 적용
		field.SetValue(this, newValue);

		Debug.Log($"[AddStat] {statName}: {current} → {newValue}");
	}
}
