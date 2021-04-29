using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;

public class Transport_UnitScript : UnitScript
{
    [Header("--------------- SCRIPTABLE TRANSPORT ---------------")]
    //Scriptable qui contient les statistiques propres au transport
    [SerializeField] Transport_Unit_SO _transportUnitSo;
    public Transport_Unit_SO TransportUnitSo => _transportUnitSo;

    [Header("CONCERNE LE TRANSPORT")]

    [Header("Liste des unit�s transport�s")]
    // 1. Liste contenant les unit�s transport�es par l'unit�.
    [SerializeField] private List<GameObject> _unitTransport = null;
    public List<GameObject> UnitTransport => _unitTransport;

    [SerializeField] private int _transportMaxCapacity;
    public int TransportMaxCapacity => _transportMaxCapacity;

    // 2. Concerne les tiles adjacentes � l'unit� de transport.
    [SerializeField] private List<GameObject> _createTileTransport;
    public List<GameObject> CreateTileTransport => _createTileTransport;

    //UI du nombre d'unit� transport�.
    SpriteRenderer CurrentTransportUnit;

    //Boolean pour monter 

    private GameObject notch;


    public void Start()
    {
        //AddUnitToTransportList();
        NumberOfInstantiate(_transportMaxCapacity);
        //Permet de d�finir si l'unit� fait 

        //DeployUnitTransport();

        //UpdateStatsInstantiate();
    }

    //Cr�er la liste des tiles autour de l'unit� de transport.
    public void CreateTileForAddingUnit()
    {
        int typeTileID = ActualTiledId;
        foreach (int idtyleIndex in PlayerStatic.GetNeighbourDiag(typeTileID, TilesManager.Instance.TileList[typeTileID].GetComponent<TileScript>().Line, false))
        {
            GameObject tile = TilesManager.Instance.TileList[idtyleIndex];
            //Si le num�ro n'est pas pr�sent dans la liste, tu l'ajoutes.
            if (!_createTileTransport.Contains(tile))
            {
                _createTileTransport.Add(tile);
                if (CheckConditions(tile) == true)
                {
                    _createTileTransport.Remove(tile);
                }

            }
        }
    }

    /// <summary>
    /// Permet d'ajouter une unit� dans la liste des transports
    /// </summary>
    void AddUnitToTransportList()
    {
        CreateTileForAddingUnit();
        foreach (GameObject tile in _createTileTransport)
        {
            if (!TransportUnitSo.UnitTransport.Contains(tile.GetComponent<TileScript>().Unit) && tile.GetComponent<TileScript>().Unit != null)
            {
                UnitTransport.Add(tile.GetComponent<TileScript>().Unit);
                tile.GetComponent<TileScript>().Unit.SetActive(false);
                tile.GetComponent<TileScript>().RemoveUnitFromTile();
            }
        }
        GameManager.Instance._eventCall -= AddUnitToTransportList;
    }


