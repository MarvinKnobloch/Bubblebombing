using UnityEngine;

public class sound : MonoBehaviour
{
    private AudioSource m_AudioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    [ContextMenu("Play sound")]
    public void PlaySound()
    {
        m_AudioSource.Play();
    }
}
