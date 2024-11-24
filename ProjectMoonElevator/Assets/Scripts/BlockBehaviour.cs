using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
[Serializable]
public class BlockBehaviour : MonoBehaviour
{
    public BlockData BlockData;
    [HideInInspector] public Rigidbody2D RB;
    public int Health;
    public bool IsPlaced = false;

    [SerializeField] private GameEvent _spawnBlock;
    [SerializeField] private GameEvent _blockDestroyed;
    [SerializeField] private GameEvent _RumbleEvent;
    [SerializeField] private GameEvent _playPlaceBlock;
    [SerializeField] private FloatReference _highestPointY;
    [SerializeField] private GameObject _blocks;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private BlockShape _blockShape;
    [SerializeField] private BoolReference _camIsMoving;
    [SerializeField] private BoolReference _isSpawningTurret;
    [SerializeField] private FloatReference _amountOfBlacks;
    [SerializeField] private FloatReference _amountOfReds;
    [SerializeField] private FloatReference _amountOfBlues;
    [SerializeField] private FloatReference _amountOfGreens;
    [SerializeField] private FloatReference _blocksDestroyed;

    private bool _doCooldown = true;
    private bool _doFreeFall = false;
    private bool _doRumble = true;
    private Vector3 _currentRotation;

    // Start is called before the first frame update
    void Start()
    {
        Health = BlockData.Health;
        RB = GetComponent<Rigidbody2D>();
        RB.gravityScale = 0f;

        if (_blockShape == BlockShape.J || _blockShape == BlockShape.L || _blockShape == BlockShape.S|| _blockShape == BlockShape.Z || _blockShape == BlockShape.T)
            transform.position += new Vector3(2.5f, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (_camIsMoving.value || _isSpawningTurret.value)
        {
            StopCoroutine(DoMoveDown(0));
            return;
        }
        else if (!IsPlaced) MoveDown();
        if (Health <= 0) Destroy(gameObject);
    }

    private void OnDestroy()
    {
        SetBlockAmount(-1, this);
        _blocksDestroyed.variable.value += 1;
        _blockDestroyed.Raise();
        if (!BlockData.IsExplosive) return;
        List<Collider2D> list = new List<Collider2D>();
        list = Physics2D.OverlapCircleAll(transform.position, 3).ToList<Collider2D>();
        _RumbleEvent.Raise(this, new Vector3(0.3f, 0.3f, 0.5f));
        foreach (Collider2D obj in list)
        {
            BlockBehaviour block = obj.GetComponent<BlockBehaviour>();
            if (block == null) return;
            block.Health -= 1;
            var dir = (block.transform.position - transform.position);
            float wearoff = 1 - (dir.magnitude / 30);
            block.RB.AddForce(dir.normalized * 50 * wearoff, ForceMode2D.Impulse);

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_doRumble)
        {
            _RumbleEvent.Raise(this, new Vector3(0.2f, 0.2f, 0.5f));
            _playPlaceBlock.Raise();
            _doRumble = false;
        }
        if(IsPlaced) return;

        BlockBehaviour block = collision.gameObject.GetComponent<BlockBehaviour>();
        SetBlockAmount(1, this);
        if (_highestPointY.value < transform.position.y) _highestPointY.variable.value += 5;

        _spawnBlock.Raise();
        IsPlaced = true;
        RB.gravityScale = 2;

        transform.rotation = Quaternion.Euler(_currentRotation);

        if (BlockData.Type.Equals(BlockType.Red))
        {
            RB.constraints = RigidbodyConstraints2D.FreezeAll;
            RB.gravityScale = 0;
        }

        if (block == null) return;

        if (block.BlockData.Type.Equals(BlockType.Green) && BlockData.Type.Equals(BlockType.Green))
        {
            transform.SetParent(block.gameObject.transform);
        }
    }

    private void MoveDown()
    {
        if(_doFreeFall) return;
        if(!_doCooldown) return;
        StartCoroutine(DoMoveDown(BlockData.Speed));
        _doCooldown = false;
    }

    private IEnumerator DoMoveDown(float timer)
    {
        yield return new WaitForSeconds(timer);
        MoveDownBlock();
        _doCooldown = true;
    }

    public void MoveDownBlock()
    {
        transform.position += new Vector3(0, -5, 0);
        if (IsYOccupied())
        {
            transform.position += new Vector3(0, +5, 0);
            PlaceBlock();
        }
    }

    private void PlaceBlock()
    {
        RB.gravityScale = 2;
        _doFreeFall = true;
    }

    public void MoveLeft(InputAction.CallbackContext ctx)
    {
        if (_camIsMoving.value || _isSpawningTurret.value) return;
        if (IsPlaced) return;
        if (!ctx.performed) return;
        Vector3 newPos = transform.position + new Vector3(-5, 0, 0);
        transform.position = newPos;
        if (!IsValidX())
        {
            newPos = transform.position + new Vector3(5, 0, 0);
            transform.position = newPos;
        }
    }

    public void MoveRight(InputAction.CallbackContext ctx)
    {
        if (_camIsMoving.value || _isSpawningTurret.value) return;
        if (IsPlaced) return;
        if (!ctx.performed) return;
        Vector3 newPos = transform.position + new Vector3(5, 0, 0);
        transform.position = newPos;
        if (!IsValidX())
        {
            newPos = transform.position + new Vector3(-5, 0, 0);
            transform.position = newPos;
        }
    }

    public void MoveDown(InputAction.CallbackContext ctx)
    {
        if (_camIsMoving.value || _isSpawningTurret.value) return;
        if (IsPlaced) return;
        if (!ctx.performed) return;
        StopCoroutine(DoMoveDown(0));
        Vector3 newPos = transform.position + new Vector3(0, -5, 0);
        transform.position = newPos;
        if (IsYOccupied())
        {
            newPos = transform.position + new Vector3(0, 5, 0);
            transform.position = newPos;
        }
    }
    public void RotateLeft(InputAction.CallbackContext ctx)
    {
        if (_camIsMoving.value || _isSpawningTurret.value) return;
        if (IsPlaced) return;
        if (!ctx.performed) return;
        transform.eulerAngles += new Vector3(0, 0, 90);
        if (_blockShape != BlockShape.O) transform.position += new Vector3(2.5f, 0, 0);
        _currentRotation = transform.rotation.eulerAngles;
        if (IsValidX() || !IsYOccupied()) return;
        transform.eulerAngles += new Vector3(0, 0, -90);
        if (_blockShape != BlockShape.O) transform.position += new Vector3(-2.5f, 0, 0);
        _currentRotation = transform.rotation.eulerAngles;
    }

    public void RotateRight(InputAction.CallbackContext ctx)
    {
        if (_camIsMoving.value || _isSpawningTurret.value) return;
        if (IsPlaced) return;
        if (!ctx.performed) return;
        transform.eulerAngles += new Vector3(0, 0, -90);
        if (_blockShape != BlockShape.O) transform.position += new Vector3(2.5f, 0, 0);
        _currentRotation = transform.rotation.eulerAngles;
        if (IsValidX() || !IsYOccupied()) return;
        transform.eulerAngles += new Vector3(0, 0, 90);
        if (_blockShape != BlockShape.O) transform.position += new Vector3(-2.5f, 0, 0);
        _currentRotation = transform.rotation.eulerAngles;
    }

    private bool IsValidX()
    {
        foreach (Transform subBlock in _blocks.transform)
        {
            Vector3 newPos = subBlock.transform.position;
            Vector2 screenPos = Camera.main.WorldToScreenPoint(newPos);
            if (screenPos.x >= Screen.width || screenPos.x < 0 || 
                !CheckForEmpty(newPos, new Vector3(0,0,0)) || !CheckForEmpty(newPos, new Vector3(0, 0, 0))) return false;
        }
        return true;
    }
    private bool IsYOccupied()
    {
        foreach (Transform subBlock in _blocks.transform)
        {
            Vector3 newPos = subBlock.transform.position;
            if (!CheckForEmpty(newPos, new Vector3(0,-5,0)))
            {
                return true;
            }
        }
        return false;
    }

    private bool CheckForEmpty(Vector3 pos, Vector3 dir)
    {
        Vector3 posToCheck = pos + dir + new Vector3(0, 0, 1);

        Debug.DrawRay(posToCheck, Vector3.back, Color.yellow, 2);

        RaycastHit2D hit = Physics2D.Raycast(posToCheck, Vector3.back, 3, _layerMask);
        if (hit)
        {
            BlockBehaviour hitBlock;
            hitBlock = hit.collider.gameObject.GetComponent<BlockBehaviour>();
            if (hitBlock != null)
            {
                if (hitBlock == this) return true;
                else return false;
            }
            else return false;
        }
        return true;
    }

    public void TakeDamage()
    {
        Debug.Log("TakeDamage");
        Health -= 1;
        return;
    }

    private void SetBlockAmount(int amount, BlockBehaviour block)
    {
        if (block.BlockData.Type == BlockType.Black) _amountOfBlacks.variable.value += amount;
        if (block.BlockData.Type == BlockType.Red) _amountOfReds.variable.value += amount;
        if (block.BlockData.Type == BlockType.Blue) _amountOfBlues.variable.value += amount;
        if (block.BlockData.Type == BlockType.Green) _amountOfGreens.variable.value += amount;
    }
}

public enum BlockShape 
{
    I,
    J,
    L,
    O,
    S,
    T,
    Z
}

