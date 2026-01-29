using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;

    [SerializeField] private AudioSource SFXObject;

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
        AudioSource audioSource = Instantiate(SFXObject, spawntransform.position, Quaternion.identity);

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
}
