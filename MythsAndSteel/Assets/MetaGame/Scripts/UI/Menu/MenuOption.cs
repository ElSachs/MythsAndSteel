using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/*
Ce Script permet d'afficher ou d'enlever les Options en appuyant sur Echap. 
Il fait apparaitre une autre sc�ne qui va se superposer � la sc�ne principal.
 */

[CreateAssetMenu(menuName = "META/Option menu")]
public class MenuOption : ScriptableObject
{
   bool menuOptionActiv� = false;
   
    public void ActiveMenu()
    {
        menuOptionActiv� = true;
       SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
    }

    public void DesactivMenu()
    { 
        menuOptionActiv� = false;
        SceneManager.UnloadSceneAsync(1);
    }
}
