using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EasyButtons;

/*
Ce Script g�re la phase d'activation : le choix de la carte d'activation, la confirmation du choix pour chaque joueur,
ainsi que la comparaison entre les deux cartes choisies pour savoir quelle joueur a l'initiative et combien d'unit� chaque joueur peut activer.
Ce script renvoie comme principals informations :
- une bool IsPlayer1Starting pour savoir qui commence
- une float J1Derni�reValeurActivation et une float J2Derni�reValeurActivation pour savoir le montant d'unit� activable pendant le tour
- Une list de CarteActivation CarteActivationUtilis�e pour savoir quelle carte activation ont �tait utilis�e depuis le d�but de la partie
*/

public class PhaseActivation : MonoBehaviour{
    //Variables pour le joueur avec les cartes bleu
    //Carte du joueur 1
    [SerializeField] private List<CarteActivation> RedCartesActivation = new List<CarteActivation>();

    //Panel qui contient les cartes Activation Bleu
    [SerializeField] private GameObject RedPlayerPanel;

    bool J1Choix = true;
    bool J1Verif = false;
    CarteActivation J1CarteVerif = null;

    List<CarteActivation> J1CartesNonVerif = new List<CarteActivation>();
    private float J1Derni�reValeurActivation;
    
    private bool _j1CarteChoisie = false;
    public bool J1CarteChoisie => _j1CarteChoisie;

    //Variables pour le joueur 2
    //Carte du joueur 2
    [SerializeField] List<CarteActivation> BlueCartesActivation = new List<CarteActivation>();
    
    //Panel qui contient les cartes Activation Rouge
    [SerializeField] private GameObject BluePlayerPanel;

    bool J2Choix = true;
    bool J2Verif = false;
    CarteActivation J2CarteVerif = null;
    List<CarteActivation> J2CartesNonVerif = new List<CarteActivation>();
    private float J2Derni�reValeurActivation;

    private bool _j2CarteChoisie = false;
    public bool J2CarteChoisie => _j2CarteChoisie;

    //Variables communes
    private List<CarteActivation> CarteActivationUtilis�e = new List<CarteActivation>();

    [SerializeField] private GameObject _result = null;

    bool _canChooseCard = false;

    private void Start(){
        UIInstance.Instance.UpdateActivationLeft();
        UIInstance.Instance.CanvasActivation.SetActive(false);
        GameManager.Instance.ManagerSO.GoToActivationPhase += ActivateActivationPhase;
        GameManager.Instance.ManagerSO.GoToActivationPhase += ResetPhaseActivation;
        GameManager.Instance.ManagerSO.GoToOrgoneJ1Phase += DesactivateActivationPhase;
        _result.SetActive(false);
    }

    void Update(){
        if(_canChooseCard)
        {
            //Joueur 1
            if(!_j1CarteChoisie){
                //On v�rifie si le joueur 1 choisit sa carte
                if(J1Verif && Input.GetKeyDown(J1CarteVerif.inputCarteActivation)){
                    foreach(CarteActivation Carteactivations in RedCartesActivation){
                        RedPlayerPanel.transform.GetChild(Carteactivations.IndexCarteActivation).GetComponent<Image>().color = new Color(1f, 0f, 0f, 1f);
                    }

                    J1Derni�reValeurActivation = float.Parse(J1CarteVerif.valeurActivation) / 10;
                    CarteActivationUtilis�e.Add(J1CarteVerif);
                    RedCartesActivation.Clear();

                    foreach(CarteActivation carteactivations in J1CartesNonVerif){
                        RedCartesActivation.Add(carteactivations);
                    }

                    //Le joueur 1 a choisit sa carte
                    _j1CarteChoisie = true;
                    if(CheckIfBothPlayerHasChoose()) ShowResult();

                    J1Verif = false;

                }

                else if(J1Verif){
                    if(Input.anyKeyDown)
                    {
                        foreach(CarteActivation Carteactivation in J1CartesNonVerif)
                        {
                            if(Input.GetKeyDown(Carteactivation.inputCarteActivation))
                            {
                                foreach(CarteActivation Carteactivations in RedCartesActivation)
                                {
                                    RedPlayerPanel.transform.GetChild(Carteactivations.IndexCarteActivation).GetComponent<Image>().color = new Color(1f, 0f, 0f, 1f);
                                }
                                J1Choix = true;
                                J1Verif = false;
                            }
                        }
                    }
                }

                //Premi�re partie du code qui s'execute, on regarde si le joueur appuie sur une touche,
                //et si c'est le cas on sauvegarde la carte qui correspond � la touche press�e et les autres dans deux list diff�rentes.
                //On passe alors � la phase de v�rification
                else if(J1Choix){
                    J1CarteVerif = null;
                    J1CartesNonVerif.Clear();

                    if(Input.anyKey)
                    {
                        foreach(CarteActivation carteactivation in RedCartesActivation)
                        {
                            if(Input.GetKeyDown(carteactivation.inputCarteActivation))
                            {
                                foreach(CarteActivation carteactivations in RedCartesActivation)
                                {
                                    Image image = RedPlayerPanel.transform.GetChild(carteactivations.IndexCarteActivation).GetComponent<Image>();
                                    image.color = new Color(0f, 1f, 0f, 1f);
                                    J1CarteVerif = carteactivation;
                                    if(carteactivations == J1CarteVerif) { }
                                    else if(carteactivations != J1CarteVerif)
                                    {
                                        J1CartesNonVerif.Add(carteactivations);
                                    }
                                }
                                J1Verif = true;
                                J1Choix = false;
                            }
                        }
                    }
                }
            }

            //Meme principe que pour le joueur 1 mais avec les variables que le joueur 2
            if(!_j2CarteChoisie){
                if(J2Verif && Input.GetKeyDown(J2CarteVerif.inputCarteActivation)){
                    foreach(CarteActivation Carteactivations in BlueCartesActivation){
                        BluePlayerPanel.transform.GetChild(Carteactivations.IndexCarteActivation).GetComponent<Image>().color = new Color(0f, 0f, 1f, 1f);
                    }

                    J2Derni�reValeurActivation = float.Parse(J2CarteVerif.valeurActivation) / 10;
                    CarteActivationUtilis�e.Add(J2CarteVerif);
                    BlueCartesActivation.Clear();
                  
                    foreach(CarteActivation carteactivations in J2CartesNonVerif){
                        BlueCartesActivation.Add(carteactivations);
                    }

                    //Le joueur 2 a choisit sa carte
                    _j2CarteChoisie = true;
                    if(CheckIfBothPlayerHasChoose()) ShowResult();

                    J2Verif = false;
                }
                else if(J2Verif){

                    foreach(CarteActivation Carteactivation in J2CartesNonVerif){
                        if(Input.GetKeyDown(Carteactivation.inputCarteActivation)){
                            foreach(CarteActivation Carteactivations in BlueCartesActivation)
                            {
                                BluePlayerPanel.transform.GetChild(Carteactivations.IndexCarteActivation).GetComponent<Image>().color = new Color(0f, 0f, 1f, 1f);
                            }
                            J2Choix = true;
                            J2Verif = false;
                           
                        }
                    }
                }

                else if(J2Choix){

                    J2CarteVerif = null;
                    J2CartesNonVerif.Clear();
                    foreach(CarteActivation carteactivation in BlueCartesActivation)
                    {
                        if(Input.GetKeyDown(carteactivation.inputCarteActivation)){
                            foreach(CarteActivation carteactivations in BlueCartesActivation)
                            {
                                Image image = BluePlayerPanel.transform.GetChild(carteactivations.IndexCarteActivation).GetComponent<Image>();
                                image.color = new Color(0f, 1f, 0f, 1f);
                                J2CarteVerif = carteactivation;
                                if(carteactivations == J2CarteVerif){}
                                else if(carteactivations != J2CarteVerif){
                                    J2CartesNonVerif.Add(carteactivations);
                                }
                            }

                            J2Verif = true;
                            J2Choix = false;
                        }
                    }
                }

            }
        }
    }

