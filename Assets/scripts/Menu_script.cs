using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_script : MonoBehaviour
{
    public GameObject canva;
    public bool paused;

    void Start(){
        paused=false;
        Time.timeScale=1f;
    }

    public void Resume(){
        ResumeGame();
    }

    void ResumeGame(){
        paused = false;
        canva.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void PauseGame()
    {
        paused = true;
        canva.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Lobby(){
        SceneManager.LoadScene(0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused) ResumeGame();
            else PauseGame();
        }
    }
}