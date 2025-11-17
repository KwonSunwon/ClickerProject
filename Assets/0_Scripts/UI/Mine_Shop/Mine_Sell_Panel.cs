using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Mine_Sell_Panel : MonoBehaviour
{
	MineralSlot _slot;
	[SerializeField] Image Mine_Image;
	[SerializeField] TextMeshProUGUI Mine_Name_Text;
	[SerializeField] TextMeshProUGUI Mine_Ea_Text;
	[SerializeField] Mine_Sell_Button _Mine_Sell_Button;
	public void Init(MineralSlot slot)
    {
		_slot = slot;
		if (Mine_Image != null)
			Mine_Image.sprite = _slot.Sprite;

		if (Mine_Name_Text != null)
			Mine_Name_Text.text = _slot.Type.ToString();

		if (Mine_Ea_Text != null)
			Mine_Ea_Text.text = $"{_slot.Amount}";

		if (_Mine_Sell_Button != null)
			_Mine_Sell_Button.Init(slot);
	}

	private void FixedUpdate()
	{
		if (Mine_Ea_Text != null)
			Mine_Ea_Text.text = $"{_slot.Amount}";
	}
}
