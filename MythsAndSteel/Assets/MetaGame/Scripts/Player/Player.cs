using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player
{
    List<GameObject> ArmyUnitsList; //Liste de toutes les unit�s de l'arm�e

    List<GameObject> CreatorPointsUnits; //Liste des unit�s qui permettent de cr�er des unit�s autour d'elle

    List<Unit_SO> CreableUnits; //Liste des unit�s cr�ables gr�ce au menu renfort

    List<EventCard> EventCardInHand; //liste des cartes event dans la main du joueur

    public int EventsCardNumberLeft; //Nombre de cartes �v�nements restantes

    public int OrgoneValue; //Nombre de charges d'orgone actuel

    public int Ressource; //Nombre de Ressources actuel

    public int ActivationLeft; //Nombre d'activation restante

    public int ActivationCardValue; //Valeur de la carte activation pos�e

    public int GoalCapturePointsNumber; //Nombre d'objectif actuellement captur�

    public int OrgonePowerLeft; //Nombre de pouvoirs d'orgone encore activable

    public int LastKnownOrgoneValue; //Permet de se souvenir de la derni�re valeur d'orgone avant Update

    public GameObject TileCentreZoneOrgone; //Tile qui correspond au centre de la zone d'Orgone

    public string ArmyName; //nom de l'arm�e

    public bool HasCreateUnit; //est ce que le joueur a cr�er une unit� durant sont tour

    public bool ActivationCardChoose;//Est ce que le joueur a choisit une carte activation

    public bool IsMovingOrgoneArea; //Est ce que le joueur est en train de bouger la zone d'orgone

    public bool HasMoveOrgoneArea; //Est que le joueur a d�j� boug� la zone d'orgone

    public bool OrgoneExplose(){
        return OrgoneValue > 5 ? true : false;
    }

    /// <summary>
    /// Change la valeur (pos/neg) de la jauge d'orgone.
    /// </summary>
    /// <param name="Value">Valeur positive ou n�gative.</param>
    public void ChangeOrgone(int Value){
        OrgoneValue += Value;
        Debug.Log(OrgoneValue);
    }

    /// <summary>
    /// Update l'UI de la jauge d'orgone en fonction du nombre de charge
    /// </summary>
    public void UpdateOrgoneUI(){
        // oskour Paul !
    }

    /// <summary>
    /// Appelle l'explosion d'orgone
    /// </summary>
    public void MakeOrgoneExplosion(){
        // oskour Paul !
    } 
}
