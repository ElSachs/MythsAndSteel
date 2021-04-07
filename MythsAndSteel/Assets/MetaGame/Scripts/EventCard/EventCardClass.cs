using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName ="Event Scriptable")]
public class EventCardClass : ScriptableObject{
    //Nombre de cartes events
    [SerializeField] private int _numberOfEventCard = 0;
    public int NumberOfEventCard => _numberOfEventCard;

    //Liste des cartes events
    [SerializeField] private List<EventCard> _eventCardList = new List<EventCard>();
    public List<EventCard> EventCardList => _eventCardList;

    [SerializeField] private float _spaceBetweenTwoEvents = 0f;

    int _redPlayerPos = 0;
    int _bluePlayerPos = 0;

    private void OnValidate(){
        int number = 0;
        foreach(EventCard card in _eventCardList){
            if(card._isEventInFinalGame){
                number++;
            }
        }

        _numberOfEventCard = number;
    }

    #region RemoveEvent
    /// <summary>
    /// Eneleve la carte event d'un joueur
    /// </summary>
    /// <param name="ev"></param>
    void RemoveEvents(MYthsAndSteel_Enum.EventCard ev){
        if(PlayerScript.Instance.EventCardList._eventCardRedPlayer.Contains(ev)){
            PlayerScript.Instance.EventCardList._eventCardRedPlayer.Remove(ev);

            foreach(GameObject gam in PlayerScript.Instance.EventCardList._eventGamRedPlayer){
                if(gam.GetComponent<EventCardContainer>().EventCardInfo._eventType == ev){
                    RemoveEventGam(gam, 1);
                    break;
                }
            }
        }
        else{
            PlayerScript.Instance.EventCardList._eventCardBluePlayer.Remove(ev);

            foreach(GameObject gam in PlayerScript.Instance.EventCardList._eventGamBluePlayer){
                if(gam.GetComponent<EventCardContainer>().EventCardInfo._eventType == ev){
                    RemoveEventGam(gam, 2);
                    break;
                }
            }
        }
    }

    /// <summary>
    /// d�truit une carte event
    /// </summary>
    /// <param name="gam"></param>
    void RemoveEventGam(GameObject gam, int player){
        if(player == 1){
            PlayerScript.Instance.EventCardList._eventGamRedPlayer.Remove(gam);
            Destroy(gam);
        }
        else if(player == 2){
            PlayerScript.Instance.EventCardList._eventGamBluePlayer.Remove(gam);
            Destroy(gam);
        }
        else{
            Debug.LogError("Vous essayez d'enlever une carte event a un joueur qui n'existe pas");
        }

        UpdateVisualUI(PlayerScript.Instance.EventCardList._eventGamRedPlayer, 1);
        UpdateVisualUI(PlayerScript.Instance.EventCardList._eventGamBluePlayer, 2);
    }
    #endregion RemoveEvent

    #region UpdateEventUI
    /// <summary>
    /// Met a jour la position des cartes events dans l'interface
    /// </summary>
    /// <param name="gam"></param>
    public void UpdateVisualUI(List<GameObject> gam, int player){
        if(player == 1){
            if(PlayerScript.Instance.EventCardList._eventCardRedPlayer.Count <= 3){
                ResetEventParentPos(1);

                UpdateEventList(gam, player);
                UpdateButtonPlayer(UIInstance.Instance.ButtonEventRedPlayer._upButton, UIInstance.Instance.ButtonEventRedPlayer._downButton, false, PlayerScript.Instance.EventCardList._eventGamRedPlayer.Count, _redPlayerPos);
            }
            else{
                UpdateEventList(gam, player);
                UpdateButtonPlayer(UIInstance.Instance.ButtonEventRedPlayer._upButton, UIInstance.Instance.ButtonEventRedPlayer._downButton, true, PlayerScript.Instance.EventCardList._eventGamRedPlayer.Count, _redPlayerPos);
            }
        }
        else if(player == 2)
        {
            if(PlayerScript.Instance.EventCardList._eventCardBluePlayer.Count <= 3){
                ResetEventParentPos(2);

                UpdateEventList(gam, player);
                UpdateButtonPlayer(UIInstance.Instance.ButtonEventBluePlayer._upButton, UIInstance.Instance.ButtonEventBluePlayer._downButton, false, PlayerScript.Instance.EventCardList._eventGamBluePlayer.Count, _bluePlayerPos);
            }
            else{
                UpdateEventList(gam, player);
                UpdateButtonPlayer(UIInstance.Instance.ButtonEventBluePlayer._upButton, UIInstance.Instance.ButtonEventBluePlayer._downButton, true, PlayerScript.Instance.EventCardList._eventGamBluePlayer.Count, _bluePlayerPos);
            }
        }
        else{
            Debug.LogError("Vous essayez d'd'update l'ui d'un joueur qui n'existe pas");
        }
    }
    
