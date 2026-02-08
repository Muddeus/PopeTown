using UnityEngine;
using UnityEngine.UI;

public class DefaultMusicVolume : MonoBehaviour
{
    public SoundMixerManager mixer;
    [Header("Set default slider value at launch.")]
    [Range(0f,1f)]public float value;
    void Awake()
    {
        Slider slider = GetComponent<Slider>();
        slider.value = value;
        mixer.SetMusicVolume(value);
    }
}
