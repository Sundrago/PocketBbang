using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public enum SFX_tag
{
    win,
    loose,
    whislte_start,
    whistle_end,
    winItem,
    scrumb_big,
    scrumb_small,
    scrumb_fail
}

public class AudioCtrl : SerializedMonoBehaviour
{
    public static AudioCtrl Instance;

    [SerializeField] private AudioSource sfx_source, bgm_source;

    [TableList(ShowIndexLabels = true)] [SerializeField]
    private List<AudioData> audioDatas;

    private AudioData bgmPlaying;
    private readonly float bgmVolume = 0.8f;

    private float sfxVolume = 0.8f;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetVolume();
    }

    public void PlaySFXbyTag(SFX_tag tag)
    {
        foreach (var data in audioDatas)
            if (data.tag == tag)
                sfx_source.PlayOneShot(data.src, data.volume * sfxVolume);
        //sfx_source.PlayOneShot(audioClips[(int)tag]);
    }

    public void SetVolume()
    {
        var mute = PlayerPrefs.GetInt("muteAudio", 0) == 1;

        sfxVolume = 1f;
        sfx_source.volume = mute ? 0 : 1;
        bgm_source.volume = mute ? 0 : 1;
    }

    public void PlayBGM(SFX_tag tag)
    {
        print("PLAY BGM : " + tag);
        foreach (var data in audioDatas)
            if (data.tag == tag)
            {
                bgmPlaying = data;
                bgm_source.clip = data.src;
                bgm_source.volume = data.volume * bgmVolume;
                bgm_source.Play();
                return;
            }
    }

#if UNITY_EDITOR
    [Button]
    private void AddSFXItems()
    {
        foreach (SFX_tag sfxTag in Enum.GetValues(typeof(SFX_tag)))
        {
            var alreadyHasKey = false;

            for (var i = 0; i < audioDatas.Count; i++)
                if (audioDatas[i].tag == sfxTag)
                {
                    alreadyHasKey = true;
                    break;
                }

            if (!alreadyHasKey)
            {
                var newData = new AudioData();
                newData.tag = sfxTag;
                audioDatas.Add(newData);
            }
        }
    }
#endif

    [Serializable]
    public class AudioData
    {
        public SFX_tag tag;
        public AudioClip src;

        [Range(0f, 1f)] public float volume = 0.8f;
    }
}