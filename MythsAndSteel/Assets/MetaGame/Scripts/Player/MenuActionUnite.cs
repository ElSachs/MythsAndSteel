using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Ce script g�re l'Affichage de Menu d'Action d'une Unit� (d�placement, attaque, pouvoir) et tout ses v�rifications.
/// </summary>
public class MenuActionUnite : MonoBehaviour
{
    [SerializeField] private MouseCommand _mousecommand;
    public MouseCommand MouseCommand => _mousecommand;
    
    //PlayerScripts
    private bool J1aEncorePointsActivation = true;
    private bool J2aEncorePointsActivation = true;

    // a rajoutter dans le playerscriptunite
    [SerializeField] bool pouvoiractif = true;
    [SerializeField] private GameObject UIMenuActionUnite;
    [SerializeField] private RectTransform _moveBtn;
    [SerializeField] private RectTransform _atkBtn;
    [SerializeField] private RectTransform _powerBtn;
    [SerializeField] private RectTransform _quitBtn;

    [SerializeField] private GameObject _movePanel = null;

    private void Start(){
        _movePanel.SetActive(false);
        UIMenuActionUnite.SetActive(false);
    }

    /// <summary>
    /// Affiche le panneau avec les boutons
    /// </summary>
    public void ShowPanel(){
        if(J1aEncorePointsActivation && GameManager.Instance.IsPlayerRedTurn){
            menuaffichage(RaycastManager.Instance.UnitInTile.GetComponent<UnitScript>(), pouvoiractif);
        }
        else if(J2aEncorePointsActivation && !GameManager.Instance.IsPlayerRedTurn){
            menuaffichage(RaycastManager.Instance.UnitInTile.GetComponent<UnitScript>(), pouvoiractif);
        }
    }

    /// <summary>
    /// Ferme le panneau avec les boutons
    /// </summary>
    public void closePanel(){
        UIMenuActionUnite.SetActive(false);
        _moveBtn.gameObject.GetComponent<MouseOverUI>().StopOver();
        _atkBtn.gameObject.GetComponent<MouseOverUI>().StopOver();
        _powerBtn.gameObject.GetComponent<MouseOverUI>().StopOver();
        _quitBtn.gameObject.GetComponent<MouseOverUI>().StopOver();
    }

    /// <summary>
    /// Cette fonction g�re l'affichage des diff�rentes actions que le joueur va pouvoir effectuer sur l'unit� contre un point activation.
    /// </summary>
    void menuaffichage(UnitScript unitscript, bool capaActive)
    {
      //V�rifie que l'unit� � une capacit� active et d�sactive le bouton si elle en a pas
        if (capaActive == false)
        {
            _powerBtn.gameObject.SetActive(false);
        }
        //V�rifie si l'unit� a effectuer tout son d�placement si c'est le cas alors d�cale la position des boutons en supprimant le bouton de d�placement
        if (unitscript.IsMoveDone == true)
        {
            _moveBtn.gameObject.SetActive(false);
        }

        // place le menu � cot� de l'unit� selectionn�e
        MouseCommand.ActivateUI(UIMenuActionUnite, 0.5f, 3, false, true);
    }
    public void d�placement()
    {
        Mouvement.Instance.StartMvmtForSelectedUnit();
        Mouvement.Instance.Selected = true;
        closePanel();
    }
    public void attaque()
    {
        Debug.Log("attaque");
        closePanel();
    }

    public void capacit�()
    {
        Debug.Log("pouvoir");
        closePanel();
    }



    public void ShowMovementPanel(){
        _movePanel.SetActive(true);
    }

    public void CloseMovementPanel(){
        _movePanel.SetActive(false);
    }
}
