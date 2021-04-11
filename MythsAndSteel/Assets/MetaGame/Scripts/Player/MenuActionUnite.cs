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
    
    // a rajoutter dans le playerscriptunite
    [SerializeField] bool pouvoiractif = true;
    [SerializeField] private GameObject UIMenuActionUnite;
    [SerializeField] private RectTransform _moveBtn;
    [SerializeField] private RectTransform _atkBtn;
    [SerializeField] private RectTransform _powerBtn;
    [SerializeField] private RectTransform _quitBtn;

    [SerializeField] private GameObject _movePanel = null;

    [SerializeField] private bool _isOpen = false;
    public bool IsOpen => _isOpen;

    private void Start(){
        _movePanel.SetActive(false);
        UIMenuActionUnite.SetActive(false);
    }

    /// <summary>
    /// Affiche le panneau avec les boutons
    /// </summary>
    public void ShowPanel(){
        UnitScript unit = RaycastManager.Instance.UnitInTile.GetComponent<UnitScript>();
        if((PlayerScript.Instance.RedPlayerInfos.ActivationLeft > 0 || (unit.MoveLeft + unit.MoveSpeedBonus > 0 && unit._hasStartMove) || (unit.IsMoveDone && !unit._isActionDone)) && GameManager.Instance.IsPlayerRedTurn && _isOpen == false){
            menuaffichage(RaycastManager.Instance.UnitInTile.GetComponent<UnitScript>(), pouvoiractif);
            _isOpen = true;
        }
        else if((PlayerScript.Instance.BluePlayerInfos.ActivationLeft > 0 || (unit.MoveLeft + unit.MoveSpeedBonus > 0 && unit._hasStartMove) || (unit.IsMoveDone && !unit._isActionDone)) && !GameManager.Instance.IsPlayerRedTurn && _isOpen == false)
        {
            menuaffichage(RaycastManager.Instance.UnitInTile.GetComponent<UnitScript>(), pouvoiractif);
            _isOpen = true;
        }
    }

    /// <summary>
    /// Ferme le panneau avec les boutons
    /// </summary>
    public void closePanel(){
        _isOpen = false;
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
        int numberOfButtonHide = 0;
      //V�rifie que l'unit� � une capacit� active et d�sactive le bouton si elle en a pas
        if (capaActive == false)
        {
            _powerBtn.gameObject.SetActive(false);
        }

        //V�rifie si l'unit� a effectuer tout son d�placement si c'est le cas alors d�cale la position des boutons en supprimant le bouton de d�placement
        if (unitscript.IsMoveDone == true){
            _moveBtn.gameObject.SetActive(false);
            numberOfButtonHide++;
        }
        else{
            _moveBtn.gameObject.SetActive(true);
        }

        //V�rifie si l'unit� a effectuer tout son d�placement si c'est le cas alors d�cale la position des boutons en supprimant le bouton de d�placement
        if(unitscript._isActionDone == true)
        {
            _atkBtn.gameObject.SetActive(false);
            _powerBtn.gameObject.SetActive(false);
            numberOfButtonHide += 2;
        }
        else
        {
            _atkBtn.gameObject.SetActive(true);
            _powerBtn.gameObject.SetActive(true);
        }

        UIMenuActionUnite.GetComponent<RectTransform>().sizeDelta = new Vector2(117.7325f, 90 - (17 * numberOfButtonHide));

        // place le menu � cot� de l'unit� selectionn�e
        MouseCommand.ActivateUI(UIMenuActionUnite, 0.5f, 3, false, true);
    }

    /// <summary>
    /// Pour le bouton d�placement
    /// </summary>
    public void d�placement()
    {
        Mouvement.Instance.StartMvmtForSelectedUnit();
        Mouvement.Instance.Selected = true;
        closePanel();
    }

    /// <summary>
    /// Pour le bouton attaque
    /// </summary>
    public void attaque()
    {
        Attaque.Instance.StartAttackSelectionUnit();
        closePanel();
    }

    /// <summary>
    /// Pour le bouton pouvoir
    /// </summary>
    public void capacit�()
    {
      
        Debug.Log("pouvoir");
        closePanel();
    }
  
    /// <summary>
    /// Affiche le bouton pour se d�placer
    /// </summary>
    public void ShowMovementPanel(){
        _movePanel.SetActive(true);
    }

    /// <summary>
    /// Affiche le bouton pour se d�placer
    /// </summary>
    public void CloseMovementPanel(){
        _movePanel.SetActive(false);
    }
}
