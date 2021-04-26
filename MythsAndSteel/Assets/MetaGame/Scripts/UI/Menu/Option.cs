using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    #region Variables
    //---------Audio--------------
    public AudioMixer audioMixer; //Variable qui d�fini quel AudioMixer on modifie

    //--------R�solution----------
    public Dropdown ResolutionDropdown;//Variable qui d�fini quel dropdown on modifie
    Resolution[] resolutions;//Array de toute 
    int currentResolutionIndex = 0;
    Resolution OldResolution;
    int Oldindex;

    bool FirstTime = true;

    [SerializeField] GameObject ValidationPanel;

    //-------Avertissement--------
    bool isAvertissement;
    #endregion 

    private void Start()
    {
        SetEffectvolume(PlayerPrefs.GetFloat("Effectvolume"));
        SetMusicVolume(PlayerPrefs.GetFloat("MusicVolum"));


        resolutions = Screen.resolutions; //R�cup�re toute les r�soluttion possible
        ResolutionDropdown.ClearOptions();//Enl�ve les options de bases du Dropdown


        //--------Convertie les r�solution en String pour les afficher dans le Dropdown---------
        List<string> options = new List<string>();


        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && //Cherche la r�solution actuelle de l'�cran
               resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        //----------------------------------------------------------------------------------------

        ResolutionDropdown.AddOptions(options); //Ajoute toutes les r�solution possible au Dropdown (en string)
        ResolutionDropdown.value = currentResolutionIndex; //Assigne la r�solution actuelle au Dropdown
        ResolutionDropdown.RefreshShownValue();//Actualise le Dropdown pour afficher la r�solution actuelle
    }

    #region R�solution

    public void SetResolution(int resolutionIndex) //
    {

        OldResolution = resolutions[currentResolutionIndex];
        Oldindex = currentResolutionIndex;

        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        currentResolutionIndex = resolutionIndex;
        Debug.Log("rr");
        if (!FirstTime)
        {
            StartCoroutine(Validation());
        }
        else
        {
            FirstTime = false;
        }
    }

    public void ResetResolution()
    {
        Debug.Log(OldResolution);
        Screen.SetResolution(OldResolution.width, OldResolution.height, Screen.fullScreen);

        ResolutionDropdown.value = Oldindex;
        currentResolutionIndex = Oldindex;
        ResolutionDropdown.RefreshShownValue();

        ValidationPanel.SetActive(false);

        
    }

    public IEnumerator Validation()
    {
        ValidationPanel.SetActive(true);
        yield return new WaitForSeconds(5);

        ResetResolution();
    }
    #endregion

    #region Audio
    public void ActiveVolume(bool isVolumeOn)
    {
        if (isVolumeOn)
        {
            audioMixer.SetFloat("Master", 0);
        }
        else
        {
            audioMixer.SetFloat("Master", -80);
        }
    }
    public void SetEffectvolume (float volume)
    {
        audioMixer.SetFloat("Effect", volume);
        PlayerPrefs.SetFloat("EffectVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("Music", volume);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }
    #endregion

    #region Avertissement
    public void SetAvertissement(bool IsAverti)
    {
        isAvertissement = IsAverti;
    }
    #endregion

    

}
