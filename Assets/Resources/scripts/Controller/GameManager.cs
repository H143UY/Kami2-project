using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int selectedLevel;
    public bool NextLevel;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        this.RegisterListener(EventID.LoseGame, (sender, param) =>
        {
            SceneManager.LoadScene("LoseGame");
        });
        this.RegisterListener(EventID.WinGame, (sender, param) =>
        {
            SceneManager.LoadScene("WinGame");
        });
    }
    private void Start()
    {
        NextLevel = false;
    }
    public void LoadScene()
    {
        SceneManager.LoadScene("GamePlay");
    }
    public void ChoseLevel()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("GamePlay");
    }
    public void SetLevel(int levelIndex)
    {
        selectedLevel = levelIndex;
    }
    public void QuitGame()
    {
        GridManager.Instance.SaveGame();
        Application.Quit();
    }
    public void ReloadLevel()
    {
        Debug.Log("Reloading level...");
        GridManager.Instance.ClearGrid();
        GridManager.Instance.GenerateGrid();
    }
    
}
