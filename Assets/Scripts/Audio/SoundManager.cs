using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource m_SelfAudioSource;
    private float m_FadeTime = 3.0f;
    private float currTime = 0.0f;
    private float currVolume = 0.0f;
    private AudioSource currAudioSource;

    public static SoundManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            return;
        }

        Destroy(gameObject);
    }

    public void PlayOneShotSound(AudioClip sound, float volume = 1, Nullable<Vector3> pos = null)
    {
        if (pos != null)
        {
            m_SelfAudioSource.transform.position = pos.Value;
        }

        m_SelfAudioSource.PlayOneShot(sound, volume);
    }

    public void LoadAndPlaySound(AudioClip sound, float volume, bool looping = false, AudioSource otherAudioSource = null, Nullable<Vector3> pos = null)
    {
        if (otherAudioSource != null)
        {
            currAudioSource = otherAudioSource;
        }
        else
        {
            currAudioSource = m_SelfAudioSource;
        }

        if (currAudioSource.isPlaying)
        {
            currAudioSource.Stop();
        }

        if (pos != null)
        {
            currAudioSource.transform.position = pos.Value;
        }

        currAudioSource.volume = volume;
        currAudioSource.loop = looping;
        currAudioSource.clip = sound;
        currAudioSource.Play();
    }

    public void StopLoadedSound(AudioSource otherAudioSource = null)
    {
        if (currAudioSource.isPlaying)
        {
            currAudioSource.Stop();
        }
    }

    public void FadeOutLoadedSound(float fadeOutTime)
    {
        m_FadeTime = fadeOutTime;
        currVolume = currAudioSource.volume;
        currTime = 0.0f;

        StartCoroutine(Co_FadeOut());
    }

    private bool FadeOutHandler()
    {
        currTime += Time.deltaTime;
        currAudioSource.volume = Mathf.Lerp(currVolume, 0, currTime / m_FadeTime);
        
        return currTime / m_FadeTime >= 1.0f;
    }

    private IEnumerator Co_FadeOut()
    {
        yield return new WaitUntil(FadeOutHandler);
    }
}