    void DeployUnitTransport()
    {
        CreateTileForAddingUnit();
        if (0 < UnitTransport.Count)
        {
            foreach (GameObject tile in _createTileTransport)
            {
                if (_unitTransport.Count == 1)
                {
                    Debug.Log("Je suis ici");
                    tile.GetComponent<TileScript>().AddUnitToTile(_unitTransport[0]);
                    tile.GetComponent<TileScript>().Unit.SetActive(true);
                    _unitTransport.Remove(_unitTransport[0]);
                    Debug.Log(_unitTransport.Count);
                    break;
                }
                if (_unitTransport.Count == 2)
                {
                    Debug.Log("Je suis ici");
                    tile.GetComponent<TileScript>().AddUnitToTile(_unitTransport[1]);
                    tile.GetComponent<TileScript>().Unit.SetActive(true);
                    _unitTransport.Remove(_unitTransport[1]);
                    Debug.Log(_unitTransport.Count);
                    break;
                }

                if (_unitTransport.Count == 3)
                {
                    tile.GetComponent<TileScript>().AddUnitToTile(_unitTransport[2]);
                    tile.GetComponent<TileScript>().Unit.SetActive(true);
                    _unitTransport.Remove(_unitTransport[2]);
                    Debug.Log(_unitTransport.Count);
                    break;
                }

                if (_unitTransport.Count == 4)
                {
                    tile.GetComponent<TileScript>().AddUnitToTile(_unitTransport[3]);
                    tile.GetComponent<TileScript>().Unit.SetActive(true);
                    _unitTransport.Remove(_unitTransport[3]);
                    Debug.Log(_unitTransport.Count);
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Cette fonction va permettre de mettre un sprite d'une liste trouv� � partir d'un index � un objet. C'est la fonction qui met � jour l'affichage des slots vides et plein du menu transport. 
    /// </summary>

    public void ClearList(List<GameObject> nameList)
    {
        nameList.Clear();
    }

    /// <summary>
    /// Permet d'instancier et d'update les statistiques des IconUI
    /// </summary>
    public void NumberOfInstantiate(int numberOfUnitTransport)
    {
        GameObject TransportIconUI1 = Instantiate(UIInstance.Instance.EmplacementIconTopUnitTransport._emplacementTransport[0], gameObject.transform);
        if (_unitTransport.Count >= 1)
        {
            UpdateIconStats(UIInstance.Instance.EmplacementIconTopUnitTransport._spriteTransport, 1, UIInstance.Instance.EmplacementIconTopUnitTransport._emplacementTransport[0].GetComponent<SpriteRenderer>());
        }
        else
        {
            UpdateIconStats(UIInstance.Instance.EmplacementIconTopUnitTransport._spriteTransport, 0, UIInstance.Instance.EmplacementIconTopUnitTransport._emplacementTransport[0].GetComponent<SpriteRenderer>());
        }

        if (numberOfUnitTransport >= 2)
        {
            if (_unitTransport.Count >= 2)
            {
                UpdateIconStats(UIInstance.Instance.EmplacementIconTopUnitTransport._spriteTransport, 1, UIInstance.Instance.EmplacementIconTopUnitTransport._emplacementTransport[1].GetComponent<SpriteRenderer>());
                GameObject TransportIconUI2 = Instantiate(UIInstance.Instance.EmplacementIconTopUnitTransport._emplacementTransport[1], gameObject.transform);
            }
            else
            {
                UpdateIconStats(UIInstance.Instance.EmplacementIconTopUnitTransport._spriteTransport, 0, UIInstance.Instance.EmplacementIconTopUnitTransport._emplacementTransport[1].GetComponent<SpriteRenderer>());
                GameObject TransportIconUI2 = Instantiate(UIInstance.Instance.EmplacementIconTopUnitTransport._emplacementTransport[1], gameObject.transform);
            }

            if (numberOfUnitTransport >= 3)
            {
                if (_unitTransport.Count >= 3)
                {
                    UpdateIconStats(UIInstance.Instance.EmplacementIconTopUnitTransport._spriteTransport, 1, UIInstance.Instance.EmplacementIconTopUnitTransport._emplacementTransport[2].GetComponent<SpriteRenderer>());
                    GameObject TransportIconUI2 = Instantiate(UIInstance.Instance.EmplacementIconTopUnitTransport._emplacementTransport[2], gameObject.transform);
                }
                else
                {
                    UpdateIconStats(UIInstance.Instance.EmplacementIconTopUnitTransport._spriteTransport, 0, UIInstance.Instance.EmplacementIconTopUnitTransport._emplacementTransport[2].GetComponent<SpriteRenderer>());
                    GameObject TransportIconUI2 = Instantiate(UIInstance.Instance.EmplacementIconTopUnitTransport._emplacementTransport[2], gameObject.transform);
                }

                if (numberOfUnitTransport >= 4)
                {
                    Debug.Log("je me suis update gentillement");
                    if (_unitTransport.Count >= 4)
                    {
                        UpdateIconStats(UIInstance.Instance.EmplacementIconTopUnitTransport._spriteTransport, 1, UIInstance.Instance.EmplacementIconTopUnitTransport._emplacementTransport[3].GetComponent<SpriteRenderer>());
                        GameObject TransportIconUI2 = Instantiate(UIInstance.Instance.EmplacementIconTopUnitTransport._emplacementTransport[3], gameObject.transform);
                    }
                    else
                    {
                        UpdateIconStats(UIInstance.Instance.EmplacementIconTopUnitTransport._spriteTransport, 0, UIInstance.Instance.EmplacementIconTopUnitTransport._emplacementTransport[3].GetComponent<SpriteRenderer>());
                        GameObject TransportIconUI2 = Instantiate(UIInstance.Instance.EmplacementIconTopUnitTransport._emplacementTransport[3], gameObject.transform);
                    }
                }
            }
        }
    }

    public void UpdateIconStats(Sprite[] listSprite, int transportID, SpriteRenderer CurrentSpriteTransportUI)
    {
        CurrentSpriteTransportUI.sprite = listSprite[transportID];
    }

    public override void UpdateUnitStat()
    {
        _transportMaxCapacity = _transportUnitSo.MaxCapacityTransport;
    }

    public bool CheckConditions(GameObject tileCheck)
    {
        if (tileCheck.GetComponent<TileScript>().Unit != null)
        {
            return true;
        }

        if (tileCheck.GetComponent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Ravin)
    || tileCheck.GetComponent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Eau))
        {
            return true;
        }
        return false;
    }
}