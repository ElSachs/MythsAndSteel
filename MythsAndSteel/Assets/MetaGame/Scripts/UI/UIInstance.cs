using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInstance : MonoSingleton<UIInstance>{
    [Header("UI Manager")]
    //Avoir en r�f�rence l'UI Manager pour appeler des fonctions � l'int�rieur
    [SerializeField] private UIManager _uiManager = null;
    public UIManager UiManager => _uiManager;

    //Le canvas en jeu pour afficher des menus
    [SerializeField] private GameObject _canvasTurnPhase = null;
    public GameObject CanvasTurnPhase => _canvasTurnPhase;

    [Header("Phase De Jeu")]
    //Le panneau � afficher lorsque l'on change de phase
    [SerializeField] private GameObject _switchPhaseObject = null;
    public GameObject SwitchPhaseObject => _switchPhaseObject;

    //Le canvas en jeu pour la phase d'activation
    [SerializeField] private GameObject _canvasActivation = null;
    public GameObject CanvasActivation => _canvasActivation;

    [Header("Cartes �v�nements")]
    //L'objet d'event � afficher lorsqu'une nouvelle carte event est pioch�e
    [SerializeField] private GameObject _eventCardObject = null;
    public GameObject EventCardObject => _eventCardObject;


}
