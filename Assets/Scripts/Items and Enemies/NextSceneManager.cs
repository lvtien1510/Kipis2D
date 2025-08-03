using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextSceneManager : MonoBehaviour
{
    AudioManager audioManager;
    public void NextScene(string sceneName)
    {
        SceneTransitionManager.Instance.TransitionToScene(sceneName);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
