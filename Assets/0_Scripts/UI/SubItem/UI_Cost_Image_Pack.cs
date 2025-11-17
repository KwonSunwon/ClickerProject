using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Cost_Image_Pack : UI_Base
{
	enum Images
	{
		UI_Skill_Cost_Image
	}

	enum Texts
	{
		UI_Skill_Cost_Text
	}
	public override void Init()
	{
	}

	public void SetCost(BigNumber cost)
	{
		if (GetTMP((int)Texts.UI_Skill_Cost_Text) == null)
		{
			Bind<TextMeshProUGUI>(typeof(Texts));
		}
			

		Get<TextMeshProUGUI>((int)Texts.UI_Skill_Cost_Text).text = cost.ToString();
	}

	public void SetMineralImage(Sprite sprite)
	{
		if (GetImage((int)Images.UI_Skill_Cost_Image) == null)
		{
			Bind<Image>(typeof(Images));
		}
		GetImage((int)Images.UI_Skill_Cost_Image).sprite = sprite;
	}
}
