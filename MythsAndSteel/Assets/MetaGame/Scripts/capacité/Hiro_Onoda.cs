using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hiro_Onoda : Capacity
{
    public override void StartCpty()
    {
        List<GameObject> unitlist = new List<GameObject>();
        List<int> idTileNeigh = PlayerStatic.GetNeighbourDiag(gameObject.GetComponent<UnitScript>().ActualTiledId, TilesManager.Instance.TileList[gameObject.GetComponent<UnitScript>().ActualTiledId].GetComponent<TileScript>().Line, false);
        foreach (int gam in idTileNeigh)
        {
            if(TilesManager.Instance.TileList[gam].GetComponent<TileScript>().Unit != null)
            {
           UnitScript unit = TilesManager.Instance.TileList[gam].GetComponent<TileScript>().Unit.GetComponent<UnitScript>();
            if (unit.UnitSO.typeUnite == MYthsAndSteel_Enum.TypeUnite.Infanterie && unit.UnitSO.IsInRedArmy == GameManager.Instance.IsPlayerRedTurn)
            {
                unitlist.Add(unit.gameObject); 
            }

            }
        }
        Debug.Log(unitlist.Count);
        if(unitlist.Count>=2)
        {

        EventUnit(2, GameManager.Instance.IsPlayerRedTurn ? true : false , unitlist, "Regroupement Instantan�", "�tes-vous s�r de vouloir d�placer ces deux unit�s � c�t� de Hiro Onoda");
        GameManager.Instance._eventCall += UseCpty;
        }
    }

    void UseCpty()
    {
        List<GameObject> tileList = new List<GameObject>();
        List<int> unitNeigh = PlayerStatic.GetNeighbourDiag(GetComponent<UnitScript>().ActualTiledId, TilesManager.Instance.TileList[GetComponent<UnitScript>().ActualTiledId].GetComponent<TileScript>().Line, false);
        foreach (int i in unitNeigh)
        {
            tileList.Add(TilesManager.Instance.TileList[i]);
        }

        EventTile(2, GameManager.Instance.IsPlayerRedTurn , tileList, "Regroupement Instantan�", "�tes-vous sur de vouloir d�placer les unit�s sur ces case?", false);
        GameManager.Instance._eventCall += EndCpty;

    }

    public override void EndCpty()
    {
        for(int i=0; i < GameManager.Instance.UnitChooseList.Count; i++ )
        {
            GameManager.Instance.TileChooseList[i].GetComponent<TileScript>().AddUnitToTile(GameManager.Instance.UnitChooseList[i].gameObject);
        }
    }
}
