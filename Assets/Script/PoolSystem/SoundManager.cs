using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Singleton;
    private void Awake()
    {
        Singleton = this;
    }
    [SerializeField] AudioSource audioSource;

    private void Start()
    {
        PoolSystem.Singleton.AddObjectToPooledObject(audioSource.gameObject, 20, transform);
    }

    public void PlayAudio(AudioClip audioClip)
    {
        AudioSource source = PoolSystem.Singleton.SpawnFromPool(audioSource.name, transform.position, Quaternion.identity).GetComponent<AudioSource>();
        source.PlayOneShot(audioClip);
    }

    // public class Sound
    // {
    //     public string audioName;
    //     public audi
    // }
}