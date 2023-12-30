
using UnityEngine;

//
// Computer Space 1971 v2020.10.23
//
// created 2020.10.16
//


public class EnemyScoreController : MonoBehaviour
{
    public static EnemyScoreController enemyScoreController;

    public SpriteRenderer[] enemyScoreDigit;


    private void Awake()
    {
        enemyScoreController = this;
    }


    public void UpdateEnemyScoreDisplay()
    {
        enemyScoreDigit[1].sprite = GameController.gameController.number[GameController.gameController.enemyScore % 16];
        enemyScoreDigit[0].sprite = GameController.gameController.number[GameController.gameController.enemyScore / 16];
    }


} // end of class
