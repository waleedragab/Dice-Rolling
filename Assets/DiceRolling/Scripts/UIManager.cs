using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text playerScoreText;
    public Text botScoreText;
    public Text currentPlayTurnText;
    public Text currentRoundText;
    public Text rollValueText;
    public Text gameOverText;
    public Button rollButton;
    public GameObject rerollPanel;
    public GameObject gameOverPanel;
    public GameObject exitPanel;

    public void SetPlayerScoreText(string text)
    {
        playerScoreText.text = text;
    }

    public void SetBotScoreText(string text)
    {
        botScoreText.text = text;
    }

    public void SetCurrentPlayTurnText(string text)
    {
        currentPlayTurnText.text = text;
    }

    public void SetCurrentRoundText(string text)
    {
        currentRoundText.text = text;
    }

    public void SetRollValueText(string text)
    {
        rollValueText.text = text;
    }

    public void SetGameOverText(string text)
    {
        gameOverText.text = text;
    }

    public void ToggleRerollPanel()
    {
        rerollPanel.SetActive(!rerollPanel.activeSelf);
    }

    public void ToggleExitPanel()
    {
        exitPanel.SetActive(!exitPanel.activeSelf);
    }

    public void ShowGameOverPanel()
    {
        gameOverPanel.SetActive(true);
    }
}
