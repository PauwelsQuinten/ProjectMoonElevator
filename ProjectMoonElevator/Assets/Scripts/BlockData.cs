using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockData", menuName = "ScriptableObjects / Block Data")]
public class BlockData : ScriptableObject
{
    public BlockType Type;
    public int Health;
    public float Speed;
    public bool IsExplosive;
    public bool IsMagnetic;
    [Range(0,100)]public int SpawnChance = 100;
}

public enum BlockType { Red, Green, Blue, Black }
