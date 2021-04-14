using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouvement : MonoSingleton<Mouvement>
{
    #region ListeTile&Id
    [Header("LISTES DES CASES")]
    [SerializeField] private int[] neighbourValue; // +1 +9 +10...

    [SerializeField] private List<int> newNeighbourId = new List<int>(); // Voisins atteignables avec le range de l'unité.
    public List<int> _selectedTileId => selectedTileId;

    [SerializeField] private List<int> selectedTileId = new List<int>(); // Cases selectionnées par le joueur.
    public List<int> _newNeighbourId => newNeighbourId;
    private List<int> temp = new List<int>(); //

    #endregion
    #region RenduDeplacement
    [SerializeField] private float speed = 1; // Speed de déplacement de l'unité 
    float speed1; // speed de base.
    private GameObject mStart; // mT Start. 
    private GameObject mEnd; // mT End.
    private GameObject mUnit; // mT Unité.
    int MvmtIndex = 1; // Numéro du mvmt actuel dans la liste selectedTileId;
    [SerializeField] bool Launch = false; // Evite les répétitions dans updatingmove();
    #endregion
    #region InfoUnit

    //Déplacement restant de l'unité au départ
    int MoveLeftBase = 0;

    [Header("INFOS DE L UNITE")]
    //Est ce que l'unité a commencé à choisir son déplacement
    [SerializeField] private bool _isInMouvement;
    public bool IsInMouvement
    {
        get
        {
            return _isInMouvement;
        }
        set
        {
            _isInMouvement = value;
        }
    }

    //Est ce qu'une unité est sélectionnée
    [SerializeField] private bool _selected;
    public bool Selected
    {
        get
        {
            return _selected;
        }
        set
        {
            _selected = value;
        }
    }
    private bool RouteBonus = false; // Le bonus de route a-t-il été séléctionné ?
    private bool check = false; // Si une condition bloquant le mvmt de l'unité a été détéctée.
    // Mouvement en cours de traitement ?
    [SerializeField] private bool _mvmtRunning = false;
    public bool MvmtRunning => _mvmtRunning;
    #endregion InfoUnit
    #region RenduSpriteTile
    [Header("SPRITES POUR LES CASES")]
    [SerializeField] private Sprite _tileSprite = null;
    [SerializeField] private Sprite _selectedSprite = null;
    public Sprite selectedSprite
    {
        get
        {
            return _selectedSprite;
        }
    }
    [SerializeField] private List<MYthsAndSteel_Enum.TerrainType> EffectToCheck;

    #endregion RenduSpriteTile

    private void Update()
    {
        // Permet d'effectuer le moveTowards de l'unité à sa prochaine case.
        UpdatingMove(mUnit, mStart, mEnd);
    }

    /// <summary>
    /// Cette fonction "highlight" les cases atteignables par l'unité sur la case sélectionnée.
    /// </summary>
    /// <param name="tileId">Tile centrale</param>
    /// <param name="Range">Range de l'unité</param>
    public void Highlight(int tileId, int Range, int lasttileId)
    {
        if (Range > 0)
        {
            foreach (int ID in PlayerStatic.GetNeighbourDiag(tileId, TilesManager.Instance.TileList[tileId].GetComponent<TileScript>().Line, false))
            {
                if(ID == lasttileId) { continue; }
                TileScript TileSc = TilesManager.Instance.TileList[ID].GetComponent<TileScript>();
                bool i = false;
                if (GameManager.Instance.IsPlayerRedTurn)
                {
                    if (TilesManager.Instance.TileList[ID].GetComponent<TileScript>().Unit != null)
                    {
                        if (!TilesManager.Instance.TileList[ID].GetComponent<TileScript>().Unit.GetComponent<UnitScript>().UnitSO.IsInRedArmy)
                        {
                            i = true;
                        }
                        if (TilesManager.Instance.TileList[ID].GetComponent<TileScript>().Unit.GetComponent<UnitScript>().UnitSO.IsInRedArmy && Range == 1)
                        {
                            i = true;
                        }
                    }
                }
                if (!GameManager.Instance.IsPlayerRedTurn)
                {
                    if (TilesManager.Instance.TileList[ID].GetComponent<TileScript>().Unit != null)
                    {
                        if (TilesManager.Instance.TileList[ID].GetComponent<TileScript>().Unit.GetComponent<UnitScript>().UnitSO.IsInRedArmy)
                        {
                            i = true;
                        }
                        if (!TilesManager.Instance.TileList[ID].GetComponent<TileScript>().Unit.GetComponent<UnitScript>().UnitSO.IsInRedArmy && Range == 1)
                        {
                            i = true;
                        }
                    }
                }
                foreach (MYthsAndSteel_Enum.TerrainType Type in TileSc.TerrainEffectList)
                {
                    if (EffectToCheck.Contains(Type))
                    {
                        if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Ravin, ID) || PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Eau, ID))
                        {
                            i = true;
                            break;
                        }
                        if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Rivière_Est, tileId) && PlayerStatic.CheckDirection(tileId, ID) == "Est" && !PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Pont_Est, tileId))
                        {
                            i = true;
                            break;
                        }
                        if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Rivière_Nord, tileId) && PlayerStatic.CheckDirection(tileId, ID) == "Nord" && !PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Pont_Nord, tileId))
                        {
                            i = true;
                            break;
                        }
                        if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Rivière_Sud, tileId) && PlayerStatic.CheckDirection(tileId, ID) == "Sud" && !PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Pont_Sud, tileId))
                        {
                            i = true;
                            break;
                        }
                        if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Rivière_Ouest, tileId) && PlayerStatic.CheckDirection(tileId, ID) == "Ouest" && !PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Pont_Ouest, tileId))
                        {
                            i = true;
                            break;
                        }
                        if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Mont, ID) || PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Forêt, ID))
                        {
                            if (Range >= 2 && !i)
                            {
                                i = true;
                                TilesManager.Instance.TileList[ID].GetComponent<TileScript>().ActiveChildObj(MYthsAndSteel_Enum.ChildTileType.MoveSelect ,_selectedSprite);
                                if (!newNeighbourId.Contains(ID))
                                {
                                    newNeighbourId.Add(ID);
                                }
                                Highlight(ID, Range - 2, tileId);
                                break;
                            }
                            else
                            {
                                i = true;
                                break;
                            }
                        }
                    }
                }
                if (!i)
                {
                    TilesManager.Instance.TileList[ID].GetComponent<TileScript>().ActiveChildObj(MYthsAndSteel_Enum.ChildTileType.MoveSelect, _selectedSprite);
                    if (!newNeighbourId.Contains(ID))
                    {
                        newNeighbourId.Add(ID);
                    }                        
                    Highlight(ID, Range - 1, tileId);
                }
            }
        }
        // Si il s'agit d'une route et que la range est de 0.
        if(PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Route, tileId) && Range == 0)
        {
            foreach (int ID in PlayerStatic.GetNeighbourDiag(tileId, TilesManager.Instance.TileList[tileId].GetComponent<TileScript>().Line, false))
            {
                if (ID == lasttileId) { continue; }
                TileScript TileSc = TilesManager.Instance.TileList[ID].GetComponent<TileScript>();
                bool i = false;
                if (TilesManager.Instance.TileList[ID].GetComponent<TileScript>().Unit != null)
                {
                    i = true;
                }
                foreach (MYthsAndSteel_Enum.TerrainType Type in TileSc.TerrainEffectList)
                {
                    if (EffectToCheck.Contains(Type))
                    {
                        if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Ravin, ID) || PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Eau, ID))
                        {
                            i = true;
                            break;
                        }
                        if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Rivière_Est, tileId) && PlayerStatic.CheckDirection(tileId, ID) == "Est" && !PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Pont_Est, tileId))
                        {
                            i = true;
                            break;
                        }
                        if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Rivière_Nord, tileId) && PlayerStatic.CheckDirection(tileId, ID) == "Nord" && !PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Pont_Nord, tileId))
                        {
                            i = true;
                            break;
                        }
                        if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Rivière_Sud, tileId) && PlayerStatic.CheckDirection(tileId, ID) == "Sud" && !PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Pont_Sud, tileId))
                        {
                            i = true;
                            break;
                        }
                        if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Rivière_Ouest, tileId) && PlayerStatic.CheckDirection(tileId, ID) == "Ouest" && !PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Pont_Ouest, tileId))
                        {
                            i = true;
                            break;
                        }
                        if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Mont, ID) || PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Forêt, ID))
                        {
                            i = true;
                        }
                    }
                }
                if (!i)
                {
                    TilesManager.Instance.TileList[ID].GetComponent<TileScript>().ActiveChildObj(MYthsAndSteel_Enum.ChildTileType.MoveSelect, _selectedSprite);
                    if (!newNeighbourId.Contains(ID))
                    {
                        newNeighbourId.Add(ID);
                    }
                    Highlight(ID, -1, tileId);
                }
            }
        }
        // Si il s'agit d'une route et que la range est de 1.
        if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Route, tileId) && Range == 1)
        {
            foreach (int ID in PlayerStatic.GetNeighbourDiag(tileId, TilesManager.Instance.TileList[tileId].GetComponent<TileScript>().Line, false))
            {
                if (ID == lasttileId) { continue; }
                TileScript TileSc = TilesManager.Instance.TileList[ID].GetComponent<TileScript>();
                bool i = false;
                if (TilesManager.Instance.TileList[ID].GetComponent<TileScript>().Unit != null)
                {
                    i = true;
                }
                foreach (MYthsAndSteel_Enum.TerrainType Type in TileSc.TerrainEffectList)
                {
                    if (EffectToCheck.Contains(Type))
                    {
                        if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Ravin, ID) || PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Eau, ID))
                        {
                            i = true;
                            break;
                        }
                        if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Rivière_Est, tileId) && PlayerStatic.CheckDirection(tileId, ID) == "Est" && !PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Pont_Est, tileId))
                        {
                            i = true;
                            break;
                        }
                        if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Rivière_Nord, tileId) && PlayerStatic.CheckDirection(tileId, ID) == "Nord" && !PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Pont_Nord, tileId))
                        {
                            i = true;
                            break;
                        }
                        if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Rivière_Sud, tileId) && PlayerStatic.CheckDirection(tileId, ID) == "Sud" && !PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Pont_Sud, tileId))
                        {
                            i = true;
                            break;
                        }
                        if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Rivière_Ouest, tileId) && PlayerStatic.CheckDirection(tileId, ID) == "Ouest" && !PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Pont_Ouest, tileId))
                        {
                            i = true;
                            break;
                        }
                    }
                }
                if (!i)
                {
                    TilesManager.Instance.TileList[ID].GetComponent<TileScript>().ActiveChildObj(MYthsAndSteel_Enum.ChildTileType.MoveSelect, _selectedSprite);
                    if (!newNeighbourId.Contains(ID))
                    {
                        newNeighbourId.Add(ID);
                    }
                    Highlight(ID, -1, lasttileId);
                }
            }
        }
    }

    /// <summary>
    /// Lance le mouvement d'une unité avec une range défini.
    /// </summary>
    /// <param name="tileId">Tile de l'unité</param>
    /// <param name="Range">Mvmt de l'unité</param>
    public void StartMvmtForSelectedUnit()
    {
        GameObject tileSelected = RaycastManager.Instance.ActualTileSelected;

        if (tileSelected != null)
        {
            mUnit = tileSelected.GetComponent<TileScript>().Unit;
            if (!mUnit.GetComponent<UnitScript>().IsMoveDone)
            {
                MoveLeftBase = mUnit.GetComponent<UnitScript>().MoveLeft;
                StartMouvement(TilesManager.Instance.TileList.IndexOf(tileSelected), mUnit.GetComponent<UnitScript>().MoveSpeed - (mUnit.GetComponent<UnitScript>().MoveSpeed - MoveLeftBase) + mUnit.GetComponent<UnitScript>().MoveSpeedBonus);
            }
            else
            {
                _selected = false;
            }
        }
        else
        {
            _selected = false;
        }
    }

    /// <summary>
    /// Lance le mvmt d'une unité séléctionnée avec sa range.
    /// </summary>
    /// <param name="tileId"></param>
    /// <param name="Range"></param>
    public void StartMouvement(int tileId, int Range)
    {
        if (!_mvmtRunning && !_isInMouvement)
        {
            _isInMouvement = true;
            selectedTileId.Add(tileId);
            List<int> ID = new List<int>();
            ID.Add(tileId);

            // Lance l'highlight des cases dans la range.
            Highlight(tileId, Range, tileId);
            UIInstance.Instance.DesactivateNextPhaseButton();
        }
    }

    /// <summary>
    /// Arête le Mouvement pour l'unité selectionnée (menu, cases highlights...)
    /// </summary>
    public void StopMouvement(bool forceStop)
    {
        if(newNeighbourId.Count > 0)
        {
            foreach(int Neighbour in newNeighbourId) // Supprime toutes les tiles.
            {
                if(TilesManager.Instance.TileList[Neighbour] != null && TilesManager.Instance.TileList[Neighbour].GetComponent<TileScript>()._Child.Count != 0)
                {
                    TilesManager.Instance.TileList[Neighbour].GetComponent<TileScript>().DesActiveChildObj(MYthsAndSteel_Enum.ChildTileType.MoveSelect);
                }
            }
        }

        if(selectedTileId.Count > 1)
        {
            foreach(int NeighbourSelect in selectedTileId) // Si un path de mvmt était séléctionné.
            {
                if(TilesManager.Instance.TileList[NeighbourSelect] != null)
                {
                    TilesManager.Instance.TileList[NeighbourSelect].GetComponent<TileScript>().DesActiveChildObj(MYthsAndSteel_Enum.ChildTileType.MoveSelect);
                }
            }
        }

        // Clear de toutes les listes et stats.
        RouteBonus = false;
        selectedTileId.Clear();
        newNeighbourId.Clear();
        mStart = null;
        mEnd = null;
        _isInMouvement = false;
        _selected = false;

        mUnit.GetComponent<UnitScript>().MoveLeft = forceStop ? MoveLeftBase : mUnit.GetComponent<UnitScript>().MoveLeft;

        if(!forceStop) mUnit.GetComponent<UnitScript>().checkMovementLeft();

        mUnit = null;

        RaycastManager.Instance.ActualTileSelected = null;

        UIInstance.Instance.ActivateNextPhaseButton();

        _mvmtRunning = false;
    }

    /// <summary>
    /// Ajoute la tile à TileSelected. Pour le mvmt du joueur => Check egalement toutes les conditions de déplacement.
    /// </summary>
    /// <param name="tileId">Tile</param>
    public void AddMouvement(int tileId)
    {
        check = false;
        if (_isInMouvement)
        {
            if (newNeighbourId.Contains(tileId)) // Si cette case est dans la range de l'unité.
            {
                if (selectedTileId.Contains(tileId))
                {
                    // Supprime toutes les cases sélectionnées à partir de l'ID tileId.
                    for (int i = selectedTileId.IndexOf(tileId); i < selectedTileId.Count; i++)
                    {
                        if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Forêt, selectedTileId[i]) || PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Mont, selectedTileId[i]))
                        {
                            // Redistribution du Range à chaque suppression de case.
                            if (RouteBonus)
                            {
                                RouteBonus = false;
                                if (mUnit.GetComponent<UnitScript>().MoveLeft + 1 > mUnit.GetComponent<UnitScript>().UnitSO.MoveSpeed)
                                {
                                    mUnit.GetComponent<UnitScript>().MoveSpeedBonus += 1;
                                }
                                else
                                {
                                    mUnit.GetComponent<UnitScript>().MoveLeft += 1;
                                }
                            }
                            else
                            {
                                if (mUnit.GetComponent<UnitScript>().MoveLeft + 2 > mUnit.GetComponent<UnitScript>().UnitSO.MoveSpeed)
                                {
                                    int moveToAdd = 2 - (mUnit.GetComponent<UnitScript>().UnitSO.MoveSpeed - mUnit.GetComponent<UnitScript>().MoveLeft);
                                    mUnit.GetComponent<UnitScript>().MoveLeft = mUnit.GetComponent<UnitScript>().UnitSO.MoveSpeed;
                                    mUnit.GetComponent<UnitScript>().MoveSpeedBonus += moveToAdd;
                                }
                                else
                                {
                                    mUnit.GetComponent<UnitScript>().MoveLeft += 2;
                                }
                            }

                            temp.Add(selectedTileId[i]);
                            TilesManager.Instance.TileList[selectedTileId[i]].GetComponent<TileScript>().ActiveChildObj(MYthsAndSteel_Enum.ChildTileType.MoveSelect ,_selectedSprite); // Repasse les sprites en apparence "séléctionnable".
                        }
                        else
                        {
                            // Redistribution du Range à chaque suppression de case.
                            if (mUnit.GetComponent<UnitScript>().MoveLeft + 1 > mUnit.GetComponent<UnitScript>().UnitSO.MoveSpeed)
                            {
                                int moveToAdd = 1 - (mUnit.GetComponent<UnitScript>().UnitSO.MoveSpeed - mUnit.GetComponent<UnitScript>().MoveLeft);
                                mUnit.GetComponent<UnitScript>().MoveLeft = mUnit.GetComponent<UnitScript>().UnitSO.MoveSpeed;
                                mUnit.GetComponent<UnitScript>().MoveSpeedBonus += moveToAdd;
                            }
                            else
                            {
                                if (RouteBonus)
                                {
                                    RouteBonus = false;
                                }
                                else
                                {
                                    mUnit.GetComponent<UnitScript>().MoveLeft += 1;
                                }
                            }

                            temp.Add(selectedTileId[i]);
                            TilesManager.Instance.TileList[selectedTileId[i]].GetComponent<TileScript>().ActiveChildObj(MYthsAndSteel_Enum.ChildTileType.MoveSelect, _selectedSprite); // Repasse les sprites en apparence "séléctionnable".
                        }
                    }
                    foreach (int i in temp)
                    {
                        selectedTileId.Remove(i);
                    }
                    temp.Clear();

                }  // Si cette case est déjà selectionnée.
                else if (PlayerStatic.IsNeighbour(tileId, selectedTileId[selectedTileId.Count - 1], TilesManager.Instance.TileList[tileId].GetComponent<TileScript>().Line, false))
                {
                    // et qu'il reste du mvmt, on assigne la nouvelle case selectionnée à la liste SelectedTile.
                    if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Rivière_Est, tileId) && PlayerStatic.CheckDirection(tileId, selectedTileId[selectedTileId.Count - 1]) == "Est" && !PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Pont_Est, tileId))
                    {
                        check = true;
                    }
                    if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Rivière_Nord, tileId) && PlayerStatic.CheckDirection(tileId, selectedTileId[selectedTileId.Count - 1]) == "Nord" && !PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Pont_Nord, tileId))
                    {
                        check = true;
                    }
                    if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Rivière_Sud, tileId) && PlayerStatic.CheckDirection(tileId, selectedTileId[selectedTileId.Count - 1]) == "Sud" && !PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Pont_Sud, tileId))
                    {
                        check = true;
                    }
                    if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Rivière_Ouest, tileId) && PlayerStatic.CheckDirection(tileId, selectedTileId[selectedTileId.Count - 1]) == "Ouest" && !PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Pont_Ouest, tileId))
                    {
                        check = true;
                    }
                    if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Forêt, tileId) || PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Mont, tileId))
                    {
                        if (mUnit.GetComponent<UnitScript>().MoveLeft >= 2 && !check)
                        {
                            check = true;
                            mUnit.GetComponent<UnitScript>().MoveLeft -= 2; // sup 2 mvmt.
                            selectedTileId.Add(tileId);
                            TilesManager.Instance.TileList[tileId].GetComponent<TileScript>().ActiveChildObj(MYthsAndSteel_Enum.ChildTileType.MoveSelect, _tileSprite);
                        }
                        else if (mUnit.GetComponent<UnitScript>().MoveLeft + mUnit.GetComponent<UnitScript>().MoveSpeedBonus >= 2 && !check)
                        {
                            int moveToDecrease = 2;
                            moveToDecrease -= mUnit.GetComponent<UnitScript>().MoveLeft;
                            mUnit.GetComponent<UnitScript>().MoveLeft = 0;
                            mUnit.GetComponent<UnitScript>().MoveSpeedBonus -= moveToDecrease;
                        }
                        else if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Route, selectedTileId[selectedTileId.Count - 1]) && (mUnit.GetComponent<UnitScript>().MoveLeft + mUnit.GetComponent<UnitScript>().MoveSpeedBonus) == 1 && !RouteBonus && !check)
                        {
                            RouteBonus = true;
                            check = true;
                            if (mUnit.GetComponent<UnitScript>().MoveLeft > 0)
                            {
                                mUnit.GetComponent<UnitScript>().MoveLeft--;
                            }
                            else
                            {
                                mUnit.GetComponent<UnitScript>().MoveSpeedBonus--;
                            }
                            selectedTileId.Add(tileId);
                            TilesManager.Instance.TileList[tileId].GetComponent<TileScript>().ActiveChildObj(MYthsAndSteel_Enum.ChildTileType.MoveSelect, _tileSprite);
                        }
                        else
                        {
                            check = true;
                            Debug.Log("La tile d'ID : " + tileId + " est une foret ou un mont.");
                        }
                    }
                    if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Route, selectedTileId[selectedTileId.Count - 1]) && (mUnit.GetComponent<UnitScript>().MoveLeft + mUnit.GetComponent<UnitScript>().MoveSpeedBonus) == 0 && !RouteBonus)
                    {
                        RouteBonus = true;
                        check = true;
                        selectedTileId.Add(tileId);
                        TilesManager.Instance.TileList[tileId].GetComponent<TileScript>().ActiveChildObj(MYthsAndSteel_Enum.ChildTileType.MoveSelect, _tileSprite);
                    }
                    if (!check)
                    {
                        if (mUnit.GetComponent<UnitScript>().MoveLeft > 0)
                        {
                            mUnit.GetComponent<UnitScript>().MoveLeft--; // sup 1 mvmt.
                            selectedTileId.Add(tileId);
                            TilesManager.Instance.TileList[tileId].GetComponent<TileScript>().ActiveChildObj(MYthsAndSteel_Enum.ChildTileType.MoveSelect, _tileSprite);
                        }
                        else if (mUnit.GetComponent<UnitScript>().MoveSpeedBonus > 0)
                        {
                            mUnit.GetComponent<UnitScript>().MoveSpeedBonus--; // sup 1 mvmt.
                            selectedTileId.Add(tileId);
                            TilesManager.Instance.TileList[tileId].GetComponent<TileScript>().ActiveChildObj(MYthsAndSteel_Enum.ChildTileType.MoveSelect, _tileSprite);
                        }
                    }
                } // Sinon, si cette case est bien voisine de l'ancienne selection. 
                else // Sinon cette case est trop loin de l'ancienne seletion.
                {
                    Debug.Log("La tile d'ID : " + tileId + " est trop loin de la tile d'ID: " + selectedTileId[selectedTileId.Count - 1]);
                }
            }
            // Sinon cette case est hors de la range de l'unité.
            else
            {
                Debug.Log("La tile d'ID : " + tileId + " est trop loin de la tile d'ID: " + selectedTileId[selectedTileId.Count - 1]);
            }
        }
        if(selectedTileId.Count > 1)
        {
            UIInstance.Instance.ActivationUnitPanel.ShowMovementPanel();
        }
        else
        {
            UIInstance.Instance.ActivationUnitPanel.CloseMovementPanel();
        }
    }

    /// <summary>
    /// Détruit les enfants qui ne sont pas dans la liste de déplacement.
    /// </summary>
    public void DeleteChildWhenMove(){
        foreach(int Neighbour in newNeighbourId) // Supprime toutes les tiles.
        {
            if(TilesManager.Instance.TileList[Neighbour] != null && !_selectedTileId.Contains(Neighbour))
            {
                TilesManager.Instance.TileList[Neighbour].GetComponent<TileScript>().DesActiveChildObj(MYthsAndSteel_Enum.ChildTileType.MoveSelect);
            }
        }
    }

    /// <summary>
    /// Assigne le prochain mouvement demandé à l'unité. Change les stats de l'ancienne et de la nouvelle case. Actualise les informations de position de l'unité.
    /// </summary>
    public void ApplyMouvement()
    {
        //Ferme le panneau de déplacement
        UIInstance.Instance.ActivationUnitPanel.CloseMovementPanel();

        if (TilesManager.Instance.TileList[_selectedTileId[_selectedTileId.Count - 1]].GetComponent<TileScript>().Unit != null)
        {
            if (GameManager.Instance.IsPlayerRedTurn == TilesManager.Instance.TileList[_selectedTileId[_selectedTileId.Count - 1]].GetComponent<TileScript>().Unit.GetComponent<UnitScript>().UnitSO.IsInRedArmy)
            {
                UIInstance.Instance.ActivationUnitPanel.ShowMovementPanel();
                Debug.Log("Vous ne pouvez pas terminer votre mouvement sur une unité alliée.");
                return;
            }
        }

        GameObject tileSelected = RaycastManager.Instance.ActualTileSelected;

        if (tileSelected != null && (_selectedTileId.Count != 0 && _selectedTileId.Count != 1))
        {
            _mvmtRunning = true;
            mStart = tileSelected; // Assignation du nouveau départ.
            mEnd = TilesManager.Instance.TileList[selectedTileId[MvmtIndex]];  // Assignation du nouvel arrirée.

            mUnit.GetComponent<UnitScript>()._hasStartMove = true;

            foreach (int Neighbour in newNeighbourId) // Désactive toutes les cases selectionnées par la fonction Highlight.
            {
                if (!selectedTileId.Contains(Neighbour))
                {
                    TilesManager.Instance.TileList[Neighbour].GetComponent<TileScript>().DesActiveChildObj(MYthsAndSteel_Enum.ChildTileType.MoveSelect); // Assigne un sprite empty à toutes les anciennes cases "neighbour"
                }
            }
        }
    }

    /// <summary>
    /// Coroutine d'attente entre chaque case. Probablement pendant ce temps que l'on devra appliquer les effets de case.
    /// </summary>
    /// <returns>Temps à définir</returns>
    private IEnumerator MvmtEnd()
    {
        mEnd.GetComponent<TileScript>().DesActiveChildObj(MYthsAndSteel_Enum.ChildTileType.MoveSelect); // La case dépassée redevient une "empty"
        mEnd.GetComponent<TileScript>().AddUnitToTile(mStart.GetComponent<TileScript>().Unit); // L'unité de la case d'arrivée devient celle de la case de départ.
        mStart.GetComponent<TileScript>().RemoveUnitFromTile(); // L'ancienne case n'a plus d'unité.
        mUnit = mEnd.GetComponent<TileScript>().Unit;
        mUnit.GetComponent<UnitScript>().ActualTiledId = TilesManager.Instance.TileList.IndexOf(mEnd);
        RaycastManager.Instance.ActualTileSelected = mEnd;
        mStart = mEnd;
        mEnd = null;

        yield return new WaitForSeconds(.25f); // Temps d'attente.
        if (MvmtIndex < selectedTileId.Count - 1) // Si il reste des mvmts à effectuer dans la liste SelectedTile.
        {
            MvmtIndex++;
            ApplyMouvement();
        }
        else // Si il ne reste aucun mvmt dans la liste SelectedTile.
        {
            MvmtIndex = 1;
            StopMouvement(false); // Arête le mvmt de l'unité.
        }
        Launch = false; // Reset de la bool Launch
    }

    /// <summary>
    /// Cette fonction lance l'animation de translation de l'unité entre les cases.
    /// </summary>
    /// <param name="Unit">The unit gameobject.</param>
    /// <param name="StartPos">start position tile</param>
    /// <param name="EndPos">end position tile</param>
    private void UpdatingMove(GameObject Unit, GameObject StartPos, GameObject EndPos)
    {    
        if (Unit != null && StartPos != null && EndPos != null)
        {
            AnimationUpdate(Unit, EndPos);
            Unit.transform.position = Vector2.MoveTowards(Unit.transform.position, EndPos.transform.position, speed1); // Application du mvmt.
            speed1 = Mathf.Abs((Vector2.Distance(mUnit.transform.position, mEnd.transform.position) * speed * Time.deltaTime)); // Régulation de la vitesse. (effet de ralentissement) 
            if(Vector2.Distance(mUnit.transform.position, mEnd.transform.position) <= 0.05f && Launch == false) // Si l'unité est arrivée.
            {
                Unit.GetComponent<UnitScript>().Animation.SetFloat("X", 0);
                Unit.GetComponent<UnitScript>().Animation.SetFloat("Y", 0);
                Launch = true;
                StartCoroutine(MvmtEnd()); // Lancer le prochain mvmt avec délai. 
            }
            else // Sinon appliqué l'opacité à la case d'arrivée en fonction de la distance unité - arrivée.
            {
                for(int i = 0; i < mEnd.GetComponent<TileScript>()._Child.Count; i++)
                {
                    if(mEnd.GetComponent<TileScript>()._Child[i].tag == "Moveselectable")
                    {
                        mEnd.GetComponent<TileScript>()._Child[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, Vector2.Distance(mUnit.transform.position, mEnd.transform.position));
                    }
                }

            }
        }
    }

    /// <summary>
    /// Lance l'animation de déplacement de l'unité
    /// </summary>
    /// <param name="Unit"></param>
    /// <param name="EndPos"></param>
    private void AnimationUpdate(GameObject Unit, GameObject EndPos)
    {
        Unit.GetComponent<UnitScript>().Animation.SetFloat("X", EndPos.transform.position.x - Unit.transform.position.x);
        Unit.GetComponent<UnitScript>().Animation.SetFloat("Y", EndPos.transform.position.y - Unit.transform.position.y);
        Unit.GetComponent<SpriteRenderer>().flipX = Unit.GetComponent<UnitScript>().Animation.GetFloat("X") > 0;
    }
}