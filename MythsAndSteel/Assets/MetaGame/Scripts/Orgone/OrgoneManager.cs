using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Ce Script va g�rer l'utilisation des charges d'orgones. Tous les scripts des charges d'orgone d�rive de ce dernier. 
C'est pour cela qu'il ne faut surtout pas le MODIFIER !! 
 */ 
public class OrgoneManager : MonoSingleton<OrgoneManager>
{
    #region Variables
    [Header("PARENT JAUGE D'ORGONE")]
    //Jauge d'orgone joueur rouge
    [SerializeField] private GameObject _redPlayerPanelOrgone = null;
    public GameObject RedPlayerPanelOrgone => _redPlayerPanelOrgone;

    //Jauge d'orgone joueur bleu
    [SerializeField] private GameObject _bluePlayerPanelOrgone = null;
    public GameObject BluePlayerPanelOrgone => _bluePlayerPanelOrgone;

    [Header("ZONE ORGONE")]
    //Est ce qu'une jauge d'orgone est s�lectionn�e
    [SerializeField] private bool _selected = false;
    public bool Selected => _selected;

    //Zone d'orgone joueur rouge
    [SerializeField] private GameObject _redPlayerZone = null;
    public GameObject RedPlayerZone => _redPlayerZone;

    //Zone d'orgone joueur bleu
    [SerializeField] private GameObject _bluePlayerZone = null;
    public GameObject BluePlayerZone => _bluePlayerZone;

    #endregion Variables

    public void ReleaseZone(){
        if(GameManager.Instance.IsPlayerRedTurn){
            _redPlayerZone.GetComponent<ZoneOrgone>().ReleaseZone();
        }
        else{
            _bluePlayerZone.GetComponent<ZoneOrgone>().ReleaseZone();
        }

        _selected = false;
    }

    public void StartToMoveZone(){
        if(GameManager.Instance.IsPlayerRedTurn && !_redPlayerZone.GetComponent<ZoneOrgone>().HasMoveOrgoneArea)
        {
            _redPlayerZone.GetComponent<ZoneOrgone>().AddOrgoneAtRange();
            _selected = true;
        }
        else if(!GameManager.Instance.IsPlayerRedTurn && !_bluePlayerZone.GetComponent<ZoneOrgone>().HasMoveOrgoneArea)
        {
            _bluePlayerZone.GetComponent<ZoneOrgone>().AddOrgoneAtRange();
            _selected = true;
        }
    }

    /// <summary>
    /// Permet de connaitre la nouvelle valeur de la jauge d'orgone en fonction d'un variation positif ou n�gatif
    /// </summary>
    public int ChangeOrgone(int currentValue, int fluctuation){
        int newValue = currentValue + fluctuation;
        return newValue;
    }

    /// <summary>
    /// D�termine et applique l'animation de l'UI de la jauge d'orgone. A D�TERMINER !
    /// </summary>
    public void UpdateOrgoneUI()
    {
        /* Pour les animations voici ma proposition
         il faudrait faire une liste de boutons avec les boutons d'une jauge d'orgone
         il faudrait la valeur actuelle de l'orgone 
         et la variation
         on applique ensuite un for qui d�pend qui s'arrete quand on a appliquer tout la fluctuation de l'orgone et 
        qui a chaque fois applique l'animation pour chaque boutons concern�s
        on aurait du coup un valeur sp�cifique temporaire qui corresponderait � la valeur actuelle qui se baisse au fur et � mesure pour comptabiliser le nombre d'animation qui reste � faire
        
         */
    }
}

namespace MythsAndSteel.Orgone{
    public static class OrgoneCheck{

        /// <summary>
        /// Determine si le joueur peut utiliser un pouvoir d'orgone
        /// </summary>
        /// <param name="cost"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        public static bool CanUseOrgonePower(int cost, int player){
            bool canUse = false;

            if(GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.OrgoneJ1 || GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.OrgoneJ2){
                if(GameManager.Instance.IsPlayerRedTurn == (player == 1? true : false)){
                    if(player == 1){
                        if(PlayerScript.Instance.RedPlayerInfos.OrgonePowerLeft > 0 && PlayerScript.Instance.RedPlayerInfos.OrgoneValue >= cost){
                            canUse = true;
                        }
                    }
                    else{
                        if(PlayerScript.Instance.BluePlayerInfos.OrgonePowerLeft > 0 && PlayerScript.Instance.BluePlayerInfos.OrgoneValue >= cost){
                            canUse = true;
                        }
                    }
                }
            }

            return canUse;
        }
    }
}
