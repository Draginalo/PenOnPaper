using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource m_SelfAudioSource;
    [SerializeField] private AudioSource m_MusicAudioSource;
    private float m_MusicFadeTime = 3.0f;
    private float currMusicTime = 0.0f;
    private float currMusicVolume = 0.0f;

    private List<AudioPack> audioPack = new();

    public Coroutine fadeOutHandleCoroutine;

    public Coroutine fadeOutMusicHandleCoroutine;
    public Coroutine fadeInMusicHandleCoroutine;

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

        //To handle if previous music is still fading out
        //if (newPack.state == AudioState.FADING_IN)
        //{
        //    newPack.state = AudioState.NONE;
        //}

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

        AudioPack pack = audioPack[index];
        if (pack.state == AudioState.FADING_IN)
        {
            pack.state = AudioState.NONE;
        }

        audioPack[index] = pack;

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

        AudioPack pack = audioPack[index];
        if (pack.state == AudioState.FADING_IN)
        {
            pack.state = AudioState.NONE;
        }

        audioPack[index] = pack;

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


    ////MUSIC
    public void LoadAndPlayMusic(AudioClip music, float volume = 1.0f)
    {
        //To handle if previous music is still fading out
        if (fadeOutMusicHandleCoroutine != null)
        {
            StopCoroutine(fadeOutMusicHandleCoroutine);
            fadeOutMusicHandleCoroutine = null;
        }

        if (m_MusicAudioSource.isPlaying)
        {
            m_MusicAudioSource.Stop();
        }

        m_MusicAudioSource.volume = volume;
        m_MusicAudioSource.loop = true;
        m_MusicAudioSource.clip = music;
        m_MusicAudioSource.Play();
    }

    public void StopLoadedMusic()
    {
        if (fadeOutMusicHandleCoroutine != null)
        {
            StopCoroutine(fadeOutMusicHandleCoroutine);
            fadeOutMusicHandleCoroutine = null;
        }

        if (m_MusicAudioSource != null && m_MusicAudioSource.isPlaying)
        {
            m_MusicAudioSource.Stop();
        }
    }

    public void FadeOutLoadedMusic(float fadeOutTime)
    {
        if (fadeInMusicHandleCoroutine != null)
        {
            StopCoroutine(fadeInMusicHandleCoroutine);
            fadeInMusicHandleCoroutine = null;
        }

        m_MusicFadeTime = fadeOutTime;
        currMusicVolume = m_MusicAudioSource.volume;
        currMusicTime = 0.0f;

        if (fadeOutMusicHandleCoroutine == null)
        {
            fadeOutMusicHandleCoroutine = StartCoroutine(Co_FadeOutMusic());
        }
    }

    private bool FadeOutMusicHandler()
    {
        currMusicTime += Time.deltaTime;

        m_MusicAudioSource.volume = Mathf.Lerp(currMusicVolume, 0, currMusicTime / m_MusicFadeTime);

        return currMusicTime / m_MusicFadeTime >= 1.0f;
    }

    private IEnumerator Co_FadeOutMusic()
    {
        yield return new WaitUntil(FadeOutMusicHandler);
        fadeOutMusicHandleCoroutine = null;
    }

    public void FadeInMusic(AudioClip music, float volume, float fadeInTime = 1.0f)
    {
        if (fadeOutMusicHandleCoroutine != null)
        {
            StopCoroutine(fadeOutMusicHandleCoroutine);
            fadeOutMusicHandleCoroutine = null;
        }

        LoadAndPlayMusic(music, 0.0f);

        m_MusicFadeTime = fadeInTime;
        currMusicVolume = volume;
        currMusicTime = 0.0f;

        if (fadeInMusicHandleCoroutine == null)
        {
            fadeInMusicHandleCoroutine = StartCoroutine(Co_FadeInMusic());
        }
    }

    private bool FadeInMusicHandler()
    {
        currMusicTime += Time.deltaTime;

        m_MusicAudioSource.volume = Mathf.Lerp(0, currMusicVolume, currMusicTime / m_MusicFadeTime);

        return currMusicTime / m_MusicFadeTime >= 1.0f;
    }

    private IEnumerator Co_FadeInMusic()
    {
        yield return new WaitUntil(FadeInMusicHandler);
        fadeInMusicHandleCoroutine = null;
    }
}
