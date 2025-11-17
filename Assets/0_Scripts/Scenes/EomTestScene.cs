using System.Collections;
using UnityEngine;

public class EomTestScene : BaseScene
{
	protected override void Init()
	{
		base.Init();
		MineralManager mineralManager = Managers.Mineral;
		//Managers.UI.ShowSceneUI<UI_Scene_Skill>();
		{
			GameObject prefab = Resources.Load<GameObject>("Prefabs/UI/Scene/UI_Scene_Skill");
			GameObject ui = Instantiate(prefab);
		}
		{
			GameObject prefab = Resources.Load<GameObject>("Prefabs/UI/UI_Mine_Shop");
			GameObject ui = Instantiate(prefab);
		}
	}

	public override void Clear()
	{
		throw new System.NotImplementedException();
	}

}
