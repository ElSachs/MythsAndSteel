using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RaycastManager : MonoSingleton<RaycastManager>
{
    #region Appel de Script
    public MouseCommand _mouseCommand;
    #endregion
    #region Variables
    [Header("INFO DU RAYCAST")]
    //Les layer qui sont détectés par le raycast
    [SerializeField] private LayerMask _layerM;

    //tile qui se trouve sous le raycast
    [SerializeField] private GameObject _tile;
    public GameObject Tile => _tile;

    //Dernière tile en mémoire par ce script
    GameObject _lastTile = null;

    //tile qui se trouve sous le raycast
    [SerializeField] private GameObject _unitInTile;
    public GameObject UnitInTile => _unitInTile;

    //Lorsque le joueur clique sur une tile
    [SerializeField] private GameObject _actualTileSelected;
    public GameObject ActualTileSelected
    {
        get
        {
            return _actualTileSelected;
        }
        set
        {
            _actualTileSelected = value;
        }
    }

    //Lorsque le joueur clique sur une tile et qu'il y a une unité
    [SerializeField] private GameObject _actualUnitSelected;
    public GameObject ActualUnitSelected => _actualUnitSelected;

    [Header("PANNEAU DES BOUTONS QUAND CLIC SUR UNITE")]
    //Menu a activer quand clic sur unité
    [SerializeField] private GameObject _menuForUnit = null;

    //Est ce que les joueurs peuvent jouer
    bool _isInTurn = false;

    //Event pour quand le joueur clique sur un bouton pour passer à la phase suivante
    public delegate void TileRaycastChange();
    public event TileRaycastChange OnTileChanged;
    #endregion Variables

    private void Start()
    {
        OnTileChanged += RaycastManager_OnTileChanged;
    }

    private void RaycastManager_OnTileChanged()
    {
        UIInstance.Instance.CallUpdateUI(_lastTile);
    }

    void Update(){
        //obtient le premier objet touché par le raycast
        RaycastHit2D hit = GetRaycastHit();

        //Remplace le gameObject Tile pour avoir en avoir une sauvegarde
        _tile = hit.collider != null ? hit.collider.gameObject : null;

        //Assigne l'unité si la tile qui est sélectionnée possède une unité
        _unitInTile = _tile != null ? _tile.GetComponent<TileScript>().Unit != null ? _tile.GetComponent<TileScript>().Unit : null : null;

        //Permet de combiner le Shift et le click gauche de la souris.
        if (_unitInTile == true){
            //Si le joueur a utilisé le Shift puis leclick, le joueur est considéré comme click et on applique les fonctions propres au bouton des panneaux. De plus, le mouseOver est désactivé.
            if (_mouseCommand._checkIfPlayerAsClic == true && _mouseCommand._hasCheckUnit == false)
            {
                _mouseCommand.ShiftClick();
                CallMouseCommand();
            }
            else if(_mouseCommand._checkIfPlayerAsClic == false)
            {
                //Si le joueur n'a pas continué sa combinaison d'action( Shif+clic), alors quand ma souris reste sur une case sans cliqué, l'interface résumé des statistiques s'active.
                _mouseCommand.MouseOverWithoutClick();
            }
        }
        else
        {
            //Si la case ne comporte pas d'unité alors le MouseOver ne s'active pas et n'affiche par l'interface résumé des statistiques.
            _mouseCommand.MouseExitWithoutClick();
        }

        //Si la tile change
        if (_tile != _lastTile || _isInTurn != GameManager.Instance.IsInTurn)
        {
            _isInTurn = GameManager.Instance.IsInTurn;
            _lastTile = _tile;
            OnTileChanged();
        }
    }

    /// <summary>
    /// Quand tu cliques sur une unité
    /// </summary>
    public void Select(){
        //Lorsque le joueur choisit une unité
        if(GameManager.Instance.ChooseUnitForEvent){
            if(_unitInTile != null){
                if(GameManager.Instance.UnitChooseList.Contains(_unitInTile)){
                    GameManager.Instance.RemoveUnitToList(_unitInTile);
                }
                else{
                    if(GameManager.Instance.SelectableUnit.Contains(UnitInTile)){
                        if(!GameManager.Instance.IllusionStratégique){
                            Debug.Log("je suis passé par la");
                            GameManager.Instance.AddUnitToList(_unitInTile);
                        }

                        //Pour la carte événement Illusion Stratégique
                        else{
                            if(GameManager.Instance.UnitChooseList.Count > 0){
                                //est ce qu'il y avait déjà une unité dans la liste
                                if(GameManager.Instance.UnitChooseList[0].GetComponent<UnitScript>().UnitSO.IsInRedArmy == _unitInTile.GetComponent<UnitScript>().UnitSO.IsInRedArmy){
                                    GameManager.Instance.AddUnitToList(_unitInTile);
                                }
                                else { }
                            }
                            else{
                                GameManager.Instance.AddUnitToList(_unitInTile);
                            }
                        }
                    }
                }
            }
        }

        //lorsque le joueur choisit une case
        else if(GameManager.Instance.ChooseTileForEvent){
            if(_tile != null){
                GameManager.Instance.AddTileToList(_tile);
            }
        }

        //lorsque le joueur peut cliquer sur les unités normalement
        else{
            //Si le mouvement n'a pas été lancé
            if(GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ1 || GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ2)
            {
                if(!Mouvement.Instance.Selected && !Attaque.Instance.Selected)
                {
                    if(_unitInTile != null)
                    {
                        if(CanUseUnitWhenClic(_unitInTile.GetComponent<UnitScript>()))
                        {
                            _actualTileSelected = _tile;
                            _actualUnitSelected = _actualTileSelected.GetComponent<TileScript>().Unit;
                            _menuForUnit.GetComponent<MenuActionUnite>().ShowPanel();
                        }
                    }
                }

                //Si le mouvement a été lancé
                else if(Mouvement.Instance.Selected)
                {
                    if(Mouvement.Instance.IsInMouvement && !Mouvement.Instance.MvmtRunning)
                    {
                        if(_tile != _actualTileSelected)
                        {
                            Mouvement.Instance.AddMouvement(TilesManager.Instance.TileList.IndexOf(_tile));
                        }
                        else
                        {
                            Mouvement.Instance.StopMouvement(true);
                            UIInstance.Instance.ActivationUnitPanel.CloseMovementPanel();
                        }
                    }
                }
                else if(Attaque.Instance.Selected)
                {
                    if(Attaque.Instance.IsInAttack)
                    {
                        if(_tile != _actualTileSelected)
                        {
                            //Clique sur une unité
                        }
                        else
                        {
                            Attaque.Instance.StopAttack();
                        }
                    }
                }
            }
            else if(GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.OrgoneJ1 || GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.OrgoneJ2){
                if(GameManager.Instance.IsPlayerRedTurn && _tile == PlayerScript.Instance.RedPlayerInfos.TileCentreZoneOrgone)
                {
                    OrgoneManager.Instance.StartToMoveZone();
                }
                else if(!GameManager.Instance.IsPlayerRedTurn && _tile == PlayerScript.Instance.BluePlayerInfos.TileCentreZoneOrgone)
                {
                    OrgoneManager.Instance.StartToMoveZone();
                }
            }
        }
    }

    /// <summary>
    /// Est ce que l'unité qui a été cliquée fait partie de l'armée
    /// </summary>
    /// <param name="uniTouch"></param>
    /// <returns></returns>
    bool CanUseUnitWhenClic(UnitScript uniTouch)
    {
        if(uniTouch.IsActivationDone == false)
        {
            if(GameManager.Instance.IsPlayerRedTurn)
            {
                if(!PlayerStatic.CheckIsUnitArmy(uniTouch, true) && !uniTouch.UnitStatus.Contains(MYthsAndSteel_Enum.UnitStatut.Possédé))
                {
                    return false;
                }
            }
            else
            {
                if(!PlayerStatic.CheckIsUnitArmy(uniTouch, false) && !uniTouch.UnitStatus.Contains(MYthsAndSteel_Enum.UnitStatut.Possédé))
                {
                    return false;
                }
            }


            if(GameManager.Instance.ActualTurnPhase != MYthsAndSteel_Enum.PhaseDeJeu.ActionJ1 && GameManager.Instance.ActualTurnPhase != MYthsAndSteel_Enum.PhaseDeJeu.ActionJ2)
            {
                return false;
            }

            bool isDeactivate = false;
            foreach(MYthsAndSteel_Enum.TypeUnite type in PlayerScript.Instance.DisactivateUnitType)
            {
                if(uniTouch.UnitSO.typeUnite == type)
                {
                    isDeactivate = true;
                }
            }

            if(isDeactivate)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Permet d'obtenir les objets touchés par le raycast
    /// </summary>
    /// <returns></returns>
    public RaycastHit2D GetRaycastHit()
    {
        Vector2 mouseDirection = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        Ray2D ray = new Ray2D(Camera.main.ScreenToWorldPoint(Input.mousePosition), mouseDirection);
        return Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, _layerM);
    }

    public void CallMouseCommand(){
        _mouseCommand.buttonAction(UIInstance.Instance.PageButton);
        _mouseCommand.MouseExitWithoutClick();
    }
}