using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Dice dieOne;
    public Dice dieTwo;

    public List<int> playerRoundScores;
    public List<int> botRoundScores;

    [Range(1, 100)]
    public int BotChoosingRerollChancePercentage = 50;

    int dieOneValue = 0;
    int dieTwoValue = 0;

    bool playerDoubleOdd = false;
    bool botDoubleEven = false;

    int currentRound = 1;
    int playerWonRounds = 0;
    int botWonRounds = 0;

    int playerpoints = 0;
    int botPoints = 0;

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
        playerRoundScores = new List<int>();
        botRoundScores = new List<int>();

        playerTurn = true;
        PromptPlayerToRoll();
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

    void PromptPlayerToRoll()
    {
        Debug.Log("You Can Roll");

    }

    void RollDice()
    {
        dieOne.RollDie();
        dieTwo.RollDie();
    }

    void ResetDice()
    {
        dieOne.ResetDie();
        dieTwo.ResetDie();
        diceRolled = false;
    }

    public void UpdateRoundScore(int value1, int value2)
    {
        if (!rerollRound) //normal round
        {
            if (playerTurn)
            {
                CalculatePlayerScore(value1, value2);

                playerTurn = false;
                Invoke("ResetDice", 3);
                Invoke("RollDice", 3.5f);
            }
            else //bot turn
            {
                CalculateBotScore(value1, value2);

                playerTurn = true;
                Invoke("ResetDice", 3);
                Invoke("AskForReroll", 3.5f);
            }
        }
        else
        {
            playerDoubleOdd = false;
            botDoubleEven = false;

            if (playerTurn)
            {
                CalculatePlayerScoreReroll(value1, value2);

                playerTurn = false;
                Invoke("ResetDice", 3);
                if (botChoseReroll)
                    Invoke("RollDice", 3.5f);
                else
                    Invoke("FinalizeRound", 3.5f);
            }
            else //bot turn
            {
                CalculateBotScoreReroll(value1, value2);

                playerTurn = true;
                Invoke("ResetDice", 3);
                Invoke("FinalizeRound", 3.5f);
            }
            playerChoseReroll = false;
            botChoseReroll = false;
        }
    }

    void CalculatePlayerScore(int value1, int value2)
    {
        playerOrigianlRoundScore = value1 + value2;
        if (value1 == value2)
        {
            playerRoundScores.Add(0);
            if (value1 % 2 == 1)
                playerDoubleOdd = true;
            else
                playerDoubleOdd = false;
        }
        else
            playerRoundScores.Add(value1 + value2);
    }

    void CalculatePlayerScoreReroll(int value1, int value2)
    {
        playerOrigianlRoundScore = value1 + value2;
        if (value1 == value2)
            playerRoundScores[currentRound - 1] = 0;
        else
            playerRoundScores[currentRound - 1] = value1 + value2;
    }

    void CalculateBotScore(int value1, int value2)
    {
        botOriginalRoundScore = value1 + value2;
        if (value1 == value2)
        {
            botRoundScores.Add(0);
            if (value1 % 2 == 0)
                botDoubleEven = true;
            else
                botDoubleEven = false;
        }
        else
            botRoundScores.Add(value1 + value2);
    }

    void CalculateBotScoreReroll(int value1, int value2)
    {
        botOriginalRoundScore = value1 + value2;
        if (value1 == value2)
            botRoundScores[currentRound - 1] = 0;
        else
            botRoundScores[currentRound - 1] = value1 + value2;
    }

    void AskForReroll()
    {
        //bot reroll decision making
        if (remainingBotRerolls > 0
            && botRoundScores[currentRound - 1] < playerRoundScores[currentRound - 1]
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
        Debug.Log("Reroll??");
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
            playerTurn = true;
            PromptPlayerToRoll();
        }
        else if(botChoseReroll)
        {
            remainingBotRerolls--;
            playerTurn = false;
            RollDice();
        }
    }
    void FinalizeRound()
    {
        rerollRound = false;

        if (playerRoundScores[currentRound - 1] > botRoundScores[currentRound - 1])
            playerWonRounds++;
        else if (botRoundScores[currentRound - 1] > playerRoundScores[currentRound - 1])
            botWonRounds++;
        else // draw
        {
            if (playerOrigianlRoundScore % 2 == 0)
                playerWonRounds++;
            else
                botWonRounds++;
        }

        if (playerWonRounds == 6)
            FinalizeGame(true);
        else if (botWonRounds == 6)
            FinalizeGame(false);
        else
        {
            currentRound++;
            PromptPlayerToRoll();
        }
            
    }

    void FinalizeGame(bool playerWon)
    {
        //throw new NotImplementedException();
    }
}
