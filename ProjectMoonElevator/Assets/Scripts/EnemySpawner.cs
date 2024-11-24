using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private int _amountOfBlocksSpawned;
    [SerializeField] FloatReference _maxY;
    [SerializeField] FloatReference _amountOfRed;
    [SerializeField] GameObject _enemy;
    [SerializeField] int _spawnRate;

    [SerializeField] GameEvent _playSiren;
    private void Update()
    {
        if (_amountOfBlocksSpawned >= _spawnRate)
        {
            _amountOfBlocksSpawned = 0;
            if (DoSpawnEnemy()) SpawnEnemy();
        }
    }

    private bool DoSpawnEnemy()
    {
        int index = Random.Range(1, 3);
        if (index == 2) return true;
        return false;
    }

    private void SpawnEnemy()
    {
        if(_amountOfRed.value >= 1) _playSiren.Raise();
        int side = Random.Range(1, 3);
        int RandomYPos = Random.Range(0, (int)_maxY.value + 1);
        Vector3 newSpawnPos = new Vector3(0, RandomYPos, 0);
        if(side == 1)
        {
            newSpawnPos.x = -50;
            UFOBehaviour UFO = Instantiate(_enemy, newSpawnPos, transform.rotation).GetComponent<UFOBehaviour>();
            UFO.SpawnedSide = SpawnSide.Left;
        }
        else if (side == 2)
        {
            newSpawnPos.x = 50;
            UFOBehaviour UFO = Instantiate(_enemy, newSpawnPos, transform.rotation).GetComponent<UFOBehaviour>();
            UFO.SpawnedSide = SpawnSide.Right;
        }
    }

    public void BlockedSpawned()
    {
        _amountOfBlocksSpawned++;
    }
}
