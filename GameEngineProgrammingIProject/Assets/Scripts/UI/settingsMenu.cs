using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

public class settingsMenu : MonoBehaviour {

    private const string masterVolKey = "Master Volume";
    private const string musicVolKey = "Music Volume";
    private const string soundVolKey = "Sounds Volume";

    #pragma warning disable 649
    [Header("Audio Settings")]
    [SerializeField]
    private AudioMixer audioMixer;
    [SerializeField]
    private AnimationCurve volumeCurve;
    [SerializeField]
    private Slider masterVolSlider;
    [SerializeField]
    private Slider musicVolSlider;
    [SerializeField]
    private Slider soundVolSlider;

    [Header("Graphics Settings")]
    [SerializeField]
    private Dropdown resolutionDropdown;
    [SerializeField]
    private Toggle fullScreenToggle;
    [SerializeField]
    private Dropdown qualityDropdown;
    #pragma warning restore 649

    private void Awake()
    {
        resolutionDropdown.ClearOptions();
        var resolutions = new List<string>();
        foreach(var resolution in Screen.resolutions)
        {
            resolutions.Add(string.Format("{0}\tx\t{1}",resolution.width,resolution.height));
        }
        resolutionDropdown.AddOptions(resolutions);
        qualityDropdown.ClearOptions();
        qualityDropdown.AddOptions(QualitySettings.names.ToList());
  
    }
    private void OnEnable()
    {
        //set my sliders to match my saved values
        masterVolSlider.normalizedValue = PlayerPrefs.GetFloat(masterVolKey, 1f);
        musicVolSlider.normalizedValue = PlayerPrefs.GetFloat(musicVolKey, 1f);
        soundVolSlider.normalizedValue = PlayerPrefs.GetFloat(soundVolKey, 1f);
        qualityDropdown.value = QualitySettings.GetQualityLevel();
        resolutionDropdown.value = Screen.resolutions.ToList().FindIndex(obj => obj.width == Screen.width && obj.height == Screen.height);
        fullScreenToggle.isOn = Screen.fullScreen;
    }

    private void Update()
    {
        audioMixer.SetFloat(masterVolKey, volumeCurve.Evaluate(masterVolSlider.normalizedValue));
        audioMixer.SetFloat(musicVolKey, volumeCurve.Evaluate(musicVolSlider.normalizedValue));
        audioMixer.SetFloat(soundVolKey, volumeCurve.Evaluate(soundVolSlider.normalizedValue));
        QualitySettings.SetQualityLevel(qualityDropdown.value,true);
    }

    /// <summary>
    /// Applys setting changes
    /// </summary>
    public void apply()
    {
        PlayerPrefs.SetFloat(masterVolKey, masterVolSlider.normalizedValue);
        PlayerPrefs.SetFloat(musicVolKey, musicVolSlider.normalizedValue);
        PlayerPrefs.SetFloat(soundVolKey, soundVolSlider.normalizedValue);
        Screen.SetResolution(Screen.resolutions[resolutionDropdown.value].width, Screen.resolutions[resolutionDropdown.value].height,fullScreenToggle.isOn);


    }

    /// <summary>
    /// Reverts all settings changes
    /// </summary>
    public void revert()
    {
        //set settings to stored values
        audioMixer.SetFloat(masterVolKey, volumeCurve.Evaluate(PlayerPrefs.GetFloat(masterVolKey, 1f)));
        audioMixer.SetFloat(musicVolKey, volumeCurve.Evaluate(PlayerPrefs.GetFloat(musicVolKey ,1f)));
        audioMixer.SetFloat(soundVolKey, volumeCurve.Evaluate(PlayerPrefs.GetFloat(soundVolKey, 1f)));
        
    }
}
