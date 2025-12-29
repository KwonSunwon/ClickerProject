using TMPro;
using UnityEngine;

public class CycleInfoPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _cycleText;

    public void Init(int cycle)
    {
        _cycleText.text = $"벽의 강도 {cycle}단계";
    }
}
