using UnityEngine;

public class FloorDetection : MonoBehaviour
{
    [SerializeField]
    private ChickenController chicken;

    void OnTriggerEnter2D(Collider2D collision)
    {
        chicken.TouchFloor(); 
    }
}
