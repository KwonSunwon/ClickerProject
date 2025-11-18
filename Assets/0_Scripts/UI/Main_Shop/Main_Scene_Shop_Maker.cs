using Steamworks;
using UnityEngine;

public class Main_Scene_Shop_Maker : MonoBehaviour
{
    [SerializeField] private int id;
    void Start()
    {
		MakeShopPanel(id);
	}

    void Update()
    {
        
    }

	public void MakeShopPanel(int id)
	{
		string path = "Prefabs/UI/SubItem/Main_Secne_Shop_Panel";
		GameObject prefab = Resources.Load<GameObject>(path);

		if (prefab == null)
		{
			Debug.LogError($"Prefab not found at Resources/{path}");
			return;
		}

		GameObject inst = Instantiate(prefab, transform);
		var shopPanel = inst.GetComponent<Main_Secne_Shop_Panel>();

		if (shopPanel == null)
		{
			Debug.LogError("Main_Scene_Shop_Panel component not found on instantiated prefab.");
			return;
		}

		// 5. 자신의 id로 초기화
		shopPanel.Init(id);
	}
}
