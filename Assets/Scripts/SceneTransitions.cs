using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneTransitions : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            LoadNextScene();
        }

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

    }

    private void LoadNextScene()
    {
        int currentSceneNumber = SceneManager.GetActiveScene().buildIndex;
        currentSceneNumber++;
        SceneManager.LoadScene(currentSceneNumber % SceneManager.sceneCountInBuildSettings);
    }
}
