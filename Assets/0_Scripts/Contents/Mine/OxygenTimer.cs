using System;
using System.Collections;
using UnityEngine;

public class OxygenTimer : MonoBehaviour
{
    private float _totalOxygen;
    private float _currentOxygen;

    public float TotalOxygen { get; set; }

    public event Action OnOxygenDepleted;

    private bool _isRunning = false;

    public void StartTimer(float totalOxygen = 0)
    {
        if (totalOxygen > 0)
            _currentOxygen = totalOxygen;
        else
            _currentOxygen = _totalOxygen;

        if (!_isRunning) {
            _isRunning = true;
            StartCoroutine(Timer(_currentOxygen));
        }
        else {
            Debug.LogWarning("@OxygenTimer - Oxygen timer is already running.");
        }
    }

    private IEnumerator Timer(float time)
    {
        yield return new WaitForSeconds(time);

        OnOxygenDepleted?.Invoke();
        _isRunning = false;
    }
}
