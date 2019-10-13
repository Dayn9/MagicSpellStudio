using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScenes : MonoBehaviour
{

    public void LoadGameScene()
    {
        SceneManager.LoadScene("SampleScene");
        Time.timeScale = 0;
    }

    public void LoadMenuScene()
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1;
    }
}
