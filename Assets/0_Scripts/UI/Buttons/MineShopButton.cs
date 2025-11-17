using UnityEngine;
using UnityEngine.EventSystems;

public class MineShopButton : MonoBehaviour, IPointerClickHandler
{
    private GameObject _MineShop;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (_MineShop == null)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/UI/UI_Mine_Shop");
            GameObject ui = Instantiate(prefab);
			_MineShop = ui;
		}
        else
        {
            _MineShop.SetActive(!_MineShop.activeSelf);
        }
            
    }
}
