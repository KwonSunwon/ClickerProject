using UnityEngine;

public class VeinDrill : MonoBehaviour
{
    VeinView veinView;
	[SerializeField] GetOre getOre;
	bool isDrillActive;
	float getCooltime;

	private void Start()
	{
		veinView = GetComponent<VeinView>();
		isDrillActive = false;
	}

	public void ActiveDrill()
	{
		isDrillActive = true;
	}

	private void Update()
	{
		if (!isDrillActive) return;
		getCooltime += Time.deltaTime;

		if (getCooltime > 1f)
		{
			getCooltime = 0f;

			getOre.Init((MineralType)veinView.Type, new BigNumber(10));
			Managers.Mineral.Add((MineralType)veinView.Type, new BigNumber(10));
		}
		
	}



}
