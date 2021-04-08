using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Tout ce qui est dans ce script est � transf�rer le Player Class ! 
 */
public class JaugeOrgone : MonoSingleton<JaugeOrgone>
{
//Variable � mettre dans la Class Player
  public int OrgonePowerLeftRedPlayer = 1;
    public int OrgonePowerLeftBluePlayer = 1;
    public int ValueOrgoneRedPlayer = 3;
    public int ValueOrgoneBluePlayer = 4;
    public bool showPanelValidate = true;

    /// <summary>
    /// Permet de connaitre la nouvelle valeur de la jauge d'orgone en fonction d'un variation positif ou n�gatif
    /// </summary>
    public int ChangeOrgone(int currentValue, int fluctuation)
    {
        int newValue = currentValue + fluctuation;
        return newValue;
    }
    /// <summary>
    /// D�termine et applique l'animation de l'UI de la jauge d'orgone. A D�TERMINER !
    /// </summary>
    public void UpdateOrgoneUI()
    {
        Debug.Log(OrgonePowerLeftRedPlayer);
        Debug.Log(OrgonePowerLeftBluePlayer);
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
