using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using TMPro;

/// <summary>
/// Script contenant les controles clavier+souris pour l'UI ainsi que le MouseOver quand la souris survole un élément d'UI.
/// </summary>
public class MouseCommand : MonoBehaviour
{
    #region Variables
    public bool _checkIfPlayerAsClic;

    public bool _hasCheckUnit = false;

    [Header("UI STATISTIQUE UNITE")]
    //Le panneau à afficher lorsqu'on souhaite voir les statistiques de l'unité en cliquant.
    [SerializeField] private GameObject _mouseOverUI;
    public GameObject MouseOverUI => _mouseOverUI;
    //Le panneau ou les panneaux à afficher lorsqu'on souhaite le shift click sur l'unité
    [SerializeField] private List<GameObject> _shiftUI;
    public List<GameObject> ShiftUI => _shiftUI;

    [Header("UI RENFORT UNITE")]
    [SerializeField] private GameObject _renfortUI;
    public GameObject RenfortUI => _renfortUI;

    [SerializeField] private List<GameObject> _elementMenuRenfort = null;
    public List<GameObject> ElementOfMenuRenfort => _elementMenuRenfort;



    [Header("DELAI ATTENTE MOUSE OVER")]
    //Paramètre de délai qui s'applique à la couritine.
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
    
    #region UpdateStatsShiftClick
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
        //Synchronise le texte de la valeur de la distance d'attaque de l'unité avec l'emplacement d'UI.
        UIInstance.Instance.PageUnitStat._rangeGam.GetComponent<TextMeshProUGUI>().text = RaycastManager.Instance.UnitInTile.GetComponent<UnitScript>().AttackRange.ToString();
        //Synchronise le texte de la valeur de la vitesse de l'unité avec l'emplacement d'UI.
        UIInstance.Instance.PageUnitStat._moveGam.GetComponent<TextMeshProUGUI>().text = RaycastManager.Instance.UnitInTile.GetComponent<UnitScript>().MoveSpeed.ToString();

        //Synchronise le texte de l'UI de la avec l'emplacement d'UI.
        UIInstance.Instance.AttackStat._rangeMinDamageGam.GetComponent<TextMeshProUGUI>().text = RaycastManager.Instance.UnitInTile.GetComponent<UnitScript>().NumberRangeMin.x.ToString() + " - " + RaycastManager.Instance.UnitInTile.GetComponent<UnitScript>().NumberRangeMin.y.ToString();
        UIInstance.Instance.AttackStat._rangeMaxDamageGam.GetComponent<TextMeshProUGUI>().text = RaycastManager.Instance.UnitInTile.GetComponent<UnitScript>().NumberRangeMax.x.ToString() + " - " + RaycastManager.Instance.UnitInTile.GetComponent<UnitScript>().NumberRangeMax.y.ToString();
        UIInstance.Instance.AttackStat._minDamageValueGam.GetComponent<TextMeshProUGUI>().text = RaycastManager.Instance.UnitInTile.GetComponent<UnitScript>().DamageMinimum.ToString();
        UIInstance.Instance.AttackStat._maxDamageValueGam.GetComponent<TextMeshProUGUI>().text = RaycastManager.Instance.UnitInTile.GetComponent<UnitScript>().DamageMaximum.ToString();

        //Statistique de la Page 2 du Carnet.  
        //Compléter avec les Images des Tiles.
        //UIInstance.Instance.MiddleImageTerrain[0].GetComponent<Image>().sprite = RaycastManager.Instance.Tile.GetComponent<TileScript>().TerrainEffectList[0].Image
        //UIInstance.Instance.MiddleImageTerrain[1].GetComponent<Image>().sprite = RaycastManager.Instance.Tile.GetComponent<TileScript>().TerrainEffectList[1].Image;

