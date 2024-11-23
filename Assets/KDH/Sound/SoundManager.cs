using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ESfx
{

}

public enum EBgm
{

}

public class SoundManager : SingleTon<SoundManager>
{
    [SerializeField] List<AudioClip> sfxClips;
    [SerializeField] List<AudioClip> bgmclips;

    AudioSource bgmPlayer;
    public void SFXPlay(ESfx sfxType)
    {
        GameObject go = new GameObject(sfxType.ToString() + "Sound");
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.clip = sfxClips[(int)sfxType];
        audioSource.Play();

        Destroy(go, sfxClips[(int)sfxType].length);
    }
    public void PlayBGM(EBgm bgm)
    {
        bgmPlayer.clip = bgmclips[(int)bgm];

        bgmPlayer.Play();
    }
}
