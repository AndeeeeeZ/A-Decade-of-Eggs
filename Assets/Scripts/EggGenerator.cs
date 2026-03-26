using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class EggGenerator : MonoBehaviour
{
    [SerializeField] private GameObject EggPrefab;
    // Use for hierarchy organization
    [SerializeField] private GameObject EggParent;
    [SerializeField] private int maxEggAmount;
    [SerializeField] private Image eggCountIndicator;
    [SerializeField] private Sprite[] eggCountSprites;
    private int currentEggAmount;
    private List<GameObject> Eggs = new List<GameObject>();
    private GameInputActions inputs;

    private void Awake()
    {
        inputs = new GameInputActions();
    }

    private void Start()
    {
        UpdateEggCountIndicator();
    }

    private void OnEnable()
    {
        inputs.Enable();
        inputs.Eggs.Move.performed += Move;
        inputs.Eggs.Move.canceled += Move;
    }

    private void OnDisable()
    {
        inputs.Eggs.Move.performed -= Move;
        inputs.Eggs.Move.canceled -= Move;
        inputs.Disable();
    }

    public void ProduceEgg(Vector3 spawnLocation)
    {
        if (currentEggAmount >= maxEggAmount)
            return;

        UnmarkLastEgg();

        GameObject egg = Instantiate(EggPrefab, spawnLocation, Quaternion.identity, EggParent.transform);
        Eggs.Add(egg);
        currentEggAmount++;

        // Update movement for the new egg
        // Since movement is only applied on buttom presses
        int moveInput = (int)inputs.Eggs.Move.ReadValue<float>();
        egg.GetComponent<EggController>().Move(moveInput);

        UpdateEggCountIndicator();
    }

    // Get last egg's position
    public Vector3 GetLastEggPosition()
    {
        if (Eggs.Count == 0)
        {
            Debug.LogWarning("No more egg");
            return Vector3.zero;
        }
        GameObject lastEgg = Eggs[Eggs.Count - 1];
        Vector3 pos = lastEgg.transform.position;
        return pos;
    }
    
    // Move all eggs together
    public void Move(InputAction.CallbackContext context)
    {
        int x = (int)context.ReadValue<float>();

        for (int i = 0; i < Eggs.Count(); i++)
        {
            Eggs[i].GetComponent<EggController>().Move(x);
        }
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
        UpdateEggCountIndicator();
    }

    public bool HaveEggLeft()
    {
        return maxEggAmount - currentEggAmount > 0;
    }

    public int GetCurrentEggAmount()
    {
        return currentEggAmount;
    }
    private void UpdateEggCountIndicator()
    {
        if (eggCountIndicator != null)
        {
            eggCountIndicator.sprite = eggCountSprites[maxEggAmount - currentEggAmount];
        }
    }

    private void UnmarkLastEgg()
    {
        if (currentEggAmount > 0)
            Eggs[Eggs.Count - 1].GetComponent<EggController>().UnmarkAsLastEgg();
    }
}
