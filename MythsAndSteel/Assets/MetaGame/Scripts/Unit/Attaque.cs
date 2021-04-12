using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Attaque : MonoSingleton<Attaque>
{
    #region Variables
    [SerializeField] private int[] neighbourValue; // +1 +9 +10...

    public List<int> _newNeighbourId => newNeighbourId;
    [SerializeField] private List<int> newNeighbourId = new List<int>(); // Voisins atteignables avec le range de l'unit�.

    public List<int> _selectedTileId => selectedTileId;
    [SerializeField] private List<int> selectedTileId = new List<int>(); // Cases selectionn�es par le joueur.

    //Est ce que l'unit� a commenc� � choisir son d�placement ?
    [SerializeField] private bool _isInAttack;
    public bool IsInAttack
    {
        get
        {
            return _isInAttack;
        }
        set
        {
            _isInAttack = value;
        }
    }

    //Est ce qu'une unit� est s�lectionn�e ?
    [SerializeField] private bool _selected;
    public bool Selected
    {
        get
        {
            return _selected;
        }
        set
        {
            _selected = value;
        }
    }

    // Vie de l'ennemi cibl� 
    [SerializeField] int _EnnemyLife;
    public int Life => _EnnemyLife;

    //Port�e d'attaque
    [SerializeField] int _attackRange;
    public int AttackRange => _attackRange;

    //D�gats minimum inflig�s 
    [SerializeField] int _damageMinimum;
    public int DamageMinimum => _damageMinimum;

    // Range d'attaque (-) 
    [SerializeField] Vector2 _numberRangeMin;
    public Vector2 NumberRangeMin => _numberRangeMin;

    // D�gats maximum inflig�s
    [SerializeField] int _damageMaximum;
    public int DamageMaximum => _damageMaximum;

    // Range d'attaque (+) 
    [SerializeField] Vector2 _numberRangeMax;
    public Vector2 NumberRangeMax => _numberRangeMax;

    // Range d'attaque (+) 
    [SerializeField] bool _isActionDone;
    public bool IsActionDone => _isActionDone;


    float firstDiceFloat, secondDiceFloat;
    int firstDiceInt, secondDiceInt, DiceResult;

    GameObject selectedUnit;
    GameObject selectedUnitEnnemy;

    [Header("SPRITES POUR LES CASES")]
    [SerializeField] private Sprite _selectedSprite = null;

    public Sprite selectedSprite
    {
        get
        {
            return _selectedSprite;
        }
    }

    #endregion Variables

    void Randomdice()
    {
        firstDiceFloat = Random.Range(1f, 6f);
        secondDiceFloat = Random.Range(1f, 6f);
        firstDiceInt = (int)firstDiceFloat;
        secondDiceInt = (int)secondDiceFloat;
        DiceResult = firstDiceInt + secondDiceInt + selectedUnit.GetComponent<UnitScript>().DiceBonus;
        Debug.Log("Dice Result : " + DiceResult);
    }

    void UnitAttackOneRange(Vector2 _numberRangeMin, int _damageMinimum, int DiceResult)
    {
        if (DiceResult >= _numberRangeMin.x && DiceResult <= _numberRangeMin.y)
        {
            selectedUnitEnnemy.GetComponent<UnitScript>().TakeDamage(_damageMinimum);
            Debug.Log("Damage : " + _damageMinimum);
        }
        if (DiceResult < _numberRangeMin.x)
        {
            Debug.Log("Damage : " + null);
            selectedUnitEnnemy.GetComponent<UnitScript>().TakeDamage(0);
        }
    }

    void UnitAttackTwoRanges(Vector2 _numberRangeMin, int _damageMinimum, Vector2 _numberRangeMax, int _damageMaximum, int DiceResult)
    {
        if (DiceResult >= _numberRangeMin.x && DiceResult <= _numberRangeMin.y)
        {
            selectedUnitEnnemy.GetComponent<UnitScript>().TakeDamage(_damageMinimum);
            Debug.Log("Damage : " + _damageMinimum);
        }
        if (DiceResult >= _numberRangeMax.x && DiceResult <= _numberRangeMax.y)
        {
            selectedUnitEnnemy.GetComponent<UnitScript>().TakeDamage(_damageMaximum);
            Debug.Log("Damage : " + _damageMaximum);
        }
        if (DiceResult < _numberRangeMin.x)
        {
            selectedUnitEnnemy.GetComponent<UnitScript>().TakeDamage(0);
            Debug.Log("Damage : " + null);
        }
    }

    void ChooseAttackType(Vector2 _numberRangeMin, int _damageMinimum, Vector2 _numberRangeMax, int _damageMaximum, int DiceResult)
    {
        if (_numberRangeMax.x == 0 && _numberRangeMax.y == 0)
        {
            UnitAttackOneRange(_numberRangeMin, _damageMinimum, DiceResult);
        }

        else
        {
            UnitAttackTwoRanges(_numberRangeMin, _damageMinimum, _numberRangeMax, _damageMaximum, DiceResult);
        }
    }

    public void Highlight(int tileId, int Range) // Highlight des cases dans la range d'attaque de l'unit�
    {
        if (Range > 0)
        {
            foreach (int ID in PlayerStatic.GetNeighbourDiag(tileId, TilesManager.Instance.TileList[tileId].GetComponent<TileScript>().Line, false))
            {
                TileScript TileSc = TilesManager.Instance.TileList[ID].GetComponent<TileScript>();
                bool i = false;

                if (TileSc.Unit != null)
                {
                    if (GameManager.Instance.IsPlayerRedTurn == TileSc.Unit.GetComponent<UnitScript>().UnitSO.IsInRedArmy)
                    {
                        i = true;
                    }
                }

                if (!i)
                {
                    TileSc.ActiveChildObj(MYthsAndSteel_Enum.ChildTileType.AttackSelect, _selectedSprite);
                    if (!newNeighbourId.Contains(ID))
                    {
                        newNeighbourId.Add(ID);
                    }
                    Highlight(ID, Range - 1);
                }
            }
        }
    }

    public void StartAttackSelectionUnit() // V�rifie si l'unit� selectionn� peut attaqu� + r�cup�re la port�e de l'unit�
    {
        GameObject tileSelected = RaycastManager.Instance.ActualTileSelected;

        if (tileSelected != null)
        {
            selectedUnit = tileSelected.GetComponent<TileScript>().Unit;
            if (!selectedUnit.GetComponent<UnitScript>()._isActionDone)
            {
                _selected = true;
                GetStats();
                StartAttack(TilesManager.Instance.TileList.IndexOf(tileSelected), selectedUnit.GetComponent<UnitScript>().AttackRange + selectedUnit.GetComponent<UnitScript>().AttackRangeBonus);
            }
            else
            {
                _selected = false;
            }
        }
        else
        {
            _selected = false;
        }
    }

    public void StartAttack(int tileId, int Range) // Pr�pare l'Highlight des tiles ciblables & passe le statut de l'unit� en -> _isInAttack
    {
        if (!_isInAttack)
        {
            _isInAttack = true;
            selectedTileId.Add(tileId);
            List<int> ID = new List<int>();
            ID.Add(tileId);

            // Lance l'highlight des cases dans la range de l'unit�.
            Highlight(tileId, Range);
        }
    }

    public void StopAttack() // Arr�te l'attaque de l'unit� select (UI + possibilit� d'attaquer) 
    {
        foreach (int Neighbour in newNeighbourId) // Supprime toutes les tiles.
        {
            if (TilesManager.Instance.TileList[Neighbour] != null)
            {
                TilesManager.Instance.TileList[Neighbour].GetComponent<TileScript>().DesActiveChildObj(MYthsAndSteel_Enum.ChildTileType.AttackSelect);
            }
        }

        // Clear de toutes les listes et stats
        selectedTileId.Clear();
        newNeighbourId.Clear();

        _isInAttack = false;
        _selected = false;

        DiceResult = 0;
        firstDiceFloat = 0f;
        secondDiceFloat = 0f;
        firstDiceInt = 0;
        secondDiceInt = 0;
        _attackRange = 0;
        _damageMinimum = 0;
        _damageMaximum = 0;
        _numberRangeMin.x = 0;
        _numberRangeMin.y = 0;
        _numberRangeMax.x = 0;
        _numberRangeMax.y = 0;

        RaycastManager.Instance.ActualTileSelected = null;

        Debug.Log("Attaque Stop");
    }

    public void Attack(int tileId)
    {
        GameObject TileSelectedForAttack = TilesManager.Instance.TileList[tileId];
        if (_isInAttack)
        {
            if (TileSelectedForAttack != null)
            {

                selectedUnitEnnemy = TileSelectedForAttack.GetComponent<TileScript>().Unit;
                if (selectedUnitEnnemy != null)
                {
                    _EnnemyLife = selectedUnitEnnemy.GetComponent<UnitScript>().Life;
                    ApplyAttack();
                }
                else
                {
                    StopAttack();
                }
            }
            else
            {
                StopAttack();
            }
        }
        else
        {
            StopAttack();
        }
    }

    public void GetStats()
    {
        _attackRange = selectedUnit.GetComponent<UnitScript>().AttackRange; // R�cup�ration de la Port�e
        _damageMinimum = selectedUnit.GetComponent<UnitScript>().DamageMinimum; // R�cup�ration des D�gats Maximum
        _damageMaximum = selectedUnit.GetComponent<UnitScript>().DamageMaximum; // D�gats Minimums
        _numberRangeMin.x = selectedUnit.GetComponent<UnitScript>().NumberRangeMin.x; // R�cup�ration de la Range min - x
        _numberRangeMin.y = selectedUnit.GetComponent<UnitScript>().NumberRangeMin.y; // R�cup�ration de la Range min - y 
        _numberRangeMax.x = selectedUnit.GetComponent<UnitScript>().NumberRangeMax.x; // R�cup�ration de la Range min - x
        _numberRangeMax.y = selectedUnit.GetComponent<UnitScript>().NumberRangeMax.y; // R�cup�ration de la Range min - y

        // Applique les bonus/malus de terrains
        if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Bosquet, selectedUnit.GetComponent<UnitScript>().ActualTiledId))
        {
            _numberRangeMin.x += 1;
            _numberRangeMin.y += 1;
            _numberRangeMax.x += 1;
        }
        if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Colline, selectedUnit.GetComponent<UnitScript>().ActualTiledId))
        {
            selectedUnit.GetComponent<UnitScript>().AttackRangeBonus = 1;
        }
        if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Plage, selectedUnit.GetComponent<UnitScript>().ActualTiledId) && selectedUnit.GetComponent<Unit_SO>().typeUnite == MYthsAndSteel_Enum.TypeUnite.Infanterie)
        {
            _numberRangeMin.x += -2;
            _numberRangeMin.y += -1;
            _numberRangeMax.x += -1;
        }
        if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Haute_colline, selectedUnit.GetComponent<UnitScript>().ActualTiledId))
        {
            selectedUnit.GetComponent<UnitScript>().AttackRangeBonus = 1;
        }
        if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Haute_colline, selectedUnitEnnemy.GetComponent<UnitScript>().ActualTiledId))
        {
            if (!PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Haute_colline, selectedUnit.GetComponent<UnitScript>().ActualTiledId))
            {
                if (!PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Colline, selectedUnit.GetComponent<UnitScript>().ActualTiledId))
                {
                    _numberRangeMin.x += 2;
                    _numberRangeMin.y += 2;
                    _numberRangeMax.x += 2;
                }
            }
        }
        else
        {
            selectedUnit.GetComponent<UnitScript>().AttackRangeBonus = 0;
        }
    }

    public void ApplyAttack()
    {
        Randomdice();
        ChooseAttackType(_numberRangeMin, _damageMinimum, _numberRangeMax, _damageMaximum, DiceResult);
        StopAttack();
        IsInAttack = false;

        selectedUnit.GetComponent<UnitScript>()._isActionDone = true;
        selectedUnit.GetComponent<UnitScript>().checkActivation();
    }
}