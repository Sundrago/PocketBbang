using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource[] sound = new AudioSource[3];
    [SerializeField] private AudioSource[] soundFX = new AudioSource[5];
    public int currentPlaying = -1;
    private int soundCount = 7;

    public static AudioController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        soundCount = sound.Length;
        StopMusic();
    }

    public void StopMusic()
    {
        for (var j = 0; j < soundCount; j++) sound[j].Stop();
    }

    public void ToggleMute(bool mute)
    {
        for (var j = 0; j < soundCount; j++) sound[j].mute = mute;
    }

    public void PlayMusic(int i)
    {
        if (currentPlaying == i) return;
        sound[i].Play();
        if (currentPlaying != -1) sound[currentPlaying].Stop();
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