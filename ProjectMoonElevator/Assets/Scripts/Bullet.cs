using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D _rb;
    [SerializeField] private GameEvent _rumble;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.AddForce(-transform.up * 50, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BlockBehaviour block = collision.gameObject.GetComponent<BlockBehaviour>();
        UFOBehaviour UFO = collision.gameObject.GetComponent<UFOBehaviour>();
        if (block != null)
        {
            block.TakeDamage();
            _rumble.Raise(this, new Vector3(0.1f, 0.1f, 0.15f));
        }
        if (UFO != null)
        {
            UFO.TakeDamage();
        }
        if(collision.gameObject.layer != LayerMask.NameToLayer("Trigger"))
        {
            Destroy(gameObject);
        }
    }
}
