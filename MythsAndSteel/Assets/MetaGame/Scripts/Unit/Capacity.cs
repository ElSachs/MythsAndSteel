using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu (menuName = "Capacit�")]
public class Capacity : MonoBehaviour
{
    public string CapacityName = "";
    [TextArea]
    public string Description = "";

    [SerializeField] bool _ActiveCapacity;
    public bool IsActive => _ActiveCapacity;

    [SerializeField] int _CapacityCost;
    public int CapacityCost => _CapacityCost;

    public void SoulevementDesMarees()
    {
        Debug.Log("Active de la capacit� Soulevement Des Mar�es");
    }

    public void SoulevementDesTerres()
    {
        Debug.Log("Active de la capacit� Soulevement Des Terres");
    }

    public Capacity capacite ;

}
