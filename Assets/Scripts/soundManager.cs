using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SoundType
{
    FOOTSTEPS,
    PUNCH,
    GRUNT,
    IMPACT,
    HURT,
    GRUNT2,
}
[RequireComponent(typeof(AudioSource))]
public class soundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] soundlist;
    private static soundManager instance;
    private AudioSource audioSource;

    private void Awake()
    {
        // instance = this;
    }

    private void Start()
    {
       // audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(SoundType sound, float volume = 1)
    {
       // instance.audioSource.PlayOneShot(instance.soundlist[(int)sound],volume);
    }
}
