using UnityEngine;
using UnityEngine.EventSystems;

public class MiningStartButton : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        MineManager.Instance.StartMining();
    }
}
