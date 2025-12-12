using UnityEngine;
using UnityEngine.EventSystems;

public class MineResultSkillButton : MonoBehaviour, IPointerDownHandler
{
    private GameObject _mineResult;
    private GameObject _skillShop;

    public void Start()
    {
        _mineResult ??= transform.parent.gameObject;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _mineResult.SetActive(false);

        if (_skillShop == null) {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/UI/Scene/UI_Scene_Skill");
            GameObject ui = Instantiate(prefab);
            _skillShop = ui;
        }
        else {
            _skillShop.SetActive(!_skillShop.activeSelf);
        }
    }
}
