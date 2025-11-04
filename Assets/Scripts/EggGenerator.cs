using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EggGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject EggPrefab;

    [SerializeField]
    private GameObject EggParent;

    [SerializeField]
    private int maxEggAmount;

    private int currentEggAmount;

    private List<GameObject> Eggs = new List<GameObject>();
    // private EggController lastEgg;
    public void ProduceEgg(Vector3 spawnLocation)
    {
        if (currentEggAmount < maxEggAmount)
        {
            if (currentEggAmount > 0)
                Eggs[Eggs.Count - 1].GetComponent<EggController>().UnmarkAsLastEgg();
            GameObject egg = Instantiate(EggPrefab, spawnLocation, Quaternion.identity, EggParent.transform);
            Eggs.Add(egg);
            currentEggAmount++;
        }
    }

    // Get last egg's position and destroy all eggs
    public Vector3 GetLastEggPosition()
    {
        if (Eggs.Count == 0)
        {
            Debug.LogWarning("No more egg");
            return Vector3.zero;
        }
        GameObject lastEgg = Eggs[Eggs.Count - 1];
        Vector3 pos = lastEgg.transform.position;

        RemoveAllEggs();
        return pos;
    }

    public void RemoveAllEggs()
    {
        currentEggAmount = 0; 
        for (int i = Eggs.Count - 1; i >= 0; i--)
        {
            GameObject tempEgg = Eggs[i];
            Eggs.Remove(tempEgg);
            Destroy(tempEgg);
        }
    }

    public bool HaveEggLeft()
    {
        return currentEggAmount - maxEggAmount != 0;
    }

    public int GetCurrentEggAmount()
    {
        return currentEggAmount; 
    }


    public void Move(int x)
    {
        for (int i = 0; i < Eggs.Count(); i++)
        {
            Eggs[i].GetComponent<EggController>().Move(x);
        }
    }
}
