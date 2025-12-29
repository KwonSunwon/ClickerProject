using UnityEngine;
using UnityEngine.EventSystems;

public class MiningStartButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject _cyclePanel;

    public void OnPointerClick(PointerEventData eventData)
    {
        MineManager.Instance.StartMining();

        if (_cyclePanel != null) {
            Destroy(_cyclePanel);
        }
    }
}
