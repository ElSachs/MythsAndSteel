using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoSingleton<PlayerScript>
{
    //List<MYthsAndSteel_Enum.EventCard> EventCardUse; D�j� pr�sente normalement
    public List<UnitScript> Unites;//Liste des Unit�s
    public List<MYthsAndSteel_Enum.TypeUnite> DisactivateUnitType = new List<MYthsAndSteel_Enum.TypeUnite>();
    
    [SerializeField] bool _Army1WinAtTheEnd;
    public bool Army1WinAtTheEnd => _Army1WinAtTheEnd;

    public UnitScript actualUnitClic;
    public  GameObject TileUnderMouse;

    void Update()
    {
        TileUnderMouse = RaycastManager.Instance.Tile;
    }
    public void DesactivateUnitType(MYthsAndSteel_Enum.TypeUnite DesactiveUnit)//D�sactive un type d'unit�
    {
        
       for (int i=0; i < Unites.Count; i++ )
        {
            
            if (Unites[i].UniteType == DesactiveUnit)
            {
                Unites[i].isActivable = false;
                DisactivateUnitType.Add(DesactiveUnit);
            }
        }
    }

    

    public void ActivateAllUnitType()//active tous les types d'unit�s
    {
        for (int i = 0; i < Unites.Count; i++)
        {
          Unites[i].isActivable = true;
        }
    }
    
    public void CheckkArmy()//fonciton qui lance le CheckArmy
    {

        Debug.Log(CheckArmy(2));
    }
    public bool CheckArmy(int Joueur)//Est ce qu'il reste des unit�s dans l'arm�e du joueur
    {
        switch (Joueur)
        {
            case 1:
                foreach (UnitScript us in Unites)
                {
                    if (us.UnitSO.IsInArmy1)
                    {
                        return true;
                    }
                }
                break;
            case 2:
                foreach (UnitScript us in Unites)
                {
                    if (!us.UnitSO.IsInArmy1)
                    {
                        return true;
                    }
                }
                break;
        }
        return false;
    }
    
}
