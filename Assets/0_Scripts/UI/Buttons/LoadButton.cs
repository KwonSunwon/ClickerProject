using UnityEngine;
using UnityEngine.EventSystems;

public class LoadButton : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Managers.Save.Load();
    }
}
