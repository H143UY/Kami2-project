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
    public TextMeshProUGUI PrizeText;
    private int current_prize;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        this.RegisterListener(EventID.LoseGame, (sender, param) =>
        {
            SceneManager.LoadScene("LoseGame");
        });
        this.RegisterListener(EventID.addPrize, (sender, param) =>
        {
            current_prize += 10;
        });
        this.RegisterListener(EventID.WinGame, (sender, param) =>
        {
            SceneManager.LoadScene("WinGame");
        });
    }
    private void Start()
    {
        NextLevel = false;
        current_prize = 0;
    }
    private void Update()
    {
        PrizeText.text = current_prize.ToString();
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
    public void AddPrize()
    {
        AdsController.Instance.ShowRewardedAd();
    }
}
