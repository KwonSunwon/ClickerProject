using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Mine_Sell_Button : MonoBehaviour,IPointerClickHandler
{
    [SerializeField] TextMeshProUGUI Total_Gold_Text;
    MineralSlot _slot;

	public void Init(MineralSlot slot)
    {
        _slot = slot;
		if (Total_Gold_Text != null)
			Total_Gold_Text.text = $"{_slot.Amount}";

	}

	public void OnPointerClick(PointerEventData eventData)
	{
		_slot.Amount -= _slot.Amount;
	}

	private void FixedUpdate()
	{
		if (Total_Gold_Text != null)
			Total_Gold_Text.text = $"{_slot.Amount}";
	}
}
