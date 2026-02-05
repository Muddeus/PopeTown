using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;

    public AudioClip notes1; // Sound 1


    public AudioClip[] periwinkleSpeaks;
    

    [SerializeField] private AudioSource sFXObject;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlaySound(AudioClip audioClip, Transform spawntransform, float volume)
    {
        //spawn in gameobject
        AudioSource audioSource = Instantiate(sFXObject, spawntransform.position, Quaternion.identity);

        //assign the audioclip
        audioSource.clip = audioClip;

        //assign volume
        audioSource.volume = volume;

        //play sound
        audioSource.Play();

        //get length of sound clip
        float clipLength = audioSource.clip.length;

        //destroy clip
        Destroy(audioSource.gameObject, clipLength);
    }

    public void PlayRandomSound(AudioClip[] audioClip, Transform spawntransform, float volume)
    {
        //assign a random index
        int random = Random.Range(0, audioClip.Length);

        //spawn in gameobject
        AudioSource audioSource = Instantiate(sFXObject, spawntransform.position, Quaternion.identity);

        //assign the audioclip
        audioSource.clip = audioClip[random];

        //assign volume
        audioSource.volume = volume;

        //play sound
        audioSource.Play();

        //get length of sound clip
        float clipLength = audioSource.clip.length;

        //destroy clip
        Destroy(audioSource.gameObject, clipLength);
    }
}
[System.Serializable]
public enum Sound
{
    None,
    Notes,
    
}