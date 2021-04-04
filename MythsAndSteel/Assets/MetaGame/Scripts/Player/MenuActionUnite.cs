using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Ce script g�re l'Affichage de Menu d'Action d'une Unit� (d�placement, attaque, pouvoir) et tout ses v�rifications.
/// </summary>
public class MenuActionUnite : MonoBehaviour
{
    public MouseCommand mousecommand;
    
    private bool J1unit = true;
    //PlayerScripts
    private bool J1aEncorePointsActivation = true;
    private bool J2aEncorePointsActivation = true;
    // a rajoutter dans le playerscriptunite
     bool pouvoiractif = false;
    [SerializeField]
    private GameObject UIMenuActionUnite;
    [SerializeField]
    private RectTransform Button1;
    [SerializeField]
    private RectTransform Button2;
    [SerializeField]
    private RectTransform Button3;
    [SerializeField]
  
    
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        //On v�rifie qu'on dans la phase d'action du joueur 1
        if (GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ1)
        {

            // Si le joueur clique sur une tile ou il y a pas d'unit� ferme le menu d'action
                if (RaycastManager.Instance.UnitInTile == false && Input.GetMouseButtonDown(0))
                {
                    UIMenuActionUnite.SetActive(false);
                }
            //On v�rifie si le joueur clique sur une tile ou il y a une tile
            if (RaycastManager.Instance.UnitInTile == true && Input.GetMouseButtonDown(0))
            {
                // On v�rifie si le joueur a encore des points d'activations
                if (J1aEncorePointsActivation == true)
                {
                    // On v�rifie que l'unit� cliqu� appartient au joueur 1 puis on lance l'affichage du menu avec la fonction menuaffichage
                    if (PlayerStatic.CheckIsUnitArmy(RaycastManager.Instance.UnitInTile.GetComponent<UnitScript>(), J1unit) == true)
                    {

                      
                        menuaffichage(RaycastManager.Instance.UnitInTile.GetComponent<UnitScript>(), pouvoiractif);
                    }

                }
            }
        }
        //M�me principe mais pour le joueur2
        if (GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ2)
        {
            if (RaycastManager.Instance.UnitInTile == false && Input.GetMouseButtonDown(0))
            {
                UIMenuActionUnite.SetActive(false);
            }


            if (RaycastManager.Instance.UnitInTile == true && Input.GetMouseButtonDown(0))
            {
                if (RaycastManager.Instance.UnitInTile == false && Input.GetMouseButtonDown(0))
                {
                    UIMenuActionUnite.SetActive(false);
                }
                if (J1aEncorePointsActivation == true)
                {

                    if (PlayerStatic.CheckIsUnitArmy(RaycastManager.Instance.UnitInTile.GetComponent<UnitScript>(), J1unit) == false)
                    {

                      
                        menuaffichage(RaycastManager.Instance.UnitInTile.GetComponent<UnitScript>(), pouvoiractif);
                    }

                }
            }
        }




    }
    /// <summary>
    /// Cette fonction g�re l'affichage des diff�rentes actions que le joueur va pouvoir effectuer sur l'unit� contre un point activation.
    /// </summary>
    void menuaffichage(UnitScript unitscript, bool capaActive)
    {
      //V�rifie que l'unit� � une capacit� active et d�sactive le bouton si elle en a pas
        if (capaActive == false)
        {
            Button3.gameObject.SetActive(false);
        }
        //V�rifie si l'unit� a effectuer tout son d�placement si c'est le cas alors d�cale la position des boutons en supprimant le bouton de d�placement
        if (unitscript.IsMoveDone == true)
        {
            Button3.position = Button2.position;
            Button2.position = Button1.position;
            Button1.gameObject.SetActive(false);

 

        }
        // place le menu � cot� de l'unit� selectionn�e
        mousecommand.ActivateUI(UIMenuActionUnite, 2, 2, false) ;
    }
    public void d�placement()
    {
        Debug.Log("d�placement");
        UIMenuActionUnite.SetActive(false);
    }
    public void attaque()
    {
        Debug.Log("attaque");
        UIMenuActionUnite.SetActive(false);
    }

    public void capacit�()
    {
        Debug.Log("pouvoir");
        UIMenuActionUnite.SetActive(false);
    }
}
