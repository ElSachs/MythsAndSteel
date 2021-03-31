using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class Attaque : MonoSingleton<Attaque> // Script AUC
{
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

    #region Variables
    //Port�e d'attaque
    [SerializeField] int _attackRange;
    public int AttackRange => _attackRange;

    //D�gats minimum inflig�s 
    [SerializeField] int _damageMinimum;
    public int DamageMinimum => _damageMinimum;

    // --
    [SerializeField] Vector2 _numberRangeMin;
    public Vector2 NumberRangeMin => _numberRangeMin;

    //D�gats maximum inflig�s
    [SerializeField] int _damageMaximum;
    public int DamageMaximum => _damageMaximum;

    // --
    [SerializeField] Vector2 _numberRangeMax;
    public Vector2 NumberRangeMax => _numberRangeMax;
    float o, p, x;
    #endregion Variables

    void Start() // Randomize � la mani�re des d�s
    {
        Randomdice();
    }

    void Randomdice()
    {
        o = Random.Range(1f, 6f);
        p = Random.Range(1f, 6f);
        x = o + p;
        Debug.Log(x);
    }

    void UnitAttack(Vector2 _numberRangeMin, int _damageMinimum, Vector2 _numberRangeMax, int _damageMaximum, float x)
    {
        if (x >= _numberRangeMin.x && x <= _numberRangeMin.y)
        {
            Debug.Log("Damage : " + _damageMinimum);
        }
        if (x >= _numberRangeMax.x && x <= _numberRangeMax.y)
        {
            Debug.Log("Damage : " + _damageMaximum);
        }
        if (x < _numberRangeMin.x)
        {
            Debug.Log("Damage : " + 0);
        }
    }

    void UnitAttackOne(Vector2 _numberRangeMin, int _damageMinimum, float x)
    {
        if (x >= _numberRangeMin.x && x <= _numberRangeMin.y)
        {
            Debug.Log("Damage : " + _damageMinimum);
        }
        if (x < _numberRangeMin.x)
        {
            Debug.Log("Damage : " + 0);
        }
    }

    void ChooseAttackType(Vector2 _numberRangeMin, int _damageMinimum, Vector2 _numberRangeMax, int _damageMaximum, float x)
    {
        if (_numberRangeMax.x == 0 && _numberRangeMax.y == 0)
        {
            UnitAttackOne(_numberRangeMin, _damageMinimum, x);
            Debug.Log("Une range de d�gat");
        }

        else
        {
            UnitAttack(_numberRangeMin, _damageMinimum, _numberRangeMax, _damageMaximum, x);
            Debug.Log("Deux ranges de d�gats");
        }
    }

    public void HighlightCaseForAttack(int tileId, int _attackRange) {
        if (_attackRange > 0)
        {
            foreach (int ID in PlayerStatic.GetNeighbourDiag(tileId, TilesManager.Instance.TileList[tileId].GetComponent<TileScript>().Line, false))
            {
                if (!newNeighbourId.Contains(ID))
                {
                    TilesManager.Instance.TileList[ID].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("selectedtileattack");
                    newNeighbourId.Add(ID);
                }
                HighlightCaseForAttack(ID, _attackRange - 1);
            }
        }
    }
}