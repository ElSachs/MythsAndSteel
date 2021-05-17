using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChargeOrgone : MonoBehaviour
{
    //Est ce que les charges d'orgone appartient au joueur rouge
    [SerializeField] private bool _isRedOrgonePower = false;
    public virtual void LaunchChargeOrgone1(int cost)
    {
        Debug.Log("Charge 1");
        if (!MythsAndSteel.Orgone.OrgoneCheck.CanUseOrgonePower(cost, _isRedOrgonePower ? 1 : 2)) return;
    }

    /// <summary>
    /// Fonction qui permet de cr�er une deuxi�me charge d'orgone
    /// </summary>
    /// <param name="cost"></param>
    public virtual void LaunchChargeOrgone2(int cost)
    {
        Debug.Log("Charge 2");
        if (!MythsAndSteel.Orgone.OrgoneCheck.CanUseOrgonePower(cost, _isRedOrgonePower ? 1 : 2)) return;
    }

    /// <summary>
    /// Fonction qui permet de cr�er une troisi�me charge d'orgone
    /// </summary>
    /// <param name="cost"></param>
    public virtual void LaunchChargeOrgone3(int cost)
    {
        Debug.Log("Charge 3");
        if (!MythsAndSteel.Orgone.OrgoneCheck.CanUseOrgonePower(cost, _isRedOrgonePower ? 1 : 2)) return;
    }

    /// <summary>
    /// Fonction qui permet de cr�er une quatri�me charge d'orgone
    /// </summary>
    /// <param name="cost"></param>
    public virtual void LaunchChargeOrgone4(int cost)
    {
        Debug.Log("Charge 4");
        if (!MythsAndSteel.Orgone.OrgoneCheck.CanUseOrgonePower(cost, _isRedOrgonePower ? 1 : 2)) return;
    }

    /// <summary>
    /// Fonction qui permet de cr�er une cinqui�me charge d'orgone
    /// </summary>
    /// <param name="cost"></param>
    public virtual void LaunchChargeOrgone5(int cost)
    {
        Debug.Log("Charge 5");
        if (!MythsAndSteel.Orgone.OrgoneCheck.CanUseOrgonePower(cost, _isRedOrgonePower ? 1 : 2)) return;
    }
    /// <summary>
    /// Fonction qui permet de cr�er une charge d'orgone
    /// </summary>
    /// <param name="cost"></param>
    public virtual void ChargeOrgone1(int cost){
        Debug.Log("Charge 1");
        if(!MythsAndSteel.Orgone.OrgoneCheck.CanUseOrgonePower(cost, _isRedOrgonePower? 1 : 2)) return;
    }

    /// <summary>
    /// Fonction qui permet de cr�er une deuxi�me charge d'orgone
    /// </summary>
    /// <param name="cost"></param>
    public virtual void ChargeOrgone2(int cost){
        Debug.Log("Charge 2");
        if(!MythsAndSteel.Orgone.OrgoneCheck.CanUseOrgonePower(cost, _isRedOrgonePower ? 1 : 2)) return;
    }

    /// <summary>
    /// Fonction qui permet de cr�er une troisi�me charge d'orgone
    /// </summary>
    /// <param name="cost"></param>
    public virtual void ChargeOrgone3(int cost){
        Debug.Log("Charge 3");
        if(!MythsAndSteel.Orgone.OrgoneCheck.CanUseOrgonePower(cost, _isRedOrgonePower ? 1 : 2)) return;
    }

    /// <summary>
    /// Fonction qui permet de cr�er une quatri�me charge d'orgone
    /// </summary>
    /// <param name="cost"></param>
    public virtual void ChargeOrgone4(int cost){
        Debug.Log("Charge 4");
        if(!MythsAndSteel.Orgone.OrgoneCheck.CanUseOrgonePower(cost, _isRedOrgonePower ? 1 : 2)) return;
    }

    /// <summary>
    /// Fonction qui permet de cr�er une cinqui�me charge d'orgone
    /// </summary>
    /// <param name="cost"></param>
    public virtual void ChargeOrgone5(int cost){
        Debug.Log("Charge 5");
        if(!MythsAndSteel.Orgone.OrgoneCheck.CanUseOrgonePower(cost, _isRedOrgonePower ? 1 : 2)) return;
    }

}
