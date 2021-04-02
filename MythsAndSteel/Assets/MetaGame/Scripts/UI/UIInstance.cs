using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    [Header("PANNEAU D'ACTIVATION D'UNE UNITE")]
    [SerializeField] private GameObject _activationUnitPanel = null;
    public GameObject ActivationUnitPanel => _activationUnitPanel;


    [Header("CARTES EVENEMENTS")]
    //L'objet d'event � afficher lorsqu'une nouvelle carte event est pioch�e.
    [SerializeField] private GameObject _eventCardObject = null;
    public GameObject EventCardObject => _eventCardObject;

    [Header("LISTES DES BOUTONS D ACTIONS")]
    [Tooltip("0 et  1 sont pour les boutons quitter, 2 et 3 sont pour switch entre la Page 1 et la Page 2")]
    //Les premiers chiffres (pour le moment 0 et 1 d�terminent les bouttons � quitter les menus), les derniers d�terminent les buttons des switch de menu.
    [SerializeField] private List<Button> _buttonID;
    public List<Button> ButtonId => _buttonID;
    [Space]
    [Header("LISTES DES ELEMENTS UI POUR LE MOUSEOVER")]
    [Tooltip("Tous les �l�ments qui composent l'UI dans le MOUSEOVER")]
    //Texte qui indique le nom de l'unit�.
    [SerializeField] private GameObject _titlePanelMouseOver;
    public GameObject TitlePanelMouseOver => _titlePanelMouseOver;
    //Ordre : 0 => Texte Vie, 1 => Texte Port�e et 2 => Texte D�placement
    [SerializeField] private List<GameObject> _mouseOverStats;
    public List<GameObject> MouseOverStats => _mouseOverStats;
    
    [Space]
    
    [Header("LISTES DES ELEMENTS UI POUR LE SHIFT CLIC DE LA PAGE 1")]
    [Tooltip("Tous les �l�ments qui composent l'UI pour le Shift Clic de la Page 1")]
    
    //Texte qui indique le nom de l'unit�.
    [SerializeField] private GameObject _titlePanelShiftClicPage1;
    public GameObject TitlePanelShiftClicPage1 => _titlePanelShiftClicPage1;
    
    //Ordre : 0 => Texte Vie, 1 => Texte Port�e et 2 => Texte D�placement
    [SerializeField] private List<GameObject> _middleStatistiques;
    public List<GameObject> MiddleStatistique => _middleStatistiques;

    //Ordre : 0 => Icon Vie, 1 => Icon Port�e et 2 => Icon D�placement
    [SerializeField] private List<GameObject> _middleImage;
    public List<GameObject> MiddleImage => _middleImage;

    //Ordre : 0 => Valeur Min entre deux valeurs, 1 => Valeur Min entre deux valeurs, 2 => Degats Min et 3 Degats Max
    [SerializeField] private List<GameObject> _basseStatistique;
    public List<GameObject> BasseStatistique => _basseStatistique;

    

    [Header("LISTES DES ELEMENTS UI POUR LE SHIFT CLIC DE LA PAGE 2")]
    [Tooltip("Tous les �l�ments qui composent l'UI pour le Shift Clic de la Page 2")]
    //Texte qui indique le nom de l'unit�.
    [SerializeField] private GameObject _titlePanelShiftClicPage2;
    public GameObject TitlePanelShiftClicPage2 => _titlePanelShiftClicPage2;    
    
    [SerializeField] private List<GameObject> _middleImageTerrain;
    public List<GameObject> MiddleImageTerrain => _middleImageTerrain;

    [SerializeField] private List<GameObject> _middleTextTerrain;
    public List<GameObject> MiddleTextTerrain => _middleTextTerrain;
}
