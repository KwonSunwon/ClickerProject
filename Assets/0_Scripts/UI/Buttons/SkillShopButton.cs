using UnityEngine;
using UnityEngine.EventSystems;

public class SkillShopButton : MonoBehaviour, IPointerClickHandler
{
    private GameObject _skillShop;
    public void OnPointerClick(PointerEventData eventData)
    {
        if(_skillShop == null)
        {
			GameObject prefab = Resources.Load<GameObject>("Prefabs/UI/Scene/UI_Scene_Skill");
			GameObject ui = Instantiate(prefab);
            _skillShop = ui;
		}
        else
        {
            _skillShop.SetActive(!_skillShop.activeSelf);
        }
        
    }
}
