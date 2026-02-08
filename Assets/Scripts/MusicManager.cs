using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager ins;
    
    
    private AudioSource audioSource;
    private AudioClip nextSong;
    private float timePlaying;
    private float songLength;
    public float timeToFadeOut;
    private float countdown;
    
    public AudioClip townSquare1;
    public AudioClip mayor;
    public AudioClip inspectBody;
    
    private void Awake()
    {
        if (ins == null)
        {
            ins = this;
        }
    }
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        countdown = 0;
        timePlaying = 0;
    }
    public void PlayMusic(AudioClip music)
    {
        nextSong = music;
        countdown = timeToFadeOut;
        timePlaying = 0;
        songLength = music.length;
    }
    void Update()
    {
        if (nextSong != null) // if theres a song in queue
        {
            if (audioSource.clip != null) // if there's something playing
            {
                audioSource.volume = countdown / timeToFadeOut;
                if (countdown <= 0)
                {
                    audioSource.Stop();
                    audioSource.clip = nextSong;
                    audioSource.volume = 1;
                    audioSource.Play();
                    nextSong = null;
                }
            }
            else // if nothing playing
            {
                audioSource.clip = nextSong;
                audioSource.volume = 1;
                audioSource.Play();
                nextSong = null;
            }

            if(timePlaying > songLength + timeToFadeOut) audioSource.Stop();
            if (audioSource.isPlaying == false) audioSource.clip = null;

        }
        countdown -= Time.deltaTime;
        timePlaying += Time.deltaTime;
    }

    public void PlayTownSquare()
    {
        if(audioSource.clip != townSquare1) PlayMusic(townSquare1);
    }
    public void PlayMayor()
    {
        if(audioSource.clip != mayor)PlayMusic(mayor);
    }

    public void PlayInspectBody()
    {
        if(audioSource.clip != inspectBody)PlayMusic(inspectBody);
    }
}
