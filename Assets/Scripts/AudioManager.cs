using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    [SerializeField] private AudioSource audioSourcePrototype;



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    

    public void PlaySoundFX(AudioClip clip, Transform playPosition, float volume) 
    {
        AudioSource audioSource = Instantiate(audioSourcePrototype, playPosition.position, Quaternion.identity);
        float randomVolumeOffset = Random.Range(-0.05f, 0.05f);
        float randomPitchOffet = Random.Range(-0.2f, 0.2f);
        audioSource.clip = clip;
        audioSource.pitch += randomPitchOffet;
        audioSource.volume = volume + randomVolumeOffset;

        audioSource.Play();

        Destroy(audioSource.gameObject, audioSource.clip.length);
    }
}
