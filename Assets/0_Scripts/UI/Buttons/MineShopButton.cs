using UnityEngine;
using UnityEngine.EventSystems;

public class MineShopButton : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/UI/UI_Mine_Shop");
        GameObject ui = Instantiate(prefab);
    }
}
