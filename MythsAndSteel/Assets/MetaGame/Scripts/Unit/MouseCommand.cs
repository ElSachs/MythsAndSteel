using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using TMPro;

/// <summary>
/// Script contenant les controles clavier+souris pour l'UI ainsi que le MouseOver quand la souris survole un �l�ment d'UI.
/// </summary>
public class MouseCommand : MonoBehaviour
{
    #region Variable
    [SerializeField] private bool _checkIfPlayerAsClic;
    public bool CheckIfPlayerAsClic => _checkIfPlayerAsClic;

    [Header("UI STATIQUE UNITE")]
    //Le panneau � afficher lorsqu'on souhaite voir les statistiques de l'unit� en cliquant.
    [SerializeField] private GameObject _mouseOverUI;
    public GameObject MouseOverUI => _mouseOverUI;
    //Le panneau ou les panneaux � afficher lorsqu'on souhaite le shift click sur l'unit�
    [SerializeField] private List<GameObject> _shiftUI;
    public List<GameObject> ShiftUI => _shiftUI;
    [Header("DELAI ATTENTE MOUSE OVER")]
    //Param�tre de d�lai qui s'applique � la couritine.
    [SerializeField] private float _timeToWait = 2f;
    public float TimeToWait => _timeToWait;
    [Header("VALEUR POSITION UI")]
    //Permet de modifier la position de l'UI dans l'escpace
    [SerializeField] private int _offsetX;
    public int OffSetX => _offsetX;
    [SerializeField] private int _offsetY;
    public int OffSetY => _offsetY;
    [SerializeField] private int _maxoffsetX = 4;
    [SerializeField] private int _maxoffsetY = 4;
    #endregion Varaible

    #region UpdateStats
    void UpdateUIStats()
    {
        //Statistique pour le MouseOver.
        UIInstance.Instance.TitlePanelMouseOver.GetComponent<TextMeshProUGUI>().text = RaycastManager.Instance.UnitInTile.GetComponent<UnitScript>().UnitSO.UnitName;
        UIInstance.Instance.MouseOverStats[0].GetComponent<TextMeshProUGUI>().text = RaycastManager.Instance.UnitInTile.GetComponent<UnitScript>().Life.ToString();
        UIInstance.Instance.MouseOverStats[1].GetComponent<TextMeshProUGUI>().text = RaycastManager.Instance.UnitInTile.GetComponent<UnitScript>().AttackRange.ToString();
        UIInstance.Instance.MouseOverStats[2].GetComponent<TextMeshProUGUI>().text = RaycastManager.Instance.UnitInTile.GetComponent<UnitScript>().MoveSpeed.ToString();

        //Statistique de la Page 1 du Carnet.
        //Synchronise le texte du titre.
        UIInstance.Instance.TitlePanelShiftClicPage1.GetComponent<TextMeshProUGUI>().text = RaycastManager.Instance.UnitInTile.GetComponent<UnitScript>().UnitSO.UnitName;
        //Synchronise le texte de la vie avec l'emplacement d'UI.
        UIInstance.Instance.MiddleStatistique[0].GetComponent<TextMeshProUGUI>().text = RaycastManager.Instance.UnitInTile.GetComponent<UnitScript>().Life.ToString();
        //Synchronise le texte de la valeur de la distance d'attaque de l'unit� avec l'emplacement d'UI.
        UIInstance.Instance.MiddleStatistique[1].GetComponent<TextMeshProUGUI>().text = RaycastManager.Instance.UnitInTile.GetComponent<UnitScript>().AttackRange.ToString();
        //Synchronise le texte de la valeur de la vitesse de l'unit� avec l'emplacement d'UI.
        UIInstance.Instance.MiddleStatistique[2].GetComponent<TextMeshProUGUI>().text = RaycastManager.Instance.UnitInTile.GetComponent<UnitScript>().MoveSpeed.ToString();

        //Synchronise le texte de l'UI de la avec l'emplacement d'UI.
        UIInstance.Instance.BasseStatistique[0].GetComponent<TextMeshProUGUI>().text = RaycastManager.Instance.UnitInTile.GetComponent<UnitScript>().NumberRangeMin.x.ToString() + " - " + RaycastManager.Instance.UnitInTile.GetComponent<UnitScript>().NumberRangeMin.y.ToString();
        UIInstance.Instance.BasseStatistique[1].GetComponent<TextMeshProUGUI>().text = RaycastManager.Instance.UnitInTile.GetComponent<UnitScript>().NumberRangeMax.x.ToString() + " - " + RaycastManager.Instance.UnitInTile.GetComponent<UnitScript>().NumberRangeMax.y.ToString();
        UIInstance.Instance.BasseStatistique[2].GetComponent<TextMeshProUGUI>().text = RaycastManager.Instance.UnitInTile.GetComponent<UnitScript>().DamageMinimum.ToString();
        UIInstance.Instance.BasseStatistique[3].GetComponent<TextMeshProUGUI>().text = RaycastManager.Instance.UnitInTile.GetComponent<UnitScript>().DamageMaximum.ToString();

        //Statistique de la Page 2 du Carnet.  
        //Compl�ter avec les Images des Tiles.
        //UIInstance.Instance.MiddleImageTerrain[0].GetComponent<Image>().sprite = RaycastManager.Instance.Tile.GetComponent<TileScript>().TerrainEffectList[0].Image
        //UIInstance.Instance.MiddleImageTerrain[1].GetComponent<Image>().sprite = RaycastManager.Instance.Tile.GetComponent<TileScript>().TerrainEffectList[1].Image;

        //Si la tile ne contient pas d'effet de terrain, on n'affiche pas d'information. Si la tile contient 1 effet, on affiche et met � jour l'effet de la case. Si la tile contient 2 effets, on affiche les 2 Effets.
        if (RaycastManager.Instance.Tile.GetComponent<TileScript>().TerrainEffectList.Count == 0)
        {
            UIInstance.Instance.MiddleTextTerrain[4].SetActive(false);
            UIInstance.Instance.MiddleTextTerrain[5].SetActive(false);
        }
        //Si la tile contient 1 effet, on affiche et met � jour l'effet de la case.
        else if (RaycastManager.Instance.Tile.GetComponent<TileScript>().TerrainEffectList.Count == 1)
        {
            UIInstance.Instance.MiddleTextTerrain[4].SetActive(true);
            UIInstance.Instance.MiddleTextTerrain[0].GetComponent<TextMeshProUGUI>().text = RaycastManager.Instance.Tile.GetComponent<TileScript>().TerrainEffectList[0].ToString();
            UIInstance.Instance.MiddleTextTerrain[2].GetComponent<TextMeshProUGUI>().text = "A l'attention des m�tacogneurs, les effets sont en progs comme les images";
            //Si la tile contient 2 effets, on affiche les 2 Effets.
            if (RaycastManager.Instance.Tile.GetComponent<TileScript>().TerrainEffectList.Count > 1)
            {
                UIInstance.Instance.MiddleTextTerrain[5].SetActive(true);
                UIInstance.Instance.MiddleTextTerrain[1].GetComponent<TextMeshProUGUI>().text = RaycastManager.Instance.Tile.GetComponent<TileScript>().TerrainEffectList[1].ToString();
            }
        }
    }
    #endregion UpdateStats

