using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ExitController : MonoBehaviour
{
    public UnityEvent OnPlayerReachingExit;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player has reached the exit");
            OnPlayerReachingExit?.Invoke();
        }
    }
}
