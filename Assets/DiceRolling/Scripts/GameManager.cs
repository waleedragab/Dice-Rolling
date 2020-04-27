using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Debug")]
    public bool isDebugEnabled = false;
    [Range(1, 6)]
    public int playerDebugDieOneValue;
    [Range(1, 6)]
    public int playerDebugDieTwoValue;
    [Range(1, 6)]
    public int botDebugDieOneValue;
    [Range(1, 6)]
    public int botDebugDieTwoValue;

    [Header("Normal")]
    public UIManager uiManager;
    public Dice dieOne;
    public Dice dieTwo;
    public int totalNumbeOfRounds = 11;

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

    // Start is called before the first frame update
    void Start()
    {
        SetPlayerTurn();
        uiManager.SetCurrentRoundText(currentRound.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        if (!diceRolled && dieOne.landed && dieTwo.landed)
        {
            diceRolled = true;
            if (isDebugEnabled) //Manually set dice values
            {
                if (playerTurn)
                {
                    dieOneValue = playerDebugDieOneValue;
                    dieTwoValue = playerDebugDieTwoValue;
                }
                else
                {
                    dieOneValue = botDebugDieOneValue;
                    dieTwoValue = botDebugDieTwoValue;
                }
            }
            else
            {
                dieOneValue = dieOne.getDieValue();
                dieTwoValue = dieTwo.getDieValue();
            }
            
            UpdateRoundScore(dieOneValue, dieTwoValue);
        }
    }

    public void RollDice()
    {
        uiManager.SetRollValueText("");
        dieOne.RollDie();
        dieTwo.RollDie();
    }

    void ResetDice()
    {
        dieOne.ResetDie();
        dieTwo.ResetDie();
        diceRolled = false;
        uiManager.SetRollValueText("");
    }

    void SetPlayerTurn()
    {
        playerTurn = true;
        uiManager.rollButton.interactable = true;
        if (playerChoseReroll)
            uiManager.SetCurrentPlayTurnText("Player's Reroll Round!");
        else
            uiManager.SetCurrentPlayTurnText("Player's Turn!");
    }

    void SetBotTurn()
    {
        playerTurn = false;
        uiManager.rollButton.interactable = false;
        if (playerChoseReroll)
            uiManager.SetCurrentPlayTurnText("Bot's Reroll Round!");
        else
            uiManager.SetCurrentPlayTurnText("Bot's Turn!");
    }

    public void UpdateRoundScore(int value1, int value2)
    {
        uiManager.SetRollValueText(value1 + " + " + value2 + " = " + (value1 + value2));
        if (!rerollRound) //normal round
        {
            if (playerTurn)
            {
                CalculateRollScore(value1, value2, true);
                CheckDoubleOdd(value1, value2);
                SetBotTurn();
                Invoke("ResetDice", 3);
                Invoke("RollDice", 3.5f);
            }
            else //bot turn
            {
                CalculateRollScore(value1, value2, false);
                CheckDoubleEven(value1, value2);
                Invoke("ResetDice", 3);
                Invoke("AskForReroll", 3.5f);
            }
        }
        else
        {
            if (playerTurn)
            {
                CalculateRollScore(value1, value2, true);
                SetBotTurn();
                Invoke("ResetDice", 3);
                if (botChoseReroll)
                    Invoke("RollDice", 3.5f);
                else
                    Invoke("FinalizeRound", 3.5f);
            }
            else //bot turn
            {
                CalculateRollScore(value1, value2, false);
                Invoke("ResetDice", 3);
                Invoke("FinalizeRound", 3.5f);
            }
        }
    }

    void CheckDoubleOdd(int value1, int value2)
    {
        if (value1 == value2 && value1 % 2 == 1)
            playerDoubleOdd = true;
    }

    void CheckDoubleEven(int value1, int value2)
    {
        if (value1 == value2 && value1 % 2 == 0)
            botDoubleEven = true;
    }

    void CalculateRollScore(int value1, int value2, bool player)
    {
        if (player)
        {
            playerOrigianlRoundScore = value1 + value2;
            if (value1 == value2)
                playerRoundScore = 0;
            else
                playerRoundScore = value1 + value2;
        }
        else //bot
        {
            botOriginalRoundScore = value1 + value2;
            if (value1 == value2)
                botRoundScore = 0;
            else
                botRoundScore = value1 + value2;
        }
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
        uiManager.ToggleRerollPanel();
    }

    public void RerollConfirmation(bool playerWillReroll)
    {
        playerChoseReroll = playerWillReroll;
        uiManager.ToggleRerollPanel();
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

        if (playerWonRounds == (totalNumbeOfRounds / 2) + 1)
            FinalizeGame(true);
        else if (botWonRounds == (totalNumbeOfRounds / 2) + 1)
            FinalizeGame(false);
        else
        {
            currentRound++;
            uiManager.SetCurrentRoundText(currentRound.ToString());
            SetPlayerTurn();
        }
            
    }

    void PlayerWon()
    {
        playerWonRounds++;
        uiManager.SetPlayerScoreText(playerWonRounds.ToString());
        uiManager.SetRollValueText("Player Won This Round!");
    }

    void BotWon()
    {
        botWonRounds++;
        uiManager.SetBotScoreText(botWonRounds.ToString());
        uiManager.SetRollValueText("Bot Won This Round!");
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void OpenExitPanel()
    {
        uiManager.ToggleExitPanel();
    }

    public void CloseExitPanel()
    {
        uiManager.ToggleExitPanel();
    }
    void FinalizeGame(bool playerWon)
    {
        uiManager.ShowGameOverPanel();
        if (playerWon)
        {
            uiManager.SetGameOverText("You Won!");
            int playerGamesWon = PlayerPrefs.GetInt("PlayerGamesWon", 0);
            PlayerPrefs.SetInt("PlayerGamesWon", ++playerGamesWon);
        }
        else
        {
            uiManager.SetGameOverText("You Lost!");
            int botGamesWon = PlayerPrefs.GetInt("BotGamesWon", 0);
            PlayerPrefs.SetInt("BotGamesWon", ++botGamesWon);
        }
    }
}
