using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarbelGestion : MonoBehaviour
{


    public enum Direction
    {
        Nord, Sud, Est, Ouest, Unknown
    }
    [SerializeField] private int BarbelDamage = 2;

    [SerializeField] private Direction Direct;

    public Sprite Horizontal;
    public Sprite Vertical;
    public List<Barbel> BarbelActive;

    public void CreateBarbel(int tileId)
    {
        List<MYthsAndSteel_Enum.TerrainType> T = TilesManager.Instance.TileList[tileId].GetComponent<TileScript>().TerrainEffectList;
        Direction D = Direction.Unknown;
        if (T.Contains(MYthsAndSteel_Enum.TerrainType.Barbel�_Est) || T.Contains(MYthsAndSteel_Enum.TerrainType.Barbel�_Nord) || T.Contains(MYthsAndSteel_Enum.TerrainType.Barbel�_Sud) || T.Contains(MYthsAndSteel_Enum.TerrainType.Barbel�_Ouest))
        {
            List<GameObject> G = TilesManager.Instance.TileList[tileId].GetComponent<TileScript>()._Child;
            foreach (GameObject Child in G)
            {
                if (Child.TryGetComponent<Barbel>(out Barbel B))
                {
                    if (B.Direc == Direct)
                    {
                        D = Direct;
                    }
                }
            }
            if (D != Direction.Unknown)
            {
                Delete(tileId, D);
            }
            else
            {
                switch (Direct)
                {
                    case Direction.Est:
                        if (tileId + 1 <= 80)
                        {
                            List<MYthsAndSteel_Enum.TerrainType> T2 = TilesManager.Instance.TileList[tileId + 1].GetComponent<TileScript>().TerrainEffectList;
                            if (T2.Contains(MYthsAndSteel_Enum.TerrainType.Barbel�_Ouest))
                            {
                                Delete(tileId + 1, Direction.Ouest);
                            }
                        }
                        break;
                    case Direction.Ouest:
                        if (tileId - 1 >= 0)
                        {
                            List<MYthsAndSteel_Enum.TerrainType> T2 = TilesManager.Instance.TileList[tileId - 1].GetComponent<TileScript>().TerrainEffectList;
                            if (T2.Contains(MYthsAndSteel_Enum.TerrainType.Barbel�_Est))
                            {
                                Delete(tileId - 1, Direction.Est);
                            }
                        }
                        break;
                    case Direction.Nord:
                        if (tileId + 9 <= 80)
                        {
                            List<MYthsAndSteel_Enum.TerrainType> T2 = TilesManager.Instance.TileList[tileId + 9].GetComponent<TileScript>().TerrainEffectList;
                            if (T2.Contains(MYthsAndSteel_Enum.TerrainType.Barbel�_Sud))
                            {
                                Delete(tileId + 9, Direction.Sud);
                            }
                        }
                        break;
                    case Direction.Sud:
                        if (tileId - 9 >= 0)
                        {
                            List<MYthsAndSteel_Enum.TerrainType> T2 = TilesManager.Instance.TileList[tileId - 9].GetComponent<TileScript>().TerrainEffectList;
                            if (T2.Contains(MYthsAndSteel_Enum.TerrainType.Barbel�_Nord))
                            {
                                Delete(tileId - 9, Direction.Nord);
                            }
                        }
                        break;
                }
            }
            BarbelCreationAfterVerification(tileId, Direct);
        }
        else
        {
            BarbelCreationAfterVerification(tileId, Direct);
        }
    }

    public void Delete(int tileId, Direction Direction)
    {
        TileScript TS = TilesManager.Instance.TileList[tileId].GetComponent<TileScript>();
        switch (Direction)
        {
            case Direction.Est: 
                TS.RemoveEffect(MYthsAndSteel_Enum.TerrainType.Barbel�_Est);
                if (tileId + 1 <= 80)
                {
                    TileScript TSE = TilesManager.Instance.TileList[tileId + 1].GetComponent<TileScript>();
                    TSE.RemoveEffect(MYthsAndSteel_Enum.TerrainType.Barbel�_Ouest);
                }
                break;
            case Direction.Nord:
                TS.RemoveEffect(MYthsAndSteel_Enum.TerrainType.Barbel�_Nord);
                if (tileId + 9 <= 80)
                {
                    TileScript TSN = TilesManager.Instance.TileList[tileId + 9].GetComponent<TileScript>();
                     TSN.RemoveEffect(MYthsAndSteel_Enum.TerrainType.Barbel�_Sud);
                }
                break;
            case Direction.Sud:
                TS.RemoveEffect(MYthsAndSteel_Enum.TerrainType.Barbel�_Sud);
                if (tileId - 9 >= 0)
                {
                    TileScript TSS = TilesManager.Instance.TileList[tileId - 9].GetComponent<TileScript>();
                    TSS.RemoveEffect(MYthsAndSteel_Enum.TerrainType.Barbel�_Nord);
                }
                break;
            case Direction.Ouest:
                TS.RemoveEffect(MYthsAndSteel_Enum.TerrainType.Barbel�_Ouest);
                if (tileId - 1 >= 0)
                {
                    TileScript TSO = TilesManager.Instance.TileList[tileId - 1].GetComponent<TileScript>();
                     TSO.RemoveEffect(MYthsAndSteel_Enum.TerrainType.Barbel�_Est);
                }
                break;
        }
    }

    MYthsAndSteel_Enum.TerrainType TY;
    protected void BarbelCreationAfterVerification(int tileId, Direction Direction)
    {
        foreach (TerrainType T in GameManager.Instance.Terrain.EffetDeTerrain)
        {
            foreach (MYthsAndSteel_Enum.TerrainType T1 in T._eventType)
            {
                if (T1 == MYthsAndSteel_Enum.TerrainType.Barbel�_Nord) // Aucune diff�rence c'est le m�me enfant.
                {
                    GameObject Child = Instantiate(T.Child, transform.position, Quaternion.identity);
                    Child.transform.parent = TilesManager.Instance.TileList[tileId].GetComponent<TileScript>().transform;
                    Child.transform.localScale = new Vector3(1, 1, 1);
                    TilesManager.Instance.TileList[tileId].GetComponent<TileScript>()._Child.Add(Child);
                    Child.GetComponentInChildren<Barbel>().BarbelG = this;
                    Child.GetComponentInChildren<Barbel>().Direc = Direction;
                    Child.GetComponentInChildren<Barbel>().TurnLeft = 2;
                    BarbelActive.Add(Child.GetComponentInChildren<Barbel>());
                    Child.transform.localPosition = Vector3.zero;

                    TileScript TS = TilesManager.Instance.TileList[tileId].GetComponent<TileScript>();
                    if (Direction == Direction.Nord)
                    {
                        TY = MYthsAndSteel_Enum.TerrainType.Barbel�_Nord; TS.TerrainEffectList.Add(MYthsAndSteel_Enum.TerrainType.Barbel�_Nord);
                        if (tileId + 9 <= 80)
                        {
                            TileScript TSN = TilesManager.Instance.TileList[tileId + 9].GetComponent<TileScript>();
                            TSN.TerrainEffectList.Add(MYthsAndSteel_Enum.TerrainType.Barbel�_Sud);
                        }
                    }
                    if (Direction == Direction.Sud)
                    {
                        TY = MYthsAndSteel_Enum.TerrainType.Barbel�_Sud; TS.TerrainEffectList.Add(MYthsAndSteel_Enum.TerrainType.Barbel�_Sud);
                        if (tileId - 9 >= 0)
                        {
                            TileScript TSS = TilesManager.Instance.TileList[tileId - 9].GetComponent<TileScript>();
                            TSS.TerrainEffectList.Add(MYthsAndSteel_Enum.TerrainType.Barbel�_Nord);
                        }
                    }
                    if (Direction == Direction.Est)
                    {
                        TY = MYthsAndSteel_Enum.TerrainType.Barbel�_Est; TS.TerrainEffectList.Add(MYthsAndSteel_Enum.TerrainType.Barbel�_Est);
                        if (tileId + 1 <= 80)
                        {
                            TileScript TSE = TilesManager.Instance.TileList[tileId + 1].GetComponent<TileScript>();
                            TSE.TerrainEffectList.Add(MYthsAndSteel_Enum.TerrainType.Barbel�_Ouest);
                        }
                    }
                    if (Direction == Direction.Ouest)
                    {
                        TY = MYthsAndSteel_Enum.TerrainType.Barbel�_Ouest; TS.TerrainEffectList.Add(MYthsAndSteel_Enum.TerrainType.Barbel�_Ouest); 
                        if (tileId - 1 >= 0)
                        {
                            TileScript TSO = TilesManager.Instance.TileList[tileId - 1].GetComponent<TileScript>();
                            TSO.TerrainEffectList.Add(MYthsAndSteel_Enum.TerrainType.Barbel�_Est); 
                        }
                    }
                    Child.GetComponent<ChildEffect>().Type = TY;
                }
            }
        }
    }

    public void Turn()
    {
        foreach (Barbel b in BarbelActive)
        {
            b.TurnLeft--;
        }
    }



    public void RandomBarbel(int Number)
    {
        for(int p = 0; p <= Number; p++)
        {
        int i = Random.Range(1, 5);
        int w = Random.Range(1, 80);
        Direction D;
        switch (i)
        {
            case 1: Direct = Direction.Nord; break;
            case 2: Direct = Direction.Sud; break;
            case 3: Direct = Direction.Est; break;
            case 4: Direct = Direction.Ouest; break;
        }
        CreateBarbel(w);
        }
    }
}
