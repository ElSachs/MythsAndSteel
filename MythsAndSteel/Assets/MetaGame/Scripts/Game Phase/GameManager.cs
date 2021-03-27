using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*
    Ce script est le Game Manager. Il va g�rer toutes les phases du jeu, les diff�rents tours de jeu, ...
    Il sera aussi la pour changer de phase, changer de tour, ...
    Il est indispensable au jeu!!!

    NE LE SUPPRIMEZ PAS DE VOTRE SCENE!!!
*/

public class GameManager : MonoSingleton<GameManager>{

    #region Variables
    [Header("Info du tour actuel")]
    //Correspond � la valeur du tour actuel
    [SerializeField] private int _actualTurnNumber = 0;
    public int ActualTurnNumber => _actualTurnNumber;

    //Permet de savoir si c'est le joueur 1 (TRUE) ou le joueur 2 (FALSE) qui commence durant ce tour
    [SerializeField] private bool _isPlayer1Starting = false;
    public bool IsPlayer1Starting => _isPlayer1Starting;

    //Permet de savoir si c'est le joueur 1 (TRUE) ou le joueur 2 (FALSE) qui joue actuellement
    [SerializeField] private bool _isPlayer1Turn = false;
    public bool IsPlayer1Turn => _isPlayer1Turn;

    //Est ce que les joueurs sont actuellement dans un tour de jeu?
    [SerializeField] private bool _isInTurn = false;
    public bool IsInTurn => _isInTurn;


    [Header("Info de la phase de jeu actuelle")]
    //Correspond � la phase actuelle durant le tour
    [SerializeField] private MYthsAndSteel_Enum.PhaseDeJeu _actualTurnPhase = MYthsAndSteel_Enum.PhaseDeJeu.Debut;
    public MYthsAndSteel_Enum.PhaseDeJeu ActualTurnPhase => _actualTurnPhase;

    //Le panneau qui est affich� sur l'�cran du joueur
    GameObject actualSwitchPhasePanel = null;


    [Header("Les R�f�rences des phases")]
    //Objet pour la phase d'activation
    [SerializeField] private GameObject _phaseActivationObj = null;

    //Event pour quand le joueur clique sur un bouton pour passer � la phase suivante
    public delegate void ClickButtonSwitchPhase();
    public event ClickButtonSwitchPhase OnClicked;
    #endregion Variables

    /// <summary>
    /// Permet d'initialiser le script
    /// </summary>
    private void Start(){
        OnClicked += OnclickedEvent;
        _isInTurn = true;
    }

    /// <summary>
    /// Fonction qui se lance uniquement lorsque le jeu est lanc� dans l'�diteur de unity
    /// </summary>
//#if UNITY_EDITOR
    private void Update(){
        if(Input.GetKeyDown(KeyCode.A) && actualSwitchPhasePanel == null && _actualTurnPhase != MYthsAndSteel_Enum.PhaseDeJeu.Activation){
            NextPhase();
        }

        if(Input.GetKeyDown(KeyCode.E) && actualSwitchPhasePanel == null && _actualTurnPhase != MYthsAndSteel_Enum.PhaseDeJeu.Activation){
            int randomPhase = Random.Range(0, 7);
            GoToPhase((MYthsAndSteel_Enum.PhaseDeJeu) randomPhase);
        }
    }
//#endif

    /// <summary>
    /// Obtenir la phase suivante et appeler la fonction qui permet de passer � la phase suivante
    /// </summary>
    public void NextPhase(){
        //Creation de la variable de phase de jeu suivante
        MYthsAndSteel_Enum.PhaseDeJeu nextPhase = MYthsAndSteel_Enum.PhaseDeJeu.Debut;

        //Savoir l'id de la phase de jeu actuelle
        int idPhaseActuel = (int)_actualTurnPhase;

        //Id de la phase de jeu suivante correpsond � l'id de la phase de jeu actuelle + 1
        int idPhaseSuivante = idPhaseActuel + 1;
        
        //Check si le joueur reste dans les bonnes phases
        if(idPhaseSuivante > 6){
            if(_actualTurnNumber < 11){
                NextTurn(true);
            } else {
                VictoryForArmy(1);
            }
            return;
        }
        //Retourner l'id de la phase suivante dans l'enum
        nextPhase = (MYthsAndSteel_Enum.PhaseDeJeu) idPhaseSuivante;

        //Aller a la phase obtenue
        GoToPhase(nextPhase);
    }

