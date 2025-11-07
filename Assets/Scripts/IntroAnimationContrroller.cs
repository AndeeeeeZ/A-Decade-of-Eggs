using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class IntroAnimationContrroller : MonoBehaviour
{
    [SerializeField]
    private string SceneName;

    [SerializeField]
    private VideoPlayer videoPlayer;

    private void Start()
    {
        videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, "Videos/intro.mp4");
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene(SceneName);
    }

    public void ToNextScene()
    {
        SceneManager.LoadScene(SceneName);
    }
}
