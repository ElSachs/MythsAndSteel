using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
Ce Script fait partie de la liste de script qui d�termine les caract�ristiques et le pouvoir d'une charge de la jauge d'orgone. 
 */
public class ChargeOrgone1Blue : ChargeOrgoneClass
{

    /// <summary>
    /// Permet de sp�cifier pour cette charge les variables d�crites dans le script ChargeOrgoneClass
    /// </summary>
    public void SetupVariable()
    {
        IsPowerForRedPlayer = false;



        cout = -1;
    }
    public override void UtilisationChargeOrgone()
    {
        SetupVariable();
        base.UtilisationChargeOrgone();

    }
    public override void RaiseEvent()
    {

        base.RaiseEvent();
        Debug.Log("Je suis la charge1 du joueur bleu");
    }
}