    /// <summary>
    /// Aller � la phase de jeu renseigner en param�tre
    /// </summary>
    /// <param name="phaseToGoTo"></param>
    public void GoToPhase(MYthsAndSteel_Enum.PhaseDeJeu phaseToGoTo){
        //Change le statut de la phase de jeu
        _actualTurnPhase = phaseToGoTo;
        _isInTurn = false;

        //Affiche le panneau qui montre au joueur qu'il a chang� de phase
        SwitchPhaseObjectUI(false);

        //Selon la phase effectue certaines actions
        switch(_actualTurnPhase){
            case MYthsAndSteel_Enum.PhaseDeJeu.Debut:
                break;

            case MYthsAndSteel_Enum.PhaseDeJeu.Activation:
                OnClicked += PhaseActivation;
                break;

            case MYthsAndSteel_Enum.PhaseDeJeu.OrgoneJ1:
                break;

            case MYthsAndSteel_Enum.PhaseDeJeu.ActionJ1:
                break;

            case MYthsAndSteel_Enum.PhaseDeJeu.OrgoneJ2:
                break;

            case MYthsAndSteel_Enum.PhaseDeJeu.ActionJ2:
                break;

            case MYthsAndSteel_Enum.PhaseDeJeu.Strategie:
                break;

        }
    }

    /// <summary>
    /// Permet d'aller au tour suivant. Soit au d�but du tour suivant soit � la m�me phase mais au prochain tour
    /// </summary>
    /// <param name="goToBeginningOfTurn"></param>
    public void NextTurn(bool goToBeginningOfTurn){
        _actualTurnNumber++;

        if(goToBeginningOfTurn) {
            GoToPhase(MYthsAndSteel_Enum.PhaseDeJeu.Debut);
        }
        else { }
    }

    /// <summary>
    /// Donne la victoire � une arm�e
    /// </summary>
    /// <param name="armeeGagnante"></param>
    public void VictoryForArmy(int armeeGagnante){
        Debug.Log($"Arm�e {armeeGagnante} a gagn�");
    }

    /// <summary>
    /// Fonction qui est appell�e lorsque l'event est appell� (event lors du clic sur le bouton pour passer � la phase suivante)
    /// </summary>
    public void OnclickedEvent(){
        //Les joueurs peuvent � nouveau jouer
        _isInTurn = true;

        //D�truit le panneau de changement de phase
        SwitchPhaseObjectUI(true);

        //Supprime toutes les fonctions dans l'event
        OnClicked = null;
        OnClicked += OnclickedEvent;
    }

    /// <summary>
    /// Permet d'appeler l'event pour passer a une autre phase
    /// </summary>
    public void CallEventToSwitchPhase(){
        OnClicked();
    }

    #region UIFunction
    /// <summary>
    /// Affiche le panneau d'indication de changement de phase. Les joueurs doivent cliquer sur un bouton pour passer la phase
    /// </summary>
    public void SwitchPhaseObjectUI(bool destroy){
        //Ajoute le menu o� il faut cliquer
        if(!destroy) {
            //Instantie le panneau de transition entre deux phases et le garde en m�moire
            GameObject phaseObj = Instantiate(UIInstance.Instance.SwitchPhaseObject, UIInstance.Instance.CanvasTurnPhase.transform.position, 
                                              Quaternion.identity, UIInstance.Instance.CanvasTurnPhase.transform);
            actualSwitchPhasePanel = phaseObj;

            //Variable qui permet d'avoir le texte � afficher au d�but de la phase
            string textForSwitch = "";

            if(ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.Debut) textForSwitch = "Tour suivant : Vous passez � la phase" + " " + _actualTurnPhase.ToString();
            else textForSwitch = "Vous passez � la phase" + " " + _actualTurnPhase.ToString();

            //Change le texte en jeu
            phaseObj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = textForSwitch;
        }

        //Supprime le menu o� il faut cliquer
        else {
            if(actualSwitchPhasePanel != null){
                Destroy(actualSwitchPhasePanel);
            }
        }
    }
    #endregion UIFunction

    /// <summary>
    /// Permet d'avoir en r�f�rence si c'est le joueur 1 qui commence ou le joueur 2
    /// </summary>
    /// <param name="player1"></param>
    public void SetPlayerStart(bool player1){
        _isPlayer1Starting = player1;
    }


    #region Phase
    /// <summary>
    /// Ajoute la phase d'activation � l'event
    /// </summary>
    public void PhaseActivation(){
        UIInstance.Instance.UiManager.ActivateActivationPhase(true);
        OnClicked += _phaseActivationObj.GetComponent<PhaseActivation>().ResetPhaseActivation;
    }


    #endregion Phase

}
