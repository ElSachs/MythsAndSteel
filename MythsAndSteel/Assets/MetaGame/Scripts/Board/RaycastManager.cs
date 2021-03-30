using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastManager : MonoSingleton<RaycastManager>{
    #region Variables
    //Les layer qui sont d�tect�s par le raycast
    [SerializeField] private LayerMask layerM;

    //tile qui se trouve sous le raycast
    [SerializeField] private GameObject _tile;
    public GameObject Tile => _tile;
    //Derni�re tile en m�moire par ce script
    GameObject _lastTile = null;

    //tile qui se trouve sous le raycast
    [SerializeField] private GameObject _unitInTile;
    public GameObject UnitInTile => _unitInTile;
    [SerializeField] private GameObject actualTileSelected;
    public GameObject _actualTileSelected
    {
        get
        {
            return actualTileSelected;
        }
        set
        {
            actualTileSelected = value;
        }
    }

    //Est ce que les joueurs peuvent jouer
    bool _isInTurn = false;

    //Event pour quand le joueur clique sur un bouton pour passer � la phase suivante
    public delegate void TileRaycastChange();
    public event TileRaycastChange OnTileChanged;
    #endregion Variables

    void Update(){
        //obtient le premier objet touch� par le raycast
        RaycastHit2D hit = GetRaycastHit();

        //Remplace le gameObject Tile pour avoir en avoir une sauvegarde
        _tile = hit.collider != null? hit.collider.gameObject : null;
        
        //Assigne l'unit� si la tile qui est s�lectionn�e poss�de une unit�
        _unitInTile = _tile != null ? _tile.GetComponent<TileScript>().Unit != null ? _tile.GetComponent<TileScript>().Unit : null : null;

        //Si la tile change
        if(_tile != _lastTile || _isInTurn != GameManager.Instance.IsInTurn){
            _isInTurn = GameManager.Instance.IsInTurn;
            _lastTile = _tile;
            OnTileChanged();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(_tile != null)
            {
                _tile.GetComponent<TileScript>().Select();
            }
        }
    }
    
    /// <summary>
    /// Permet d'obtenir les objets touch�s par le raycast
    /// </summary>
    /// <returns></returns>
    RaycastHit2D GetRaycastHit(){
        Vector2 mouseDirection = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        Ray2D ray = new Ray2D(Camera.main.ScreenToWorldPoint(Input.mousePosition), mouseDirection);
        return Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, layerM);
    }


}