using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private GameObject _gunEnd;
    [SerializeField] private GameObject _gunPivot;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private float _fireRate;
    [SerializeField] private GameEvent _PlayLaserShoot;

    [SerializeField] private FloatReference _AmmoAmount;

    private GameObject _target;
    private float _timer;

    private void Update()
    {
        if (_target == null) return;

        RotateGun();
        ShootBullet();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<UFOBehaviour>() == null) return;
        _target = collision.gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<UFOBehaviour>() == null) return;
        _target = null;
    }
    private void RotateGun()
    {
        Vector3 lookAtPos = _gunPivot.transform.InverseTransformPoint(_target.transform.position);
        float angle = Mathf.Atan2(lookAtPos.y, lookAtPos.x) * Mathf.Rad2Deg + 90;
        _gunPivot.transform.Rotate(0, 0, angle);
    }

    private void ShootBullet()
    {
        _timer += Time.deltaTime;

        if (_timer > _fireRate)
        {
            if (_AmmoAmount.value <= 0) return;
            _AmmoAmount.variable.value--;
            _timer = 0;
            GameObject bullet = Instantiate(_bullet, _gunEnd.transform.position, _gunPivot.transform.rotation);
            _PlayLaserShoot.Raise();
        }
    }
}
