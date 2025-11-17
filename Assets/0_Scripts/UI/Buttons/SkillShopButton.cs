using UnityEngine;
using UnityEngine.EventSystems;

public class SkillShopButton : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/UI/Scene/UI_Scene_Skill");
        GameObject ui = Instantiate(prefab);
    }
}