    /// <summary>
    /// Update la position des events dans la liste
    /// </summary>
    /// <param name="gam"></param>
    /// <param name="player"></param>
    void UpdateEventList(List<GameObject> gam, int player)
    {
        if(player == 1){
            if(_redPlayerPos == 0){
                //D�place les events � leurs bonnes positions
                gam[0].transform.position = UIInstance.Instance.RedEventDowntrans.position;

                if(gam.Count > 1){
                    for(int i = 1; i < gam.Count; i++){
                        gam[i].transform.position = new Vector3(gam[i - 1].transform.position.x,
                                                                gam[i - 1].transform.position.y + _spaceBetweenTwoEvents,
                                                                gam[i - 1].transform.position.z);
                    }
                }
            }
            else{
                if(gam.Count > 1){
                    for(int i = 1; i < gam.Count; i++){
                        gam[i].transform.position = new Vector3(gam[i - 1].transform.position.x,
                                                                gam[i - 1].transform.position.y + _spaceBetweenTwoEvents,
                                                                gam[i - 1].transform.position.z);
                    }
                }
            }
        }

        else if(player == 2){
            if(_bluePlayerPos == 0){
                //D�place les events � leurs bonnes positions
                gam[0].transform.position = UIInstance.Instance.BlueEventDowntrans.position;

                if(gam.Count > 1){
                    for(int i = 1; i < gam.Count; i++){
                        gam[i].transform.position = new Vector3(gam[i - 1].transform.position.x,
                                                                gam[i - 1].transform.position.y + _spaceBetweenTwoEvents,
                                                                gam[i - 1].transform.position.z);
                    }
                }
            }
            else{
                if(gam.Count > 1){
                    for(int i = 1; i < gam.Count; i++){
                        gam[i].transform.position = new Vector3(gam[i - 1].transform.position.x,
                                                                gam[i - 1].transform.position.y + _spaceBetweenTwoEvents,
                                                                gam[i - 1].transform.position.z);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Met a jour le visuel des boutons et leur interaction
    /// </summary>
    /// <param name="upButton"></param>
    /// <param name="downButton"></param>
    /// <param name="active"></param>
    void UpdateButtonPlayer(GameObject upButton, GameObject downButton, bool active, int numberOfCard, int pos){
        upButton.GetComponent<Image>().sprite = active ? pos == numberOfCard - 3? UIInstance.Instance.DesactivateButtonSprite : UIInstance.Instance.ActivateButtonSprite : UIInstance.Instance.DesactivateButtonSprite;
        upButton.GetComponent<Button>().interactable = pos == numberOfCard - 3 ? false : active;
        downButton.GetComponent<Image>().sprite = active ? pos == 0 ? UIInstance.Instance.DesactivateButtonSprite : UIInstance.Instance.ActivateButtonSprite : UIInstance.Instance.DesactivateButtonSprite;
        downButton.GetComponent<Button>().interactable = pos == 0 ? false : active;
    }

    /// <summary>
    /// Update la position du parent des cartes events d'un joueur
    /// </summary>
    void UpdateEventsParentPos(int player)
    {
        float multiplier = 1.17f;
        if(player == 1)
        {
            UIInstance.Instance.RedPlayerEventtransf.GetChild(0).localPosition = new Vector3(UIInstance.Instance.RedPlayerEventtransf.GetChild(0).localPosition.x, -_spaceBetweenTwoEvents * _redPlayerPos / multiplier, UIInstance.Instance.RedPlayerEventtransf.GetChild(0).localPosition.z);
        }
        else if(player == 2)
        {
            UIInstance.Instance.BluePlayerEventtransf.GetChild(0).localPosition = new Vector3(UIInstance.Instance.BluePlayerEventtransf.GetChild(0).localPosition.x, -_spaceBetweenTwoEvents * _bluePlayerPos / multiplier, UIInstance.Instance.BluePlayerEventtransf.GetChild(0).localPosition.z);
        }
        else
        {
            Debug.LogError("vous essayez de d�placer les cartes events d'un joueur qui n'existe pas");
        }
    }

    /// <summary>
    /// Reset la position du parent des cartes events d'un joueur
    /// </summary>
    /// <param name="player"></param>
    void ResetEventParentPos(int player)
    {
        if(player == 1)
        {
            _redPlayerPos = 0;
            UIInstance.Instance.RedPlayerEventtransf.GetChild(0).localPosition = Vector3.zero;
        }
        else if(player == 2)
        {
            _bluePlayerPos = 0;
            UIInstance.Instance.BluePlayerEventtransf.GetChild(0).localPosition = Vector3.zero;
        }
        else
        {
            Debug.LogError("vous essayez de d�placer les cartes events d'un joueur qui n'existe pas");
        }
    }
    #endregion UpdateEventUI

    #region ButtonInput
    /// <summary>
    /// Quand le joueur appuie sur le bouton pour monter dans la liste des cartes events
    /// </summary>
    /// <param name="player"></param>
    public void GoUp(int player){
        if(player == 1){
            _redPlayerPos++;
            UpdateEventsParentPos(1);
            UpdateButtonPlayer(UIInstance.Instance.ButtonEventRedPlayer._upButton, UIInstance.Instance.ButtonEventRedPlayer._downButton, true, PlayerScript.Instance.EventCardList._eventGamRedPlayer.Count, _redPlayerPos);
        }
        else if(player == 2){
            _bluePlayerPos++;
            UpdateEventsParentPos(2);
            UpdateButtonPlayer(UIInstance.Instance.ButtonEventBluePlayer._upButton, UIInstance.Instance.ButtonEventBluePlayer._downButton, true, PlayerScript.Instance.EventCardList._eventGamBluePlayer.Count, _bluePlayerPos);
        }
        else{
            Debug.LogError("vous essayez de d�placer les cartes events d'un joueur qui n'existe pas");
        }
    }

    /// <summary>
    /// Quand le joueur appuie pour descendre dans la liste des cartes events
    /// </summary>
    /// <param name="player"></param>
    public void GoDown(int player){
        if(player == 1){
            _redPlayerPos--;
            UpdateEventsParentPos(1);
            UpdateButtonPlayer(UIInstance.Instance.ButtonEventRedPlayer._upButton, UIInstance.Instance.ButtonEventRedPlayer._downButton, true, PlayerScript.Instance.EventCardList._eventGamRedPlayer.Count, _redPlayerPos);
        }
        else if(player == 2){
            _bluePlayerPos--;
            UpdateEventsParentPos(2);
            UpdateButtonPlayer(UIInstance.Instance.ButtonEventBluePlayer._upButton, UIInstance.Instance.ButtonEventBluePlayer._downButton, true, PlayerScript.Instance.EventCardList._eventGamBluePlayer.Count, _bluePlayerPos);
        }
        else{
            Debug.LogError("vous essayez de d�placer les cartes events d'un joueur qui n'existe pas");
        }
    }
    #endregion ButtonInput

    #region Evenement

    //A finir d�s qu'on peut ajouter une unit�
    #region D�ploiement Acc�l�r�
    public void D�ploiementAcc�l�r�()
    {
        foreach(GameObject tile in GameManager.Instance.TileChooseList){
            Debug.Log("Le jeu ajoute une infante");
        }

        GameManager.Instance.TileChooseList.Clear();

        RemoveEvents(MYthsAndSteel_Enum.EventCard.D�ploiement_acc�l�r�);
    }

    public void LaunchD�ploiementAcc�l�r�(){
        int player = DeterminArmy(MYthsAndSteel_Enum.EventCard.D�ploiement_acc�l�r�);
        List<GameObject> gamList = new List<GameObject>();
        
        //obtien les cases voisines pour chaque unit� de l'arm�e
        foreach(GameObject unit in player == 1? PlayerScript.Instance.UnitRef.UnitListRedPlayer : PlayerScript.Instance.UnitRef.UnitListBluePlayer){
            List<int> neighTile = PlayerStatic.GetNeighbourDiag(unit.GetComponent<UnitScript>().ActualTiledId, TilesManager.Instance.TileList[unit.GetComponent<UnitScript>().ActualTiledId].GetComponent<TileScript>().Line, false);

            //Check les effets de terrain pour voir si il doit ajouter la tile � la liste
            foreach(int i in neighTile){
                //Obtient la direction de la case par rapport � l'unit�
                string dir = PlayerStatic.CheckDirection(unit.GetComponent<UnitScript>().ActualTiledId, i);

                if(TilesManager.Instance.TileList[i].GetComponent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Ravin) || TilesManager.Instance.TileList[i].GetComponent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Eau)) {
                    //La tile n'est pas ajout�e
                }
                else { 
                    switch(dir){
                        case "Nord":
                            if(!TilesManager.Instance.TileList[i].GetComponent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Rivi�re_Sud)){
                                gamList.Add(TilesManager.Instance.TileList[i]);
                            }
                            break;
                        case "Sud":
                            if(!TilesManager.Instance.TileList[i].GetComponent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Rivi�re_Nord)){
                                gamList.Add(TilesManager.Instance.TileList[i]);
                            }
                            break;
                        case "Est":
                            if(!TilesManager.Instance.TileList[i].GetComponent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Rivi�re_Ouest)){
                                gamList.Add(TilesManager.Instance.TileList[i]);
                            }
                            break;
                        case "Ouest":
                            if(!TilesManager.Instance.TileList[i].GetComponent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Rivi�re_Est)){
                                gamList.Add(TilesManager.Instance.TileList[i]);
                            }
                            break;
                    }
                }
            }
        }

