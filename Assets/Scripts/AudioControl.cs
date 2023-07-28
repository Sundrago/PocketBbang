using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioControl : MonoBehaviour
{
    public AudioSource[] sound = new AudioSource[3];
    public AudioSource[] soundFX = new AudioSource[5];
    public int currentPlaying = -1;
    int soundCount = 7;
    // Start is called before the first frame update
    void Start()
    {
        soundCount = sound.Length;
        StopMusic();
    }

    public void StopMusic()
    {
        for (int j = 0; j < soundCount; j++)
        {
            sound[j].Stop();
        }
    }

    public void ToggleMute(bool mute)
    {
        for (int j = 0; j < soundCount; j++)
        {
            sound[j].mute = mute;
        }
    }

    public void PlayMusic(int i)
    {
        if (currentPlaying == i) return;
        sound[i].Play();
        if(currentPlaying != -1) sound[currentPlaying].Stop();
        currentPlaying = i;
    }

    public void PauseMusic()
    {
        sound[currentPlaying].Pause();
    }

    public void ResumeMusic()
    {
        sound[currentPlaying].Play();
    }

    public void PlaySoundFx(int i)
    {
        soundFX[i].Play();
    }
}