    #region ActivateUI
    /// <summary>
    /// Permet d'activer un �l�ment de l'UI en utilisant un Raycast distint de la position et d'assigner une position custom par rapport au Canvas (Conflit avec le Canvas).
    /// </summary>
    /// <param name="uiElements"></param>
    /// <param name="offSetX"></param>
    /// <param name="offSetY"></param>
    public void ActivateUI(GameObject uiElements, float offSetX, float offSetY, bool switchPage = false)
    {
        //Reprendre la position du raycast qui a s�lectionn� la tile
        RaycastHit2D hit = RaycastManager.Instance.GetRaycastHit();
        //Je stop l'ensemble des coroutines en cour.
        Vector3 pos = new Vector3(0, 0, 0);
        StopAllCoroutines();

        //Si le joueur change de Page.
        if (!switchPage)
        {
            //Permet de repositionner la position de l'UI en fonction de 
            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + offSetX, hit.transform.position.y + offSetY, hit.transform.position.z));
            if (hit.transform.position.x >= _maxoffsetX)
            {
                pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - offSetX, hit.transform.position.y + offSetY, hit.transform.position.z));
                if (hit.transform.position.y >= _maxoffsetY)
                {
                    pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - offSetX, hit.transform.position.y - offSetY, hit.transform.position.z));
                }
            }
        }

        //Je d�termine une position de r�f�rence dans l'espace (pour que la position de l'UI soit par rapport au Canvas et non � l'objet).
        else
        {
            pos = new Vector3(offSetX, offSetY, ShiftUI[0].transform.position.z);
        }

        //Rendre l'�l�ment visible.
        uiElements.SetActive(true);
        //Si la position de l'UI est diff�rente de celle de la position de r�f�rence alors tu prends cette position comme r�f�rence.
        if (uiElements.transform.position != pos)
        {
            uiElements.transform.position = pos;
        }
        //Met le focus de la souris sur l'UI mouse over
        EventSystem.current.SetSelectedGameObject(uiElements);

    }
    #endregion ActivateUI

    #region ControleDesClicks
    /// <summary>
    /// Permet de d�terminer quand le joueur appuie sur le Shift puis le clic Gauche de la souris.
    /// </summary>
    public void ShiftClick()
    {
        //Si le joueur appuie sur la touche Shift Gauche (Attention, il y a 2 shifts sur un clavier !)
        if (Input.GetKey("left shift"))
        {
            //Si le joueur appuie sur le click gauche de sa souris.
            if (Input.GetMouseButtonDown(0))
            {
                //Si le joueur a �x�cut� les actions pr�c�dentes, il est consid�r� comme quoi le joueur a cliqu� donc on active le premier panneau.
                _checkIfPlayerAsClic = true;
                ActivateUI(ShiftUI[0], _offsetX, _offsetY);
                UpdateUIStats();
            }
        }
    }



    /// <summary>
    /// Permet de d�terminer et d'afficher un �l�ment quand la souris passe au dessus d'une tuile poss�dant une unit�.
    /// </summary>
    public void MouseOverWithoutClick()
    {
        //Si le joueur n'a pas cliqu�, alors tu lances la coroutine.
        if (_checkIfPlayerAsClic == false)
        {
            //Coroutine : Une coroutine est une fonction qui peut suspendre son ex�cution (yield) jusqu'� la fin de la YieldInstruction donn�e.
            StartCoroutine(ShowObject(TimeToWait));
            UpdateUIStats();
        }
        else
        {
            //Si le joueur click, alors je cache le MouseOver.
            MouseExitWithoutClick();
        }
    }

    /// <summary>
    /// Fonction pour d�sactiver en MouseOver.
    /// </summary>
    public void MouseExitWithoutClick()
    {
        //Arrete l'ensemble des coroutines dans la sc�ne.
        StopAllCoroutines();
        _mouseOverUI.SetActive(false);
    }

    /// <summary>
    /// Correspond au param�tre qu'on rentre dans la coroutine.
    /// </summary>
    /// <param name="Timer"></param>
    /// <returns></returns>
    IEnumerator ShowObject(float TimeToWait)
    {
        //J'utilise un d�lai pour que le boutton apparaisse apr�s un d�lai.
        yield return new WaitForSeconds(TimeToWait);
        //J'active l'�l�ment et je lui assigne des param�tres.
        ActivateUI(MouseOverUI, _offsetX, _offsetY - 2);
    }
    #endregion ControleDesClicks

    #region SwitchPages
    /// <summary>
    /// JE prends une liste de boutton, a chaque bouton j'assigne un index. 
    /// </summary>
    /// <param name="button"></param>
    public void buttonAction(List<Button> button)
    {
        //0 et  1 sont pour les boutons quitter, 2 et 3 sont pour switch entre la Page 1 et la Page 2
        button[0].onClick.AddListener(clickQuit);
        button[1].onClick.AddListener(clickQuit);
        button[2].onClick.AddListener(switchWindows1);
        button[3].onClick.AddListener(switchWindows2);

        //Fonction qui permet de cacher les Pages 1 et 2 du carnet.
        void clickQuit()
        {
            //Je retourne la valeur comme quoi il a click� � false car il a fini son action de Shift+Clic et d�sactive les 2 pages.
            _checkIfPlayerAsClic = false;
            ShiftUI[0].SetActive(false);
            ShiftUI[1].SetActive(false);
        }

        //Change de page lorsque le joueur regarde les statistiques avanc�es
        //Switch entre la page 1 et la page 2.
        void switchWindows1()
        {
            //J'active le Panneau 2 car le joueur a cliqu� sur le bouton permettant de transitionner de la page 1 � la page 2. De plus, je masque la page 1.
            ActivateUI(ShiftUI[1], ShiftUI[0].transform.position.x, ShiftUI[0].transform.position.y, true);
            ShiftUI[0].SetActive(false);
        }

        //Switch entre la page 2 et la page 1.
        void switchWindows2()
        {
            //J'active le Panneau 1 car le joueur a cliqu� sur le bouton permettant de transitionner de la page 2 � la page 1. De plus, je masque la page 2.
            ActivateUI(ShiftUI[0], ShiftUI[1].transform.position.x, ShiftUI[1].transform.position.y, true);
            ShiftUI[1].SetActive(false);
        }
    }
    #endregion SwitchPages
}
