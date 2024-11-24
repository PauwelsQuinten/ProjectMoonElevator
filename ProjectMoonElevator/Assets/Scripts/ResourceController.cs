using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceController : MonoBehaviour
{
    [SerializeField] private FloatReference _blueBlockCount;
    [SerializeField] private FloatReference _blackBlockCount;

    [SerializeField] private FloatReference _AmmoCount;
    [SerializeField] private FloatReference _GunSuply;

    [SerializeField] private BoolReference _isSpawning;

    [SerializeField] private float _spawnRateGuns = 2;
    [SerializeField] private float _spawnRateAmmo = 4;

    private float _timer;
    private int _amountOfAmmoToAdd;
    // Update is called once per frame
    void Update()
    {
        if (_isSpawning.value) return;
        _timer += Time.deltaTime;

        if(_timer > 4)
        {
            _timer = 0;
            GenerateAmmo();
            GenerateGuns();
        }
    }

    private void GenerateAmmo()
    {
        _AmmoCount.variable.value += (int)(_blackBlockCount.value / _spawnRateAmmo);
    }

    private void GenerateGuns()
    {
        float lasGunSuply = _GunSuply.value;
        _GunSuply.variable.value += (int)(_blueBlockCount.value / _spawnRateGuns);
        if(lasGunSuply != _GunSuply.value) _blueBlockCount.variable.value = 0;
    }
}
