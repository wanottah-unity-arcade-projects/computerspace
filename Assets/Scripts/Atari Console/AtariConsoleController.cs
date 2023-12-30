
using UnityEngine;
using UnityEngine.UI;

//
// Atari Video Game Console Controller v2020.10.24
//
// created 17-12-2019
//


public class AtariConsoleController : MonoBehaviour
{
    public static AtariConsoleController atariConsoleController;


    // reference to audio controller script
    private AudioController audioController;

    // reference to atari game data script
    private AtariGameData atariGameData;

    // reference to atari game data controller script
    private AtariGameDataController atariGameDataController;

    // reference to game controller script
    private GameController gameController;


    // reference to game title text
    [SerializeField] private Text gameTitleText = null;


    // reference to console switches
    [SerializeField] private Button consoleModeSwitch;

    [SerializeField] private Button coinSlot;

    [SerializeField] private Button settingSwitch;

    [SerializeField] private Button leftDifficultySwitch = null;
    [SerializeField] private Button rightDifficultySwitch;

    [SerializeField] private Button audioSelectionSwitch = null;
    [SerializeField] private Slider masterVolumeSlider = null;
    [SerializeField] private Slider musicVolumeSlider = null;
    [SerializeField] private Slider sfxVolumeSlider = null;

    [SerializeField] private Button gameNumberSelectionSwitch;
    [SerializeField] private Button gameStartSwitch = null;

    [SerializeField] private Button gameResumeSwitch = null;


    // reference to console switch text components
    [SerializeField] private Text settingSwitchText = null;

    [SerializeField] private Text leftDifficultySwitchText = null;
    [SerializeField] private Text rightDifficultySwitchText = null;

    [SerializeField] private Text audioSelectionSwitchText = null;
    [SerializeField] private Text masterVolumeControlText = null;
    [SerializeField] private Text musicVolumeControlText = null;
    [SerializeField] private Text sfxVolumeControlText = null;

    [SerializeField] private Text gameNumberSelectedText = null;
    [SerializeField] private Text numberOfPlayersText = null;


    // console switch modes
    [HideInInspector] public int consoleMode;

    [HideInInspector] public int tvMode;
    [HideInInspector] public int settingMode;

    [HideInInspector] public int leftDifficulty;
    [HideInInspector] public int rightDifficulty;

    [HideInInspector] public int audioModeSelected;
    [HideInInspector] public float masterVolume;
    [HideInInspector] public float musicVolume;
    [HideInInspector] public float sfxVolume;

    [HideInInspector] public int gameNumberSelected;
    [HideInInspector] public int numberOfPlayers;


    // console switch flag
    [HideInInspector] public bool inSetAudioMode;


    // console flag
    [HideInInspector] public bool initialisingConsoleSystem;


    // switch labels
    private const string COLOUR_TV_SETTING_SWITCH_TEXT = "COLOUR";
    private const string BW_TV_SETTING_SWITCH_TEXT = "B/W";
    private const string AUDIO_SETTING_SWITCH_TEXT = "AUDIO";

    private const string DIFFICULTY_A_SWITCH_TEXT = "A";
    private const string DIFFICULTY_B_SWITCH_TEXT = "B";

    private const string MASTER_VOLUME_SWITCH_TEXT = "MASTER";
    private const string MUSIC_VOLUME_SWITCH_TEXT = "MUSIC";
    private const string SFX_VOLUME_SWITCH_TEXT = "SFX";


    // console mode switch
    public const int CONSOLE_VISIBLE = 1;
    public const int CONSOLE_HIDDEN = -1;


    // setting switch
    private const int NUMBER_OF_TV_MODES = 2;
    private const int NUMBER_OF_SETTINGS = 3;

    public const int BW_TV = 1;
    public const int COLOUR_TV = 2;
    public const int AUDIO = 3;


    // volume setting switch
    private const int NUMBER_OF_AUDIO_CONTROLS = 3;

    public const int MASTER_VOLUME = 1;
    public const int MUSIC_VOLUME = 2;
    public const int SFX_VOLUME = 3;


    // difficulty switches
    private const int DIFFICULTY_A = 1;
    private const int DIFFICULTY_B = -1;


    public const int ONE_PLAYER = 1;
    public const int TWO_PLAYERS = 2;
    public const int THREE_PLAYERS = 3;
    public const int FOUR_PLAYERS = 4;


    public const int NO_GAME_SELECTED = 0;




    void Awake()
    {
        atariConsoleController = this;

        audioController = AudioController.instance;

        atariGameData = GetComponent<AtariGameData>();

        atariGameDataController = GetComponent<AtariGameDataController>();

        gameController = FindObjectOfType<GameController>();
    }


