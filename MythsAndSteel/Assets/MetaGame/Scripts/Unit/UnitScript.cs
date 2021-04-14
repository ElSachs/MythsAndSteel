﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EasyButtons;

[RequireComponent(typeof(SpriteRenderer))]
public class UnitScript : MonoBehaviour
{
    #region Variables
    [Header("--------------- STATS DE BASE DE L'UNITE ---------------")]
    //Scriptable qui contient les stats de base de l'unité
    [SerializeField] Unit_SO _unitSO;
    public Unit_SO UnitSO => _unitSO;

    [Header("------------------- VIE -------------------")]
    [Header("------------------- STAT EN JEU -------------------")]
    //Vie actuelle
    [SerializeField] int _life;
    public int Life => _life;

    // Bouclier actuelle
    [SerializeField] int _shield;
    public int Shield => _shield;

    [Header("-------------------- ATTAQUE -------------------")]
    //Portée
    [SerializeField] int _attackRange;
    public int AttackRange => _attackRange;
    public int AttackRangeBonus = 0;

    [Space]
    //Dégats minimum infligé
    [SerializeField] Vector2 _numberRangeMin;
    public Vector2 NumberRangeMin => _numberRangeMin;
    [SerializeField] int _damageMinimum;
    public int DamageMinimum => _damageMinimum;

    [Space]
    //Dégats maximum infligé
    [SerializeField] Vector2 _numberRangeMax;
    public Vector2 NumberRangeMax => _numberRangeMax;
    [SerializeField] int _damageMaximum;
    public int DamageMaximum => _damageMaximum;

    //Dégât bonus
    [SerializeField] int _damageBonus;
    public int DamageBonus => _damageBonus;

    //Bonus aux lancés de dé
    [SerializeField] private int _diceBonus = 0;
    public int DiceBonus => _diceBonus;


    [Header("------------------- DEPLACEMENT -------------------")]
    //Vitesse de déplacement
    [SerializeField] int _moveSpeed;
    public int MoveSpeed => _moveSpeed;
    public int MoveSpeedBonus = 0;

    [Header("------------------- CAPACITES -------------------")]
    public Capacity[] ListeDesCapacites;

    [Header("------------------- COUT DE CREATION -------------------" )]
    // Coût de création
    [SerializeField] int _creationCost;
    public int CreationCost => _creationCost;

    [Header("------------------- DEPLACEMENT RESTANT -------------------")]
    // Déplacement réstant de l'unité durant cette activation
    [SerializeField] int _moveLeft;
    public int MoveLeft
    {
        get
        {
            return _moveLeft;
        }
        set
        {
            _moveLeft = value;
        }
    }

    [Header("------------------- CASE DE L'UNITE -------------------")]
    //Valeur (id) de la case sur laquelle se trouve l'unité
    [SerializeField] int _actualTileld;
    public int ActualTiledId
    {
        get
        {
            return _actualTileld;
        }
        set
        {
            _actualTileld = value;
        }
    }

    //déplacement actuel de l'unité pour la fonction "MoveWithPath"
    int _i;
    public int i => _i;

    [Header("------------------- ACTIVATION UNITE -------------------")]
    //A commencer à se déplacer
    public bool _hasStartMove = false;

    //lorsque le joueur a fini d'utiliser tous ses points de déplacement
    [SerializeField] bool _isMoveDone;
    public bool IsMoveDone => _isMoveDone;

    //lorsque le joueur a effectué soit une attaque soit un pouvoir actif
    public bool _isActionDone;

    //lorsque l'activation a totalement été finie
    [SerializeField] bool _isActivationDone;
    public bool IsActivationDone => _isActivationDone;

    [Header("------------------- CHEMIN DE DEPLACEMENT -------------------")]
    //Chemin que l'unité va emprunter
    [SerializeField] List<int> _pathtomake;
    public List<int> Pathtomake => _pathtomake;

    [Header("------------------- STAUT DE L'UNITE -------------------")]
    //Statut que possède l'unité
    [SerializeField] private List<MYthsAndSteel_Enum.UnitStatut> _unitStatus = new List<MYthsAndSteel_Enum.UnitStatut>();
    public List<MYthsAndSteel_Enum.UnitStatut> UnitStatus => _unitStatus;

    bool hasUseActivation = false;

