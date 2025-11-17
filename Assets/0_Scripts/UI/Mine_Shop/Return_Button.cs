using UnityEngine;
using UnityEngine.EventSystems;

public class Return_Button : MonoBehaviour, IPointerClickHandler
{
	[SerializeField] GameObject Parent_GO;
	public void OnPointerClick(PointerEventData eventData)
	{
		Destroy(Parent_GO);
	}
}
