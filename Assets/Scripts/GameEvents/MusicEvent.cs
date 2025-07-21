using UnityEngine;

public class MusicEvent : GameEvent
{
    [SerializeField] private AudioClip music;
    [SerializeField] private float musicVolume = 1.0f;
    [SerializeField] private bool fadeOutMusic;
    [SerializeField] private bool fadeInMusic;
    [SerializeField] private float fadeTime = 0.0f;
    [SerializeField] private bool playAsOneShotSound;

    public void SetMusic(AudioClip music)
    {
        this.music = music;
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
    }

    public override void Execute()
    {
        base.Execute();

        if (playAsOneShotSound)
        {
            SoundManager.instance.PlayOneShotSound(music, musicVolume);
        }
        else
        {
            if (music != null)
            {
                if (fadeInMusic)
                {
                    SoundManager.instance.FadeInMusic(music, musicVolume);
                }
                else
                {
                    SoundManager.instance.LoadAndPlayMusic(music, musicVolume);
                }
            }
            else
            {
                if (fadeOutMusic)
                {
                    SoundManager.instance.FadeOutLoadedMusic(fadeTime);
                }
                else
                {
                    SoundManager.instance.StopLoadedMusic();
                }
            }
        }
        
        GameEventCompleted(this);
    }
}
