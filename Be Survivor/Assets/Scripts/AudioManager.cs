using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> audioClipList;
    private AudioSource audioSource;
    public static AudioManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public enum AudioCallers
    {
        Shot =0 ,
        Projectile=1,
        End=2,
    }

    public void PlayAudio(AudioCallers audio)
    {
        audioSource.PlayOneShot(audioClipList[(int)audio]);
    }
}
