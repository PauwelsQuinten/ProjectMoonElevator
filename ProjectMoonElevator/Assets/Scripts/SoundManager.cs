using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip _blockPlaced;
    [SerializeField] private AudioClip _UfoMove;
    [SerializeField] private AudioClip _LaserShoot;
    [SerializeField] private AudioClip _Siren;

    [SerializeField] private AudioSource _audioSource1;
    [SerializeField] private AudioSource _audioSource2;
    public void PlayBlockPlaced()
    {
        _audioSource2.clip = _blockPlaced;
        _audioSource2.volume = 1f;
        _audioSource2.pitch = 0.5f;
        _audioSource2.Play();
    }

    public void PlayUfoMove()
    {
        _audioSource1.clip = _UfoMove;
        _audioSource1.volume = 0.2f;
        _audioSource1.pitch = 0.7f;
        if (_audioSource1.isPlaying) return;
        _audioSource1.Play();
    }

    public void StopUfoMove()
    {
        if (_audioSource1.clip != _UfoMove) return;
        StartCoroutine(DoStopUfoMove());
    }

    private IEnumerator DoStopUfoMove()
    {
        yield return new WaitForSeconds(0.5f);
        _audioSource1.Stop();
    }

    public void PlayLaserShoot()
    {
        _audioSource2.clip = _LaserShoot;
        _audioSource2.volume = 0.8f;
        _audioSource2.pitch = 2f;
        _audioSource2.Play();
    }

    public void PlaySiren()
    {
        _audioSource2.clip = _Siren;
        _audioSource2.volume = 0.5f;
        _audioSource2.pitch = 0.5f;
        _audioSource2.Play();
    }
}
