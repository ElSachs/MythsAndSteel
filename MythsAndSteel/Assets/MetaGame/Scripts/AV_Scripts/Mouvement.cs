using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Mouvement : MonoSingleton<Mouvement> // Script AV.
{
    [SerializeField] private int[] neighbourValue; // +1 +9 +10...

    [SerializeField] private List<int> newNeighbourId = new List<int>(); // Voisins atteignables avec le range de l'unit�.
    public List<int> _selectedTileId => selectedTileId;

    [SerializeField] private List<int> selectedTileId = new List<int>(); // Cases selectionn�es par le joueur.
    public List<int> _newNeighbourId => newNeighbourId;

    [SerializeField] private float speed = 1; // Speed de d�placement de l'unit� 

    [SerializeField] private GameObject mStart; // mT Start. 
    [SerializeField] private GameObject mEnd; // mT End.
    [SerializeField] private GameObject mUnit; // mT Unit�.

    [SerializeField] private List<GameObject> outTileLeft; 
    [SerializeField] private List<GameObject> outTileRight;

    private int LocalRange = 0; // permet de g�rer la range d'une unit� en local. 
    private List<int> temp = new List<int>(); //

    private void Update()
    {
        // Permet d'effectuer le moveTowards de l'unit� � sa prochaine case.
        UpdatingMove(mUnit, mStart, mEnd); 
    }

    /// <summary>
    /// Cette fonction "highlight" les cases atteignables par l'unit� sur la case s�lectionn�e.
    /// </summary>
    /// <param name="tileId">Tile centrale</param>
    /// <param name="Range">Range de l'unit�</param>
    public void Highlight(int tileId, int Range)
    {
        if (Range > 0)
        {
            foreach (int ID in PlayerStatic.GetNeighbourDiag(tileId, TilesManager.Instance.TileList[tileId].GetComponent<TileScript>().Line, false))
            {
                if (!newNeighbourId.Contains(ID))
                {
                    TilesManager.Instance.TileList[ID].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("selectedtile");
                    newNeighbourId.Add(ID);
                } 
                Highlight(ID, Range - 1);
            }
        }
    }

    /// <summary>
    /// Lance le mouvement d'une unit� avec une range d�fini.
    /// </summary>
    /// <param name="tileId">Tile de l'unit�</param>
    /// <param name="Range">Mvmt de l'unit�</param>
    public void StartMvmtForSelectedUnit()
    {
        if (TilesManager.Instance._actualTileSelected != null)
        {
            StartMouvement(TilesManager.Instance.TileList.IndexOf(TilesManager.Instance._actualTileSelected), TilesManager.Instance._actualTileSelected.GetComponent<TileScript>().Unit.GetComponent<UnitScript>().MoveSpeed);
        }
    }

    public void StartMouvement(int tileId, int Range)
    {
        TilesManager.Instance._Mouvement = true;
        LocalRange = Range;
        selectedTileId.Add(tileId);
        List<int> ID = new List<int>();
        ID.Add(tileId);
        Highlight(tileId, Range); // Lance l'highlight des cases dans la range.
    }

    /// <summary>
    /// Ar�te le Mouvement pour l'unit� selectionn�e (menu, cases highlights...)
    /// </summary>
    public void StopMouvement()
    {        
        foreach (int Neighbour in newNeighbourId) // Supprime toutes les tiles.
        {
            TilesManager.Instance.TileList[Neighbour].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("empty"); // Assigne un sprite empty � toutes les anciennes cases "neighbour".
        }
        if(TilesManager.Instance._actualTileSelected != null) // Si une case �tait s�l�ctionn�e.
        {
            //Tiles.Instance._actualTileSelected.GetComponent<TileScript>().Unit.GetComponent<UnitScript>().DemandMenu.enabled = false;
            //Tiles.Instance._actualTileSelected.GetComponent<TileScript>().Unit.GetComponent<UnitScript>().Menu.enabled = false;
        }
        foreach (int NeighbourSelect in selectedTileId) // Si un path de mvmt �tait s�l�ctionn�.
        {
            TilesManager.Instance.TileList[NeighbourSelect].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("empty");
            TilesManager.Instance.TileList[NeighbourSelect].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
        }
        // Clear de toutes les listes et stats.
        selectedTileId.Clear(); 
        newNeighbourId.Clear();
        mStart = null;
        mEnd = null;
        mUnit = null;
        LocalRange = 0;
        TilesManager.Instance._Mouvement = false;
        TilesManager.Instance._Selected = false;
        TilesManager.Instance._actualTileSelected = null;
    }

    /// <summary>
    /// Ajoute la tile � TileSelected. Pour le mvmt du joueur => Check egalement toutes les conditions de d�placement.
    /// </summary>
    /// <param name="tileId">Tile</param>
    public void AddMouvement(int tileId) 
    {
        if (TilesManager.Instance._Mouvement)
        {
            if (newNeighbourId.Contains(tileId)) // Si cette case est dans la range de l'unit�.
            {
                if (selectedTileId.Contains(tileId)) // Si cette case est d�j� selectionn�e.
                {
                    if (tileId == selectedTileId[0]) // la premi�re case est selectionn�e = Le joueur a cliqu� sur l'unit�.
                    {
                        Debug.Log("stop");
                        StopMouvement(); // D�sactive le mvmt.
                    } 
                    else // Sinon suppr�ssion de toutes les cases s�l�ctionn�es apr�s celle-ci.
                    {
                        for (int i = selectedTileId.IndexOf(tileId); i < selectedTileId.Count; i++) // Supprime toutes les cases s�lectionn�es � partir de l'ID tileId.
                        {
                            Debug.Log("REMOVE");
                            LocalRange++; // Redistribution du Range � chaque suppression de case.
                            temp.Add(selectedTileId[i]);
                            TilesManager.Instance.TileList[selectedTileId[i]].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("selectedtile"); // Repasse les sprites en apparence "s�l�ctionnable".
                        }
                        foreach (int i in temp)
                        {
                            selectedTileId.Remove(i);
                        }
                        temp.Clear();
                    }
                }
                else if (PlayerStatic.IsNeighbour(tileId, selectedTileId[selectedTileId.Count - 1], TilesManager.Instance.TileList[tileId].GetComponent<TileScript>().Line, false)) // Sinon, si cette case est bien voisine de l'ancienne selection. 
                {
                    if(LocalRange > 0) // et qu'il reste du mvmt, on assigne la nouvelle case selectionn�e � la liste SelectedTile.
                    {
                        LocalRange--; // sup 1 mvmt.
                        selectedTileId.Add(tileId); 
                        TilesManager.Instance.TileList[tileId].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("selecttile");
                        //Tiles.Instance._actualTileSelected.GetComponent<TileScript>().Unit.GetComponent<UnitScript>()._DemandMenu.enabled = true;
                    }
                }
                else // Sinon cette case est trop loin de l'ancienne seletion.
                {
                    Debug.Log("La tile d'ID : " + tileId + " est trop loin de la tile d'ID: " + selectedTileId[selectedTileId.Count - 1]);
                }
            }
            else // Sinon cette case est hors de la range de l'unit�.
            {
                Debug.Log("La tile d'ID : " + tileId + " est trop loin de la tile d'ID: " + selectedTileId[selectedTileId.Count - 1]);
            }
        }
    }

    /// <summary>
    /// Lance mvmt.
    /// </summary>
    int MvmtIndex = 1; // Num�ro du mvmt actuel dans la liste selectedTileId;
    [SerializeField] bool Launch = false; // Evite les r�p�titions dans updatingmove();
    /// <summary>
    /// Assigne le prochain mouvement demand� � l'unit�. Change les stats de l'ancienne et de la nouvelle case. Actualise les informations de position de l'unit�.
    /// </summary>
    public void ApplyMouvement()
    {
        mUnit = TilesManager.Instance._actualTileSelected.GetComponent<TileScript>().Unit; // Assignation de l'unit�.
        mStart = TilesManager.Instance._actualTileSelected; // Assignation du nouveau d�part.
        mEnd = TilesManager.Instance.TileList[selectedTileId[MvmtIndex]];  // Assignation du nouvel arrir�e.
        //mUnit.GetComponent<UnitScript>()._DemandMenu.enabled = false; // D�sactive le menu (Se d�placer, annuler)
        //mUnit.GetComponent<UnitScript>()._Menu.enabled = false; // D�sactive le menu global de l'unit�.
        foreach (int Neighbour in newNeighbourId) // D�sactive toutes les cases selectionn�es par la fonction Highlight.
        {
            if (!selectedTileId.Contains(Neighbour))
            {
                TilesManager.Instance.TileList[Neighbour].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("empty"); // Assigne un sprite empty � toutes les anciennes cases "neighbour"
            }
        }
        Debug.Log("Actual tile target: " + TilesManager.Instance.TileList[selectedTileId[MvmtIndex]]);
    }
    /// <summary>
    /// Coroutine d'attente entre chaque case. Probablement pendant ce temps que l'on devra appliquer les effets de case.
    /// </summary>
    /// <returns>Temps � d�finir</returns>
    IEnumerator MvmtEnd()
    {
        mEnd.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("empty"); // La case d�pass�e redevient une "empty"
        mEnd.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255); // La case reprend sa couleur d'origine.
        mUnit.GetComponent<UnitScript>().MoveLeft--;  // Suppression d'un point de mvmt � l'unit�.
        mEnd.GetComponent<TileScript>().AddUnitToTile(mStart.GetComponent<TileScript>().Unit); // L'unit� de la case d'arriv�e devient celle de la case de d�part.
        mStart.GetComponent<TileScript>().RemoveUnitFromTile(); // L'ancienne case n'a plus d'unit�.
        mUnit = mEnd.GetComponent<TileScript>().Unit;
        mUnit.GetComponent<UnitScript>().ActualTiledId = TilesManager.Instance.TileList.IndexOf(mEnd); 
        TilesManager.Instance._actualTileSelected = mEnd; 
        mStart = mEnd;
        mEnd = null;
        yield return new WaitForSeconds(1); // Temps d'attente.
        if (MvmtIndex < selectedTileId.Count - 1) // Si il reste des mvmts � effectuer dans la liste SelectedTile.
        {
            MvmtIndex++;
            ApplyMouvement();
        }
        else // Si il ne reste aucun mvmt dans la liste SelectedTile.
        {
            Debug.Log("STOP");
            MvmtIndex = 1;
            StopMouvement(); // Ar�te le mvmt de l'unit�.
        }
        Launch = false; // Reset de la bool Launch
    }

    /// <summary>
    /// Cette fonction lance l'animation de translation de l'unit� entre les cases.
    /// </summary>
    /// <param name="Unit">The unit gameobject.</param>
    /// <param name="StartPos">start position tile</param>
    /// <param name="EndPos">end position tile</param>
    float speed1;
    void UpdatingMove(GameObject Unit, GameObject StartPos, GameObject EndPos)
    {
        if (Unit != null && StartPos != null && EndPos != null)
        {
            Unit.transform.position = Vector2.MoveTowards(Unit.transform.position, EndPos.transform.position, speed1); // Application du mvmt.
            speed1 = Mathf.Abs((Vector2.Distance(mUnit.transform.position, mEnd.transform.position) * speed)); // R�gulation de la vitesse. (effet de ralentissement) 
            if (Vector2.Distance(mUnit.transform.position, mEnd.transform.position) <= 0.05f && Launch == false) // Si l'unit� est arriv�e.
            {
                Launch = true;
                StartCoroutine(MvmtEnd()); // Lancer le prochain mvmt avec d�lai. 
            }
            else // Sinon appliqu� l'opacit� � la case d'arriv�e en fonction de la distance unit� - arriv�e.
            {
                mEnd.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, Vector2.Distance(mUnit.transform.position, mEnd.transform.position));
            }
        }
    }
}


