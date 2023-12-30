
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Knight Rider LED Controller v2020.08.31


public class KnightRiderController : MonoBehaviour
{
    // reference to audio controller script
    private AudioController audioController;



    public GameObject[] leds;

    private int startLed;
    private int endLed;

    private int led;
    private int ledDirection;

    private int ledSequence;

    private float mainTimer;

    private float timer;
    private bool canCount;
    private bool doOnce;
    private bool initialiseKitt;

    private const float LED_ON = 1f;
    private const float LED_OFF = 0f;

    private const int ALL_LEDS = -1;

    private const int LED_0 = 0;
    private const int LED_1 = 1;
    private const int LED_2 = 2;
    private const int LED_3 = 3;
    private const int LED_4 = 4;
    private const int LED_5 = 5;
    private const int LED_6 = 6;
    private const int LED_7 = 7;

    private const float LED_FADE_0 = 0.87f;
    private const float LED_FADE_1 = 0.74f;
    private const float LED_FADE_2 = 0.61f;
    private const float LED_FADE_3 = 0.48f;
    private const float LED_FADE_4 = 0.35f;
    private const float LED_FADE_5 = 0.22f;
    private const float LED_FADE_6 = 0.09f;

    private const int LED_MOVING_LEFT = -1;
    private const int LED_MOVING_RIGHT = 1;

    private const int FIRST_LED_SEQUENCE = 1;
    private const int SECOND_LED_SEQUENCE = 2;
    private const int THIRD_LED_SEQUENCE = 3;

    private const float INITIALISATION_TIMER = 1f;
    private const float SEQUENCE_TIMER = 0.07f;



    void Awake()
    {
        audioController = AudioController.instance;
    }


    void Start()
    {
        mainTimer = INITIALISATION_TIMER;

        canCount = false;

        doOnce = false;

        initialiseKitt = true;

        timer = mainTimer;

        startLed = 0;

        endLed = 7;

        led = ALL_LEDS;

        ledDirection = 1;

        ledSequence = 1;

        InitialiseKnightRider(led);
    }


    private void Update()
    {
        RunTimer(led);
    }


    private void InitialiseKnightRider(int led)
    {
        ShowLed(led);
    }


    private void RunKnightRider()
    {
        led += LedDirection();

        ShowLed(led);
    }


    private void ShowLed(int led)
    {
        if (led == ALL_LEDS)
        {
            InitialiseLedState(ALL_LEDS, LED_ON);  
        }

        else
        {
            InitialiseLedState(led, LED_ON);
        }

        StartTimer();
    }


    private void ClearLed(int led)
    {
        if (led == ALL_LEDS)
        {
            InitialiseLedState(ALL_LEDS, LED_OFF);
        }

        else
        {
            InitialiseLedState(led, LED_OFF);
        }
    }


    private void SetLedState(int led, float ledState)
    {
        if (ledState == LED_ON)
        {
            leds[led].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, LED_ON);
        }

