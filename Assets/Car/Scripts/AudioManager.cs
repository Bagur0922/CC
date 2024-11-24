using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
    }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Audio Clips")]
    public Sound[] bgmSounds;
    public Sound[] sfxSounds;

    private Dictionary<string, AudioClip> bgmDictionary = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> sfxDictionary = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        // 싱글톤 패턴 구현
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // 딕셔너리에 사운드 등록
        foreach (Sound sound in bgmSounds)
        {
            bgmDictionary.Add(sound.name, sound.clip);
        }

        foreach (Sound sound in sfxSounds)
        {
            sfxDictionary.Add(sound.name, sound.clip);
        }
    }

    // BGM 재생
    public void PlayBGM(string name)
    {
        if (bgmDictionary.ContainsKey(name))
        {
            bgmSource.clip = bgmDictionary[name];
            bgmSource.loop = true;
            bgmSource.Play();
        }
        else
        {
            Debug.LogWarning($"BGM not found: {name}");
        }
    }

    // BGM 정지
    public void StopBGM()
    {
        bgmSource.Stop();
    }

    // BGM 일시정지
    public void PauseBGM()
    {
        bgmSource.Pause();
    }

    // BGM 재개
    public void ResumeBGM()
    {
        bgmSource.UnPause();
    }

    // BGM 볼륨 조절
    public void SetBGMVolume(float volume)
    {
        bgmSource.volume = Mathf.Clamp01(volume);
    }

    // 효과음 재생
    public void PlaySFX(string name)
    {
        if (sfxDictionary.ContainsKey(name))
        {
            sfxSource.PlayOneShot(sfxDictionary[name]);
        }
        else
        {
            Debug.LogWarning($"SFX not found: {name}");
        }
    }

    // 효과음 볼륨 조절
    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = Mathf.Clamp01(volume);
    }
}