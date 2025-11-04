using UnityEngine;
using UnityEngine.Events;

public class TransitionController : MonoBehaviour
{
    public UnityEvent TransitionInDone;
    public UnityEvent TransitionOutDone;

    public void FinishedTransitionIn()
    {
        TransitionInDone?.Invoke(); 
    }
    public void FinishedTransitionOut()
    {
        TransitionOutDone?.Invoke();
    }
}
