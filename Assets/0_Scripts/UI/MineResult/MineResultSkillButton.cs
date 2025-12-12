using UnityEngine;
using UnityEngine.EventSystems;

public class MineResultSkillButton : MonoBehaviour, IPointerDownHandler
{
    GameObject _mineResult;

    public void Start()
    {
        _mineResult ??= transform.parent.gameObject;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _mineResult.SetActive(false);
    }
}
