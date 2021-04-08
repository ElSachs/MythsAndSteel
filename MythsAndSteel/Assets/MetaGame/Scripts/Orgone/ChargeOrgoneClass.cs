using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Ce Script va g�rer l'utilisation des charges d'orgones. Tous les scripts des charges d'orgone d�rive de ce dernier. 
C'est pour cela qu'il ne faut surtout pas le MODIFIER !! 
 */ 
public class ChargeOrgoneClass : MonoBehaviour
{
    #region Variables
    //D�termine � qui appartient la charge d'orgone
    public bool IsPowerForRedPlayer;
    //Le co�t de la charge d'Orgone (n�gatif)
   
    public int cout;
    #endregion
    /// <summary>
    /// Fonction Principal de la charge qui pose l'ensemble des conditions et feedbacks permettant l'utilisation d'une charge d'orgone. 
    /// A NE PAS MODIFI�E !
    /// </summary>
    public virtual void UtilisationChargeOrgone()
    {
        //on v�rifie que le joueur se situe dans une phase de jeu d'orgone. 
        if (GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.OrgoneJ1 | GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.OrgoneJ2)
        {
            GameManager.Instance.DetermineWhichPlayerplay();
            // on regarde si la jauge peut �tre utilis� en fonction de la phase d'orgone et de l'initiative d�termin� par la phase activation
            if (GameManager.Instance.IsPlayerRedTurn == IsPowerForRedPlayer)
            {

                if (GameManager.Instance.IsPlayerRedTurn == true)
                {
                    // On v�rifie que le joueur peut encore utilis� une charge
                    if (JaugeOrgone.Instance.OrgonePowerLeftRedPlayer > 0)
                    {
                        // On v�rifie que la valeur de la jauge d'orgone est sup�rieur ou �gal ou co�t de la charge
                        if (JaugeOrgone.Instance.ValueOrgoneRedPlayer >= Mathf.Abs(cout))
                        {
                           //panneau de validation � modifier certainement lorsqu'il sera fait
                            if (JaugeOrgone.Instance.showPanelValidate == true)
                            {
                              //change la valeur de la jauge d'orgone applique l'animation et utilise le pouvoir de la charge. 
                                JaugeOrgone.Instance.ValueOrgoneRedPlayer = JaugeOrgone.Instance.ChangeOrgone(JaugeOrgone.Instance.ValueOrgoneRedPlayer, cout);
                                JaugeOrgone.Instance.UpdateOrgoneUI();
                                RaiseEvent();
                                JaugeOrgone.Instance.OrgonePowerLeftBluePlayer -= 1;
                            }
                            else if (JaugeOrgone.Instance.showPanelValidate == false)
                            {
                                Debug.Log("Fenetre de validation qui se ferme");
                            }
                        }
                        else
                        {
                            //feedback 
                            Debug.Log("vous n'avez pas assez de points d'orgone pour utiliser cette charge");
                        }
                    }
                    else if (JaugeOrgone.Instance.OrgonePowerLeftRedPlayer == 0)
                    {
                        Debug.Log("vous avez d�j� utilis� une charge d'orgone durant ce tour");
                        //feedback
                    }
                }
                //M�me chose mais pour le joueur Bleu
                else if (GameManager.Instance.IsPlayerRedTurn == false)
                {

                    if (JaugeOrgone.Instance.OrgonePowerLeftBluePlayer > 0)
                    {

                        if (JaugeOrgone.Instance.ValueOrgoneBluePlayer >= Mathf.Abs(cout))
                        {

                            if (JaugeOrgone.Instance.showPanelValidate == true)
                            {

                                JaugeOrgone.Instance.ValueOrgoneBluePlayer = JaugeOrgone.Instance.ChangeOrgone(JaugeOrgone.Instance.ValueOrgoneBluePlayer, cout);
                                JaugeOrgone.Instance.UpdateOrgoneUI();

                                RaiseEvent();
                                JaugeOrgone.Instance.OrgonePowerLeftBluePlayer -= 1;
                            }
                            else if (JaugeOrgone.Instance.showPanelValidate == false)
                            {
                                Debug.Log("Fenetre de validation qui se ferme");
                            }
                        }
                        else
                        {
                            //feedback 
                            Debug.Log("vous n'avez pas assez de points d'orgone pour utiliser cette charge");
                        }
                    }
                    else if (JaugeOrgone.Instance.OrgonePowerLeftBluePlayer == 0)
                    {
                        Debug.Log("vous avez d�j� utilis� une charge d'orgone durant ce tour");
                        //feedback
                    }
                }
            }
            else if (GameManager.Instance.IsPlayerRedTurn != IsPowerForRedPlayer)
            {
                //Feedback
                Debug.Log("vous ne pouvez utilisez une charge ennemi");
            }

        }
    else
    {
            //feedback
            Debug.Log("Vous devez �tre dans la phase d'orgone pour utiliser une charge orgonique");
    }
    }
    /// <summary>
    /// Cette fonction est virtuelle et doit �tre modifi� dans les scripts correspondants � chacunes des charges, il faut y impl�menter dans cette fonction le pouvoir de la charge orgonique.
    /// </summary>
    public virtual void RaiseEvent()
    {
        Debug.Log("je suis le premier raiseEvent");
    }

   
}
