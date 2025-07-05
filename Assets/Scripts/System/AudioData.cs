using UnityEngine;

[CreateAssetMenu(fileName = "AudioClip", menuName = "Game/Audio Clip")]
public class AudioData : ScriptableObject
{
    public AudioClip clip;
    public float volume = 1f;
    public float pitch = 1f;
    public float pitchVariance = 0.1f;

	public void Play(AudioSource audioSource)
	{
		audioSource.pitch = Random.Range(1 - pitchVariance, 1 + pitchVariance);
		audioSource.PlayOneShot(clip, volume);
	}
}
