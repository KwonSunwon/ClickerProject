using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class StatManager
{
	public BigNumber Gold;
	public float Air=0;
	public float MaxAir=100;
	public int Depth;

	public float ClickBaseDamage;
	public float ClickDamageMultiplier;
	public float OreGainMultiplier;
	public float TimerDecayRate;
	public float GoldGainMultiplier;
	public float TimerMaxTime;
	public float MinerWorkSpeed;
	public float OxygenEfficiency;
	public float MinerOreGain;
	public float TimerAutoRecovery;
	public float WorkerDamageMultiplier;
	public float WorkerAttackSpeed;
	public float WorkerCritChance;
	public float WorkerCritDamage;
	public float WorkerOverloadChance;
	public float ClickCriticalChance;
	public float AutoClickCount;
	public float ClickAreaRange;
	public float ClickCritDamage;
	public float ClickOxygenRecovery;
	public float ClickOreMiningChance;
	public float ClickMegaHitChance;
	public float ClickSuperCritChance;




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
		ClickBaseDamage=0;
		ClickDamageMultiplier=0;
		OreGainMultiplier= 0;
		TimerDecayRate = 0;
		GoldGainMultiplier = 0;
		TimerMaxTime = 0;	
		MinerWorkSpeed = 0;	
		OxygenEfficiency = 0;
		MinerOreGain = 0;
		TimerAutoRecovery = 0;
		WorkerDamageMultiplier = 0;
		WorkerAttackSpeed = 0;
		WorkerCritChance = 0;
		WorkerCritDamage = 0;
		WorkerOverloadChance = 0;
		ClickCriticalChance = 0;
		AutoClickCount = 0;
		ClickAreaRange = 0;
		ClickCritDamage = 0;
		ClickOxygenRecovery = 0;
		ClickOreMiningChance = 0;
		ClickMegaHitChance = 0;
		ClickSuperCritChance = 0;
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
    public void test()
    {
        //if (dir)
        //{
        //	Air += Time.fixedDeltaTime*30;
        //	if (Air > MaxAir) dir = !dir;
        //}
        //else
        //{
        //	Air-=Time.fixedDeltaTime*30;
        //	if(Air<0) dir = !dir;
        //}
    }

	public int ClickPerDamage()
	{
		float damage = ClickBaseDamage * ClickDamageMultiplier;

		// 2. 크리티컬 판정*

		if (Random.Range(0, 100) < ClickCriticalChance * 100)
		{
			damage *= ClickCritDamage;
		}

		// 3. 특수 공격 판정* 

		if (ClickSuperCritChance > 0)
		{
			if (Random.Range(0, 100) < ClickSuperCritChance * 100)
			{
				damage *= 40; // 슈퍼 크리티컬 배율*

			}
		}
		else if (ClickMegaHitChance > 0)
		{
			if (Random.Range(0, 100) < ClickMegaHitChance * 100)
			{
				damage *= 10; // 메가 히트 배율*

			}
		}

		return (int)damage;
	}

	public int ClickPerGetMine()
	{
		float ret = (3 * OreGainMultiplier);
		
		return (int)ret;

	}

	public float WorkerSpeed( float baseTimer)
	{
		return baseTimer / (1 + WorkerAttackSpeed);
	}

	public int workerDamage( float baseDamage )
	{
		return 0;
	}


}
