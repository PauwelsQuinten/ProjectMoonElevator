using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject>_RedBlocks = new List<GameObject>();
    [SerializeField] private List<GameObject> _GreenBlocks = new List<GameObject>();
    [SerializeField] private List<GameObject> _BlueBlocks = new List<GameObject>();
    [SerializeField] private List<GameObject> _BlackBlocks = new List<GameObject>();

    [SerializeField] private FloatReference _highestBlockY;

    [SerializeField] private List<BlockData> _blocks = new List<BlockData>();

    private double _redWeights;
    private double _greenWeights;
    private double _blueWeights;
    private double _blackWeights;

    private double _accumulatedWeigts;

    private System.Random _rand = new System.Random();

    // Start is called before the first frame update
    void Start()
    {
        CalculateWeigths();
        SpawnBlock();
    }

    public void SpawnBlock()
    {
        BlockType SpawnedType = GetRandomBlockType();
        //BlockType SpawnedType = BlockType.Blue;
        switch (SpawnedType)
        {
            case BlockType.Red:
                SpawnRandomBlock(_RedBlocks);
                break;
            case BlockType.Green:
                SpawnRandomBlock(_GreenBlocks);
                break;
            case BlockType.Blue:
                SpawnRandomBlock(_BlueBlocks);
                break;
            case BlockType.Black:
                SpawnRandomBlock(_BlackBlocks);
                break;
        }
    }

    private BlockType GetRandomBlockType()
    {
        int blockType = GetRandomTypeIndex();
        
        switch(blockType)
        {
            case 1:
                return BlockType.Red;
            case 2: 
                return BlockType.Green;
            case 3: 
                return BlockType.Blue;
            case 4: 
                return BlockType.Black;
        }
        return BlockType.Red;
    }

    private void SpawnRandomBlock(List<GameObject> blocks)
    {
        int indexToSpawn = UnityEngine.Random.Range(0, blocks.Count);
        GameObject block = Instantiate(blocks[indexToSpawn], transform.position, transform.rotation);
        block.transform.position = new Vector3(0, _highestBlockY.value + 45, 0);
        block.transform.parent = transform;
    }

    private void CalculateWeigths()
    {
        _accumulatedWeigts = 0;
        foreach(BlockData block in _blocks)
        {
            _accumulatedWeigts += block.SpawnChance;

            switch(block.Type)
            {
                case BlockType.Red:
                    _redWeights = _accumulatedWeigts;
                    break;
                case BlockType.Green:
                    _greenWeights = _accumulatedWeigts;
                    break;
                case BlockType.Blue:
                    _blueWeights = _accumulatedWeigts;
                    break;
                case BlockType.Black:
                    _blackWeights = _accumulatedWeigts;
                    break;
            }
        }
    }

    private int GetRandomTypeIndex()
    {
        double r = _rand.NextDouble() * _accumulatedWeigts;
        if (_redWeights >= r) return 1;
        if (_greenWeights >= r) return 2;
        if (_blueWeights >= r) return 3;
        if (_blackWeights >= r) return 4;

        return 3;
    }
}
