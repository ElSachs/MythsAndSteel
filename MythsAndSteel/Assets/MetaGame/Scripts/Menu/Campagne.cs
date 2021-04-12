using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Campagne : MonoBehaviour
{
    int SceneToLoad; //La sc�ne que l'on veut charger (se r�f�rer au build settings dans l'onglet file)
    public Scenario _Scenario; //Sc�nario S�l�ctionn� et affich�
    
    public Sprite[] SpriteMap; //Sprite des map de chaques plateau
        
    [SerializeField] int Unlocked;//Nombre actuelle de niveau d�bloqu�

    [Header("Assignations")]
    public Image ImageScenario; //Affiche La map du sc�nario actuelle
    public Text nomDeLaCampagne; //Affiche le nom de la campagne sur le bouton
    public Slider Unlockslider; // Slider qui lontre la progression de d�v�rouillage
    public GameObject LockCanvas; //Canvas affich� lorsqu'un niveau n'est pas d�bloqu�

    private void Start()
    {
        //Assigne les valeur de d�v�rouillage des sc�nario
        Unlockslider.value = Unlocked;
    }



    public enum Scenario
    {
        Rethel, Shanghai, Stalingrad, Husky, Guadalcanal, ElAlamein, Elsenborn
    }

    public void ChangeScene()
    {
        if ((int)_Scenario <= Unlocked) //Check si le sc�nario est d�bloqu�
        {
            SceneManager.LoadScene((int)_Scenario + 1);
        }
        
    }

    public void Decrease() //Fonction boutton pour montrer le sc�nario pr�c�dent
    {
        if((int)_Scenario == 0)
        {
            _Scenario = Scenario.Elsenborn;
        }
        else
        {
            _Scenario--;
        }

        Actualise();
        
    }

    public void Increase() //Fonction boutton pour montrer le sc�nario suivant
    {
        if ((int)_Scenario == 6)
        {
            _Scenario = Scenario.Rethel;
        }
        else
        {
            _Scenario++;
        }

        Actualise();
    }

    void Actualise() //Anti-voidUpdate
    {
        if ((int)_Scenario <= Unlocked) //Active et d�sactive le Canvas pour les sc�nario bloqu�
        {
            LockCanvas.SetActive(false);
        }
        else
        {
            LockCanvas.SetActive(true);
        }
        nomDeLaCampagne.text = _Scenario.ToString(); //Actualise le nom du sc�nario
        ImageScenario.sprite = SpriteMap[(int)_Scenario];//Actualsie l'image de sc�nario
    }


}
