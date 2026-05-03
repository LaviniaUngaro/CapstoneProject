using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private List<Sounds> _sfxSounds;
    [SerializeField] private List<Sounds> _backgroundMusic;

    [SerializeField] private AudioSource _backgroundSource;
    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private AudioSource _sfxLoopSource;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySFXSound(string sfxToPlay)
    {
        var sound = _sfxSounds.Find(t => t.SoundName == sfxToPlay);

        if (sound != null)
            _sfxSource.PlayOneShot(sound.AudioClip);
    }

    public void PlaySFXLoopSound(string sfxToPlay)
    {
        var sound = _sfxSounds.Find(t => t.SoundName == sfxToPlay);

        if (sound != null && !_sfxLoopSource.isPlaying)
        {
            _sfxLoopSource.clip = sound.AudioClip;
            _sfxLoopSource.loop = true;
            _sfxLoopSource.Play();
        }
    }

    public void PlayBackgroundMusic(string musicToPlay)
    {
        var sound = _backgroundMusic.Find(t => t.SoundName == musicToPlay);

        if (sound == null)
            return;

        StopAllCoroutines();
        _backgroundSource.volume = 1f;
        StopBackgroundMusic();
        _backgroundSource.clip = sound.AudioClip;
        _backgroundSource.loop = true;
        _backgroundSource.Play();
    }

    public void FadeOutBackgroundMusic(float duration = 1)
    {
        StartCoroutine(FadeOut(duration));
    }

    private IEnumerator FadeOut(float duration)
    {
        float startVolume = _backgroundSource.volume;
        float elapsed = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            _backgroundSource.volume = Mathf.Lerp(startVolume, 0, elapsed / duration);
            yield return null;
        }
        StopBackgroundMusic();
        _backgroundSource.volume = startVolume;
    }

    public void StopBackgroundMusic()
    {
        _backgroundSource.Stop();
    }

    public void StopSFXLoopSound()
    {
        _sfxLoopSource.Stop();
    }
}
