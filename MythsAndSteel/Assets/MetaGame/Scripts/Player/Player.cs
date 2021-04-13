using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Player
{
    #region Variables
    [Header("ARMY INFO")]
    //nom de l'arm�e
    public string ArmyName;

    [Header("ACTIVATION")]
    //Nombre d'activation restante
    public int ActivationLeft; 
    //Valeur de la carte activation pos�e
    public int ActivationCardValue; 

    [Header("ORGONE")]
    //Nombre de charges d'orgone actuel
    public int OrgoneValue; 
    //Nombre de pouvoirs d'orgone encore activable
    public int OrgonePowerLeft; 
    //Permet de se souvenir de la derni�re valeur d'orgone avant Update
    public int LastKnownOrgoneValue; 
    //Tile qui correspond au centre de la zone d'Orgone
    public GameObject TileCentreZoneOrgone; 

    [Header("RESSOURCE")]
    //Nombre de Ressources actuel
    public int Ressource;

    [Header("OBJECTIF")]
    //Nombre d'objectif actuellement captur�
    public int GoalCapturePointsNumber; 

    public bool HasCreateUnit; //est ce que le joueur a cr�er une unit� durant sont tour
    #endregion Variables

    /// <summary>
    /// Check si l'orgone va exploser
    /// </summary>
    /// <returns></returns>
    public bool OrgoneExplose(){
        return OrgoneValue > 5 ? true : false;
    }
    
    /// <summary>
    /// Change la valeur (pos/neg) de la jauge d'orgone.
    /// </summary>
    /// <param name="Value">Valeur positive ou n�gative.</param>
    public void ChangeOrgone(int Value, int player){
        OrgoneValue += Value;

        UpdateOrgoneUI(player);

        Debug.Log(player);
        
        if(OrgoneValue >= 5)
        {
            List<GameObject> unitList = player == 1 ? PlayerScript.Instance.UnitRef.UnitListRedPlayer : PlayerScript.Instance.UnitRef.UnitListBluePlayer;
            GameManager.Instance.StartEventModeUnit(4, player == 1 ? true : false, unitList, "Explosion d'orgone", "�tes-vous sur de vouloir infliger des d�g�ts � ces unit�s?");
            GameManager.Instance._eventCall += GiveDamageToUnitForOrgone;
            GameManager.Instance._eventCallCancel += CancelOrgone;
            unitList.Clear();
        }
        else
        {
            GameManager.Instance.IsCheckingOrgone = false;
            if(GameManager.Instance._waitToCheckOrgone != null){
                GameManager.Instance._waitToCheckOrgone();
            }
        }
    }

    /// <summary>
    /// When Orgone explode
    /// </summary>
    void GiveDamageToUnitForOrgone(){
        UIInstance.Instance.ActivateNextPhaseButton();

        foreach(GameObject unit in GameManager.Instance.UnitChooseList)
        {
            unit.GetComponent<UnitScript>().TakeDamage(1);
        }

        GameManager.Instance.UnitChooseList.Clear();
        GameManager.Instance._eventCall -= GiveDamageToUnitForOrgone;
        GameManager.Instance._eventCallCancel -= CancelOrgone;

        OrgoneValue -= 5;

        UpdateOrgoneUI(GameManager.Instance.RedPlayerUseEvent? 1 : 2);

        GameManager.Instance.IsCheckingOrgone = false;
        if(GameManager.Instance._waitToCheckOrgone != null)
        {
            GameManager.Instance._waitToCheckOrgone();
        }
    }

    /// <summary>
    /// Si le joueur appuie sur le bouton annuler 
    /// </summary>
    void CancelOrgone(){
        List<GameObject> unitList = GameManager.Instance.RedPlayerUseEvent? PlayerScript.Instance.UnitRef.UnitListRedPlayer : PlayerScript.Instance.UnitRef.UnitListBluePlayer;
        GameManager.Instance.StartEventModeUnit(4, GameManager.Instance.RedPlayerUseEvent, unitList, "Explosion d'orgone", "�tes-vous sur de vouloir infliger des d�g�ts � ces unit�s?");
        unitList.Clear();
    }

    /// <summary>
    /// Update l'UI de la jauge d'orgone en fonction du nombre de charge
    /// </summary>
    public void UpdateOrgoneUI(int player){
        if(player == 1){
            foreach(Image img in OrgoneManager.Instance.RedPlayerCharge){
                img.enabled = false;
            }

            for(int i = 0; i < OrgoneValue; i++){
                OrgoneManager.Instance.RedPlayerCharge[i].enabled = true;
            }
        }
        else{
            foreach(Image img in OrgoneManager.Instance.BluePlayerCharge)
            {
                img.enabled = false;
            }

            for(int i = 0; i < OrgoneValue; i++)
            {
                OrgoneManager.Instance.BluePlayerCharge[i].enabled = true;
            }
        }
    }

    /// <summary>
    /// Appelle l'explosion d'orgone
    /// </summary>
    public void MakeOrgoneExplosion(){
        // oskour Paul !
    }

    //AV
    [HideInInspector] public bool dontTouchThis = false;
}
