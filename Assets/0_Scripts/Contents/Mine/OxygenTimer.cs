using System;
using System.Collections;
using UnityEngine;

public class OxygenTimer : MonoBehaviour
{
    private float _totalOxygen = 300;
    private float _currentOxygen;

    public float TotalOxygen {
        get => _totalOxygen;
        set => _totalOxygen = value;
    }

    public event Action OnOxygenDepleted;
    public event Action<float> OnOxygenChanged;

    private bool _isRunning = false;

    public void StartTimer(float totalOxygen = 0)
    {
        if (_isRunning) {
            Debug.LogWarning("@OxygenTimer - Oxygen timer is already running.");
            return;
        }

        if (totalOxygen > 0)
            _currentOxygen = totalOxygen;
        else
            _currentOxygen = _totalOxygen;

        _isRunning = true;
        StartCoroutine(Loop());
    }

    private IEnumerator Loop()
    {
        while (_currentOxygen > 0) {
            _currentOxygen -= Time.deltaTime;
            OnOxygenChanged?.Invoke(_currentOxygen);
            yield return null;
        }

        _currentOxygen = 0f;

        OnOxygenChanged?.Invoke(_currentOxygen);
        OnOxygenDepleted?.Invoke();

        _isRunning = false;
    }
}
