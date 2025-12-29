using UnityEngine;

public class UI_MineralBundle : UI_Base
{
	public override void Init()
	{
		GameObject prefab = Resources.Load<GameObject>("Prefabs/UI/SubItem/UI_MineralPanel");

		//Todo: 
		for (int i = 0; i < (int)MineralType.MaxNum; i++) 
		{
			GameObject go = Instantiate(prefab, transform);

			MineralSlot slot = Managers.Mineral.GetSlot((MineralType)i);
			go.GetComponent<UI_MineralPanel>().SetSlot(slot);
		}
	}

	void Start()
    {
		Init();

	}


}
