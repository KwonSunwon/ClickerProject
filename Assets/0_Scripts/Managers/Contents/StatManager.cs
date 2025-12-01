using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class StatManager
{
	public BigNumber Gold;
	public float Air=0;
	public float MaxAir=100;
	public int Depth;

	public int Base_Click_Damage;

	//모든 광물 채굴 시 획득량 증가
	public float Mine_Get_All = 0;
	//모든 광물 채굴 시 두배 획득 확률
	public float Mine_DoubleGet_All = 0;
	//모든 광물 채굴 시 상위 광물 획득 확률
	public float Mine_Upgrade_All = 0;
	
	
	//클릭당 산소 소모량 감소율
	public float Air_Efficiency_Click = 0;
	//최대 산소량 증가
	public float Air_Max_Inc = 0;
	//클릭 시 산소 없이 채굴할 확률
	public float Air_Free = 0;


	
	//기계 채굴 속도 증가
	public float Machine_Speed_Inc = 0;
	//기계 채굴량 증가
	public float Machine_Get_Inc = 0;
	
	

	public void CalcStat()
	{
		InitStat();

		Dictionary<int, SkillNodeData> Skills = Managers.Skill.SkillMap;
		foreach (KeyValuePair<int, SkillNodeData> pair in Skills)
		{
			int id = pair.Key;
			SkillNodeData data = pair.Value;
				
			if(data.Level == 0) continue;

			if (string.IsNullOrWhiteSpace(data.Function))
				continue;

			AddStat(data.Function, data.StatPerLevel);
		}

	}

	public void InitStat()
	{
		Mine_Get_All = 0;
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

	//Todo: 제거하기
	bool dir = false;
	public void test() {
		if (dir)
		{
			Air += Time.fixedDeltaTime*30;
			if (Air > MaxAir) dir = !dir;
		}
		else
		{
			Air-=Time.fixedDeltaTime*30;
			if(Air<0) dir = !dir;
		}
	}

	public int ClickPerDamage()
	{
		return Base_Click_Damage;
	}
	public int ClickPerGetMine()
	{
		return 0;
	}


}
