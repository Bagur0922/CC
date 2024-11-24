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
        // �̱��� ���� ����
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

        // ��ųʸ��� ���� ���
        foreach (Sound sound in bgmSounds)
        {
            bgmDictionary.Add(sound.name, sound.clip);
        }

        foreach (Sound sound in sfxSounds)
        {
            sfxDictionary.Add(sound.name, sound.clip);
        }
    }

    // BGM ���
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

    // BGM ����
    public void StopBGM()
    {
        bgmSource.Stop();
    }

    // BGM �Ͻ�����
    public void PauseBGM()
    {
        bgmSource.Pause();
    }

    // BGM �簳
    public void ResumeBGM()
    {
        bgmSource.UnPause();
    }

    // BGM ���� ����
    public void SetBGMVolume(float volume)
    {
        bgmSource.volume = Mathf.Clamp01(volume);
    }

    // ȿ���� ���
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

    // ȿ���� ���� ����
    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = Mathf.Clamp01(volume);
    }
}