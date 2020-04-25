using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject statsPanel;
    public Text playerScore;
    public Text botScore;
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenStatsPanel()
    {
        statsPanel.SetActive(true);
        playerScore.text = PlayerPrefs.GetInt("PlayerGamesWon").ToString();
        botScore.text = PlayerPrefs.GetInt("BotGamesWon").ToString();
    }

    public void CloseStatsPanel()
    {
        statsPanel.SetActive(false);
    }
}
