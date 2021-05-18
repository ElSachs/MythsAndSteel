using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeBluePlayer : ChargeOrgone
{


    public override void ChargeOrgone1(int cost)
    {
        if (MythsAndSteel.Orgone.OrgoneCheck.CanUseOrgonePower(1, 2))
        {
            List<GameObject> unitList = new List<GameObject>();

            unitList.AddRange(PlayerScript.Instance.UnitRef.UnitListBluePlayer);

                GameManager.Instance.StartEventModeUnit(1, true, unitList, "Charge d'orgone 1", "�tes-vous sur de vouloir augmenter d'1 les d�g�ts de cete unit�?");
                GameManager.Instance._eventCall += UseChargeOrgone1;
            
        }
    }

    void UseChargeOrgone1()
    {
        UIInstance.Instance.ActivateNextPhaseButton();

        foreach (GameObject unit in GameManager.Instance.UnitChooseList)
        {
            unit.GetComponent<UnitScript>().AddDamageToUnit(1);
        }

        GameManager.Instance.UnitChooseList.Clear();
    }
    
    public override void ChargeOrgone3(int cost)
    {
        if (MythsAndSteel.Orgone.OrgoneCheck.CanUseOrgonePower(3, 2))
        {
            List<GameObject> tileList = new List<GameObject>();
            tileList.AddRange(TilesManager.Instance.ResourcesList);

            GameManager.Instance.StartEventModeTiles(1, true, tileList, "Charge d'orgone 3", "�tes-vous sur de vouloir voler une Ressources sur cette case?");
            GameManager.Instance._eventCall += UseChargeOrgone3;
        }
    }
    void UseChargeOrgone3()
    {
        UIInstance.Instance.ActivateNextPhaseButton();

        foreach (GameObject gam in GameManager.Instance.TileChooseList)
        {
            gam.GetComponent<TileScript>().RemoveRessources(1, 2);
        }

        GameManager.Instance.TileChooseList.Clear();
    }

    #region Charge 5 D'orgone
    public override void ChargeOrgone5(int cost)
    {
        if (MythsAndSteel.Orgone.OrgoneCheck.CanUseOrgonePower(4, 2))
        {
            OrgoneManager.Instance.ischarge5Blue = true;
            Debug.Log("B5");
            GameManager.Instance._eventCall += UseCharge5BluePlayer;
            GameManager.Instance.StartEventModeUnit(1, false, PlayerScript.Instance.UnitRef.UnitListBluePlayer, "Charge 3 d'orgone Bleu", "�tes vous s�r de vouloir d'utiliser la charge 3 d'orgone ?");
        }
    }

    void UseCharge5BluePlayer()
    {
        Debug.Log("UseCharge");
        List<GameObject> SelectTileList = new List<GameObject>();
        foreach (GameObject gam in TilesManager.Instance.TileList)
        {
            TileScript tilescript = gam.GetComponent<TileScript>();
            Debug.Log(tilescript.TileId);
            if (tilescript.Unit == null)
            {
                Debug.Log("Unit est null");
                foreach (MYthsAndSteel_Enum.TerrainType i in tilescript.TerrainEffectList)
                {
                    if (i != MYthsAndSteel_Enum.TerrainType.Point_de_ressource && i != MYthsAndSteel_Enum.TerrainType.Point_Objectif && i != MYthsAndSteel_Enum.TerrainType.UsineBleu && i != MYthsAndSteel_Enum.TerrainType.UsineRouge)
                    {
                        SelectTileList.Add(gam);
                        Debug.Log("break");
                        break;
                    }
                }
            }
        }
        GameManager.Instance._eventCall -= UseCharge5BluePlayer;
        //if (GameManager.Instance._eventCall == null) Debug.Log("Call null"); else Debug.Log("Call non null");
        GameManager.Instance.StartEventModeTiles(1, false, SelectTileList, "Tile de tp", "Etes vous sur de valid� cette case de tp?");
        //if (GameManager.Instance._eventCall == null) Debug.Log("Call null"); else Debug.Log("Call non null");
        GameManager.Instance._eventCall += DoneCharge5Blueplayer;
    }
    void DoneCharge5Blueplayer()
    {
        Debug.Log("Tu as atin le bonheur");
        GameManager.Instance.UnitChooseList[0].GetComponent<UnitScript>().ActualTiledId = GameManager.Instance.TileChooseList[0].GetComponent<TileScript>().TileId;
        GameManager.Instance.StopEventModeTile();

    }
    #endregion

}
