using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    //---------Audio--------------
    public AudioMixer audioMixer; //Variable qui d�fini quel AudioMixer on modifie

    //--------R�solution----------
    public Dropdown ResolutionDropdown;//Variable qui d�fini quel dropdown on modifie
    Resolution[] resolutions;//Array de toute 
    int currentResolutionIndex = 0;
    Resolution OldResolution;

    bool FirstTime;

    [SerializeField] GameObject ValidationPanel;

    //-------Avertissement--------
    bool isAvertissement;

    #region R�solution
    private void Start()
    {
        resolutions = Screen.resolutions; //R�cup�re toute les r�soluttion possible
        ResolutionDropdown.ClearOptions();//Enl�ve les options de bases du Dropdown


        //--------Convertie les r�solution en String pour les afficher dans le Dropdown---------
        List<string> options = new List<string>();

        
        for (int i=0; i<resolutions.Length; i++)
        {
           string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width && //Cherche la r�solution actuelle de l'�cran
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

    public void SetResolution(int resolutionIndex) //
    {
        
        OldResolution = resolutions[currentResolutionIndex];
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        Debug.Log("rr");
        if (!FirstTime)
        {
            StartCoroutine("Validation");
        }
        else
        {
            FirstTime = false;
        }
    }

    public void ResetResolution()
    {
        Screen.SetResolution(OldResolution.width, OldResolution.height, Screen.fullScreen);
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
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("Music", volume);
    }
    #endregion

    #region Avertissement
    public void SetAvertissement(bool IsAverti)
    {
        isAvertissement = IsAverti;
    }
    #endregion


}
