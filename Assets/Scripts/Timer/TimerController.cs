
using UnityEngine;

//
// Computer Space 1971 v2020.10.26
//
// created 2020.10.16
//


public class TimerController : MonoBehaviour
{
    public SpriteRenderer[] timerDigit;

    private float gameTime;

    private int level;
    private int hyperDriveController;

    private const int ENGAGED = 0;
    private const int DISENGAGED = -1;


    private void Start()
    {
        Initialise();

        ResetTimer();
    }


    private void Initialise()
    {
        level = 0;

        hyperDriveController = DISENGAGED;
    }


    public void ResetTimer()
    {
        gameTime = 0f;

        if (!GameController.gameController.gameOver)
        {
            level += 1;

            hyperDriveController = level % 2;
        }

        timerDigit[1].sprite = GameController.gameController.number[(int)gameTime];
        timerDigit[0].sprite = GameController.gameController.number[(int)gameTime];
    }


    public void UpdateTimerDisplay()
    {
        // if we are not in demo mode
        if (!GameController.gameController.inDemoMode)
        {
            // increment the game timer
            gameTime += Time.deltaTime;
        }

        // otherwise . . .
        else
        {
            return;
        }


        if (GameController.gameController.playerScore == GameController.PLAYER_WINNING_SCORE ||
            GameController.gameController.enemyScore == GameController.ENEMY_WINNING_SCORE)
        {
            GameController.gameController.gameOver = true;

            GameController.gameController.GameOver();

            return;
        }


        if ((int)gameTime > GameController.GAMEOVER_SCORE)
        {
            if (GameController.gameController.playerScore > GameController.gameController.enemyScore)
            {
                ResetTimer();

                GameController.gameController.hyperDrive.gameObject.SetActive(true);

                return;
            }
        }


        if ((int)gameTime > GameController.GAMEOVER_SCORE)
        {
            ResetTimer();

            GameController.gameController.gameOver = true;

            GameController.gameController.GameOver();

            return;
        }

        timerDigit[1].sprite = GameController.gameController.number[(int)(gameTime % 10)];
        timerDigit[0].sprite = GameController.gameController.number[(int)(gameTime / 10)];
    }


} // end of class