    #endregion Variables

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            GiveLife(1);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            TakeDamage(1);
        }
    }

    #region LifeMethods
    /// <summary>
    /// Rajoute de la vie au joueur
    /// </summary>
    /// <param name="Lifeadd"></param>
    public virtual void GiveLife(int Lifeadd)
    {
        _life += Lifeadd;
        if(_life > UnitSO.LifeMax){
            int shieldPlus = _life - UnitSO.LifeMax;
            _life = UnitSO.LifeMax;
            _shield += shieldPlus;
        }
    }

    /// <summary>
    /// Fait perdre de la vie au joueur
    /// </summary>
    /// <param name="Damage"></param>
    public virtual void TakeDamage(int Damage)
    {
        if(_shield > 0){
            _shield -= Damage;
            _life += _shield;
            CheckLife();
        }
        else
        {
            _life -= Damage;
            CheckLife();
        }

        if(TilesManager.Instance.TileList[ActualTiledId].GetComponent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.OrgoneRed)){
            PlayerScript.Instance.AddOrgone(1, 1);
        }
        else if(TilesManager.Instance.TileList[ActualTiledId].GetComponent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.OrgoneBlue)){
            PlayerScript.Instance.AddOrgone(1, 2);
        }
        else { }
    }

    /// <summary>
    /// Check la vie du joueur
    /// </summary>
    void CheckLife()
    {
        if (_life <= 0)
        {
            Death();
        }
    }

    /// <summary>
    /// Tue l'unité
    /// </summary>
    public virtual void Death()
    {
        if(TilesManager.Instance.TileList[ActualTiledId].GetComponent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.OrgoneRed)){
            PlayerScript.Instance.AddOrgone(1, 1);
        }
        else if(TilesManager.Instance.TileList[ActualTiledId].GetComponent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.OrgoneBlue)){
            PlayerScript.Instance.AddOrgone(1, 2);
        }
        else { }
        
        if(UnitSO.IsInRedArmy) PlayerScript.Instance.UnitRef.UnitListRedPlayer.Remove(this.gameObject);
        else PlayerScript.Instance.UnitRef.UnitListBluePlayer.Remove(this.gameObject);

        Destroy(gameObject);
        Debug.Log("Unité Détruite");
    }
    #endregion LifeMethods

    #region Statut
    public void AddStatutToUnit(MYthsAndSteel_Enum.UnitStatut stat){
        _unitStatus.Add(stat);
    }

    #endregion Statut

    #region ChangementStat
    /// <summary>
    /// Ajoute des dégâts supplémentaires aux unités
    /// </summary>
    /// <param name="value"></param>
    public void AddDamageToUnit(int value){
        _damageBonus += value;
    }

    /// <summary>
    /// Ajout une valeur aux lancés de dés de l'unité
    /// </summary>
    /// <param name="value"></param>
    public void AddDiceToUnit(int value){
        _diceBonus += value;
    }
    #endregion ChangementStat

    [Button]
    /// <summary>
    /// Update les stats de l'unité avec les stats de base
    /// </summary>
    public virtual void UpdateUnitStat()
    {
        //Si il n'y a pas de scriptable object alors ca arrete la fonction
        if (_unitSO == null) return;

        //Assigne les stats
        _life = _unitSO.LifeMax;
        _shield = 0;
        _attackRange = _unitSO.AttackRange;
        _moveSpeed = _unitSO.MoveSpeed;
        _creationCost = _unitSO.CreationCost;
        _damageMinimum = _unitSO.DamageMinimum;
        _damageMaximum = _unitSO.DamageMaximum;
        _numberRangeMax = _unitSO.NumberRangeMax;
        _numberRangeMin = _unitSO.NumberRangeMin;

        //Assigne le sprite de l'unité
        GetComponent<SpriteRenderer>().sprite = _unitSO.Sprite;

        ResetTurn();
    }

    /// <summary>
    /// Reset les valeurs nécéssaires pour un nouveau tour
    /// </summary>
    public virtual void ResetTurn(){
        _isActivationDone = false;
        _isMoveDone = false;
        _isActionDone = false;

        MoveSpeedBonus = 0;
        AttackRangeBonus = 0;

        hasUseActivation = false;
        _moveLeft = _unitSO.MoveSpeed;
        _hasStartMove = false;
    }

    /// <summary>
    /// Check si l'unité peut encore se déplacer
    /// </summary>
    public void checkMovementLeft()
    {
        if(UnitSO.IsInRedArmy && !hasUseActivation)
        {
            hasUseActivation = true;
            PlayerScript.Instance.RedPlayerInfos.ActivationLeft--;
        }
        else
        {
            hasUseActivation = true;
            PlayerScript.Instance.BluePlayerInfos.ActivationLeft--;
        }
         
        UIInstance.Instance.UpdateActivationLeft();

        if (_moveLeft == 0)
        {
            _isMoveDone = true;
        }
    }

    /// <summary>
    /// Check si l'unité peut encore être activée
    /// </summary>
    public void checkActivation()
    {
        if (_isActionDone){
            _isActivationDone = true;

            //Réduit le nombre d'activation restante
            if(_unitSO.IsInRedArmy || (!_unitSO.IsInRedArmy && _unitStatus.Contains(MYthsAndSteel_Enum.UnitStatut.Possédé)))
            {
                if(!_hasStartMove) PlayerScript.Instance.RedPlayerInfos.ActivationLeft--;
                UIInstance.Instance.UpdateActivationLeft();
            }
            else if(!_unitSO.IsInRedArmy || (_unitSO.IsInRedArmy && _unitStatus.Contains(MYthsAndSteel_Enum.UnitStatut.Possédé)))
            {
                if(!_hasStartMove) PlayerScript.Instance.BluePlayerInfos.ActivationLeft--;
                UIInstance.Instance.UpdateActivationLeft();
            }
        }
    }
}

