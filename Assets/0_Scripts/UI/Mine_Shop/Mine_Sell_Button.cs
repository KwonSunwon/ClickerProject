using DG.Tweening;
using TMPro;
using Unity.Mathematics;
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
			Total_Gold_Text.text = $"{_slot.Amount * math.pow(10, (int)_slot.Type)}";

	}

	public void OnPointerClick(PointerEventData eventData)
	{
		Managers.Stat.Gold += _slot.Amount*math.pow(10,(int)_slot.Type);
		_slot.Amount -= _slot.Amount;
		

	}

	private void FixedUpdate()
	{
		if (Total_Gold_Text != null)
			Total_Gold_Text.text = $"{_slot.Amount * math.pow(10, (int)_slot.Type)}";
	}

}
