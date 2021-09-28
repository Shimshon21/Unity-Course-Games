using UnityEngine.UI;
using UnityEngine;

public class OptionsController : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;
    [SerializeField] float defaultVolume = 0.8f;

    [SerializeField] Slider difficultySlider;
    [SerializeField] int defaultdifficulty = 1;

    // Start is called before the first frame update
    void Start()
    {
        volumeSlider.value = PlayerPrefController.GetMasterVolume();

        difficultySlider.value = PlayerPrefController.GetDifficulty();
    }

    // Update is called once per frame
    void Update()
    {
        var musicPlayer = FindObjectOfType<MusicPlayer>();

        if(musicPlayer)
        {
            musicPlayer.SetVolume(volumeSlider.value);
        }
        else
        {
            Debug.LogError("No music player found...start from splash screen.");
        }
    }


    public void SaveAndExit()
    {
        PlayerPrefController.SetMasterVolume(volumeSlider.value);

        PlayerPrefController.SetDifficulty(difficultySlider.value);

        FindObjectOfType<SceneLoader>().LoadMenuScene();
    }


    public void SetDefaultsValues()
    {
        volumeSlider.value = defaultVolume;

        difficultySlider.value = defaultdifficulty;
    }



}
