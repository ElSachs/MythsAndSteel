using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Va �tre convertit en pouvoir non ?
[CreateAssetMenu(menuName = "Unit/Unit Transport")]
public class Transport_Unit_SO : Unit_SO
{
    [Header("Listes des unit�s qui sont transportablles par l'unit�")]
    //Listes des unit�s qui peuvent �tre transportables.
    [SerializeField] private List<GameObject> _unitTransport;
    public List<GameObject> UnitTransport => _unitTransport;

   
    //Nombre d'unit� au maximum transportable
    [Header("Capacit� de transport maximale")]
    [Range(1,4)]
    [SerializeField] private int _maxCapacityTransport;
    public int MaxCapacityTransport => _maxCapacityTransport;
}
