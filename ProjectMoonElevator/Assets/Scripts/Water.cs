using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        BlockBehaviour block = collision.gameObject.GetComponent<BlockBehaviour>();
        if (block == null) return;
        Destroy(block.gameObject);
    }
}
