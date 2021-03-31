using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInstance : MonoSingleton<UIInstance>
{
    #region AppelDeScript
    MouseCommand mouseCommand;
    #endregion
    [Header("UI MANAGER")]
    //Avoir en r�f�rence l'UI Manager pour appeler des fonctions � l'int�rieur
    [SerializeField] private UIManager _uiManager = null;
    public UIManager UiManager => _uiManager;

    //Le canvas en jeu pour afficher des menus
    [SerializeField] private GameObject _canvasTurnPhase = null;
    public GameObject CanvasTurnPhase => _canvasTurnPhase;

    [Header("PHASE DE JEU")]
    //Le panneau � afficher lorsque l'on change de phase
    [SerializeField] private GameObject _switchPhaseObject = null;
    public GameObject SwitchPhaseObject => _switchPhaseObject;

    //Le canvas en jeu pour la phase d'activation
    [SerializeField] private GameObject _canvasActivation = null;
    public GameObject CanvasActivation => _canvasActivation;

    [Header("CARTES EVENEMENTS")]
    //L'objet d'event � afficher lorsqu'une nouvelle carte event est pioch�e
    [SerializeField] private GameObject _eventCardObject = null;
    public GameObject EventCardObject => _eventCardObject;

    [Header("LISTES DES BOUTONS D ACTIONS")]
    [Tooltip("0 et  1 sont pour les boutons quitter, 2 et 3 sont pour switch entre la Page 1 et la Page 2")]
    //Les premiers chiffres (pour le moment 0 et 1 d�terminent les bouttons � quitter les menus), les derniers d�terminent les buttons des switch de menu.
    [SerializeField] private List<Button> _buttonID;
    public List<Button> ButtonId => _buttonID;
}
