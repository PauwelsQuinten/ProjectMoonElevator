using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private FloatReference _maxY;
    [SerializeField] private InputActionReference _move;

    [SerializeField] private BoolReference _moveCam;
    [SerializeField] private BoolReference _isSpawning;

    [SerializeField] private FloatReference _currentTurretY;

    private bool _ResetCam = true;
    void Update()
    {
        float newY;
        if (_isSpawning.value)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(0, _currentTurretY.value, 0), 0.15f);
            if (_currentTurretY.value < 0) transform.position = new Vector3(0, 0, 0);
        }
        else _ResetCam = true;
        if (_moveCam.value)
        {
            newY = transform.position.y + _move.action.ReadValue<Vector2>().y;
            newY = Mathf.Clamp(newY, 0, transform.position.y + 100);
            transform.position = new Vector3(0, newY, 0);
            Debug.Log(_move.action.ReadValue<Vector2>());
        }
        else if(_ResetCam)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(0, _maxY.value + 5, 0), 0.15f);
            if (Mathf.Abs(_maxY.value + 5 - transform.position.y) <= 0.01f) _ResetCam = false;
        }
    }

    public void SetCameraToSpawnedBlock()
    {
        _ResetCam = true;
    }

    public void SetMoveCam(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        if (_moveCam.value)
        {
            _moveCam.variable.value = false;
            _ResetCam = true;
        }
        else _moveCam.variable.value = true;
    }
}
