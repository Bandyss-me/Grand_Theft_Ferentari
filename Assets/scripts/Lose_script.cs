using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lose_script : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Lobby()
    {
        SceneManager.LoadScene(0);
    }

    public void Lose()
    {
        SceneManager.LoadScene(3);
    }
}
