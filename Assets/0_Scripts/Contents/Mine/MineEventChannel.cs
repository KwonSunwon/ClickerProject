using System;
using UnityEngine;

[CreateAssetMenu(fileName = "MineEventChannel", menuName = "ScriptableObjects/EventChannels/MineEventChannel")]
public class MineEventChannel : ScriptableObject
{
    public event Action Raised;

    public void Raise()
    {
        Raised?.Invoke();
    }
}