        else if (ledState == LED_OFF)
        {
            leds[led].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, LED_OFF);
        }

        else
        {
            leds[led].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, ledState);
        }
    }


    private void InitialiseLedState(int led, float ledState)
    {
        if (led == ALL_LEDS)
        {
            for (int leds = 0; leds <= endLed; leds++)
            {
                SetLedState(leds, ledState);
            }
        }


        else
        {

            switch (ledSequence)
            {
                case FIRST_LED_SEQUENCE:

                    switch (led)
                    {
                        case LED_0:

                            SetLedState(LED_0, LED_ON);

                            break;

                        case LED_1:

                            SetLedState(LED_1, LED_ON);
                            SetLedState(LED_0, LED_FADE_0);

                            break;

                        case LED_2:

                            SetLedState(LED_2, LED_ON);
                            SetLedState(LED_1, LED_FADE_0);
                            SetLedState(LED_0, LED_FADE_1);

                            break;

                        case LED_3:

                            SetLedState(LED_3, LED_ON);
                            SetLedState(LED_2, LED_FADE_0);
                            SetLedState(LED_1, LED_FADE_1);
                            SetLedState(LED_0, LED_FADE_2);

                            break;

                        case LED_4:

                            SetLedState(LED_4, LED_ON);
                            SetLedState(LED_3, LED_FADE_0);
                            SetLedState(LED_2, LED_FADE_1);
                            SetLedState(LED_1, LED_FADE_2);
                            SetLedState(LED_0, LED_FADE_3);

                            break;

                        case LED_5:

                            SetLedState(LED_5, LED_ON);
                            SetLedState(LED_4, LED_FADE_0);
                            SetLedState(LED_3, LED_FADE_1);
                            SetLedState(LED_2, LED_FADE_2);
                            SetLedState(LED_1, LED_FADE_3);
                            SetLedState(LED_0, LED_FADE_4);

                            break;

                        case LED_6:

                            SetLedState(LED_6, LED_ON);
                            SetLedState(LED_5, LED_FADE_0);
                            SetLedState(LED_4, LED_FADE_1);
                            SetLedState(LED_3, LED_FADE_2);
                            SetLedState(LED_2, LED_FADE_3);
                            SetLedState(LED_1, LED_FADE_4);
                            SetLedState(LED_0, LED_FADE_5);

                            break;

                        case LED_7:

                            SetLedState(LED_7, LED_ON);     // 100
                            SetLedState(LED_6, LED_FADE_0); // 86
                            SetLedState(LED_5, LED_FADE_1); // 72
                            SetLedState(LED_4, LED_FADE_2); // 58
                            SetLedState(LED_3, LED_FADE_3); // 44
                            SetLedState(LED_2, LED_FADE_4); // 30
                            SetLedState(LED_1, LED_FADE_5); // 16
                            SetLedState(LED_0, LED_FADE_6); // 2

                            break;

                    }

                    break;

                case SECOND_LED_SEQUENCE:

                    switch (led)
                    {
                        case LED_6:

                            SetLedState(LED_7, LED_FADE_0); // 86
                            SetLedState(LED_6, LED_ON);     // 100
                            SetLedState(LED_5, LED_FADE_2); // 58
                            SetLedState(LED_4, LED_FADE_3); // 44
                            SetLedState(LED_3, LED_FADE_4); // 30
                            SetLedState(LED_2, LED_FADE_5); // 16
                            SetLedState(LED_1, LED_FADE_6); // 2
                            SetLedState(LED_0, LED_OFF);    // 0

                            break;

                        case LED_5:

                            SetLedState(LED_7, LED_FADE_1); // 72
                            SetLedState(LED_6, LED_FADE_0); // 86
                            SetLedState(LED_5, LED_ON);     // 100
                            SetLedState(LED_4, LED_FADE_4); // 30
                            SetLedState(LED_3, LED_FADE_5); // 16
                            SetLedState(LED_2, LED_FADE_6); // 2
                            SetLedState(LED_1, LED_OFF);    // 0
                            SetLedState(LED_0, LED_OFF);    // 0

                            break;

                        case LED_4:

                            SetLedState(LED_7, LED_FADE_2); // 58
                            SetLedState(LED_6, LED_FADE_1); // 72
                            SetLedState(LED_5, LED_FADE_0); // 86
                            SetLedState(LED_4, LED_ON);     // 100
                            SetLedState(LED_2, LED_FADE_6); // 2
                            SetLedState(LED_2, LED_OFF);    // 0
                            SetLedState(LED_1, LED_OFF);    // 0
                            SetLedState(LED_0, LED_OFF);    // 0

                            break;

                        case LED_3:

                            SetLedState(LED_7, LED_FADE_3); // 44
                            SetLedState(LED_6, LED_FADE_2); // 58
                            SetLedState(LED_5, LED_FADE_1); // 72
                            SetLedState(LED_4, LED_FADE_0); // 86
                            SetLedState(LED_3, LED_ON);     // 100
                            SetLedState(LED_2, LED_OFF);    // 0
                            SetLedState(LED_1, LED_OFF);    // 0
                            SetLedState(LED_0, LED_OFF);    // 0

                            break;

                        case LED_2:

                            SetLedState(LED_7, LED_FADE_4); // 30
                            SetLedState(LED_6, LED_FADE_3); // 44
                            SetLedState(LED_5, LED_FADE_2); // 58
                            SetLedState(LED_4, LED_FADE_1); // 72
                            SetLedState(LED_3, LED_FADE_0); // 86
                            SetLedState(LED_2, LED_ON);     // 100
                            SetLedState(LED_1, LED_OFF);    // 0
                            SetLedState(LED_0, LED_OFF);    // 0

                            break;

                        case LED_1:

                            SetLedState(LED_7, LED_FADE_5); // 16
                            SetLedState(LED_6, LED_FADE_4); // 30
                            SetLedState(LED_5, LED_FADE_3); // 44
                            SetLedState(LED_4, LED_FADE_2); // 58
                            SetLedState(LED_3, LED_FADE_1); // 72
                            SetLedState(LED_2, LED_FADE_0); // 86
                            SetLedState(LED_1, LED_ON);     // 100
                            SetLedState(LED_0, LED_OFF);    // 0

                            break;

                        case LED_0:

                            SetLedState(LED_7, LED_FADE_6); // 2
                            SetLedState(LED_6, LED_FADE_5); // 16
                            SetLedState(LED_5, LED_FADE_4); // 30
                            SetLedState(LED_4, LED_FADE_3); // 44
                            SetLedState(LED_3, LED_FADE_2); // 58
                            SetLedState(LED_2, LED_FADE_1); // 72
                            SetLedState(LED_1, LED_FADE_0); // 86
                            SetLedState(LED_0, LED_ON);     // 100

                            break;

                    }

                    break;

                case THIRD_LED_SEQUENCE:

                    switch (led)
                    {
                        case LED_1:

                            SetLedState(LED_0, LED_FADE_0); // 86
                            SetLedState(LED_1, LED_ON);     // 100
                            SetLedState(LED_2, LED_FADE_2); // 58
                            SetLedState(LED_3, LED_FADE_3); // 44
                            SetLedState(LED_4, LED_FADE_4); // 30
                            SetLedState(LED_5, LED_FADE_5); // 16
                            SetLedState(LED_6, LED_FADE_6); // 2
                            SetLedState(LED_7, LED_OFF);    // 0

                            break;

                        case LED_2:

                            SetLedState(LED_0, LED_FADE_1); // 72
                            SetLedState(LED_1, LED_FADE_0); // 86
                            SetLedState(LED_2, LED_ON);     // 100
                            SetLedState(LED_3, LED_FADE_4); // 30
                            SetLedState(LED_4, LED_FADE_5); // 16
                            SetLedState(LED_5, LED_FADE_6); // 2
                            SetLedState(LED_6, LED_OFF);    // 0
                            SetLedState(LED_7, LED_OFF);    // 0

                            break;

                        case LED_3:

                            SetLedState(LED_0, LED_FADE_2); // 58
                            SetLedState(LED_1, LED_FADE_1); // 72
                            SetLedState(LED_2, LED_FADE_0); // 86
                            SetLedState(LED_3, LED_ON);     // 100
                            SetLedState(LED_4, LED_FADE_6); // 2
                            SetLedState(LED_5, LED_OFF);    // 0
                            SetLedState(LED_6, LED_OFF);    // 0
                            SetLedState(LED_7, LED_OFF);    // 0

                            break;

                        case LED_4:

                            SetLedState(LED_0, LED_FADE_3); // 44
                            SetLedState(LED_1, LED_FADE_2); // 58
                            SetLedState(LED_2, LED_FADE_1); // 72
                            SetLedState(LED_3, LED_FADE_0); // 86
                            SetLedState(LED_4, LED_ON);     // 100
                            SetLedState(LED_5, LED_OFF);    // 0
                            SetLedState(LED_6, LED_OFF);    // 0
                            SetLedState(LED_7, LED_OFF);    // 0

                            break;

                        case LED_5:

                            SetLedState(LED_0, LED_FADE_4); // 30
                            SetLedState(LED_1, LED_FADE_3); // 44
                            SetLedState(LED_2, LED_FADE_2); // 58
                            SetLedState(LED_3, LED_FADE_1); // 72
                            SetLedState(LED_4, LED_FADE_0); // 86
                            SetLedState(LED_5, LED_ON);     // 100
                            SetLedState(LED_6, LED_OFF);    // 0
                            SetLedState(LED_7, LED_OFF);    // 0

                            break;

                        case LED_6:

                            SetLedState(LED_0, LED_FADE_5); // 16
                            SetLedState(LED_1, LED_FADE_4); // 30
                            SetLedState(LED_2, LED_FADE_3); // 44
                            SetLedState(LED_3, LED_FADE_2); // 58
                            SetLedState(LED_4, LED_FADE_1); // 72
                            SetLedState(LED_5, LED_FADE_0); // 86
                            SetLedState(LED_6, LED_ON);     // 100
                            SetLedState(LED_7, LED_OFF);    // 0

                            break;

                        case LED_7:

                            SetLedState(LED_0, LED_FADE_6); // 2
                            SetLedState(LED_1, LED_FADE_5); // 16
                            SetLedState(LED_2, LED_FADE_4); // 30
                            SetLedState(LED_3, LED_FADE_3); // 44
                            SetLedState(LED_4, LED_FADE_2); // 58
                            SetLedState(LED_5, LED_FADE_1); // 72
                            SetLedState(LED_6, LED_FADE_0); // 86
                            SetLedState(LED_7, LED_ON);     // 100

                            break;

                    }

                    break;

            }
        }
    }


    private int LedDirection()
    {
        if (led >= endLed)
        {
            ledDirection = LED_MOVING_LEFT;

            ledSequence = SECOND_LED_SEQUENCE;
        }


        else if (led <= startLed && ledSequence != FIRST_LED_SEQUENCE)
        {
            ledDirection = LED_MOVING_RIGHT;

            ledSequence = THIRD_LED_SEQUENCE;
        }


        return ledDirection;
    }


    private void StartTimer()
    {
        timer = mainTimer;

        canCount = true;

        doOnce = false;
    }


    private void RunTimer(int led)
    {
        if (canCount && timer >= 0.0f)
        {
            timer -= Time.deltaTime;
        }

        else if (!doOnce && timer <= 0.0f)
        {
            canCount = false;

            doOnce = true;

            timer = 0.0f;

            if (initialiseKitt)
            {
                audioController.PlayAudioClip("Knight Rider");

                initialiseKitt = false;
            }

            ClearLed(led);

            mainTimer = SEQUENCE_TIMER;

            RunKnightRider();
        }
    }



} // end of class
