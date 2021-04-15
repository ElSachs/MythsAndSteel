using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenfortPhase : MonoBehaviour
{
    #region AppelDeScript
    Player player;
    UnitReference unitReference;
    #endregion

    [SerializeField] private List<GameObject> _createTileJ1;
    public List<GameObject> CreateTileJ1 => _createTileJ1;

    [SerializeField] private List<GameObject> _createTileJ2;
    public List<GameObject> CreateTileJ2 => _createTileJ2;

    [SerializeField] private List<GameObject> _createLeader1;
    public List<GameObject> CreateLeader1 => _createLeader1;

    [SerializeField] private List<GameObject> _createLeader2;
    public List<GameObject> CreateLeader2 => _createLeader2;

    [SerializeField] List<GameObject> _usineListRed = new List<GameObject>();
    [SerializeField] List<GameObject> _usineListBlue = new List<GameObject>();

    [SerializeField] List<GameObject> _leaderListRed = new List<GameObject>();
    [SerializeField] List<GameObject> _leaderListBlue = new List<GameObject>();

    int idCreate = -1;
    bool redPlayerCreation = false;

    private void Start()
    {
        foreach(GameObject typeTile in TilesManager.Instance.TileList)
        {
            if(typeTile.GetComponent<TileScript>().TerrainEffectList.Contains((MYthsAndSteel_Enum.TerrainType.UsineRouge)))
            {
                _usineListRed.Add(typeTile);
            }
            else if(typeTile.GetComponent<TileScript>().TerrainEffectList.Contains((MYthsAndSteel_Enum.TerrainType.UsineBleu)))
            {
                _usineListBlue.Add(typeTile);
            }
        }

        foreach(GameObject unit in PlayerScript.Instance.UnitRef.UnitListRedPlayer)
        {
            if(unit.GetComponent<UnitScript>().UnitSO.typeUnite == MYthsAndSteel_Enum.TypeUnite.Leader)
            {
                _leaderListRed.Add(unit);
            }
        }

        foreach(GameObject unit in PlayerScript.Instance.UnitRef.UnitListBluePlayer)
        {
            if(unit.GetComponent<UnitScript>().UnitSO.typeUnite == MYthsAndSteel_Enum.TypeUnite.Leader)
            {
                _leaderListBlue.Add(unit);
            }
        }
    }

    /// <summary>
    /// Fais une liste des cases s�lectionnables autour de l'usine
    /// </summary>
    public void CreateRenfort()
    {
        //A inverser si l'arm�e Bleu Devient l'arm�e Rouge
        if (RaycastManager.Instance.Tile.GetComponent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.UsineRouge))
        {
            AroundCreateTileUsine(true);
            AroundLeader(true);
        }

        //A inverser si l'arm�e Rouge Devient l'arm�e Bleu
        else if (RaycastManager.Instance.Tile.GetComponent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.UsineBleu))
        {
            AroundCreateTileUsine(false);
            AroundLeader(false);

        }
    }

    #region GetTiles
    void AroundCreateTileUsine(bool usine1)
    {
        //Si l'arm�e est jouer par l'arm�e Bleu.
        if(usine1 == true)
        {
            _createTileJ1 = CreateTileList(_usineListRed);
        }
        else
        {
            _createTileJ2 = CreateTileList(_usineListBlue);
        }
    }

    /// <summary>
    /// Obtient les cases s�lectionnables
    /// </summary>
    /// <param name="usineList"></param>
    /// <returns></returns>
    List<GameObject> CreateTileList(List<GameObject> usineList)
    {
        List<GameObject> tempList = new List<GameObject>();

        foreach(GameObject typeTile in usineList)
        {
            int typeTileID = int.Parse(TilesManager.Instance.TileList.IndexOf(typeTile).ToString());
            //Debug.Log(typeTile);
            foreach(int idtyleIndex in PlayerStatic.GetNeighbourDiag(typeTileID, TilesManager.Instance.TileList[typeTileID].GetComponent<TileScript>().Line, false))
            {
                //Tu ajoutes la tile correspondant � l'usineJ2
                if(!tempList.Contains(typeTile))
                {
                    if(typeTile.GetComponent<TileScript>().Unit == null)
                    {
                        tempList.Add(typeTile);
                    }
                }
                //Si le num�ro n'est pas pr�sent dans la liste, tu l'ajoutes.
                if(!tempList.Contains(TilesManager.Instance.TileList[idtyleIndex]))
                {
                    tempList.Add(TilesManager.Instance.TileList[idtyleIndex]);
                    //Si il y un boolean qui est retourn� alors tu enl�ves les �l�ments de la liste.
                    if(CheckConditions(TilesManager.Instance.TileList[idtyleIndex], typeTileID) == true)
                    {
                        tempList.Remove(TilesManager.Instance.TileList[idtyleIndex]);
                    }
                }
            }
        }

        //Pour chaque �l�ment dans la liste, tu ajoutes l'effet TileCr�able
        foreach(GameObject typeWithoutEffect in tempList)
        {
            if(!typeWithoutEffect.GetComponent<TileScript>().EffetProg.Contains(MYthsAndSteel_Enum.EffetProg.Zone_creable))
            {
                typeWithoutEffect.GetComponent<TileScript>().EffetProg.Add(MYthsAndSteel_Enum.EffetProg.Zone_creable);
            }
        }

        return tempList;
    }

    /// <summary>
    ///  Fais une liste des cases s�lectionnables autour des leader
    /// </summary>
    /// <param name="leaderArmy1"></param>
    void AroundLeader(bool leaderArmy1)
    {
        //Si l'arm�e est jouer par l'arm�e Bleu.
        if(leaderArmy1 == true)
        {
            foreach(GameObject unit in _leaderListRed)
            {
                int typeTileID = unit.GetComponent<UnitScript>().ActualTiledId;

                //Pour chaque num�ro pr�sent dans le PlayerStatic avec la valeur qu'on a convertit pr�c�demment.
                foreach(int idtyleIndex in PlayerStatic.GetNeighbourDiag(typeTileID, TilesManager.Instance.TileList[typeTileID].GetComponent<TileScript>().Line, false))
                {
                    GameObject tile = TilesManager.Instance.TileList[idtyleIndex];
                    //Si le num�ro n'est pas pr�sent dans la liste, tu l'ajoutes.
                    if(!_createLeader1.Contains(tile))
                    {
                        _createLeader1.Add(tile);

                        //Si il y un boolean qui est retourn� alors tu enl�ves les �l�ments de la liste.
                        if(CheckConditions(tile, typeTileID) == true)
                        {
                            _createLeader1.Remove(tile);
                        }
                    }
                }
            }
            //Pour chaque �l�ment dans la liste, tu ajoutes l'effet TileCr�able
            foreach(GameObject typeWithoutEffect in _createLeader1)
            {
                if(!typeWithoutEffect.GetComponent<TileScript>().EffetProg.Contains(MYthsAndSteel_Enum.EffetProg.Zone_creable))
                {
                    typeWithoutEffect.GetComponent<TileScript>().EffetProg.Add(MYthsAndSteel_Enum.EffetProg.Zone_creable);
                }
            }
        }


        else
        {
            foreach(GameObject unit in _leaderListBlue)
            {
                int typeTileID = unit.GetComponent<UnitScript>().ActualTiledId;

                //Evite d'avoir des doublons dans la List.
                Debug.Log(typeTileID);

                //Pour chaque num�ro pr�sent dans le PlayerStatic avec la valeur qu'on a convertit pr�c�demment.
                foreach(int idtyleIndex in PlayerStatic.GetNeighbourDiag(typeTileID, TilesManager.Instance.TileList[typeTileID].GetComponent<TileScript>().Line, false))
                {
                    GameObject tile = TilesManager.Instance.TileList[idtyleIndex];
                    //Si le num�ro n'est pas pr�sent dans la liste, tu l'ajoutes.
                    if(!_createLeader2.Contains(tile))
                    {
                        _createLeader2.Add(tile);

                        //Si il y un boolean qui est retourn� alors tu enl�ves les �l�ments de la liste.
                        if(CheckConditions(tile, typeTileID) == true)
                        {
                            _createLeader2.Remove(tile);
                        }
                    }
                }
            }
            //Pour chaque �l�ment dans la liste, tu ajoutes l'effet TileCr�able
            foreach(GameObject typeWithoutEffect in _createTileJ2)
            {
                if(!typeWithoutEffect.GetComponent<TileScript>().EffetProg.Contains(MYthsAndSteel_Enum.EffetProg.Zone_creable))
                {
                    typeWithoutEffect.GetComponent<TileScript>().EffetProg.Add(MYthsAndSteel_Enum.EffetProg.Zone_creable);
                }
            }
        }
    }
    #endregion GetTiles

    /// <summary>
    /// Permet de d�terminer si autour des cases adjacentes il y a des effets de terrain qui pourraient g�ner le d�ploiement de troupes.
    /// </summary>
    /// <param name="tileCheck"></param>
    /// <returns></returns>
    bool CheckConditions(GameObject tileCheck, int origin)
    {
        //Y a une unit�?
        if(tileCheck.GetComponent<TileScript>().Unit != null)
        {
            return true;
        }

        //Y a de l'eau ou un ravin?
        if(tileCheck.GetComponent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Ravin)
            || tileCheck.GetComponent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Eau))
        {
            return true;
        }

        //RIVIERE?
        if(tileCheck.GetComponent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Rivi�re_Nord)
        && PlayerStatic.CheckDirection(origin, tileCheck.GetComponent<TileScript>().TileId) == MYthsAndSteel_Enum.Direction.Sud)
        {
            if(!tileCheck.GetComponent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Pont_Nord))
            {
                return true;
            }
        }

        if(tileCheck.GetComponent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Rivi�re_Sud)
        && PlayerStatic.CheckDirection(origin, tileCheck.GetComponent<TileScript>().TileId) == MYthsAndSteel_Enum.Direction.Nord)
        {
            if(!tileCheck.GetComponent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Pont_Sud))
            {
                return true;
            }
        }

        if(tileCheck.GetComponent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Rivi�re_Ouest)
        && PlayerStatic.CheckDirection(origin, tileCheck.GetComponent<TileScript>().TileId) == MYthsAndSteel_Enum.Direction.Est)
        {
            if(!tileCheck.GetComponent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Rivi�re_Ouest))
            {
                return true;
            }
        }

        if(tileCheck.GetComponent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Rivi�re_Est)
        && PlayerStatic.CheckDirection(origin, tileCheck.GetComponent<TileScript>().TileId) == MYthsAndSteel_Enum.Direction.Ouest)
        {
            if(!tileCheck.GetComponent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Rivi�re_Est))
            {
                return true;
            }
        }

        return false;

        //Ajouter les barbelets.
    }

    #region Cr�erUnit�
    /// <summary>
    /// Permet de cr�er une unit�
    /// </summary>
    public void craftUnit(int unitId)
    {
        //Ins�rer la fonction magique permettant de s�lectionner les cases pour intanstier les troupes.
        //Les troupes sont en r�f�rance dans la liste de UnitR�f�rence _unitClassCreableListRedPlayer et _unitClassCreableListBluePlayer
        if(GameManager.Instance.IsPlayerRedTurn)
        {
            List<GameObject> tileList = new List<GameObject>();
            tileList.AddRange(GameManager.Instance.RenfortPhase.CreateLeader1);
            tileList.AddRange(GameManager.Instance.RenfortPhase.CreateTileJ1);

            idCreate = unitId;
            redPlayerCreation = true;

            GameManager.Instance.StartEventModeTiles(1, true, tileList, "Cr�ation d'unit�", "�tes-vous sur de vouloir cr�er une unit� sur cette case");
            GameManager.Instance._eventCall += CreateNewUnit;
        }
        else
        {
            List<GameObject> tileList = new List<GameObject>();
            tileList.AddRange(GameManager.Instance.RenfortPhase.CreateLeader2);
            tileList.AddRange(GameManager.Instance.RenfortPhase.CreateTileJ2);

            idCreate = unitId;
            redPlayerCreation = true;

            GameManager.Instance.StartEventModeTiles(1, false, tileList, "Cr�ation d'unit�", "�tes-vous sur de vouloir cr�er une unit� sur cette case");
            GameManager.Instance._eventCall += CreateNewUnit;
        }
    }

    /// <summary>
    /// Cr�e une nouvelle unit� sur le terrain au niveau de la tile s�lectionn�e
    /// </summary>
    void CreateNewUnit(){
        if(redPlayerCreation)
        {
            GameObject obj = Instantiate(PlayerScript.Instance.UnitRef.UnitClassCreableListRedPlayer[idCreate], GameManager.Instance.TileChooseList[0].transform.position, Quaternion.identity);
            GameManager.Instance.TileChooseList[0].GetComponent<TileScript>().AddUnitToTile(obj);
            GameManager.Instance._eventCall -= CreateNewUnit;
            PlayerScript.Instance.RedPlayerInfos.HasCreateUnit = true;
        }
        else
        {
            GameObject obj = Instantiate(PlayerScript.Instance.UnitRef.UnitClassCreableListBluePlayer[idCreate], GameManager.Instance.TileChooseList[0].transform.position, Quaternion.identity);
            GameManager.Instance.TileChooseList[0].GetComponent<TileScript>().AddUnitToTile(obj);
            GameManager.Instance._eventCall -= CreateNewUnit;
            PlayerScript.Instance.BluePlayerInfos.HasCreateUnit = true;
        }
    }
    #endregion Cr�erUnit�
}