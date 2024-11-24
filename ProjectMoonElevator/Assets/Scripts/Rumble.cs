using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Rumble : MonoBehaviour
{
    private Gamepad _controller;
    // Start is called before the first frame update
    void Start()
    {
        _controller = Gamepad.current;
        DontDestroyOnLoad(gameObject);
    }

    public void DoRumble(Component sender, object obj)
    {
        if (_controller == null) return;
        Vector3 Info = (Vector3)obj;
        float MinRumble = Info.x;
        float MaxRumble = Info.y;
        float cooldown = Info.z;
        _controller.SetMotorSpeeds(MinRumble, MaxRumble);
        StartCoroutine(DoCooldown(cooldown));
    }

    private IEnumerator DoCooldown(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        _controller.SetMotorSpeeds(0, 0);
    }
}
