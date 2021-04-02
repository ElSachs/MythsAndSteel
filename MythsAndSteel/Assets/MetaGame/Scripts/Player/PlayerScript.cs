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
    public bool ArmyRedWinAtTheEnd => _Army1WinAtTheEnd;

    /// <summary>
    /// D�sactive un type d'unit�
    /// </summary>
    /// <param name="DesactiveUnit"></param>
    public void DesactivateUnitType(MYthsAndSteel_Enum.TypeUnite DesactiveUnit)
    {
        DisactivateUnitType.Add(DesactiveUnit);
    }


    /// <summary>
    /// active tous les types d'unit�s
    /// </summary>
    public void ActivateAllUnitType()
    {
        DisactivateUnitType.Clear();
    }
    

    /// <summary>
    /// Est ce qu'il reste des unit�s dans l'arm�e du joueur
    /// </summary>
    /// <param name="Joueur"></param>
    /// <returns></returns>
    public bool CheckArmy(int Joueur)
    {
        switch (Joueur)
        {
            case 1:
                foreach (UnitScript us in Unites)
                {
                    if (us.UnitSO.IsInRedArmy)
                    {
                        return true;
                    }
                }
                break;
            case 2:
                foreach (UnitScript us in Unites)
                {
                    if (!us.UnitSO.IsInRedArmy)
                    {
                        return true;
                    }
                }
                break;
        }
        return false;
    }
    
}
