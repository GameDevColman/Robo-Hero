using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{
    public AudioSource backgroundMusicSource;

    public void PlaySound(AudioClip audioClip)
    {
        AudioSource.PlayClipAtPoint(audioClip, SceneManagerScript.Instance.playerScript.transform.position);
    }

    public void PlayBackgroundMusic(AudioClip audioClip)
    {
        backgroundMusicSource.clip = audioClip;
        backgroundMusicSource.Play();
    }
}
