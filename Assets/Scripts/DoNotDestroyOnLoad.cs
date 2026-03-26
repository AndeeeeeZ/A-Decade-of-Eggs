using UnityEngine;

// Attach to any object to move it to don't destroy on load
// Designed to use on audio source to make the background music continuous between levels
public class DoNotDestroyOnLoad : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(gameObject); 
    }
}
