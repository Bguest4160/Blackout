using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneLoader : MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit!");
    }

    public void MainSceneLoader()
    {
        SceneManager.LoadScene("Scene 1");
    }











































































































    
}
