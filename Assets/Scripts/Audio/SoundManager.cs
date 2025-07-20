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
    private List<AudioPack> audioPack = new();

    public Coroutine fadeOutHandleCoroutine;

    public static SoundManager instance;

    private enum AudioState
    {
        NONE,
        FADING_IN,
        FADING_OUT
    }

    private struct AudioPack
    {
        public AudioSource audioSource;
        public float currTime;
        public float currVolume;
        public float fadeTime;
        public Coroutine sfxFadeInCoroutine;
        public AudioState state;
    }

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
        AudioPack newPack = new AudioPack();

        if (otherAudioSource != null)
        {
            newPack.audioSource = otherAudioSource;
        }
        else
        {
            newPack.audioSource = m_SelfAudioSource;
        }

        if (newPack.audioSource.isPlaying)
        {
            newPack.audioSource.Stop();
        }

        if (pos != null)
        {
            newPack.audioSource.transform.position = pos.Value;
        }

        newPack.audioSource.volume = volume;
        newPack.audioSource.loop = looping;
        newPack.audioSource.clip = sound;
        newPack.audioSource.Play();
        audioPack.Add(newPack);
    }

    private int FindAudioPackIndex(AudioSource audioSource)
    {
        int index = -1;
        for (int i = 0; i < audioPack.Count; i++)
        {
            if (audioPack[i].audioSource == audioSource)
            {
                index = i;
                break;
            }
        }

        return index;
    }

    public void StopLoadedSound(AudioSource otherAudioSource = null)
    {
        if (otherAudioSource == null)
        {
            otherAudioSource = m_SelfAudioSource;
        }

        int index = FindAudioPackIndex(otherAudioSource);

        if (index == -1)
        {
            return;
        }

        if (audioPack[index].sfxFadeInCoroutine != null)
        {
            StopCoroutine(audioPack[index].sfxFadeInCoroutine);
        }

        if (audioPack[index].audioSource != null && audioPack[index].audioSource.isPlaying)
        {
            audioPack[index].audioSource.Stop();
            audioPack.RemoveAt(index);
        }
    }

    public void FadeOutLoadedSound(float fadeOutTime, AudioSource otherAudioSource = null)
    {
        if (otherAudioSource == null)
        {
            otherAudioSource = m_SelfAudioSource;
        }

        int index = FindAudioPackIndex(otherAudioSource);

        if (index == -1)
        {
            return;
        }

        if (audioPack[index].sfxFadeInCoroutine != null)
        {
            StopCoroutine(audioPack[index].sfxFadeInCoroutine);
        }

        AudioPack pack = audioPack[index];

        pack.fadeTime = fadeOutTime;
        pack.currVolume = audioPack[index].audioSource.volume;
        pack.currTime = 0.0f;
        pack.state = AudioState.FADING_OUT;

        audioPack[index] = pack;

        if (fadeOutHandleCoroutine == null)
        {
            StartCoroutine(Co_FadeOut());
        }
    }

    private bool FadeOutHandler()
    {
        int counter = 0;
        for (int i = 0; i < audioPack.Count; i++)
        {
            AudioPack pack = audioPack[i];
            if (pack.state == AudioState.FADING_OUT)
            {
                counter++;
                pack.currTime += Time.deltaTime;

                //Checks for if audio source is deleted before fade out done
                if (pack.audioSource != null)
                {
                    pack.audioSource.volume = Mathf.Lerp(pack.currVolume, 0, pack.currTime / pack.fadeTime);
                }

                audioPack[i] = pack;

                if (pack.currTime / pack.fadeTime >= 1.0f)
                {
                    audioPack.RemoveAt(i);
                    counter--;
                }
            }
        }

        return counter == 0;
    }

    private void Update()
    {
        Debug.Log(audioPack.Count);
    }

    private IEnumerator Co_FadeOut()
    {
        yield return new WaitUntil(FadeOutHandler);
    }

    public void LoadAndFadeInSound(AudioClip sound, float volume, bool looping = false, float fadeInTime = 1.0f, AudioSource otherAudioSource = null, Nullable<Vector3> pos = null)
    {
        LoadAndPlaySound(sound, 0, looping, otherAudioSource, pos);

        AudioPack pack = audioPack[audioPack.Count - 1];

        pack.fadeTime = fadeInTime;
        pack.currVolume = volume;
        pack.currTime = 0.0f;
        pack.state = AudioState.FADING_IN;

        audioPack[audioPack.Count - 1] = pack;

        StartCoroutine(Co_FadeIn());
    }

    private bool FadeInHandler()
    {
        int counter = 0;
        for (int i = 0; i < audioPack.Count; i++)
        {
            AudioPack pack = audioPack[i];
            if (pack.state == AudioState.FADING_IN)
            {
                counter++;
                pack.currTime += Time.deltaTime;

                //Checks for if audio source is deleted before fade out done
                if (pack.audioSource != null)
                {
                    pack.audioSource.volume = Mathf.Lerp(0, pack.currVolume, pack.currTime / pack.fadeTime);
                }

                audioPack[i] = pack;

                if (pack.currTime / pack.fadeTime >= 1.0f)
                {
                    pack.state = AudioState.NONE;
                    counter--;
                }
            }
        }

        return counter == 0;
    }

    private IEnumerator Co_FadeIn()
    {
        yield return new WaitUntil(FadeInHandler);
    }
}