    public void InitialiseConsole(string GAME_TITLE, int TV_MODE)
    {
        // initialise game options
        atariGameDataController.SelectGame(GAME_TITLE);

        // set game title
        gameTitleText.text = GAME_TITLE;

        // default switch states
        // tv type - b/w = -1, colour = 1
        settingMode = TV_MODE;

        tvMode = settingMode;

        SettingMode(tvMode, settingMode);

        // player difficulty - A = 1, B = -1
        gameController.InitialiseDifficultySwitchSettings();

        leftDifficulty = DIFFICULTY_A;

        SetDifficulty(leftDifficulty, true);

        rightDifficulty = DIFFICULTY_A;

        SetDifficulty(rightDifficulty, false);

        // audio selection
        audioModeSelected = MASTER_VOLUME;

        inSetAudioMode = false;

        SetAudioModeSwitches(inSetAudioMode);

        // game mode
        SetPawzModeSwitches();

        // initial game number selection
        gameNumberSelected = NO_GAME_SELECTED;


        // console initialisation complete
        initialisingConsoleSystem = false;
    }


    // =============================================================================
    // not visible as a button
    // when the game arena is touched and the game is in demo mode, selects whether 
    // the atari console is visible or hidden
    // =============================================================================
    public void ConsoleModeSwitch()
    {
        if (gameController.inDemoMode)
        {
            consoleMode = -consoleMode;

            SetConsoleMode(consoleMode);
        }
    }


    public void SetConsoleMode(int consoleMode)
    {
        switch (consoleMode)
        {
            case CONSOLE_VISIBLE:

                GetComponent<Animator>().SetBool("consoleMode", true);

                break;

            case CONSOLE_HIDDEN:

                GetComponent<Animator>().SetBool("consoleMode", false);

                break;
        }
    }


    public void CoinSlot()
    {
        gameController.gameCredits += 1;

        if (gameController.gameCredits > GameController.MAXIMUM_COINS)
        {
            gameController.gameCredits = GameController.MAXIMUM_COINS;
        }

        audioController.PlayAudioClip("Coin Inserted");

        audioController.PlayAudioClip("1UP Credit");

        gameController.UpdateGameCreditsText();

        gameController.insertCoinsText.gameObject.SetActive(false);

        gameController.canPlay = true;
    }


    // =============================================================================
    // selects whether the atari console is in tv or audio setting mode
    // =============================================================================
    public void SettingSwitch()
    {
        settingMode = settingMode + 1;

        if (settingMode <= NUMBER_OF_TV_MODES)
        {
            tvMode = settingMode;
        }

        if (settingMode > NUMBER_OF_SETTINGS)
        {
            settingMode = BW_TV;

            tvMode = settingMode;
        }

        SettingMode(tvMode, settingMode);
    }


    private void SettingMode(int tvMode, int settingMode)
    {
        switch (settingMode)
        {
            case BW_TV:

                inSetAudioMode = false;

                SetAudioModeSwitches(inSetAudioMode);

                settingSwitchText.text = BW_TV_SETTING_SWITCH_TEXT;

                // change display to black and white
                gameController.SetTvMode(tvMode);

                break;

            case COLOUR_TV:

                settingSwitchText.text = COLOUR_TV_SETTING_SWITCH_TEXT;

                // change display to colour
                gameController.SetTvMode(tvMode);

                break;

            case AUDIO:

                settingSwitchText.text = AUDIO_SETTING_SWITCH_TEXT;

                inSetAudioMode = true;

                SetAudioModeSwitches(inSetAudioMode);

                break;
        }
    }


    // =============================================================================
    // sets the pawz, reset and resume switches depending on the game state
    // =============================================================================
    public void SetPawzModeSwitches()
    {
        //if (initialisingConsoleSystem)
        //{
            //gamePawzSwitch.gameObject.SetActive(false);
        //}


        if (gameController.inPawzMode)
        {
            //gamePawzSwitch.gameObject.SetActive(false);

            gameStartSwitch.gameObject.SetActive(false);

            gameResumeSwitch.gameObject.SetActive(true);
        }


        if (gameController.inDemoMode)
        {
            //gamePawzSwitch.gameObject.SetActive(false);

            gameResumeSwitch.gameObject.SetActive(false);

            gameStartSwitch.gameObject.SetActive(true);
        }


        //if (gameController.inPlayMode)
        //{
            //gamePawzSwitch.gameObject.SetActive(true);
        //}

    }


    private void SetAudioModeSwitches(bool audioModeEnabled)
    {
        if (audioModeEnabled)
        {
            gameTitleText.gameObject.SetActive(false);

            leftDifficultySwitch.gameObject.SetActive(false);

            audioSelectionSwitch.gameObject.SetActive(true);

            SetAudioSelection(audioModeSelected);
        }

        else
        {
            gameTitleText.gameObject.SetActive(true);

            leftDifficultySwitch.gameObject.SetActive(true);

            audioSelectionSwitch.gameObject.SetActive(false);

            masterVolumeSlider.gameObject.SetActive(false);

            musicVolumeSlider.gameObject.SetActive(false);

            sfxVolumeSlider.gameObject.SetActive(false);
        }
    }


