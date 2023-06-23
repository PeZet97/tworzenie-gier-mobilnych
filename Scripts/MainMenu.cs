using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
   
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }

    public void SelectLevel1()
    {
        SceneManager.LoadScene("Asteroids");
    }

    public void SelectLevel2()
    {
        SceneManager.LoadScene("Trash");
    }

    public void SelectLevel3()
    {
        SceneManager.LoadScene("Flowers");
    }


}