        //Si la tile ne contient pas d'effet de terrain, on n'affiche pas d'information. Si la tile contient 1 effet, on affiche et met à jour l'effet de la case. Si la tile contient 2 effets, on affiche les 2 Effets.
        if (RaycastManager.Instance.Tile.GetComponent<TileScript>().TerrainEffectList.Count == 0)
        {
            UIInstance.Instance.MiddleTextTerrain[4].SetActive(false);
            UIInstance.Instance.MiddleTextTerrain[5].SetActive(false);
        }
        //Si la tile contient 1 effet, on affiche et met à jour l'effet de la case.
        else if (RaycastManager.Instance.Tile.GetComponent<TileScript>().TerrainEffectList.Count == 1)
        {
            UIInstance.Instance.MiddleTextTerrain[4].SetActive(true);
            UIInstance.Instance.MiddleTextTerrain[0].GetComponent<TextMeshProUGUI>().text = RaycastManager.Instance.Tile.GetComponent<TileScript>().TerrainEffectList[0].ToString();
            UIInstance.Instance.MiddleTextTerrain[2].GetComponent<TextMeshProUGUI>().text = "A l'attention des métacogneurs, les effets sont en progs comme les images";
            //Si la tile contient 2 effets, on affiche les 2 Effets.
            if (RaycastManager.Instance.Tile.GetComponent<TileScript>().TerrainEffectList.Count > 1)
            {
                UIInstance.Instance.MiddleTextTerrain[5].SetActive(true);
                UIInstance.Instance.MiddleTextTerrain[1].GetComponent<TextMeshProUGUI>().text = RaycastManager.Instance.Tile.GetComponent<TileScript>().TerrainEffectList[1].ToString();
            }
        }
    }
    #endregion UpdateStatsUpdateStatsShiftClick

    #region Boite De Pandore (Update Menu Renforts)
    void UpdateStatsMenuRenforts()
    {
        if (GameManager.Instance.IsPlayerRedTurn)
        {
            //A modifier si inversement au niveau des usines (J1 et J2)  = > changer le ActionJ2 en ActionJ1.
            if (GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ1 || GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ1)
            {
                /*
                //Update le drapeau qui se situe dans UI Instance correspondant à l'armée Rouge.
                UIInstance.Instance.EmplacementImageMenuRenfort._drapeauDuJoueur.GetComponent<SpriteRenderer>().sprite = UIInstance.Instance.StockageImage._drapeauJoueur[0];
                
                UIInstance.Instance.PageUnitéRenfort._ressourceJoueur.GetComponent<TextMeshProUGUI>().text = player.Ressource.ToString();
                if(player.Ressource <= 1)
                {
                    UIInstance.Instance.PageUnitéRenfort._ressourceJoueur.GetComponent<TextMeshProUGUI>().text = "Ressource";
                }*/

                //Permet de déterminer le nombre d'emplacements à mettre à jour sur le menu Renfort de l'Armée Rouge.
                for (int i = 2; i < unitReference.UnitClassCreableListRedPlayer.Count; i++)
                {
                    #region Update Textuelle et Image Renforts de 1 à 3 pour l'équipe Rouge
                    #region Update Textuelle Renforts de 1 à 3 pour l'équipe Rouge
                    //Active les différents UI des unités de 1 à 3.
                    _elementMenuRenfort[0].SetActive(true);
                    _elementMenuRenfort[1].SetActive(true);
                    _elementMenuRenfort[2].SetActive(true);


                    //Statistique pour l'unité1
                    UIInstance.Instance.PageUnitéRenfort._nameUnit1.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListRedPlayer[0].GetComponent<UnitScript>().UnitSO.UnitName.ToString();
                    UIInstance.Instance.PageUnitéRenfort._lifeValor1.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListRedPlayer[0].GetComponent<UnitScript>().UnitSO.LifeMax.ToString();
                    UIInstance.Instance.PageUnitéRenfort._rangeValor1.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListRedPlayer[0].GetComponent<UnitScript>().UnitSO.AttackRange.ToString();
                    UIInstance.Instance.PageUnitéRenfort._moveValor1.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListRedPlayer[0].GetComponent<UnitScript>().UnitSO.MoveSpeed.ToString();
                    UIInstance.Instance.PageUnitéRenfort._damageValor1.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListRedPlayer[0].GetComponent<UnitScript>().UnitSO.DamageMinimum.ToString();


                    //Statistique pour l'unité2
                    UIInstance.Instance.PageUnitéRenfort._nameUnit2.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListRedPlayer[1].GetComponent<UnitScript>().UnitSO.UnitName.ToString();
                    UIInstance.Instance.PageUnitéRenfort._lifeValor2.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListRedPlayer[1].GetComponent<UnitScript>().UnitSO.LifeMax.ToString();
                    UIInstance.Instance.PageUnitéRenfort._rangeValor2.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListRedPlayer[1].GetComponent<UnitScript>().UnitSO.AttackRange.ToString();
                    UIInstance.Instance.PageUnitéRenfort._moveValor2.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListRedPlayer[1].GetComponent<UnitScript>().UnitSO.MoveSpeed.ToString();
                    UIInstance.Instance.PageUnitéRenfort._damageValor2.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListRedPlayer[1].GetComponent<UnitScript>().UnitSO.DamageMinimum.ToString();


                    //Statistique pour l'unité3
                    UIInstance.Instance.PageUnitéRenfort._nameUnit3.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListRedPlayer[2].GetComponent<UnitScript>().UnitSO.UnitName.ToString();
                    UIInstance.Instance.PageUnitéRenfort._lifeValor3.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListRedPlayer[2].GetComponent<UnitScript>().UnitSO.LifeMax.ToString();
                    UIInstance.Instance.PageUnitéRenfort._rangeValor3.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListRedPlayer[2].GetComponent<UnitScript>().UnitSO.AttackRange.ToString();
                    UIInstance.Instance.PageUnitéRenfort._moveValor3.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListRedPlayer[2].GetComponent<UnitScript>().UnitSO.MoveSpeed.ToString();
                    UIInstance.Instance.PageUnitéRenfort._damageValor3.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListRedPlayer[2].GetComponent<UnitScript>().UnitSO.DamageMinimum.ToString();

                    #endregion Update Textuelle Renforts de 1 à 3 pour l'équipe Rouge

                    #region Update Image Renforts de 1 à 3
                    //Update Ressource en fonction du nombre.

                    //Image Ressource pour l'unité 1 de l'armée Rouge
                    UIInstance.Instance.RessourceUnit_PasTouche._unité1Ressource[0].SetActive(true);
                    UIInstance.Instance.RessourceUnit_PasTouche._unité1Ressource[1].SetActive(true);

                    //Si la première unité de l'armée Rouge a besoin de plus de 2 ressources.
                    if (unitReference.UnitClassCreableListRedPlayer[0].GetComponent<UnitScript>().UnitSO.CreationCost > 2)
                    {
                        UIInstance.Instance.RessourceUnit_PasTouche._unité1Ressource[2].SetActive(true);
                        UIInstance.Instance.RessourceUnit_PasTouche._unité1Ressource[3].SetActive(true);
                    }
                    else
                    {
                        UIInstance.Instance.RessourceUnit_PasTouche._unité1Ressource[2].SetActive(false);
                        UIInstance.Instance.RessourceUnit_PasTouche._unité1Ressource[3].SetActive(false);
                    }

                    //Image Ressource pour l'unité 2 de l'armée Rouge
                    UIInstance.Instance.RessourceUnit_PasTouche._unité2Ressource[0].SetActive(true);
                    UIInstance.Instance.RessourceUnit_PasTouche._unité2Ressource[1].SetActive(true);

                    //Si la deuxième unité de l'armée Rouge a besoin de plus de 2 ressources.
                    if (unitReference.UnitClassCreableListRedPlayer[1].GetComponent<UnitScript>().UnitSO.CreationCost > 2)
                    {
                        UIInstance.Instance.RessourceUnit_PasTouche._unité2Ressource[2].SetActive(true);
                        UIInstance.Instance.RessourceUnit_PasTouche._unité2Ressource[3].SetActive(true);
                    }
                    else
                    {
                        UIInstance.Instance.RessourceUnit_PasTouche._unité2Ressource[2].SetActive(false);
                        UIInstance.Instance.RessourceUnit_PasTouche._unité2Ressource[3].SetActive(false);
                    }


                    //Image Ressource pour l'unité 3 de l'armée Rouge
                    UIInstance.Instance.RessourceUnit_PasTouche._unité3Ressource[0].SetActive(true);
                    UIInstance.Instance.RessourceUnit_PasTouche._unité3Ressource[1].SetActive(true);

                    //Si la troisième unité de l'armée Rouge a besoin de plus de 2 ressources.
                    if (unitReference.UnitClassCreableListRedPlayer[2].GetComponent<UnitScript>().UnitSO.CreationCost > 2)
                    {
                        UIInstance.Instance.RessourceUnit_PasTouche._unité3Ressource[2].SetActive(true);
                        UIInstance.Instance.RessourceUnit_PasTouche._unité3Ressource[3].SetActive(true);
                    }
                    else
                    {
                        UIInstance.Instance.RessourceUnit_PasTouche._unité3Ressource[2].SetActive(false);
                        UIInstance.Instance.RessourceUnit_PasTouche._unité3Ressource[3].SetActive(false);
                    }

                    #endregion Update Image Renforts de 1 à 3
                    #endregion Update Textuelle et Image Renforts de 1 à 3 pour l'équipe Rouge

                    #region Update Textuelle et Image Renforts de 4 à 6 pour l'équipe Rouge
                    //Si la liste des unités créables comportent plus de 3 unités dans la liste de l'équipe Rouge.
                    if (i >= 3)
                    {
                        //Active l'UI de l'unité 4 de l'arrmée Rouge.
                        _elementMenuRenfort[3].SetActive(true);

                        //Statistique pour l'unité4
                        UIInstance.Instance.PageUnitéRenfort._nameUnit4.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListRedPlayer[3].GetComponent<UnitScript>().UnitSO.UnitName.ToString();
                        UIInstance.Instance.PageUnitéRenfort._lifeValor4.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListRedPlayer[3].GetComponent<UnitScript>().UnitSO.LifeMax.ToString();
                        UIInstance.Instance.PageUnitéRenfort._rangeValor4.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListRedPlayer[3].GetComponent<UnitScript>().UnitSO.AttackRange.ToString();
                        UIInstance.Instance.PageUnitéRenfort._moveValor4.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListRedPlayer[3].GetComponent<UnitScript>().UnitSO.MoveSpeed.ToString();
                        UIInstance.Instance.PageUnitéRenfort._damageValor4.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListRedPlayer[3].GetComponent<UnitScript>().UnitSO.DamageMinimum.ToString();

                        //Image Ressource pour l'unité 4 de l'armée Rouge.
                        UIInstance.Instance.RessourceUnit_PasTouche._unité4Ressource[0].SetActive(true);
                        UIInstance.Instance.RessourceUnit_PasTouche._unité4Ressource[1].SetActive(true);

                        //Si la quatrième unité de l'armée Rouge a besoin de plus de 2 ressources.
                        if (unitReference.UnitClassCreableListRedPlayer[3].GetComponent<UnitScript>().UnitSO.CreationCost > 2)
                        {
                            UIInstance.Instance.RessourceUnit_PasTouche._unité4Ressource[2].SetActive(true);
                            UIInstance.Instance.RessourceUnit_PasTouche._unité4Ressource[3].SetActive(true);
                        }
                        else
                        {
                            UIInstance.Instance.RessourceUnit_PasTouche._unité4Ressource[2].SetActive(false);
                            UIInstance.Instance.RessourceUnit_PasTouche._unité4Ressource[3].SetActive(false);
                        }

                        //Si la liste des unités créables comportent plus de 4 unités dans la liste de l'équipe Rouge.
                        if (i >= 4)
                        {
                            //Active l'UI de l'unité 5 de l'arrmée Rouge.
                            _elementMenuRenfort[4].SetActive(true);

                            //Statistique pour l'unité 5 de l'armée Rouge.
                            UIInstance.Instance.PageUnitéRenfort._nameUnit5.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListRedPlayer[4].GetComponent<UnitScript>().UnitSO.UnitName.ToString();
                            UIInstance.Instance.PageUnitéRenfort._lifeValor5.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListRedPlayer[4].GetComponent<UnitScript>().UnitSO.LifeMax.ToString();
                            UIInstance.Instance.PageUnitéRenfort._rangeValor5.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListRedPlayer[4].GetComponent<UnitScript>().UnitSO.AttackRange.ToString();
                            UIInstance.Instance.PageUnitéRenfort._moveValor5.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListRedPlayer[4].GetComponent<UnitScript>().UnitSO.MoveSpeed.ToString();
                            UIInstance.Instance.PageUnitéRenfort._damageValor5.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListRedPlayer[4].GetComponent<UnitScript>().UnitSO.DamageMinimum.ToString();

                            //Image Ressource pour l'unité 5 de l'armée Rouge.
                            UIInstance.Instance.RessourceUnit_PasTouche._unité5Ressource[0].SetActive(true);
                            UIInstance.Instance.RessourceUnit_PasTouche._unité5Ressource[1].SetActive(true);

                            //Si la cinquième unité de l'armée Rouge a besoin de plus de 2 ressources.
                            if (unitReference.UnitClassCreableListRedPlayer[4].GetComponent<UnitScript>().UnitSO.CreationCost > 2)
                            {
                                UIInstance.Instance.RessourceUnit_PasTouche._unité5Ressource[2].SetActive(true);
                                UIInstance.Instance.RessourceUnit_PasTouche._unité5Ressource[3].SetActive(true);
                            }
                            else
                            {
                                UIInstance.Instance.RessourceUnit_PasTouche._unité5Ressource[2].SetActive(false);
                                UIInstance.Instance.RessourceUnit_PasTouche._unité5Ressource[3].SetActive(false);
                            }

                            //Si la liste des unités créables comportent plus de 5 unités dans la liste de l'équipe Rouge.
                            if (i >= 5)
                            {
                                //Active l'UI de l'unité 6 de l'arrmée Rouge.
                                _elementMenuRenfort[5].SetActive(true);

                                //Statistique pour l'unité6 de l'armée Rouge.
                                UIInstance.Instance.PageUnitéRenfort._nameUnit6.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListRedPlayer[5].GetComponent<UnitScript>().UnitSO.UnitName.ToString();
                                UIInstance.Instance.PageUnitéRenfort._lifeValor6.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListRedPlayer[5].GetComponent<UnitScript>().UnitSO.LifeMax.ToString();
                                UIInstance.Instance.PageUnitéRenfort._rangeValor6.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListRedPlayer[5].GetComponent<UnitScript>().UnitSO.AttackRange.ToString();
                                UIInstance.Instance.PageUnitéRenfort._moveValor6.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListRedPlayer[5].GetComponent<UnitScript>().UnitSO.MoveSpeed.ToString();
                                UIInstance.Instance.PageUnitéRenfort._damageValor6.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListRedPlayer[5].GetComponent<UnitScript>().UnitSO.DamageMinimum.ToString();

                                //Image Ressource pour l'unité6 de l'armée Rouge.
                                UIInstance.Instance.RessourceUnit_PasTouche._unité6Ressource[0].SetActive(true);
                                UIInstance.Instance.RessourceUnit_PasTouche._unité6Ressource[1].SetActive(true);

                                //Si la sixième unité de l'armée Rouge a besoin de plus de 2 ressources.
                                if (unitReference.UnitClassCreableListRedPlayer[5].GetComponent<UnitScript>().UnitSO.CreationCost > 2)
                                {
                                    UIInstance.Instance.RessourceUnit_PasTouche._unité6Ressource[2].SetActive(true);
                                    UIInstance.Instance.RessourceUnit_PasTouche._unité6Ressource[3].SetActive(true);
                                }
                                else
                                {
                                    UIInstance.Instance.RessourceUnit_PasTouche._unité6Ressource[2].SetActive(false);
                                    UIInstance.Instance.RessourceUnit_PasTouche._unité6Ressource[3].SetActive(false);
                                }

                            }
                        }
                    }
                    #endregion Update Textuelle et Image Renforts de 4 à 6 pour l'équipe Rouge
                }
            }
        }

        else if (!GameManager.Instance.IsPlayerRedTurn)
        {
            //A modifier si inversement au niveau des usines (J1 et J2) = > changer le ActionJ1 en ActionJ2.
            if (GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ2 || GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ1)
            {
                /*
                //Update le drapeau qui se situe dans UI Instance correspondant à l'armée Bleu.
                UIInstance.Instance.EmplacementImageMenuRenfort._drapeauDuJoueur.GetComponent<SpriteRenderer>().sprite = UIInstance.Instance.StockageImage._drapeauJoueur[1];

                UIInstance.Instance.PageUnitéRenfort._ressourceJoueur.GetComponent<TextMeshProUGUI>().text = player.Ressource.ToString();
                if (player.Ressource <= 1)
                {
                    UIInstance.Instance.PageUnitéRenfort._ressourceJoueur.GetComponent<TextMeshProUGUI>().text = "Ressource";
                }*/

                //Permet de déterminer le nombre d'emplacements à mettre à jour sur le menu Renfort de l'Armée Bleu.
                for (int i = 2; i < unitReference.UnitClassCreableListBluePlayer.Count; i++)
                {

                    #region Update Textuelle et Image Renforts de 1 à 3 pour l'équipe Bleu
                    #region Update Textuelle Renforts de 1 à 3 pour l'équipe Bleu

                    //Active les différents UI des unités 1 à 3 de l'armée Bleu.
                    _elementMenuRenfort[0].SetActive(true);
                    _elementMenuRenfort[1].SetActive(true);
                    _elementMenuRenfort[2].SetActive(true);

                    //Statistique pour l'unité 1 de l'armée Bleu.
                    UIInstance.Instance.PageUnitéRenfort._nameUnit1.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListBluePlayer[0].GetComponent<UnitScript>().UnitSO.UnitName.ToString();
                    UIInstance.Instance.PageUnitéRenfort._lifeValor1.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListBluePlayer[0].GetComponent<UnitScript>().UnitSO.LifeMax.ToString();
                    UIInstance.Instance.PageUnitéRenfort._rangeValor1.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListBluePlayer[0].GetComponent<UnitScript>().UnitSO.AttackRange.ToString();
                    UIInstance.Instance.PageUnitéRenfort._moveValor1.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListBluePlayer[0].GetComponent<UnitScript>().UnitSO.MoveSpeed.ToString();
                    UIInstance.Instance.PageUnitéRenfort._damageValor1.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListBluePlayer[0].GetComponent<UnitScript>().UnitSO.DamageMinimum.ToString();


                    //Statistique pour l'unité 2 de l'armée Bleu.
                    UIInstance.Instance.PageUnitéRenfort._nameUnit2.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListBluePlayer[1].GetComponent<UnitScript>().UnitSO.UnitName.ToString();
                    UIInstance.Instance.PageUnitéRenfort._lifeValor2.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListBluePlayer[1].GetComponent<UnitScript>().UnitSO.LifeMax.ToString();
                    UIInstance.Instance.PageUnitéRenfort._rangeValor2.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListBluePlayer[1].GetComponent<UnitScript>().UnitSO.AttackRange.ToString();
                    UIInstance.Instance.PageUnitéRenfort._moveValor2.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListBluePlayer[1].GetComponent<UnitScript>().UnitSO.MoveSpeed.ToString();
                    UIInstance.Instance.PageUnitéRenfort._damageValor2.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListBluePlayer[1].GetComponent<UnitScript>().UnitSO.DamageMinimum.ToString();


                    //Statistique pour l'unité 3 de l'armée Bleu.
                    UIInstance.Instance.PageUnitéRenfort._nameUnit3.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListBluePlayer[2].GetComponent<UnitScript>().UnitSO.UnitName.ToString();
                    UIInstance.Instance.PageUnitéRenfort._lifeValor3.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListBluePlayer[2].GetComponent<UnitScript>().UnitSO.LifeMax.ToString();
                    UIInstance.Instance.PageUnitéRenfort._rangeValor3.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListBluePlayer[2].GetComponent<UnitScript>().UnitSO.AttackRange.ToString();
                    UIInstance.Instance.PageUnitéRenfort._moveValor3.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListBluePlayer[2].GetComponent<UnitScript>().UnitSO.MoveSpeed.ToString();
                    UIInstance.Instance.PageUnitéRenfort._damageValor3.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListBluePlayer[2].GetComponent<UnitScript>().UnitSO.DamageMinimum.ToString();
                    #endregion Update Textuelle Renforts de 1 à 3 pour l'équipe Bleu

                    #region Update Image Renforts de 1 à 3 pour l'équipe Bleu
                    //Update Ressource en fonction du nombre.
                    //Image Ressource pour l'unité 1 de l'armée Bleu.
                    UIInstance.Instance.RessourceUnit_PasTouche._unité1Ressource[0].SetActive(true);
                    UIInstance.Instance.RessourceUnit_PasTouche._unité1Ressource[1].SetActive(true);

                    //Si la première unité de l'armée Bleu a besoin de plus de 2 ressources.
                    if (unitReference.UnitClassCreableListBluePlayer[0].GetComponent<UnitScript>().UnitSO.CreationCost > 2)
                    {
                        UIInstance.Instance.RessourceUnit_PasTouche._unité1Ressource[2].SetActive(true);
                        UIInstance.Instance.RessourceUnit_PasTouche._unité1Ressource[3].SetActive(true);
                    }
                    else
                    {
                        UIInstance.Instance.RessourceUnit_PasTouche._unité1Ressource[2].SetActive(false);
                        UIInstance.Instance.RessourceUnit_PasTouche._unité1Ressource[3].SetActive(false);
                    }

                    //Image Ressource pour l'unité 2 de l'armée Bleu.
                    UIInstance.Instance.RessourceUnit_PasTouche._unité2Ressource[0].SetActive(true);
                    UIInstance.Instance.RessourceUnit_PasTouche._unité2Ressource[1].SetActive(true);

                    //Si la deuxième unité de l'armée Bleu a besoin de plus de 2 ressources.
                    if (unitReference.UnitClassCreableListBluePlayer[1].GetComponent<UnitScript>().UnitSO.CreationCost > 2)
                    {
                        UIInstance.Instance.RessourceUnit_PasTouche._unité2Ressource[2].SetActive(true);
                        UIInstance.Instance.RessourceUnit_PasTouche._unité2Ressource[3].SetActive(true);
                    }
                    else
                    {
                        UIInstance.Instance.RessourceUnit_PasTouche._unité2Ressource[2].SetActive(false);
                        UIInstance.Instance.RessourceUnit_PasTouche._unité2Ressource[3].SetActive(false);
                    }


                    //Image Ressource pour l'unité 3 de l'armée Bleu.
                    UIInstance.Instance.RessourceUnit_PasTouche._unité3Ressource[0].SetActive(true);
                    UIInstance.Instance.RessourceUnit_PasTouche._unité3Ressource[1].SetActive(true);

                    //Si la troisième unité de l'armée Bleu a besoin de plus de 2 ressources.
                    if (unitReference.UnitClassCreableListBluePlayer[2].GetComponent<UnitScript>().UnitSO.CreationCost > 2)
                    {
                        UIInstance.Instance.RessourceUnit_PasTouche._unité3Ressource[2].SetActive(true);
                        UIInstance.Instance.RessourceUnit_PasTouche._unité3Ressource[3].SetActive(true);
                    }
                    else
                    {
                        UIInstance.Instance.RessourceUnit_PasTouche._unité3Ressource[2].SetActive(false);
                        UIInstance.Instance.RessourceUnit_PasTouche._unité3Ressource[3].SetActive(false);
                    }

                    #endregion Update Image Renforts de 1 à 3 pour l'équipe Bleu
                    #endregion Update Textuelle et Image Renforts de 1 à 3 pour l'équipe Bleu

                    #region Update Image Textuelle et Image de 4 à 6 pour l'équipe Bleu
                    //Update Ressource en fonction du nombre.
                    //Si la liste des unités créables comportent plus de 3 unités dans la liste de l'équipe Rouge.
                    if (i >= 3)
                    {
                        //Active l'UI de l'unité 4 (oui 4 car dans une liste, le 0 est pris en compte comme l'emplacement 1).
                        _elementMenuRenfort[3].SetActive(true);

                        //Statistique pour l'unité 4 de l'armée Bleu.
                        UIInstance.Instance.PageUnitéRenfort._nameUnit4.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListBluePlayer[3].GetComponent<UnitScript>().UnitSO.UnitName.ToString();
                        UIInstance.Instance.PageUnitéRenfort._lifeValor4.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListBluePlayer[3].GetComponent<UnitScript>().UnitSO.LifeMax.ToString();
                        UIInstance.Instance.PageUnitéRenfort._rangeValor4.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListBluePlayer[3].GetComponent<UnitScript>().UnitSO.AttackRange.ToString();
                        UIInstance.Instance.PageUnitéRenfort._moveValor4.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListBluePlayer[3].GetComponent<UnitScript>().UnitSO.MoveSpeed.ToString();
                        UIInstance.Instance.PageUnitéRenfort._damageValor4.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListBluePlayer[3].GetComponent<UnitScript>().UnitSO.DamageMinimum.ToString();

                        //Image Ressource pour l'unité 4 de l'armée Bleu.
                        UIInstance.Instance.RessourceUnit_PasTouche._unité4Ressource[0].SetActive(true);
                        UIInstance.Instance.RessourceUnit_PasTouche._unité4Ressource[1].SetActive(true);

                        //Si la quatrième unité de l'armée Bleu a besoin de plus de 2 ressources.
                        if (unitReference.UnitClassCreableListBluePlayer[3].GetComponent<UnitScript>().UnitSO.CreationCost > 2)
                        {
                            UIInstance.Instance.RessourceUnit_PasTouche._unité4Ressource[2].SetActive(true);
                            UIInstance.Instance.RessourceUnit_PasTouche._unité4Ressource[3].SetActive(true);
                        }
                        else
                        {
                            UIInstance.Instance.RessourceUnit_PasTouche._unité4Ressource[2].SetActive(false);
                            UIInstance.Instance.RessourceUnit_PasTouche._unité4Ressource[3].SetActive(false);
                        }

                        //Si la liste des unités créables comportent plus de 4 unités dans la liste de l'équipe Bleu.
                        if (i >= 4)
                        {
                            //Active l'UI de l'unité 5 (oui 5 car dans une liste, le 0 est pris en compte comme l'emplacement 1).
                            _elementMenuRenfort[4].SetActive(true);

                            //Statistique pour l'unité5
                            UIInstance.Instance.PageUnitéRenfort._nameUnit5.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListBluePlayer[4].GetComponent<UnitScript>().UnitSO.UnitName.ToString();
                            UIInstance.Instance.PageUnitéRenfort._lifeValor5.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListBluePlayer[4].GetComponent<UnitScript>().UnitSO.LifeMax.ToString();
                            UIInstance.Instance.PageUnitéRenfort._rangeValor5.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListBluePlayer[4].GetComponent<UnitScript>().UnitSO.AttackRange.ToString();
                            UIInstance.Instance.PageUnitéRenfort._moveValor5.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListBluePlayer[4].GetComponent<UnitScript>().UnitSO.MoveSpeed.ToString();
                            UIInstance.Instance.PageUnitéRenfort._damageValor5.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListBluePlayer[4].GetComponent<UnitScript>().UnitSO.DamageMinimum.ToString();


                            //Image Ressource pour l'unité 5 de l'armée Bleu.
                            UIInstance.Instance.RessourceUnit_PasTouche._unité5Ressource[0].SetActive(true);
                            UIInstance.Instance.RessourceUnit_PasTouche._unité5Ressource[1].SetActive(true);

                            //Si la cinquième unité de l'armée Bleu a besoin de plus de 2 ressources.
                            if (unitReference.UnitClassCreableListBluePlayer[4].GetComponent<UnitScript>().UnitSO.CreationCost > 2)
                            {
                                UIInstance.Instance.RessourceUnit_PasTouche._unité5Ressource[2].SetActive(true);
                                UIInstance.Instance.RessourceUnit_PasTouche._unité5Ressource[3].SetActive(true);
                            }
                            else
                            {
                                UIInstance.Instance.RessourceUnit_PasTouche._unité5Ressource[2].SetActive(false);
                                UIInstance.Instance.RessourceUnit_PasTouche._unité5Ressource[3].SetActive(false);
                            }

                            //Si la liste des unités créables comportent plus de 5 unités dans la liste de l'équipe Bleu.
                            if (i >= 5)
                            {
                                //Active l'UI de l'unité 6 (oui 6 car dans une liste, le 0 est pris en compte comme l'emplacement 1).
                                _elementMenuRenfort[5].SetActive(true);

                                //Statistique pour l'unité6
                                UIInstance.Instance.PageUnitéRenfort._nameUnit6.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListBluePlayer[5].GetComponent<UnitScript>().UnitSO.UnitName.ToString();
                                UIInstance.Instance.PageUnitéRenfort._lifeValor6.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListBluePlayer[5].GetComponent<UnitScript>().UnitSO.LifeMax.ToString();
                                UIInstance.Instance.PageUnitéRenfort._rangeValor6.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListBluePlayer[5].GetComponent<UnitScript>().UnitSO.AttackRange.ToString();
                                UIInstance.Instance.PageUnitéRenfort._moveValor6.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListBluePlayer[5].GetComponent<UnitScript>().UnitSO.MoveSpeed.ToString();
                                UIInstance.Instance.PageUnitéRenfort._damageValor6.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListBluePlayer[5].GetComponent<UnitScript>().UnitSO.DamageMinimum.ToString();

                                //Image Ressource pour l'unité 6 de l'armée Bleu.
                                UIInstance.Instance.RessourceUnit_PasTouche._unité6Ressource[0].SetActive(true);
                                UIInstance.Instance.RessourceUnit_PasTouche._unité6Ressource[1].SetActive(true);

                                //Si la sixième unité de l'armée Bleu a besoin de plus de 2 ressources.
                                if (unitReference.UnitClassCreableListBluePlayer[5].GetComponent<UnitScript>().UnitSO.CreationCost > 2)
                                {
                                    UIInstance.Instance.RessourceUnit_PasTouche._unité6Ressource[2].SetActive(true);
                                    UIInstance.Instance.RessourceUnit_PasTouche._unité6Ressource[3].SetActive(true);
                                }
                                else
                                {
                                    UIInstance.Instance.RessourceUnit_PasTouche._unité6Ressource[2].SetActive(false);
                                    UIInstance.Instance.RessourceUnit_PasTouche._unité6Ressource[3].SetActive(false);
                                }
                            }
                        }
                        #endregion Update Image Textuelle et Image de 4 à 6 pour l'équipe Bleu
                    }
                }
            }
        }
    }
    #endregion Boite De Pandore (Update Menu Renforts)

    #region ActivateUI
    /// <summary>
    /// Permet d'activer un élément de l'UI en utilisant un Raycast distint de la position et d'assigner une position custom par rapport au Canvas (Conflit avec le Canvas).
    /// </summary>
    /// <param name="uiElements"></param>
    /// <param name="offSetX"></param>
    /// <param name="offSetY"></param>
    public void ActivateUI(GameObject uiElements, float lastPosX = 0, float lastPosY = 0, bool switchPage = false, bool activationMenu = false, bool mouseOver = false, bool bigStat = false)
    {
        //Reprendre la position du raycast qui a sélectionné la tile
        RaycastHit2D hit = RaycastManager.Instance.GetRaycastHit();

        //Je stop l'ensemble des coroutines en cour.
        Vector3 pos = Vector3.zero;
        StopAllCoroutines();

        //Menu d'activation d'une unité
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
            Debug.LogError("Vous essayez de positionner un objet qui ne peut pas se positionner autour de l'unité");
        }

        //Rendre l'élément visible.
        uiElements.SetActive(true);
        //Si la position de l'UI est différente de celle de la position de référence alors tu prends cette position comme référence.
        if (uiElements.transform.position != pos)
        {
            uiElements.transform.position = pos;
        }
    }
    #endregion ActivateUI

    #region MenuRenfortUI
    public void MenuRenfortUI()
    {
        ActivateUI(RenfortUI, _offsetX - _maxoffsetX, _offsetY - _maxoffsetY);
        UpdateStatsMenuRenforts();
        buttonAction(UIInstance.Instance.PageButton);
    }


    #endregion MenuRenfortUI

    #region ControleDesClicks
    /// <summary>
    /// Permet de déterminer quand le joueur appuie sur le Shift puis le clic Gauche de la souris.
    /// </summary>
    public void ShiftClick(){
        ActivateUI(ShiftUI[0], 0, 0, false, false, false, true);
        UpdateUIStats();
        _hasCheckUnit = true;
    }

    /// <summary>
    /// Permet de déterminer et d'afficher un élément quand la souris passe au dessus d'une tuile possédant une unité.
    /// </summary>
    public void MouseOverWithoutClick()
    {
        //Si le joueur n'a pas cliqué, alors tu lances la coroutine.
        if (_checkIfPlayerAsClic == false)
        {
            //Coroutine : Une coroutine est une fonction qui peut suspendre son exécution (yield) jusqu'à la fin de la YieldInstruction donnée.
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
    /// Fonction pour désactiver en MouseOver.
    /// </summary>
    public void MouseExitWithoutClick()
    {
        //Arrete l'ensemble des coroutines dans la scène.
        StopAllCoroutines();
        _mouseOverUI.SetActive(false);
    }

    /// <summary>
    /// Correspond au paramètre qu'on rentre dans la coroutine.
    /// </summary>
    /// <param name="Timer"></param>
    /// <returns></returns>
    IEnumerator ShowObject(float TimeToWait)
    {
        //J'utilise un délai pour que le boutton apparaisse après un délai.
        yield return new WaitForSeconds(TimeToWait);
        //J'active l'élément et je lui assigne des paramètres.
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
        button._quitMenuPage1.onClick.AddListener(QuitEverything);
        button._quitMenuPage2.onClick.AddListener(QuitEverything);

        //Boutton Annuler pour quitter le Menu Renfort
        button._quitMenuRenfort.onClick.AddListener(QuitEverything);


        //Boutton qui permettent de créer des unités pour l'Armée Rouge.
        if (GameManager.Instance.IsPlayerRedTurn)
        {
            for (int i = 0; i < unitReference.UnitClassCreableListBluePlayer.Count; i++)
            {
                button._clicUnit1.onClick.AddListener(craftUnit);
                button._clicUnit2.onClick.AddListener(craftUnit);
                button._clicUnit3.onClick.AddListener(craftUnit);

                if (i >= 3)
                {
                    button._clicUnit4.onClick.AddListener(craftUnit);

                    if (i >= 4)
                    {
                        button._clicUnit5.onClick.AddListener(craftUnit);
                    }
                    if (i >= 5)
                    {
                        button._clicUnit6.onClick.AddListener(craftUnit);
                    }
                }
            }
        }

        //Boutton qui permettent de créer des unités pour l'Armée Bleu.
        if (!GameManager.Instance.IsPlayerRedTurn)
        {
            for (int i = 0; i < unitReference.UnitClassCreableListRedPlayer.Count; i++)
            {
                button._clicUnit1.onClick.AddListener(craftUnit);
                button._clicUnit2.onClick.AddListener(craftUnit);
                button._clicUnit3.onClick.AddListener(craftUnit);

                if (i >= 3)
                {
                    button._clicUnit4.onClick.AddListener(craftUnit);

                    if (i >= 4)
                    {
                        button._clicUnit5.onClick.AddListener(craftUnit);
                    }
                    if (i >= 5)
                    {
                        button._clicUnit6.onClick.AddListener(craftUnit);
                    }
                } 
            }
        }

        button._rightArrowPage1.onClick.AddListener(switchWindows1);
        button._leftArrowPage2.onClick.AddListener(switchWindows2);
    }

    /// <summary>
    /// Fonction qui permet de cacher les Pages 1 et 2 du carnet.
    /// </summary>
    public void clickQuit()
    {
        //Je retourne la valeur comme quoi il a clické à false car il a fini son action de Shift+Clic et désactive les 2 pages.
        _checkIfPlayerAsClic = false;
        _hasCheckUnit = false;
        ShiftUI[0].SetActive(false);
        ShiftUI[1].SetActive(false);
    }

    /// <summary>
    /// Permet de switch entre la page 1 et la page 2
    /// </summary>
    void switchWindows1()
    {
        //J'active le Panneau 2 car le joueur a cliqué sur le bouton permettant de transitionner de la page 1 à la page 2. De plus, je masque la page 1.
        ActivateUI(ShiftUI[1], ShiftUI[0].transform.position.x, ShiftUI[0].transform.position.y, true);
        ShiftUI[0].SetActive(false);
    }
    
    /// <summary>
    /// Switch entre la page 2 et la page 1.
    /// </summary>
    void switchWindows2()
    {
        //J'active le Panneau 1 car le joueur a cliqué sur le bouton permettant de transitionner de la page 2 à la page 1. De plus, je masque la page 2.
        ActivateUI(ShiftUI[0], ShiftUI[1].transform.position.x, ShiftUI[1].transform.position.y, true);
        ShiftUI[1].SetActive(false);
    }
    #endregion SwitchPages
}