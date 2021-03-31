using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


static class PlayerStatic{
    /// <summary>
    /// Est ce que la valeur du joueur est sup�rieur ou �gale au cout
    /// </summary>
    /// <param name="Cout"></param>
    /// <param name="ValeurJoueur"></param>
    /// <returns></returns>
    public static bool IsBGreaterThanA(int Cout, int ValeurJoueur){
        //Retourne true si la valeur est plus grande que le cout
        return ValeurJoueur > Cout ? true : false;
    }

    /// <summary>
    /// Est ce que l'unit� appartient bien � cette arm�e ? On doit y ins�rer notre scriptable.
    /// </summary>
    /// <param name="unite"></param>
    /// <param name="J1"></param>
    /// <returns></returns>
    public static bool CheckIsUnitArmy(UnitScript unite, bool J1)
    {
        //Permet de dire si l'unit� fait partie de l'arm�e, par d�fault il est null (ni oui, ni non).
        bool IsUnitInArmy;
        //Si la valeur de l'arm�e est �gale au cout alors..
        if (unite.UnitSO.IsInArmy1 == J1){
            //..Oui l'unit� fait partie de l'arm�e.
            IsUnitInArmy = true;
            Debug.Log("IsUnitInArmy est vrai");
        }
        else{
            //..Non l'unit� fait partie de l'arm�e.
            IsUnitInArmy = false;
            Debug.Log("IsUnitInArmy est faux");
        }
        return IsUnitInArmy;
    }

    /// <summary>
    /// Remplacer le Unite par Tile
    /// </summary>
    /// <param name="idTile"></param>
    /// <param name="ligne"></param>
    /// <param name="diag"></param>
    /// <returns></returns>
    public static List<int> GetNeighbourDiag(int idTile, int ligne, bool diag)
    {
        //Je prend la position du raycast, a partir de cette position je prends son ID.
        //De cette position, j'ajoute +9 pour la tile au nord, -9 pour la tile au Sud, +1 pour la tile � l'est et -1 pour la tile � l'ouest.
        //Pour la diagonale, j'ajoute +10 pour la tile au Nord Est, +8 pour la tile au Nord Ouest, -10 pour le Sud Ouest et -8 pour le Sud Est.
        //De la je return la valeur de ces ID.
        List<int> currentList = new List<int>();

        int witchLeftLine = 9 * (ligne - 1);
        int witchRightLine = (9 * ligne) - 1;

        //D�finit la position sur le quadrillage
        int topTile = idTile + 9;
        int downTile = idTile - 9;
        int leftTile = idTile - 1;
        int rightTile = idTile + 1;

        if (diag){
            int lTopTile = idTile + 8;
            int rTopTile = idTile + 10;

            int lDownTile = idTile - 10;
            int rDownTile = idTile + 8;

            currentList.Add(lTopTile);
            currentList.Add(rTopTile);
            currentList.Add(lDownTile);
            currentList.Add(rDownTile);

            if (topTile > 80 || idTile == witchLeftLine){
                currentList.Remove(lTopTile);
            }

            if (topTile > 80 || idTile == witchRightLine){
                currentList.Remove(rTopTile);
            }

            if (downTile < 0 || idTile == witchLeftLine){
                currentList.Remove(lDownTile);
            }

            if (downTile < 0 || idTile == witchRightLine){
                currentList.Remove(rDownTile);
            }
        }

        currentList.Add(idTile);
        currentList.Add(topTile);
        currentList.Add(downTile);
        currentList.Add(leftTile);
        currentList.Add(rightTile);

        if (idTile == witchLeftLine){
            currentList.Remove(leftTile);
        }

        if (idTile == witchRightLine){
            currentList.Remove(rightTile);
        }
        if (downTile < 0){
            currentList.Remove(downTile);
        }

        if (topTile > 80){
            currentList.Remove(topTile);
        }
        return currentList;
    }

    /// <summary>
    /// Permet de savoir si une tile est voisine d'une autre tile
    /// </summary>
    /// <param name="Id1"></param>
    /// <param name="Id2"></param>
    /// <param name="LineId1"></param>
    /// <param name="Diag"></param>
    /// <returns></returns>
    public static bool IsNeighbour(int Id1, int Id2, int LineId1, bool Diag){
        if(GetNeighbourDiag(Id1, LineId1, Diag).Contains(Id2)){
            return true;
        }
        else{
            return false;
        }
    }

    /// <summary>
    /// Cherche si la tile en id poss�de l'effet de terrain demand�
    /// </summary>
    /// <param name="effectToCheck"></param>
    /// <param name="idTiles"></param>
    /// <returns></returns>
    public static bool CheckTiles(MYthsAndSteel_Enum.TerrainType effectToCheck, int idTiles){
        bool hasTiles = false;
        hasTiles = TilesManager.Instance.TileList[idTiles].GetComponent<TileScript>().TerrainEffectList.Contains(effectToCheck);
        return hasTiles;
    }
}

