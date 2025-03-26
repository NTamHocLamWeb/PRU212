using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicAudioSource;
    public AudioSource vfxAudioSource;

    public AudioClip musicClip;
    public AudioClip coinClip;
    public AudioClip loseClip;
    public AudioClip shotClip;
    public AudioClip hurtClip;
	public AudioClip jumpClip;

	void Start()
	{
		musicAudioSource.clip = musicClip;
        musicAudioSource.Play();
	}

    public void PlaySFX(AudioClip sfxClip)
    {
        vfxAudioSource.clip = sfxClip;
        vfxAudioSource.PlayOneShot(sfxClip);
    }

    public void StopMusicBackground()
    {
        musicAudioSource.clip = loseClip;
        musicAudioSource.Play();
    }
}
