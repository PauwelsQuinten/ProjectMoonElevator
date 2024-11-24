using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class UFOBehaviour : MonoBehaviour
{
    [SerializeField]private GameObject _gunPivot;
    [SerializeField] private GameObject _gunEnd;

    [SerializeField] private GameObject _bullet;

    [SerializeField] private GameEvent _playUfoMove;
    [SerializeField] private GameEvent _StopUfoMove;
    [SerializeField] private GameEvent _PlayLaserShoot;

    private GameObject _target;
    private float _FireRate = 4;
    private float _timer;
    private bool _leaving;
    private Vector3 _startpos;
    private float _health = 2;

    public SpawnSide SpawnedSide;

    // Start is called before the first frame update
    void Start()
    {
        _playUfoMove.Raise();
        SelectTarget();
        _startpos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(_health <= 0 || _leaving) _StopUfoMove.Raise();
        else if(_health >0 && !_leaving) _playUfoMove.Raise();
        if (_health <= 0) Destroy(gameObject);
        if (_target == null) SelectTarget();
        if (_leaving) MoveAway();
        if (_target != null)
        {
            if (_target.Equals(this.gameObject)) return;
            RotateGun();

            if(!_leaving)
                MoveUFO();

            ShootBullet();
        }
    }

    private void SelectTarget()
    {
        if (SpawnedSide == SpawnSide.Right)
        {
            GetTarget(transform.position, new Vector3(-60,0,0));
        }
        else if (SpawnedSide == SpawnSide.Left)
        {
            GetTarget(transform.position, new Vector3(60, 0, 0));
        }

        if(_target != null) transform.position = new Vector3(transform.position.x, _target.transform.position.y + 20, 0);
    }

    private void GetTarget(Vector3 pos, Vector3 dir)
    {
        Vector3 posToCheck = pos + dir + new Vector3(0, 0, 1);

        Debug.DrawRay(posToCheck, Vector3.back, Color.yellow, 2);

        RaycastHit2D hit = Physics2D.Raycast(posToCheck, Vector3.back, 3);
        if (hit)
        {
            UFOBehaviour self = hit.collider.gameObject.GetComponent<UFOBehaviour>();
            BlockBehaviour hitBlock = hit.collider.gameObject.GetComponent<BlockBehaviour>();
            if (hitBlock != null)
            {
                _target = hitBlock.gameObject;
            }
            else if(self != null)
            {
                MoveAway();
            }
        }
        else
        {
            if(SpawnedSide == SpawnSide.Left) GetTarget(transform.position, dir - new Vector3(5, 0, 0));
            if (SpawnedSide == SpawnSide.Right) GetTarget(transform.position, dir - new Vector3(-5, 0, 0));
        }
    }

    private void RotateGun()
    {
        Vector3 lookAtPos = _gunPivot.transform.InverseTransformPoint(_target.transform.position);
        float angle = Mathf.Atan2(lookAtPos.y, lookAtPos.x) * Mathf.Rad2Deg + 90;
        _gunPivot.transform.Rotate(0, 0, angle);
    }

    private void MoveUFO()
    {
        float speed = 2;
        float height = 1;
        Vector3 newPosition = _startpos;
        newPosition.y += Mathf.Sin(Time.time * speed) * height;
        transform.position = newPosition;
    }

    private void ShootBullet()
    {
        if (_leaving) return;
        _timer += Time.deltaTime;

        if(_timer > _FireRate)
        {
            _timer = 0;
            GameObject bullet = Instantiate(_bullet, _gunEnd.transform.position, _gunPivot.transform.rotation);
            _PlayLaserShoot.Raise();
        }
    }

    private void MoveAway()
    {
        if (SpawnedSide == SpawnSide.Right)
        {
            Vector3 newpos = transform.position + new Vector3(20, 0, 0);
            transform.position = Vector3.MoveTowards(transform.position, newpos , 0.2f);
            StartCoroutine(DoCooldown(5));
        }
        else if(SpawnedSide == SpawnSide.Left)
        {
            Vector3 newpos = transform.position + new Vector3(-20, 0, 0);
            transform.position = Vector3.MoveTowards(transform.position, newpos, 0.2f);
            StartCoroutine(DoCooldown(5));
        }
    }

    public void IsLeaving()
    {
        _leaving = true;
    }

    private IEnumerator DoCooldown(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        Destroy(gameObject);
    }

    public void TakeDamage()
    {
        _health--;
    }
}

public enum SpawnSide
{
    Left,
    Right
}

