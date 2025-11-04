using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class GameSceneManager : MonoBehaviour
{
    [SerializeField]
    private GameObject transition;

    [SerializeField]
    private bool playTransitionIn, playTransitionOut;

    [SerializeField]
    private string nextSceneName;

    public UnityEvent FinishedTransitionIn;

    private Animator transitionAnimator;

    private void Start()
    {
        transitionAnimator = transition.GetComponent<Animator>();
        if (transitionAnimator == null)
            Debug.LogWarning("Transition missing animator component");
        if (playTransitionIn)
            transitionAnimator?.Play("TransitionIn");
    }

    public void PlayTransitionOut()
    {
        if (playTransitionOut)
            transitionAnimator?.Play("TransitionOut");
        else
            Debug.LogWarning("Not playing scene transition out");
    }

    public void OnTransitionOutDone()
    {
        SceneManager.LoadScene(nextSceneName, LoadSceneMode.Single);
    }

    public void OnTransitionInDone()
    {
        FinishedTransitionIn?.Invoke(); 
    }
}
