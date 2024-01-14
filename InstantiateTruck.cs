using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class InstantiateTruck : MonoBehaviour
{

    public GameObject truckPrefab;
   
    private GameObject _truck = null;
    private GameObject[] _trucks;   
    // Start is called before the first frame update
    void Start()
    {
   
    }

     public void Inst() {
      //  Debug.Log(" »Õ—“¿Õ÷»»–Œ¬¿Õ»≈ √–”«Œ¬» ¿ ");
        Vector3 spawnPos = new Vector3(-138, 1, -173);
        Quaternion spawnRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
        _trucks = GameObject.FindGameObjectsWithTag("Truck");
        if (_trucks.Length <= 3)
        {
            
            _truck = Instantiate(truckPrefab, spawnPos, spawnRotation);
            
        }
       
    }

    public void TruckSpawn()
    {
        
       
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
