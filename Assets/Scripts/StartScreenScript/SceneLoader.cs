using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneLoader : MonoBehaviour
{
    public GameObject canvasObject1;
    public GameObject canvasObject2;
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit!");
    }
    public void CharacterCustom()
    {
        canvasObject1.SetActive(false);
        canvasObject2.SetActive(true);
    }

    public void MainSceneLoader()
    {
        SceneManager.LoadScene("Scene 1");
    }

    public void Back()
    {
        canvasObject1.SetActive(true);
        canvasObject2.SetActive(false);

    }











































































































    
}
