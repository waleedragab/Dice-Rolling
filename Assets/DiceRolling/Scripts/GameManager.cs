using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Dice dieOne;
    public Dice dieTwo;
    public int totalRounds = 11;

    [Range(1, 100)]
    public int BotChoosingRerollChancePercentage = 50;

    int dieOneValue = 0;
    int dieTwoValue = 0;

    int playerRoundScore;
    int botRoundScore;

    bool playerDoubleOdd = false;
    bool botDoubleEven = false;

    int currentRound = 1;
    int playerWonRounds = 0;
    int botWonRounds = 0;

    int remainingPlayerRerolls = 3;
    int remainingBotRerolls = 3;

    bool playerTurn = false;
    bool rerollRound = false;
    bool diceRolled = false;
    int playerOrigianlRoundScore = -1;
    int botOriginalRoundScore = -1;

    bool playerChoseReroll = false;
    bool botChoseReroll = false;

    public Text playerScoreText;
    public Text botScoreText;
    public Text currenPlayTurnText;
    public Text currentRoundText;
    public Text rollValueText;
    public Button rollButton;
    public Text gameOverText;
    public GameObject rerollPanel;
    public GameObject gameOverPanel;
    public GameObject exitPanel;


    // Start is called before the first frame update
    void Start()
    {
        SetPlayerTurn();
        currentRoundText.text = currentRound.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (!diceRolled && dieOne.landed && dieTwo.landed)
        {
            diceRolled = true;
            dieOneValue = dieOne.getDieValue();
            dieTwoValue = dieTwo.getDieValue();
            UpdateRoundScore(dieOneValue, dieTwoValue);
        }
    }

    public void RollDice()
    {
        rollValueText.text = "";
        dieOne.RollDie();
        dieTwo.RollDie();
    }

    void ResetDice()
    {
        dieOne.ResetDie();
        dieTwo.ResetDie();
        diceRolled = false;
        rollValueText.text = "";
    }

    void SetPlayerTurn()
    {
        playerTurn = true;
        rollButton.interactable = true;
        if (playerChoseReroll)
            currenPlayTurnText.text = "Player's Reroll Round!";
        else
            currenPlayTurnText.text = "Player's Turn!";
    }

    void SetBotTurn()
    {
        playerTurn = false;
        rollButton.interactable = false;
        if (botChoseReroll)
            currenPlayTurnText.text = "Bot's Reroll Round!";
        else
            currenPlayTurnText.text = "Bot's Turn!";
    }

    public void UpdateRoundScore(int value1, int value2)
    {
        rollValueText.text = value1 + " + " + value2 + " = " + (value1 + value2);
        if (!rerollRound) //normal round
        {
            if (playerTurn)
            {
                CalculatePlayerScore(value1, value2);

                SetBotTurn();
                Invoke("ResetDice", 3);
                Invoke("RollDice", 3.5f);
            }
            else //bot turn
            {
                CalculateBotScore(value1, value2);

                Invoke("ResetDice", 3);
                Invoke("AskForReroll", 3.5f);
            }
        }
        else
        {
            if (playerTurn)
            {
                CalculatePlayerScoreReroll(value1, value2);

                SetBotTurn();
                Invoke("ResetDice", 3);
                if (botChoseReroll)
                    Invoke("RollDice", 3.5f);
                else
                    Invoke("FinalizeRound", 3.5f);
            }
            else //bot turn
            {
                CalculateBotScoreReroll(value1, value2);

                Invoke("ResetDice", 3);
                Invoke("FinalizeRound", 3.5f);
            }
        }
    }

    void CalculatePlayerScore(int value1, int value2)
    {
        playerOrigianlRoundScore = value1 + value2;
        if (value1 == value2)
        {
            playerRoundScore = 0;
            if (value1 % 2 == 1)
                playerDoubleOdd = true;
            else
                playerDoubleOdd = false;
        }
        else
            playerRoundScore = value1 + value2;
    }

    void CalculatePlayerScoreReroll(int value1, int value2)
    {
        playerOrigianlRoundScore = value1 + value2;
        if (value1 == value2)
            playerRoundScore = 0;
        else
            playerRoundScore = value1 + value2;
    }

    void CalculateBotScore(int value1, int value2)
    {
        botOriginalRoundScore = value1 + value2;
        if (value1 == value2)
        {
            botRoundScore = 0;
            if (value1 % 2 == 0)
                botDoubleEven = true;
            else
                botDoubleEven = false;
        }
        else
            botRoundScore = value1 + value2;
    }

    void CalculateBotScoreReroll(int value1, int value2)
    {
        botOriginalRoundScore = value1 + value2;
        if (value1 == value2)
            botRoundScore = 0;
        else
            botRoundScore = value1 + value2;
    }

    void AskForReroll()
    {
        //bot reroll decision making
        if (remainingBotRerolls > 0
            && botRoundScore < playerRoundScore
            && Random.Range(BotChoosingRerollChancePercentage, 101) == 100
            && !botDoubleEven)
            botChoseReroll = true;

        if (remainingPlayerRerolls > 0 && !playerDoubleOdd)
            ShowRerollDialogue();
        else
            StartRerollRound();
    }

    

    private void ShowRerollDialogue()
    {
        rerollPanel.SetActive(true);
    }

    public void RerollConfirmation(bool playerWillReroll)
    {
        playerChoseReroll = playerWillReroll;
        rerollPanel.SetActive(false);
        StartRerollRound();
    }

    private void StartRerollRound()
    {
        if (playerChoseReroll || botChoseReroll)
            rerollRound = true;
        else
        {
            FinalizeRound();
            return;
        }
            


        if(playerChoseReroll)
        {
            remainingPlayerRerolls--;
            SetPlayerTurn();
            //PromptPlayerToRoll();
        }
        else if(botChoseReroll)
        {
            remainingBotRerolls--;
            SetBotTurn();
            RollDice();
        }
    }
    void FinalizeRound()
    {
        playerDoubleOdd = false;
        botDoubleEven = false;
        playerChoseReroll = false;
        botChoseReroll = false;
        rerollRound = false;

        if (playerRoundScore > botRoundScore)
        {
            PlayerWon();
            
        }
        else if (botRoundScore > playerRoundScore)
        {
            BotWon();


        }
        else // draw
        {
            if (playerOrigianlRoundScore % 2 == 0)
                PlayerWon();
            else
                BotWon();
        }

        if (playerWonRounds == (totalRounds / 2) + 1)
            FinalizeGame(true);
        else if (botWonRounds == (totalRounds / 2) + 1)
            FinalizeGame(false);
        else
        {
            currentRound++;
            currentRoundText.text = currentRound.ToString();
            SetPlayerTurn();
        }
            
    }

    void PlayerWon()
    {
        playerWonRounds++;
        playerScoreText.text = playerWonRounds.ToString();
        rollValueText.text = "Player Won This Round!";
    }

    void BotWon()
    {
        botWonRounds++;
        botScoreText.text = botWonRounds.ToString();
        rollValueText.text = "Bot Won This Round!";
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void OpenExitPanel()
    {
        exitPanel.SetActive(true);
    }

    public void CloseExitPanel()
    {
        exitPanel.SetActive(false);
    }
    void FinalizeGame(bool playerWon)
    {
        gameOverPanel.SetActive(true);
        if (playerWon)
        {
            gameOverText.text = "You Won!";
            int playerGamesWon = PlayerPrefs.GetInt("PlayerGamesWon", 0);
            PlayerPrefs.SetInt("PlayerGamesWon", ++playerGamesWon);
        }
        else
        {
            gameOverText.text = "You Lost!";
            int botGamesWon = PlayerPrefs.GetInt("BotGamesWon", 0);
            PlayerPrefs.SetInt("BotGamesWon", ++botGamesWon);
        }
    }
}
