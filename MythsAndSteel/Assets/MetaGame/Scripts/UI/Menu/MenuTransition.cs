using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/*
Ce Script permet d'afficher ou d'enlever les Options en appuyant sur Echap. 
Il fait apparaitre une autre sc�ne qui va se superposer � la sc�ne principal.
 */

[CreateAssetMenu(menuName = "META/Transition Menu")]
public class MenuTransition : ScriptableObject
{
    /// <summary>
    /// Active une sc�ne
    /// </summary>
    /// <param name="sceneId"></param>
    public void LoadScene(int sceneId)
    {
       SceneManager.LoadSceneAsync(sceneId, LoadSceneMode.Additive);
    }

    /// <summary>
    /// D�sactive une sc�ne
    /// </summary>
    /// <param name="sceneId"></param>
    public void DesactivMenu(int sceneId)
    { 
        SceneManager.UnloadSceneAsync(sceneId);
    }
}
