
using UnityEngine;

//
// Atari Video Game Data v2020.10.20
//
// created 17-12-2019
//


public class AtariGameData : MonoBehaviour
{
    // reference to atari game data controller script
    private AtariGameDataController atariGameDataController;


    [HideInInspector] public int NUMBER_OF_GAMES;

    [HideInInspector] public int[] gameNumber;


    private const int ONE_PLAYER = 1;
    private const int TWO_PLAYERS = 2;
    private const int THREE_PLAYERS = 3;
    private const int FOUR_PLAYERS = 4;
    private const int DOUBLES = 2;


    // games
    public const string COMPUTERSPACE   = "COMPUTER SPACE";
    public const string SOLARFOX        = "SOLAR FOX";
    public const string MAZECRAZE       = "MAZE CRAZE";
    public const string WARLORDS        = "WARLORDS";
    public const string BREAKOUT        = "BREAKOUT";
    public const string QUADRAPONG      = "QUADRA PONG";
    public const string PONG            = "PONG";


    private void Start()
    {
        atariGameDataController = GetComponent<AtariGameDataController>();
    }


    public void InitialiseGame(int numberOfGames)
    {
        gameNumber = new int[numberOfGames];
    }


    public void InitialiseGameOptions(int gameIndex, int numberOfPlayers)
    {
        gameNumber[gameIndex] = numberOfPlayers;
    }


    // ********************************* GAMES ********************************* \\
    public void ComputerSpace()
    {
        NUMBER_OF_GAMES = 2;

        InitialiseGame(NUMBER_OF_GAMES);

        // game 1
        InitialiseGameOptions(0, ONE_PLAYER);
    }


} // end of class