        if((GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ1 || GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ2) && 
            ((player == 1 && GameManager.Instance.IsPlayerRedTurn) || (player == 2 && !GameManager.Instance.IsPlayerRedTurn))){
            LaunchEventTile(1, player == 1 ? true : false, gamList);
            GameManager.Instance._eventCardCall += D�ploiementAcc�l�r�;
        }
    }
    #endregion Reprogrammation

    #region IllusionStrat�gique
    public void IllusionStrat�gique()
    {
        int firstTileId = GameManager.Instance.UnitChooseList[0].GetComponent<UnitScript>().ActualTiledId;

        TilesManager.Instance.TileList[GameManager.Instance.UnitChooseList[1].GetComponent<UnitScript>().ActualTiledId].GetComponent<TileScript>().AddUnitToTile(GameManager.Instance.UnitChooseList[0].gameObject);
        TilesManager.Instance.TileList[firstTileId].GetComponent<TileScript>().AddUnitToTile(GameManager.Instance.UnitChooseList[1].gameObject);

        GameManager.Instance.UnitChooseList.Clear();
        GameManager.Instance.IllusionStrat�gique = false;

        //Remove la carte event chez le bon joueur
        RemoveEvents(MYthsAndSteel_Enum.EventCard.Illusion_strat�gique);
    }

    public void LaunchIllusionStrat�gique()
    {
        int player = DeterminArmy(MYthsAndSteel_Enum.EventCard.Illusion_strat�gique);
        List<GameObject> unitList = new List<GameObject>();

        if(player == 1){
            foreach(GameObject gam in PlayerScript.Instance.UnitRef.UnitListRedPlayer)
            {
                if(gam.GetComponent<UnitScript>().IsActivationDone == true)
                {
                    unitList.Add(gam);
                }
            }

            unitList.AddRange(PlayerScript.Instance.UnitRef.UnitListBluePlayer);
        }
        else{
            foreach(GameObject gam in PlayerScript.Instance.UnitRef.UnitListBluePlayer)
            {
                if(gam.GetComponent<UnitScript>().IsActivationDone == true)
                {
                    unitList.Add(gam);
                }
            }

            unitList.AddRange(PlayerScript.Instance.UnitRef.UnitListRedPlayer);
        }


        GameManager.Instance.IllusionStrat�gique = true;

        if((GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ1 || GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ2) &&
            ((player == 1 && GameManager.Instance.IsPlayerRedTurn) || (player == 2 && !GameManager.Instance.IsPlayerRedTurn)))
        {
            LaunchEventUnit(2, player == 1 ? true : false, unitList);
            GameManager.Instance._eventCardCall += IllusionStrat�gique;
        }

        unitList.Clear();
    }
    #endregion IllusionStrat�gique

    #region OptimisationOrgone
    public void OptimisationOrgone()
    {
        int player = DeterminArmy(MYthsAndSteel_Enum.EventCard.Optimisation_de_l_orgone);
        if(player == 1){
            PlayerScript.Instance.RedPlayerInfos.OrgonePowerLeft++;
        }
        else{
            PlayerScript.Instance.BluePlayerInfos.OrgonePowerLeft++;
        }

        //Remove la carte event chez le bon joueur
        RemoveEvents(MYthsAndSteel_Enum.EventCard.Optimisation_de_l_orgone);
    }

    public void LaunchOptimisationOrgone()
    {
        int player = DeterminArmy(MYthsAndSteel_Enum.EventCard.Optimisation_de_l_orgone);

        if((GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.OrgoneJ1 || GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.OrgoneJ2) &&
            ((player == 1 && GameManager.Instance.IsPlayerRedTurn) || (player == 2 && !GameManager.Instance.IsPlayerRedTurn)))
        {
            GameManager.Instance._eventCardCall += IllusionStrat�gique;
        }
    }
    #endregion OptimisationOrgone

    #region PillageOrgone
    public void PillageOrgone()
    {
        int player = DeterminArmy(MYthsAndSteel_Enum.EventCard.Pillage_orgone);

        foreach(GameObject gam in GameManager.Instance.TileChooseList){
            gam.GetComponent<TileScript>().RemoveRessources(1, player);
        }

        GameManager.Instance.TileChooseList.Clear();

        //Remove la carte event chez le bon joueur
        RemoveEvents(MYthsAndSteel_Enum.EventCard.Pillage_orgone);
    }

    public void LaunchPillageOrgone()
    {
        int player = DeterminArmy(MYthsAndSteel_Enum.EventCard.Pillage_orgone);
        
        List<GameObject> tileList = new List<GameObject>();
        tileList.AddRange(TilesManager.Instance.ResourcesList);

        if((GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ1 || GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ2) &&
            ((player == 1 && GameManager.Instance.IsPlayerRedTurn) || (player == 2 && !GameManager.Instance.IsPlayerRedTurn)))
        {
            LaunchEventTile(2, player == 1 ? true : false, tileList);
            GameManager.Instance._eventCardCall += PillageOrgone;
        }

        tileList.Clear();
    }
    #endregion PillageOrgone

    #region PointeursLaser
    public void PointeursLaserOptimis�s()
    {
        foreach(GameObject unit in GameManager.Instance.UnitChooseList)
        {
            unit.GetComponent<UnitScript>().AttackRangeBonus += 1;
        }

        GameManager.Instance.UnitChooseList.Clear();

        //Remove la carte event chez le bon joueur
        RemoveEvents(MYthsAndSteel_Enum.EventCard.Pointeurs_laser_optimis�s);
    }

    public void LaunchPointeursLaserOptimis�s()
    {
        int player = DeterminArmy(MYthsAndSteel_Enum.EventCard.Pointeurs_laser_optimis�s);

        List<GameObject> unitList = new List<GameObject>();

        unitList.AddRange(player == 2 ? PlayerScript.Instance.UnitRef.UnitListBluePlayer : PlayerScript.Instance.UnitRef.UnitListRedPlayer);

        if((GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ1 || GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ2) &&
            ((player == 1 && GameManager.Instance.IsPlayerRedTurn) || (player == 2 && !GameManager.Instance.IsPlayerRedTurn)))
        {
            LaunchEventUnit(2, player == 1 ? true : false, unitList);
            GameManager.Instance._eventCardCall += PointeursLaserOptimis�s;
        }

        unitList.Clear();
    }
    #endregion PointeursLaser

    #region ArmeEpidemiologique
    public void ArmeEpidemiologique()
    {
        int player = DeterminArmy(MYthsAndSteel_Enum.EventCard.Arme_�pid�miologique);

        GameManager.Instance.UnitChooseList[0].GetComponent<UnitScript>().AddStatutToUnit(MYthsAndSteel_Enum.Statut.ArmeEpidemiologique);

        GameManager.Instance.UnitChooseList.Clear();

        //Remove la carte event chez le bon joueur
        RemoveEvents(MYthsAndSteel_Enum.EventCard.Arme_�pid�miologique);
    }

    public void LaunchArmeEpidemiologique()
    {
        int player = DeterminArmy(MYthsAndSteel_Enum.EventCard.Arme_�pid�miologique);

        List<GameObject> unitList = new List<GameObject>();
        unitList.AddRange(player == 1? PlayerScript.Instance.UnitRef.UnitListRedPlayer : PlayerScript.Instance.UnitRef.UnitListBluePlayer);

        if((GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ1 || GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ2) &&
            ((player == 1 && GameManager.Instance.IsPlayerRedTurn) || (player == 2 && !GameManager.Instance.IsPlayerRedTurn)))
        {
            LaunchEventUnit(1, player == 1 ? true : false, unitList);
            GameManager.Instance._eventCardCall += ArmeEpidemiologique;
        }

        unitList.Clear();
    }
    #endregion ArmeEpidemiologique

    #region ManeouvreStrat�gique
    public void ManoeuvreStrat�gique()
    {
        int player = DeterminArmy(MYthsAndSteel_Enum.EventCard.Manoeuvre_strat�gique);
        
        if(player == 1){
            PlayerScript.Instance.RedPlayerInfos.ActivationLeft++;
        }
        else{
            PlayerScript.Instance.BluePlayerInfos.ActivationLeft++;
        }

        //Remove la carte event chez le bon joueur
        RemoveEvents(MYthsAndSteel_Enum.EventCard.Manoeuvre_strat�gique);
    }

    public void LaunchManoeuvreStrat�gique()
    {
        int player = DeterminArmy(MYthsAndSteel_Enum.EventCard.Manoeuvre_strat�gique);

        if((GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ1 || GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ2) &&
            ((player == 1 && GameManager.Instance.IsPlayerRedTurn) || (player == 2 && !GameManager.Instance.IsPlayerRedTurn)))
        {
            GameManager.Instance._eventCardCall += ManoeuvreStrat�gique;
        }
    }
    #endregion ManeouvreStrat�gique

    #region SerumExp�rimental
    public void SerumExperimental()
    {
        foreach(GameObject unit in GameManager.Instance.UnitChooseList)
        {
            unit.GetComponent<UnitScript>().MoveSpeedBonus++;
        }

        GameManager.Instance.UnitChooseList.Clear();

        //Remove la carte event chez le bon joueur
        RemoveEvents(MYthsAndSteel_Enum.EventCard.S�rum_exp�rimental);
    }

    public void LaunchSerumExperimental()
    {
        int player = DeterminArmy(MYthsAndSteel_Enum.EventCard.S�rum_exp�rimental);

        List<GameObject> unitList = new List<GameObject>();

        unitList.AddRange(player == 2 ? PlayerScript.Instance.UnitRef.UnitListBluePlayer : PlayerScript.Instance.UnitRef.UnitListRedPlayer);

        if((GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ1 || GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ2) &&
            ((player == 1 && GameManager.Instance.IsPlayerRedTurn) || (player == 2 && !GameManager.Instance.IsPlayerRedTurn)))
        {
            LaunchEventUnit(2, player == 1 ? true : false, unitList);
            GameManager.Instance._eventCardCall += SerumExperimental;
        }

        unitList.Clear();
    }
    #endregion SerumExp�rimental

    #region ActivationDeNodus
    public void ActivationDeNodus()
    {
        int player = DeterminArmy(MYthsAndSteel_Enum.EventCard.Activation_de_nodus);
        if(player == 1){
            PlayerScript.Instance.RedPlayerInfos.OrgonePowerLeft++;
        }
        else{
            PlayerScript.Instance.BluePlayerInfos.OrgonePowerLeft++;
        }

        //Remove la carte event chez le bon joueur
        RemoveEvents(MYthsAndSteel_Enum.EventCard.Activation_de_nodus);
    }

    public void LaunchActivationDeNodus()
    {
        int player = DeterminArmy(MYthsAndSteel_Enum.EventCard.Activation_de_nodus);

        if((GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ1 || GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ2) &&
            ((player == 1 && GameManager.Instance.IsPlayerRedTurn) || (player == 2 && !GameManager.Instance.IsPlayerRedTurn)))
        {
            GameManager.Instance._eventCardCall += ActivationDeNodus;
        }
    }
    #endregion ActivationDeNodus

    #region BombardementAerien
    public void BombardementAerien()
    {
        GameManager.Instance.UnitChooseList[0].GetComponent<UnitScript>().TakeDamage(1);       

        LaunchDeplacementBombardement(GameManager.Instance.UnitChooseList[0]);
    }

    public void MoveUnitBombardement(){
        while(GameManager.Instance.UnitChooseList[0].transform.position != GameManager.Instance.TileChooseList[0].transform.position){
            GameManager.Instance.UnitChooseList[0].transform.position = Vector3.MoveTowards(GameManager.Instance.UnitChooseList[0].transform.position, GameManager.Instance.TileChooseList[0].transform.position, .7f);
            GameManager.Instance._waitEvent -= MoveUnitBombardement;
            GameManager.Instance._waitEvent += MoveUnitBombardement;
            GameManager.Instance.WaitToMove(.025f);
            return;
        }
        GameManager.Instance._waitEvent -= MoveUnitBombardement;
        TilesManager.Instance.TileList[GameManager.Instance.UnitChooseList[0].GetComponent<UnitScript>().ActualTiledId].GetComponent<TileScript>().RemoveUnitFromTile();
        GameManager.Instance.TileChooseList[0].GetComponent<TileScript>().AddUnitToTile(GameManager.Instance.UnitChooseList[0]);

        GameManager.Instance.TileChooseList.Clear();
        GameManager.Instance.UnitChooseList.Clear();

        RemoveEvents(MYthsAndSteel_Enum.EventCard.Bombardement_a�rien);
    }

    public void LaunchBombardementAerien()
    {
        int player = DeterminArmy(MYthsAndSteel_Enum.EventCard.Bombardement_a�rien);
        List<GameObject> unitList = new List<GameObject>();

        unitList.AddRange(player == 1 ? PlayerScript.Instance.UnitRef.UnitListBluePlayer : PlayerScript.Instance.UnitRef.UnitListRedPlayer);

        if((GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ1 || GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ2) &&
            ((player == 1 && GameManager.Instance.IsPlayerRedTurn) || (player == 2 && !GameManager.Instance.IsPlayerRedTurn)))
        {
            LaunchEventUnit(1, player == 1 ? true : false, unitList);
            GameManager.Instance._eventCardCall += BombardementAerien;
        }

        unitList.Clear();
    }

    void LaunchDeplacementBombardement(GameObject unit){
        int player = DeterminArmy(MYthsAndSteel_Enum.EventCard.Bombardement_a�rien);
        List<GameObject> tileList = new List<GameObject>();

        List<int> unitNeigh = PlayerStatic.GetNeighbourDiag(unit.GetComponent<UnitScript>().ActualTiledId, TilesManager.Instance.TileList[unit.GetComponent<UnitScript>().ActualTiledId].GetComponent<TileScript>().Line, false);
        foreach(int i in unitNeigh){
            tileList.Add(TilesManager.Instance.TileList[i]);
        }

        LaunchEventTile(1, player == 1 ? true : false, tileList);
        GameManager.Instance._eventCardCall += MoveUnitBombardement;

        tileList.Clear();
    }
    #endregion Reprogrammation

    #region Reprogrammation
    public void Reproggramation(){
        foreach(GameObject unit in GameManager.Instance.UnitChooseList){
            unit.GetComponent<UnitScript>().AddStatutToUnit(MYthsAndSteel_Enum.Statut.Poss�d�);
            unit.GetComponent<UnitScript>().AddDiceToUnit(-4);
        }

        GameManager.Instance.UnitChooseList.Clear();

        //Remove la carte event chez le bon joueur
        RemoveEvents(MYthsAndSteel_Enum.EventCard.Reprogrammation);
    }

    public void LaunchReproggramation(){
        int player = DeterminArmy(MYthsAndSteel_Enum.EventCard.Reprogrammation);
        List<GameObject> unitList = new List<GameObject>();

        unitList.AddRange(player == 1 ? PlayerScript.Instance.UnitRef.UnitListBluePlayer : PlayerScript.Instance.UnitRef.UnitListRedPlayer);

        if((GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ1 || GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ2) &&
            ((player == 1 && GameManager.Instance.IsPlayerRedTurn) || (player == 2 && !GameManager.Instance.IsPlayerRedTurn)))
        {
            LaunchEventUnit(1, player == 1? true : false, unitList);
            GameManager.Instance._eventCardCall += Reproggramation;
        }

        unitList.Clear();
    }
    #endregion Reprogrammation

    #region CessezLeFeu
    public void CessezLeFeu()
    {
        foreach(GameObject unit in GameManager.Instance.UnitChooseList)
        {
            unit.GetComponent<UnitScript>().AddStatutToUnit(MYthsAndSteel_Enum.Statut.PeutPasCombattre);
            unit.GetComponent<UnitScript>().AddStatutToUnit(MYthsAndSteel_Enum.Statut.Invincible);
            unit.GetComponent<UnitScript>().AddStatutToUnit(MYthsAndSteel_Enum.Statut.PeutPasPrendreDesObjectifs);
        }

        GameManager.Instance.UnitChooseList.Clear();

        //Remove la carte event chez le bon joueur
        RemoveEvents(MYthsAndSteel_Enum.EventCard.Cessez_le_feu);
    }

    public void LaunchCessezLeFeu()
    {
        int player = DeterminArmy(MYthsAndSteel_Enum.EventCard.Cessez_le_feu);
        List<GameObject> unitList = new List<GameObject>();

        unitList.AddRange(PlayerScript.Instance.UnitRef.UnitListBluePlayer);
        unitList.AddRange(PlayerScript.Instance.UnitRef.UnitListRedPlayer);

        if((GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ1 || GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ2) &&
            ((player == 1 && GameManager.Instance.IsPlayerRedTurn) || (player == 2 && !GameManager.Instance.IsPlayerRedTurn)))
        {
            LaunchEventUnit(1, player == 1 ? true : false, unitList);
            GameManager.Instance._eventCardCall += CessezLeFeu;
        }

        unitList.Clear();
    }
    #endregion Reprogrammation

    #region R�approvisionnement
    public void Reapprovisionnement()
    {
        foreach(GameObject unit in GameManager.Instance.UnitChooseList)
        {
            unit.GetComponent<UnitScript>().GiveLife(1);
        }

        GameManager.Instance.UnitChooseList.Clear();

        //Remove la carte event chez le bon joueur
        RemoveEvents(MYthsAndSteel_Enum.EventCard.R�approvisionnement);
    }

    public void LaunchReapprovisionnement()
    {
        int player = DeterminArmy(MYthsAndSteel_Enum.EventCard.R�approvisionnement);

        List<GameObject> unitList = new List<GameObject>();

        unitList.AddRange(player == 2 ? PlayerScript.Instance.UnitRef.UnitListBluePlayer : PlayerScript.Instance.UnitRef.UnitListRedPlayer);

        if((GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ1 || GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ2) &&
            ((player == 1 && GameManager.Instance.IsPlayerRedTurn) || (player == 2 && !GameManager.Instance.IsPlayerRedTurn)))
        {
            LaunchEventUnit(2, player == 1 ? true : false, unitList);
            GameManager.Instance._eventCardCall += Reapprovisionnement;
        }

        unitList.Clear();
    }
    #endregion R�approvisionnement

    #region ArmesPerforantes
    public void ArmesPerforantes()
    {
        foreach(GameObject unit in GameManager.Instance.UnitChooseList)
        {
            unit.GetComponent<UnitScript>().AddDamageToUnit(1);
        }

        GameManager.Instance.UnitChooseList.Clear();

        //Remove la carte event chez le bon joueur
        RemoveEvents(MYthsAndSteel_Enum.EventCard.Armes_perforantes);
    }

    public void LaunchArmesPerforantes()
    {
        int player = DeterminArmy(MYthsAndSteel_Enum.EventCard.Armes_perforantes);

        List<GameObject> unitList = new List<GameObject>();

        unitList.AddRange(player == 1 ? PlayerScript.Instance.UnitRef.UnitListRedPlayer : PlayerScript.Instance.UnitRef.UnitListBluePlayer);

        if((GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ1 || GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ2) &&
            ((player == 1 && GameManager.Instance.IsPlayerRedTurn) || (player == 2 && !GameManager.Instance.IsPlayerRedTurn)))
        {
            LaunchEventUnit(2, player == 1 ? true : false, unitList);
            GameManager.Instance._eventCardCall += ArmesPerforantes;
        }

        unitList.Clear();
    }
    #endregion ArmesPerforantes

    #region EntrainementRigoureux
    public void EntrainementRigoureux()
    {
        foreach(GameObject unit in GameManager.Instance.UnitChooseList)
        {
            unit.GetComponent<UnitScript>().AddDiceToUnit(3);
        }

        GameManager.Instance.UnitChooseList.Clear();

        //Remove la carte event chez le bon joueur
        RemoveEvents(MYthsAndSteel_Enum.EventCard.Entra�nement_rigoureux);
    }

    public void LaunchEntrainementRigoureux()
    {
        int player = DeterminArmy(MYthsAndSteel_Enum.EventCard.Entra�nement_rigoureux);

        List<GameObject> unitList = new List<GameObject>();

        unitList.AddRange(player == 1 ? PlayerScript.Instance.UnitRef.UnitListRedPlayer : PlayerScript.Instance.UnitRef.UnitListBluePlayer);

        if((GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ1 || GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ2) &&
            ((player == 1 && GameManager.Instance.IsPlayerRedTurn) || (player == 2 && !GameManager.Instance.IsPlayerRedTurn)))
        {
            LaunchEventUnit(1, player == 1 ? true : false, unitList);
            GameManager.Instance._eventCardCall += EntrainementRigoureux;
        }

        unitList.Clear();
    }
    #endregion EntrainementRigoureux

    /// <summary>
    /// Lance un �v�nement en appellant la fonction dans le gameManager pour choisir une unit�
    /// </summary>
    /// <param name="unitNumber"></param>
    /// <param name="opponent"></param>
    /// <param name="army"></param>
    /// <param name="redPlayer"></param>
    void LaunchEventUnit(int unitNumber, bool redPlayer, List<GameObject> unitList)
    {
        GameManager.Instance.StartEventModeUnit(unitNumber, redPlayer, unitList);
    }

    /// <summary>
    /// Lance un �v�nement en appellant la fonction dans le gameManager pour choisir une ou plusieurs cases du plateau
    /// </summary>
    /// <param name="numberOfTiles"></param>
    /// <param name="redPlayer"></param>
    /// <param name="gamList"></param>
    void LaunchEventTile(int numberOfTiles, bool redPlayer, List<GameObject> gamList){
        GameManager.Instance.StartEventModeTiles(numberOfTiles, redPlayer, gamList);
    }

    /// <summary>
    /// Determine � quelle arm�e appartient la carte
    /// </summary>
    int DeterminArmy(MYthsAndSteel_Enum.EventCard cardToFind){
        foreach(MYthsAndSteel_Enum.EventCard card in PlayerScript.Instance.EventCardList._eventCardRedPlayer)
        {
            if(card == cardToFind)
            {
                return 1;
            }
        }

        foreach(MYthsAndSteel_Enum.EventCard card in PlayerScript.Instance.EventCardList._eventCardBluePlayer)
        {
            if(card == cardToFind)
            {
                return 2;
            }
        }

        return 0;
    }
    #endregion Evenement
}

/// <summary>
/// Class qui regroupe toutes les variables pour une carte event
/// </summary>
[System.Serializable]
public class EventCard {
    public string _eventName = "";
    [TextArea] public string _description = "";
    public MYthsAndSteel_Enum.EventCard _eventType = MYthsAndSteel_Enum.EventCard.Activation_de_nodus;
    public int _eventCost = 0;
    public bool _isEventInFinalGame = true;
    public Sprite _eventSprite = null;
    public GameObject _effectToSpawn = null;
}
