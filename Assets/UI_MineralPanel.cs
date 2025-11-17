using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_MineralPanel : MonoBehaviour
{
	MineralSlot _mineralSlot;
	[SerializeField] Image _image;
	[SerializeField] TextMeshProUGUI _text;
	public void SetSlot(MineralSlot slot)
	{
		_mineralSlot = slot;
		_image.sprite = slot.Sprite;
		_text.text = $"{slot.Amount}";
	}

	private void FixedUpdate()
	{
		_text.text = $"{_mineralSlot.Amount}";
	}

}