    public void LeftDifficultySwitch()
    {
        if (leftDifficultySwitch.gameObject.activeInHierarchy)
        {
            leftDifficulty = -leftDifficulty;

            SetDifficulty(leftDifficulty, true);
        }
    }


    public void RightDifficultySwitch()
    {
        rightDifficulty = -rightDifficulty;

        SetDifficulty(rightDifficulty, false);
    }


    private void SetDifficulty(int difficultySetting, bool leftSwitch)
    {
        switch (difficultySetting)
        {
            case DIFFICULTY_A:

                if (leftSwitch)
                {
                    leftDifficultySwitchText.text = DIFFICULTY_A_SWITCH_TEXT;

                    gameController.SetLeftDifficultyA();
                }

                else
                {
                    rightDifficultySwitchText.text = DIFFICULTY_A_SWITCH_TEXT;

                    gameController.SetRightDifficultyA();
                }

                break;

            case DIFFICULTY_B:

                if (leftSwitch)
                {
                    leftDifficultySwitchText.text = DIFFICULTY_B_SWITCH_TEXT;

                    gameController.SetLeftDifficultyB();
                }

                else
                {
                    rightDifficultySwitchText.text = DIFFICULTY_B_SWITCH_TEXT;

                    gameController.SetRightDifficultyB();
                }

                break;
        }
    }


    public void GameNumberSelectionSwitch()
    {
        if (gameController.canPlay && !gameController.inPawzMode)
        {
            gameNumberSelected += 1;

            if (gameController.gameCredits == GameController.ONE_PLAYER_COINS || 
                gameNumberSelected > atariGameData.NUMBER_OF_GAMES)
            {
                gameNumberSelected = ONE_PLAYER;
            }

            SetGameSelection();
        }
    }


    public void SetGameSelection()
    {
        gameNumberSelectedText.text = gameNumberSelected.ToString();

        if (gameNumberSelected != NO_GAME_SELECTED)
        {
            numberOfPlayersText.text = atariGameData.gameNumber[gameNumberSelected - 1].ToString();

            gameController.gameOverText.gameObject.SetActive(false);

            gameController.pressStartText.gameObject.SetActive(true);
        }

        else
        {
            numberOfPlayersText.text = "0";
        }
    }


    public void AudioSelectionSwitch()
    {
        audioModeSelected += 1;

        if (audioModeSelected > NUMBER_OF_AUDIO_CONTROLS)
        {
            audioModeSelected = MASTER_VOLUME;
        }

        SetAudioSelection(audioModeSelected);
    }


    private void SetAudioSelection(int audioSelection)
    {
        switch (audioSelection)
        {
            case MASTER_VOLUME:

                audioSelectionSwitchText.text = MASTER_VOLUME_SWITCH_TEXT;

                sfxVolumeSlider.gameObject.SetActive(false);

                masterVolumeSlider.gameObject.SetActive(true);

                masterVolumeControlText.text = (masterVolumeSlider.value * 10).ToString("0");

                break;

            case MUSIC_VOLUME:

                audioSelectionSwitchText.text = MUSIC_VOLUME_SWITCH_TEXT;

                masterVolumeSlider.gameObject.SetActive(false);

                musicVolumeSlider.gameObject.SetActive(true);

                musicVolumeControlText.text = (musicVolumeSlider.value * 10).ToString("0");

                break;

            case SFX_VOLUME:

                audioSelectionSwitchText.text = SFX_VOLUME_SWITCH_TEXT;

                musicVolumeSlider.gameObject.SetActive(false);

                sfxVolumeSlider.gameObject.SetActive(true);

                sfxVolumeControlText.text = (sfxVolumeSlider.value * 10).ToString("0");

                break;
        }
    }


    public void GameStartSwitch()
    {
        switch (gameNumberSelected)
        {
            case ONE_PLAYER:

                gameController.StartOnePlayerGame();

                break;

            case TWO_PLAYERS:

                gameController.StartTwoPlayerGame();

                break;

            case THREE_PLAYERS:

                gameController.StartThreePlayerGame();

                break;

            case FOUR_PLAYERS:

                gameController.StartFourPlayerGame();

                break;
        }
    }


    public void GamePawzSwitch()
    {
        gameController.inPawzMode = true;

        gameController.inPlayMode = false;

        SetPawzModeSwitches();

        gameController.SetPawzMode();
    }


    public void GameResumeSwitch()
    {
        gameController.inPawzMode = false;

        gameController.inPlayMode = true;

        SetPawzModeSwitches();

        gameController.SetPlayMode();
    }


} // end of class
