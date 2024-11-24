using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;
using static Unity.Collections.AllocatorManager;

public class GunSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _turret;
    [SerializeField] private BoolReference _isSpawning;
    [SerializeField] private FloatReference _maxY;
    [SerializeField] private FloatReference _GunAmount;
    [SerializeField] private FloatReference _gunPosY;

    private GameObject _CurrentTurret;
    private BlockBehaviour _touchingBlock;

    private BoxCollider2D _blockCheck;

    private void Start()
    {
        _blockCheck = GetComponent<BoxCollider2D>();
    }
    private void Update()
    {
        if (_CurrentTurret != null)
        {
            _gunPosY.variable.value = transform.position.y;
        }
        if (_isSpawning.value) return;
        transform.position = new Vector3(10, _maxY.value + 10, 0);
    }

    public void MoveLeft(InputAction.CallbackContext ctx)
    {
        if (_CurrentTurret == null) return;
        if (!ctx.performed) return;
        Vector3 newPos = transform.position + new Vector3(-5, 0, 0);
        transform.position = newPos;
        if (!IsValidX(new Vector3(2.5f, 0, 0)))
        {
            newPos = transform.position + new Vector3(5, 0, 0);
            transform.position = newPos;
        }
    }

    public void MoveRight(InputAction.CallbackContext ctx)
    {
        if (_CurrentTurret == null) return;
        if (!ctx.performed) return;
        Vector3 newPos = transform.position + new Vector3(5, 0, 0);
        transform.position = newPos;
        if (!IsValidX(new Vector3(-2.5f,0,0)))
        {
            newPos = transform.position + new Vector3(-5, 0, 0);
            transform.position = newPos;
        }
    }

    public void MoveDown(InputAction.CallbackContext ctx)
    {
        if (_CurrentTurret == null) return;
        if (!ctx.performed) return;
        if (CheckForEmpty(transform.position, new Vector3(0,-5,0)))
        {
            transform.position += new Vector3(0, -5, 0);
        }
    }

    public void MoveUp(InputAction.CallbackContext ctx)
    {
        if (_CurrentTurret == null) return;
        if (!ctx.performed) return;
        if (CheckForEmpty(transform.position, new Vector3(0,5,0)))
        {
            transform.position += new Vector3(0, 5, 0);
        }
    }

    public void RotateLeft(InputAction.CallbackContext ctx)
    {
        if (_CurrentTurret == null) return;
        if (!ctx.performed) return;
        transform.Rotate(0, 0, -180);
    }

    public void RotateRight(InputAction.CallbackContext ctx)
    {
        if (_CurrentTurret == null) return;
        if (!ctx.performed) return;
        transform.Rotate(0, 0, 180);
    }

    public void SpawnTurret(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        if (_CurrentTurret != null) return;
        if(_GunAmount.value < 1) return;
        _GunAmount.variable.value -= 1;
        StartCoroutine(DoSpawnTurret());
    }

    public void DiscardTurret(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        if (_CurrentTurret == null) return;
        Destroy(_CurrentTurret);
        _isSpawning.variable.value = false;
        _GunAmount.variable.value += 1;
    }

    public void PlaceTurret(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        if (_CurrentTurret == null || _touchingBlock == null) return;
        if (!_touchingBlock.BlockData.Type.Equals(BlockType.Blue)) return;
        if (!_touchingBlock.IsPlaced) return;
        StartCoroutine(DoPlaceTurret());
    }

    private IEnumerator DoPlaceTurret()
    {
        yield return new WaitForSeconds(0.3f);
        _isSpawning.variable.value = false;
        _CurrentTurret.transform.parent = _touchingBlock.gameObject.transform;
        _CurrentTurret = null;
        _touchingBlock = null;
    }

    private IEnumerator DoSpawnTurret()
    {
        yield return new WaitForSeconds(0.3f);
        if(transform.rotation.eulerAngles.z == 0) _blockCheck.offset = new Vector2(-2.5f, 0);
        else _blockCheck.offset = new Vector2(2.5f, 0);
        _CurrentTurret = Instantiate(_turret);
        _CurrentTurret.transform.SetParent(transform);
        _CurrentTurret.transform.localPosition = Vector3.zero;
        _isSpawning.variable.value = true;
    }

    private bool IsValidX(Vector3 dir)
    {
        Vector3 newPos = transform.position;
        Vector2 screenPos = Camera.main.WorldToScreenPoint(newPos);
        if (screenPos.x >= Screen.width || screenPos.x < 0 ||
            !CheckForEmpty(newPos, dir) || !CheckForEmpty(newPos, dir)) return false;
        return true;
    }

    private bool CheckForEmpty(Vector3 pos, Vector3 dir)
    {
        Vector3 posToCheck = pos + dir + new Vector3(0, 0, 2);

        Debug.DrawRay(posToCheck, Vector3.back, Color.yellow, 2);

        RaycastHit2D hit = Physics2D.Raycast(posToCheck, Vector3.back, 3, 8);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BlockBehaviour blockBehaviour = collision.GetComponent<BlockBehaviour>();
        if(blockBehaviour != null) _touchingBlock = blockBehaviour;
    }
}