    /// <summary>
    /// Reset la phase d'activation
    /// </summary>
    public void ResetPhaseActivation(){
        //Variables pour le joueur 1
        J1CartesNonVerif.Clear();
        J1CarteVerif = null;
        RedPlayerPanel.SetActive(true);
        J1Choix = true;
        J1Verif = false;

        J1Derni�reValeurActivation = 0f;
        _j1CarteChoisie = false;

        //Variables pour le joueur 2
        J2CarteVerif = null;
        J2CartesNonVerif.Clear();
        BluePlayerPanel.SetActive(true);
        J2Choix = true;
        J2Verif = false;

        J2Derni�reValeurActivation = 0f;
        _j2CarteChoisie = false;

        UIInstance.Instance.BackgroundActivation.SetActive(true);

        _result.SetActive(false);
    }

    /// <summary>
    /// Detecte si les deux joueurs ont choisit leur carte activation
    /// </summary>
    /// <returns></returns>
    public bool CheckIfBothPlayerHasChoose(){
        bool bothHasPlay = false;
        bothHasPlay = _j1CarteChoisie && _j2CarteChoisie ? true : false;
        return bothHasPlay;
    }

    /// <summary>
    /// Lorsque les joueurs vont cliquer sur le bouton pour aller � la phase suivante
    /// </summary>
    public void ShowResult(){
        UIInstance.Instance.BackgroundActivation.SetActive(false);

        float InitiativeValeur = J1Derni�reValeurActivation - J2Derni�reValeurActivation;
        if(InitiativeValeur < 0){
            GameManager.Instance.SetPlayerStart(true);
            RedPlayerPanel.transform.GetChild(J1CarteVerif.IndexCarteActivation).GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
            BluePlayerPanel.transform.GetChild(J2CarteVerif.IndexCarteActivation).GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
            _result.SetActive(true);
        }
        else{
            GameManager.Instance.SetPlayerStart(false);
            RedPlayerPanel.transform.GetChild(J1CarteVerif.IndexCarteActivation).GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
            BluePlayerPanel.transform.GetChild(J2CarteVerif.IndexCarteActivation).GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
            _result.SetActive(true);
        }

        RedPlayerPanel.SetActive(false);
        BluePlayerPanel.SetActive(false);

        PlayerScript.Instance.RedPlayerInfos.ActivationLeft = (int) J1Derni�reValeurActivation;
        PlayerScript.Instance.BluePlayerInfos.ActivationLeft = (int) J2Derni�reValeurActivation;
        UIInstance.Instance.UpdateActivationLeft();

        UIInstance.Instance.ActivateNextPhaseButton();
    }

    /// <summary>
    /// Passe � la phase suivante
    /// </summary>
    public void DesactivateActivationPhase(){
        UIInstance.Instance.CanvasActivation.SetActive(false);
        _canChooseCard = false;
    }

    public void ActivateActivationPhase()
    {
        UIInstance.Instance.CanvasActivation.SetActive(true);
        UIInstance.Instance.DesactivateNextPhaseButton();
        _canChooseCard = true;
    }
}