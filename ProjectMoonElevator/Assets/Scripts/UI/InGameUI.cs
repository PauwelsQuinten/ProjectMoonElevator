using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _turretCounter;
    [SerializeField] private TextMeshProUGUI _ammoCounter;

    [SerializeField] private FloatReference _TurretAmount;
    [SerializeField] private FloatReference _ammoAmount;

    [SerializeField] private FloatReference _failRate;
    [SerializeField] private FloatReference _winRate;

    [SerializeField] private FloatReference _maxY;
    [SerializeField] private FloatReference _blocksDestroyed;

    void Update()
    {
        _turretCounter.text = _TurretAmount.value.ToString();
        _ammoCounter.text = _ammoAmount.value.ToString();

        if (_maxY.value > _winRate.value) SceneManager.LoadScene("Win");
        if (_blocksDestroyed.value > _failRate.value) SceneManager.LoadScene("Fail");
    }
}
