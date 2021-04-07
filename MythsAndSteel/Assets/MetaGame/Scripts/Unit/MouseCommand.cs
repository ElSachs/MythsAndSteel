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
    #region Variables
    public bool _checkIfPlayerAsClic;

    public bool _hasCheckUnit = false;

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
    [SerializeField] private float _offsetXActivationMenu;
    [SerializeField] private float _offsetYActivationMenu;
    [Space]
    [SerializeField] private float _offsetXMouseOver;
    [SerializeField] private float _offsetYMouseOver;
    [Space]
    [SerializeField] private float _offsetXStatPlus;
    [SerializeField] private float _offsetYStatPlus;
    [Space]
    [SerializeField] private Vector2 _xOffsetMin;
    [SerializeField] private Vector2 _yOffsetMin;
    [SerializeField] private Vector2 _xOffset;
    [SerializeField] private Vector2 _yOffset;
    [SerializeField] private Vector2 _xOffsetMax;
    [SerializeField] private Vector2 _yOffsetMax;
    #endregion Variables

    #region UpdateStats
    void UpdateUIStats()
    {
        //Statistique pour le MouseOver.
        UIInstance.Instance.TitlePanelMouseOver.GetComponent<TextMeshProUGUI>().text = RaycastManager.Instance.UnitInTile.GetComponent<UnitScript>().UnitSO.UnitName;
        UIInstance.Instance.MouseOverStats._lifeGam.GetComponent<TextMeshProUGUI>().text = RaycastManager.Instance.UnitInTile.GetComponent<UnitScript>().Life.ToString();
        UIInstance.Instance.MouseOverStats._rangeGam.GetComponent<TextMeshProUGUI>().text = RaycastManager.Instance.UnitInTile.GetComponent<UnitScript>().AttackRange.ToString();
        UIInstance.Instance.MouseOverStats._moveGam.GetComponent<TextMeshProUGUI>().text = RaycastManager.Instance.UnitInTile.GetComponent<UnitScript>().MoveSpeed.ToString();

        //Statistique de la Page 1 du Carnet.
        //Synchronise le texte du titre.
        UIInstance.Instance.TitlePanelShiftClicPage1.GetComponent<TextMeshProUGUI>().text = RaycastManager.Instance.UnitInTile.GetComponent<UnitScript>().UnitSO.UnitName;
        //Synchronise le texte de la vie avec l'emplacement d'UI.
        UIInstance.Instance.PageUnitStat._lifeGam.GetComponent<TextMeshProUGUI>().text = RaycastManager.Instance.UnitInTile.GetComponent<UnitScript>().Life.ToString();
        //Synchronise le texte de la valeur de la distance d'attaque de l'unit� avec l'emplacement d'UI.
        UIInstance.Instance.PageUnitStat._rangeGam.GetComponent<TextMeshProUGUI>().text = RaycastManager.Instance.UnitInTile.GetComponent<UnitScript>().AttackRange.ToString();
        //Synchronise le texte de la valeur de la vitesse de l'unit� avec l'emplacement d'UI.
        UIInstance.Instance.PageUnitStat._moveGam.GetComponent<TextMeshProUGUI>().text = RaycastManager.Instance.UnitInTile.GetComponent<UnitScript>().MoveSpeed.ToString();

        //Synchronise le texte de l'UI de la avec l'emplacement d'UI.
        UIInstance.Instance.AttackStat._rangeMinDamageGam.GetComponent<TextMeshProUGUI>().text = RaycastManager.Instance.UnitInTile.GetComponent<UnitScript>().NumberRangeMin.x.ToString() + " - " + RaycastManager.Instance.UnitInTile.GetComponent<UnitScript>().NumberRangeMin.y.ToString();
        UIInstance.Instance.AttackStat._rangeMaxDamageGam.GetComponent<TextMeshProUGUI>().text = RaycastManager.Instance.UnitInTile.GetComponent<UnitScript>().NumberRangeMax.x.ToString() + " - " + RaycastManager.Instance.UnitInTile.GetComponent<UnitScript>().NumberRangeMax.y.ToString();
        UIInstance.Instance.AttackStat._minDamageValueGam.GetComponent<TextMeshProUGUI>().text = RaycastManager.Instance.UnitInTile.GetComponent<UnitScript>().DamageMinimum.ToString();
        UIInstance.Instance.AttackStat._maxDamageValueGam.GetComponent<TextMeshProUGUI>().text = RaycastManager.Instance.UnitInTile.GetComponent<UnitScript>().DamageMaximum.ToString();

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
    public void ActivateUI(GameObject uiElements, float lastPosX = 0, float lastPosY = 0, bool switchPage = false, bool activationMenu = false, bool mouseOver = false, bool bigStat = false)
    {
        //Reprendre la position du raycast qui a s�lectionn� la tile
        RaycastHit2D hit = RaycastManager.Instance.GetRaycastHit();

        //Je stop l'ensemble des coroutines en cour.
        Vector3 pos = Vector3.zero;
        StopAllCoroutines();

        //Menu d'activation d'une unit�
        if(activationMenu)
        {
            if(hit.transform.position.x >= _xOffset.y)
            {
                if(hit.transform.position.x >= _xOffsetMax.y)
                {
                    if(hit.transform.position.y >= _yOffset.y)
                    {
                        if(hit.transform.position.y >= _yOffsetMax.y)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - _offsetXActivationMenu, hit.transform.position.y - _offsetYActivationMenu + .5f, hit.transform.position.z));
                        }
                        else
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - _offsetXActivationMenu, hit.transform.position.y, hit.transform.position.z));
                        }
                    }
                    else if(hit.transform.position.y <= _yOffset.x)
                    {
                        if(hit.transform.position.y <= _yOffsetMax.x)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - _offsetXActivationMenu, hit.transform.position.y + _offsetYActivationMenu - .5f, hit.transform.position.z));
                        }
                        else
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - _offsetXActivationMenu, hit.transform.position.y, hit.transform.position.z));
                        }
                    }
                    else
                    {
                        pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - _offsetXActivationMenu, hit.transform.position.y, hit.transform.position.z));
                    }
                }
                else
                {
                    if(hit.transform.position.y >= _yOffset.y)
                    {
                        if(hit.transform.position.y >= _yOffsetMax.y)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - _offsetXActivationMenu, hit.transform.position.y - _offsetYActivationMenu + .5f, hit.transform.position.z));
                        }
                        else
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - _offsetXActivationMenu, hit.transform.position.y, hit.transform.position.z));
                        }
                    }
                    else if(hit.transform.position.y <= _yOffset.x)
                    {
                        if(hit.transform.position.y <= _yOffsetMax.x)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - _offsetXActivationMenu, hit.transform.position.y + _offsetYActivationMenu - .5f, hit.transform.position.z));
                        }
                        else
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - _offsetXActivationMenu, hit.transform.position.y, hit.transform.position.z));
                        }
                    }
                    else
                    {
                        pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - _offsetXActivationMenu, hit.transform.position.y, hit.transform.position.z));
                    }
                }
            }

            else if(hit.transform.position.x <= _xOffset.x)
            {
                if(hit.transform.position.x <= _xOffsetMax.x)
                {
                    if(hit.transform.position.y >= _yOffset.y)
                    {
                        if(hit.transform.position.y >= _yOffsetMax.y)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXActivationMenu, hit.transform.position.y - _offsetYActivationMenu + .5f, hit.transform.position.z));
                        }
                        else
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXActivationMenu, hit.transform.position.y, hit.transform.position.z));
                        }
                    }
                    else if(hit.transform.position.y <= _yOffset.x)
                    {
                        if(hit.transform.position.y <= _yOffsetMax.x)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXActivationMenu, hit.transform.position.y + _offsetYActivationMenu - .5f, hit.transform.position.z));
                        }
                        else
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXActivationMenu, hit.transform.position.y, hit.transform.position.z));
                        }
                    }
                    else
                    {
                        pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXActivationMenu, hit.transform.position.y, hit.transform.position.z));
                    }
                }
                else
                {
                    if(hit.transform.position.y >= _yOffset.y)
                    {
                        if(hit.transform.position.y >= _yOffsetMax.y)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXActivationMenu, hit.transform.position.y - _offsetYActivationMenu + .5f, hit.transform.position.z));
                        }
                        else
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXActivationMenu, hit.transform.position.y, hit.transform.position.z));
                        }
                    }
                    else if(hit.transform.position.y <= _yOffset.x)
                    {
                        if(hit.transform.position.y <= _yOffsetMax.x)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXActivationMenu, hit.transform.position.y + _offsetYActivationMenu - .5f, hit.transform.position.z));
                        }
                        else
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXActivationMenu, hit.transform.position.y, hit.transform.position.z));
                        }
                    }
                    else
                    {
                        pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXActivationMenu, hit.transform.position.y, hit.transform.position.z));
                    }
                }
            }
            else
            {
                if(hit.transform.position.y >= _yOffsetMax.y)
                {
                    pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXActivationMenu, hit.transform.position.y - _offsetYActivationMenu, hit.transform.position.z));
                }
                else if(hit.transform.position.y <= _yOffsetMax.x)
                {
                    pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXActivationMenu, hit.transform.position.y + _offsetYActivationMenu, hit.transform.position.z));
                }
                else
                {
                    pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXActivationMenu, hit.transform.position.y, hit.transform.position.z));
                }
            }
        }

        //Menu mouseOver
        else if(mouseOver)
        {
            if(hit.transform.position.x >= _xOffset.y)
            {
                if(hit.transform.position.x >= _xOffsetMax.y)
                {
                    if(hit.transform.position.y >= _yOffsetMin.y)
                    {
                        if(hit.transform.position.y >= _yOffsetMax.y)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - _offsetXMouseOver, hit.transform.position.y - _offsetYMouseOver - 1, hit.transform.position.z));
                        }
                        else if(hit.transform.position.y <= _yOffsetMax.y && hit.transform.position.y >= _yOffset.y)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - _offsetXMouseOver, hit.transform.position.y - _offsetYMouseOver - .5f, hit.transform.position.z));
                        }
                        else
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - _offsetXMouseOver, hit.transform.position.y - _offsetYMouseOver - .5f, hit.transform.position.z));
                        }
                    }
                    else if(hit.transform.position.y <= _yOffsetMin.x)
                    {
                        if(hit.transform.position.y <= _yOffsetMax.x)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - _offsetXMouseOver, hit.transform.position.y + _offsetYMouseOver + 1, hit.transform.position.z));
                        }
                        else if(hit.transform.position.y >= _yOffsetMax.x && hit.transform.position.y <= _yOffset.x)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - _offsetXMouseOver, hit.transform.position.y + _offsetYMouseOver + .5f, hit.transform.position.z));
                        }
                        else
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - _offsetXMouseOver, hit.transform.position.y + _offsetYMouseOver + .5f, hit.transform.position.z));
                        }
                    }
                    else
                    {
                        pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - _offsetXMouseOver, hit.transform.position.y - _offsetYMouseOver - .5f, hit.transform.position.z));
                    }
                }
                else
                {
                    if(hit.transform.position.y >= _yOffsetMin.y)
                    {
                        if(hit.transform.position.y >= _yOffsetMax.y)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x, hit.transform.position.y - _offsetYMouseOver - 1, hit.transform.position.z));
                        }
                        else if(hit.transform.position.y >= _yOffset.y && hit.transform.position.y <= _yOffsetMax.y)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x, hit.transform.position.y - _offsetYMouseOver - .5f, hit.transform.position.z));
                        }
                        else
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x, hit.transform.position.y - _offsetYMouseOver - .5f, hit.transform.position.z));
                        }
                    }
                    else if(hit.transform.position.y <= _yOffsetMin.x)
                    {
                        if(hit.transform.position.y <= _yOffsetMax.x)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x, hit.transform.position.y + _offsetYMouseOver + 1, hit.transform.position.z));
                        }
                        else if(hit.transform.position.y >= _yOffsetMax.x && hit.transform.position.y <= _yOffset.x)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x, hit.transform.position.y + _offsetYMouseOver + .5f, hit.transform.position.z));
                        }
                        else
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x, hit.transform.position.y + _offsetYMouseOver + .5f, hit.transform.position.z));
                        }
                    }
                    else
                    {
                        pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x, hit.transform.position.y - _offsetYMouseOver - .5f, hit.transform.position.z));
                    }
                }
            }

            else if(hit.transform.position.x <= _xOffset.x)
            {
                if(hit.transform.position.x <= _xOffsetMax.x)
                {
                    if(hit.transform.position.y >= _yOffsetMin.y)
                    {
                        if(hit.transform.position.y >= _yOffsetMax.y)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXMouseOver, hit.transform.position.y - _offsetYMouseOver - 1, hit.transform.position.z));
                        }
                        else if(hit.transform.position.y >= _yOffset.y && hit.transform.position.y <= _yOffsetMax.y)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXMouseOver, hit.transform.position.y - _offsetYMouseOver - .5f, hit.transform.position.z));
                        }
                        else
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXMouseOver, hit.transform.position.y - _offsetYMouseOver - .5f, hit.transform.position.z));
                        }
                    }
                    else if(hit.transform.position.y <= _yOffsetMin.x)
                    {
                        if(hit.transform.position.y <= _yOffsetMax.x)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXMouseOver, hit.transform.position.y + _offsetYMouseOver + 1, hit.transform.position.z));
                        }
                        else if(hit.transform.position.y >= _yOffsetMax.x && hit.transform.position.y <= _yOffset.x)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXMouseOver, hit.transform.position.y + _offsetYMouseOver + .5f, hit.transform.position.z));
                        }
                        else
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXMouseOver, hit.transform.position.y + _offsetYMouseOver + .5f, hit.transform.position.z));
                        }
                    }
                    else
                    {
                        pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x, hit.transform.position.y - _offsetYMouseOver - .5f, hit.transform.position.z));
                    }
                }
                else
                {
                    if(hit.transform.position.y >= _yOffsetMin.y)
                    {
                        if(hit.transform.position.y >= _yOffsetMax.y)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXMouseOver, hit.transform.position.y - _offsetYMouseOver - 1, hit.transform.position.z));
                        }
                        else if(hit.transform.position.y >= _yOffset.y && hit.transform.position.y <= _yOffsetMax.y)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXMouseOver, hit.transform.position.y - _offsetYMouseOver - .5f, hit.transform.position.z));
                        }
                        else
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXMouseOver, hit.transform.position.y - _offsetYMouseOver - .5f, hit.transform.position.z));
                        }
                    }
                    else if(hit.transform.position.y <= _yOffsetMin.x)
                    {
                        if(hit.transform.position.y <= _yOffsetMax.x)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXMouseOver, hit.transform.position.y + _offsetYMouseOver + 1, hit.transform.position.z));
                        }
                        else if(hit.transform.position.y >= _yOffsetMax.x && hit.transform.position.y <= _yOffset.x)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXMouseOver, hit.transform.position.y + _offsetYMouseOver + .5f, hit.transform.position.z));
                        }
                        else
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXMouseOver, hit.transform.position.y + _offsetYMouseOver + .5f, hit.transform.position.z));
                        }
                    }
                    else
                    {
                        pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x, hit.transform.position.y - _offsetYMouseOver - .5f, hit.transform.position.z));
                    }
                }
            }
            else
            {
                if(hit.transform.position.y >= _yOffsetMax.y)
                {
                    pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - _offsetXMouseOver / 2, hit.transform.position.y - _offsetYMouseOver - .5f, hit.transform.position.z));
                }
                else if(hit.transform.position.y <= _yOffsetMax.x)
                {
                    pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - _offsetXMouseOver / 2, hit.transform.position.y + _offsetYMouseOver + .5f, hit.transform.position.z));
                }
                else
                {
                    pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - _offsetXMouseOver * 2.5f, hit.transform.position.y, hit.transform.position.z));
                }
            }
        }

        //Menu avec toutes les stats
        else if(bigStat)
        {
            if(hit.transform.position.x >= _xOffset.y)
            {
                if(hit.transform.position.x >= _xOffsetMax.y)
                {
                    if(hit.transform.position.y >= _yOffsetMin.y)
                    {
                        if(hit.transform.position.y >= _yOffsetMax.y)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - _offsetXStatPlus, hit.transform.position.y - _offsetYStatPlus - 1, hit.transform.position.z));
                        }
                        else if(hit.transform.position.y <= _yOffsetMax.y && hit.transform.position.y >= _yOffset.y)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - _offsetXStatPlus, hit.transform.position.y - _offsetYStatPlus - .5f, hit.transform.position.z));
                        }
                        else
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - _offsetXStatPlus, hit.transform.position.y - _offsetYStatPlus - .5f, hit.transform.position.z));
                        }
                    }
                    else if(hit.transform.position.y <= _yOffsetMin.x)
                    {
                        if(hit.transform.position.y <= _yOffsetMax.x)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - _offsetXStatPlus, hit.transform.position.y + _offsetYStatPlus + 1, hit.transform.position.z));
                        }
                        else if(hit.transform.position.y >= _yOffsetMax.x && hit.transform.position.y <= _yOffset.x)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - _offsetXStatPlus, hit.transform.position.y + _offsetYStatPlus + .5f, hit.transform.position.z));
                        }
                        else
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - _offsetXStatPlus, hit.transform.position.y + _offsetYStatPlus + .5f, hit.transform.position.z));
                        }
                    }
                    else
                    {
                        pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - _offsetXStatPlus, hit.transform.position.y - _offsetYStatPlus - .5f, hit.transform.position.z));
                    }
                }
                else
                {
                    if(hit.transform.position.y >= _yOffsetMin.y)
                    {
                        if(hit.transform.position.y >= _yOffsetMax.y)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x, hit.transform.position.y - _offsetYStatPlus - 1, hit.transform.position.z));
                        }
                        else if(hit.transform.position.y >= _yOffset.y && hit.transform.position.y <= _yOffsetMax.y)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x, hit.transform.position.y - _offsetYStatPlus - .5f, hit.transform.position.z));
                        }
                        else
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x, hit.transform.position.y - _offsetYStatPlus - .5f, hit.transform.position.z));
                        }
                    }
                    else if(hit.transform.position.y <= _yOffsetMin.x)
                    {
                        if(hit.transform.position.y <= _yOffsetMax.x)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x, hit.transform.position.y + _offsetYStatPlus + 1, hit.transform.position.z));
                        }
                        else if(hit.transform.position.y >= _yOffsetMax.x && hit.transform.position.y <= _yOffset.x)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x, hit.transform.position.y + _offsetYStatPlus + .5f, hit.transform.position.z));
                        }
                        else
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x, hit.transform.position.y + _offsetYStatPlus + .5f, hit.transform.position.z));
                        }
                    }
                    else
                    {
                        pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x, hit.transform.position.y - _offsetYStatPlus - .5f, hit.transform.position.z));
                    }
                }
            }

            else if(hit.transform.position.x <= _xOffset.x)
            {
                if(hit.transform.position.x <= _xOffsetMax.x)
                {
                    if(hit.transform.position.y >= _yOffsetMin.y)
                    {
                        if(hit.transform.position.y >= _yOffsetMax.y)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXStatPlus, hit.transform.position.y - _offsetYStatPlus - 1, hit.transform.position.z));
                        }
                        else if(hit.transform.position.y >= _yOffset.y && hit.transform.position.y <= _yOffsetMax.y)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXStatPlus, hit.transform.position.y - _offsetYStatPlus - .5f, hit.transform.position.z));
                        }
                        else
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXStatPlus, hit.transform.position.y - _offsetYStatPlus - .5f, hit.transform.position.z));
                        }
                    }
                    else if(hit.transform.position.y <= _yOffsetMin.x)
                    {
                        if(hit.transform.position.y <= _yOffsetMax.x)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXStatPlus, hit.transform.position.y + _offsetYStatPlus + 1, hit.transform.position.z));
                        }
                        else if(hit.transform.position.y >= _yOffsetMax.x && hit.transform.position.y <= _yOffset.x)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXStatPlus, hit.transform.position.y + _offsetYStatPlus + .5f, hit.transform.position.z));
                        }
                        else
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXStatPlus, hit.transform.position.y + _offsetYStatPlus + .5f, hit.transform.position.z));
                        }
                    }
                    else
                    {
                        pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x, hit.transform.position.y - _offsetYStatPlus - .5f, hit.transform.position.z));
                    }
                }
                else
                {
                    if(hit.transform.position.y >= _yOffsetMin.y)
                    {
                        if(hit.transform.position.y >= _yOffsetMax.y)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXStatPlus, hit.transform.position.y - _offsetYStatPlus - 1, hit.transform.position.z));
                        }
                        else if(hit.transform.position.y >= _yOffset.y && hit.transform.position.y <= _yOffsetMax.y)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXStatPlus, hit.transform.position.y - _offsetYStatPlus - .5f, hit.transform.position.z));
                        }
                        else
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXStatPlus, hit.transform.position.y - _offsetYStatPlus - .5f, hit.transform.position.z));
                        }
                    }
                    else if(hit.transform.position.y <= _yOffsetMin.x)
                    {
                        if(hit.transform.position.y <= _yOffsetMax.x)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXStatPlus, hit.transform.position.y + _offsetYStatPlus + 1, hit.transform.position.z));
                        }
                        else if(hit.transform.position.y >= _yOffsetMax.x && hit.transform.position.y <= _yOffset.x)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXStatPlus, hit.transform.position.y + _offsetYStatPlus + .5f, hit.transform.position.z));
                        }
                        else
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXStatPlus, hit.transform.position.y + _offsetYStatPlus + .5f, hit.transform.position.z));
                        }
                    }
                    else
                    {
                        pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x, hit.transform.position.y - _offsetYStatPlus - .5f, hit.transform.position.z));
                    }
                }
            }
            else
            {
                if(hit.transform.position.y >= _yOffsetMax.y)
                {
                    pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - _offsetXStatPlus / 2, hit.transform.position.y - _offsetYStatPlus - .5f, hit.transform.position.z));
                }
                else if(hit.transform.position.y <= _yOffsetMax.x)
                {
                    pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - _offsetXStatPlus / 2, hit.transform.position.y + _offsetYStatPlus + .5f, hit.transform.position.z));
                }
                else
                {
                    pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - _offsetXStatPlus * 2.5f, hit.transform.position.y, hit.transform.position.z));
                }
            }
        }
        else if(switchPage){
            pos = new Vector3(lastPosX, lastPosY, ShiftUI[0].transform.position.z);
        }
        else{
            Debug.LogError("Vous essayez de positionner un objet qui ne peut pas se positionner autour de l'unit�");
        }

        //Rendre l'�l�ment visible.
        uiElements.SetActive(true);
        //Si la position de l'UI est diff�rente de celle de la position de r�f�rence alors tu prends cette position comme r�f�rence.
        if (uiElements.transform.position != pos)
        {
            uiElements.transform.position = pos;
        }
    }
    #endregion ActivateUI

    #region ControleDesClicks
    /// <summary>
    /// Permet de d�terminer quand le joueur appuie sur le Shift puis le clic Gauche de la souris.
    /// </summary>
    public void ShiftClick(){
        ActivateUI(ShiftUI[0], 0, 0, false, false, false, true);
        UpdateUIStats();
        _hasCheckUnit = true;
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
        ActivateUI(MouseOverUI, 0, 0, false, false, true);
    }
    #endregion ControleDesClicks

    #region SwitchPages
    /// <summary>
    /// JE prends une liste de boutton, a chaque bouton j'assigne un index. 
    /// </summary>
    /// <param name="button"></param>
    public void buttonAction(StatMenuButton button)
    {
        //0 et  1 sont pour les boutons quitter, 2 et 3 sont pour switch entre la Page 1 et la Page 2
        button._quitMenuPage1.onClick.AddListener(clickQuit);
        button._quitMenuPage2.onClick.AddListener(clickQuit);
        button._rightArrowPage1.onClick.AddListener(switchWindows1);
        button._leftArrowPage2.onClick.AddListener(switchWindows2);
    }

    //Fonction qui permet de cacher les Pages 1 et 2 du carnet.
    public void clickQuit()
    {
        //Je retourne la valeur comme quoi il a click� � false car il a fini son action de Shift+Clic et d�sactive les 2 pages.
        _checkIfPlayerAsClic = false;
        _hasCheckUnit = false;
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
    #endregion SwitchPages
}