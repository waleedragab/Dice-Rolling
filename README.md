# Dice-Rolling
A simple dice rolling game.


Game Rules
1. The player rolls 2 six-sided dice.
    a. If they roll doubles, that’s a 0.
    b. Anything else is added together to become their score.
2. Then the bot rolls their own 2 dice with the same scoring system as the player.
3. The one with the highest score wins the round.
    a. If it's a tie, then an even roll total goes to the player and an odd one to the bot.
4. There are 11 rounds.
5. The winner is the one who wins the best-of-11.
6. In addition, the player and bot both get 3 rerolls each.
    a. The reroll decision and reroll itself occurs after the player and bot roll but before the game selects the round winner.
    b. If the player rolled double-odds that round, they can’t reroll.
    c. If the bot rolled double-evens that round, they can’t reroll.
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------
Game Code Info:

Dice.cs:
    void RollDie(): Performs the die rolling action
    void ResetDie(): resets the die to its original position and also resets all flags
    int getDieValue(): calculates the die value (which side is facing up) based on the die rotation and returns the value

GameManager.cs:
    void SetTurn(bool player): Sets current turn to either the player or the bot
    void UpdateRoundScore(int value1, int value2): updates the score for the round (whether normal or reroll round)
    void CalculateRollScore(int value1, int value2, bool player): calculate the score for every dice roll
    void AskForReroll(): Shows a reroll dialogue to the player if they have enough reroll and did not roll double odd this round. Also has bot reroll decision making
    void FinalizeRound(): resets flags and and declares the round winner
    void FinalizeGame(bool playerWon): Declares game winner and updates the stats

Die 3D Model: https://assetstore.unity.com/packages/3d/props/interior/board-games-145799