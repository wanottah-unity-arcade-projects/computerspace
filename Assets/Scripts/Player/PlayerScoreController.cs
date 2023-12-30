
using UnityEngine;

//
// Computer Space 1971 v2020.10.23
//
// created 2020.10.16
//


public class PlayerScoreController : MonoBehaviour
{
    public static PlayerScoreController playerScoreController;

    public SpriteRenderer[] playerScoreDigit;


    private void Awake()
    {
        playerScoreController = this;
    }


    public void UpdatePlayerScoreDisplay()
    {
        playerScoreDigit[1].sprite = GameController.gameController.number[GameController.gameController.playerScore % 16];
        playerScoreDigit[0].sprite = GameController.gameController.number[GameController.gameController.playerScore / 16];
    }


} // end of class
