using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum ESfx
{

}

public enum EBgm
{

}

public class SoundManager : SingleTon<SoundManager>
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] List<AudioClip> sfxClips;
    [SerializeField] List<AudioClip> bgmClips;

    AudioSource bgmSource;
    public void SFXPlay(ESfx sfxType)
    {
        GameObject sfxPlayer = new GameObject(sfxType.ToString() + "Sound");
        AudioSource audioSource = sfxPlayer.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Sfx")[0];
        audioSource.clip = sfxClips[(int)sfxType];
        audioSource.Play();

        Destroy(sfxPlayer, sfxClips[(int)sfxType].length);
    }
    public void PlayBGM(EBgm bgm)
    {
        bgmSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Bgm")[0];
        bgmSource.clip = bgmClips[(int)bgm];
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void SetSfxVolume(float value)
    {
        audioMixer.SetFloat("SfxVolume", value);
    }

    public void SetBgmVolume(float value)
    {
        audioMixer.SetFloat("BgmVolume", value);
    }
}
