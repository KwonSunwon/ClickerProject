using UnityEngine;

public class Mine_Shop_Content : MonoBehaviour
{
	private void Start()
	{
		Init();
	}
	public void Init()
    {
		for (int i = transform.childCount - 1; i >= 0; i--)
			Destroy(transform.GetChild(i).gameObject);

		GameObject sellItemPrefab = Resources.Load<GameObject>("Prefabs/UI/SubItem/Mine_Sell_Panel");
		if (sellItemPrefab == null)
		{
			Debug.LogError("Mine_Sell_Panel 프리팹이 없습니다! 경로 확인 필요: Resources/Prefabs/UI/SubItem/Mine_Sell_Panel");
			return;
		}

		for (int i = 0; i < (int)MineralType.MaxNum; i++)
		{
			MineralType type = (MineralType)i;

			MineralSlot slot = Managers.Mineral.GetSlot(type);
			if (slot == null)
			{
				Debug.LogWarning($"{type} 슬롯을 찾을 수 없습니다.");
				continue;
			}

			if (slot.Amount.value <= 0)
				continue;

			GameObject go = Instantiate(sellItemPrefab, transform);
			Mine_Sell_Panel panel = go.GetComponent<Mine_Sell_Panel>();
			if (panel != null)
				panel.Init(slot);

		}
	}
}
