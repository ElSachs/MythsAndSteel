using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastManager : MonoSingleton<RaycastManager>
{
    #region Appel de Script
    public MouseCommand mouseCommand;
    #endregion

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


    //Est ce que les joueurs peuvent jouer
    bool _isInTurn = false;

    //Event pour quand le joueur clique sur un bouton pour passer � la phase suivante
    public delegate void TileRaycastChange();
    public event TileRaycastChange OnTileChanged;
    #endregion Variables

    void Update()
    {
        //obtient le premier objet touch� par le raycast
        RaycastHit2D hit = GetRaycastHit();

        //Remplace le gameObject Tile pour avoir en avoir une sauvegarde
        _tile = hit.collider != null ? hit.collider.gameObject : null;

        //Assigne l'unit� si la tile qui est s�lectionn�e poss�de une unit�
        _unitInTile = _tile != null ? _tile.GetComponent<TileScript>().Unit != null ? _tile.GetComponent<TileScript>().Unit : null : null;

        //Permet de combiner le Shift et le click gauche de la souris.
        if (_unitInTile == true)
        {
            //Si il il y a une unit� sur la tile, le joueur peut utiliser ShiftClick.
            mouseCommand.ShiftClick();
            //Si le joueur a utilis� le Shift puis leclick, le joueur est consid�r� comme click et on applique les fonctions propres au bouton des panneaux. De plus, le mouseOver est d�sactiv�.
            if (mouseCommand.CheckIfPlayerAsClic == true)
            {
                mouseCommand.buttonAction(UIInstance.Instance.ButtonId);
                mouseCommand.MouseExitWithoutClick();
            }
            else
            {
                //Si le joueur n'a pas continu� sa combinaison d'action( Shif+clic), alors quand ma souris reste sur une case sans cliqu�, l'interface r�sum� des statistiques s'active.
                mouseCommand.MouseExitWithoutClick();
                mouseCommand.MouseOverWithoutClick();
            }
        }
        else
        {
            //Si la case ne comporte pas d'unit� alors le MouseOver ne s'active pas et n'affiche par l'interface r�sum� des statistiques.
            mouseCommand.MouseExitWithoutClick();
        }

        //Si la tile change
        if (_tile != _lastTile || _isInTurn != GameManager.Instance.IsInTurn)
        {
            _isInTurn = GameManager.Instance.IsInTurn;
            _lastTile = _tile;
            OnTileChanged();
        }

    }

    /// <summary>
    /// Permet d'obtenir les objets touch�s par le raycast
    /// </summary>
    /// <returns></returns>
    public RaycastHit2D GetRaycastHit()
    {
        Vector2 mouseDirection = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        Ray2D ray = new Ray2D(Camera.main.ScreenToWorldPoint(Input.mousePosition), mouseDirection);
        return Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, layerM);
    }
}