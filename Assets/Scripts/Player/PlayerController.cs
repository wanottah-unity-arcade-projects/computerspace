
using System.Collections;
using UnityEngine;

//
// Computer Space 1971 v2020.10.26
//
// created 2020.10.16
//


public class PlayerController : MonoBehaviour
{
    public static PlayerController playerController;

    // reference to audio controller script
    private AudioController audioController;

    private ScreenBoundaryController screenBoundary;

    public Transform playerShip;

    [HideInInspector] public Rigidbody2D playerShipRigidbody;

    public Transform weaponLauncher;
    public GameObject playerBullet;

    private float fireCounter;
    private float fireRate;

    private Animator thrusterAnimation;

    private const float CLOCKWISE = -1f;
    private const float COUNTER_CLOCKWISE = 1f;

    private float engineThrust;
    private float thrusterInput;

    // player start position
    private float randomPlayerPositionX;
    private float randomPlayerPositionY;

    private Vector2 playerSpawnPosition;

    [HideInInspector] public float rotationInput;
    [HideInInspector] public float rotationSpeed;

    private bool screenWrappingX;
    private bool screenWrappingY;

    private bool playingThrusterSound;
    private bool playingRotateShipSound;



    private void Awake()
    {
        playerController = this;

        audioController = AudioController.instance;

        screenBoundary = ScreenBoundaryController.screenBoundaries;
    }


    private void Start()
    {
        playerShipRigidbody = GetComponent<Rigidbody2D>();

        thrusterAnimation = GetComponentInChildren<Animator>();

        Initialise();
    }


    private void Update()
    {
        AutoFireMissile();
    }


    private void FixedUpdate()
    {
        EngageThrusters(thrusterInput * engineThrust);
    }


    private void Initialise()
    {
        screenWrappingX = false;

        screenWrappingY = false;

        engineThrust = 0.5f;

        rotationSpeed = 2f;

        fireRate = 2f;
        fireCounter = 0f;

        playingThrusterSound = false;
        playingRotateShipSound = false;
    }


    public void SpawnPlayer()
    {
        gameObject.SetActive(false);

        randomPlayerPositionX = Random.Range(
            PlayerSpawnBoundaryController.playerSpawnBoundaries.leftSpawnBoundary.position.x,
            PlayerSpawnBoundaryController.playerSpawnBoundaries.rightSpawnBoundary.position.x);

        randomPlayerPositionY = Random.Range(
            PlayerSpawnBoundaryController.playerSpawnBoundaries.topSpawnBoundary.position.y,
            PlayerSpawnBoundaryController.playerSpawnBoundaries.bottomSpawnBoundary.position.y);

        playerSpawnPosition = new Vector2(randomPlayerPositionX, randomPlayerPositionY);

        // set parent transform to new position
        transform.position = playerSpawnPosition;

        // set parent transform rotation to child transform rotation
        transform.localEulerAngles = playerShip.localEulerAngles;

        // reset child transform rotation
        playerShip.localEulerAngles = new Vector3(0f, 0f, 0f);

        GameController.gameController.playerDestroyed = false;

        gameObject.SetActive(true);
    }


    public void ShipControl()
    {
        if (!GameController.gameController.playerDestroyed)
        {
            thrusterInput = Input.GetAxis("Vertical");
            rotationInput = Input.GetAxis("Horizontal");

            if (rotationInput == 0)
            {
                audioController.StopAudioClip("Rotate Ship");

                playingRotateShipSound = false;
            }

            if (rotationInput > 0f)
            {
                if (!playingRotateShipSound)
                {
                    audioController.PlayAudioClip("Rotate Ship");

                    playingRotateShipSound = true;
                }

                RotateShip(CLOCKWISE);
            }

            if (rotationInput < 0f)
            {
                if (!playingRotateShipSound)
                {
                    audioController.PlayAudioClip("Rotate Ship");

                    playingRotateShipSound = true;
                }

                RotateShip(COUNTER_CLOCKWISE);
            }


            if (Input.GetKeyDown(KeyCode.Space))
            {
                FireMissile();
            }
        }
    }


    private void EngageThrusters(float thrust)
    {
        if (GameController.gameController.playerDestroyed)
        {
            playerShipRigidbody.velocity = Vector2.zero;

            thrusterAnimation.SetBool("playerDestroyed", true);

            return;
        }

        if (thrust < 0f)
        {
            return;
        }

        playerShipRigidbody.AddForce(transform.up * thrust);

        if (thrust > 0f)
        {
            if (!playingThrusterSound)
            {
                audioController.PlayAudioClip("Thrusters Engaged");

                playingThrusterSound = true;
            }

            thrusterAnimation.Play("Thrusters Engaged");
        }

        else
        {
            thrusterAnimation.Play("Thrusters Idle");

            audioController.StopAudioClip("Thrusters Engaged");

            playingThrusterSound = false;
        }

        ScreenWrap();
    }


    private void RotateShip(float rotationDirection)
    {
        transform.Rotate(0f, 0f, rotationDirection * rotationSpeed);
    }


    private void AutoFireMissile()
    {
        fireCounter -= Time.deltaTime;
    }


    private void FireMissile()
    {
        if (fireCounter <= 0f)
        {
            Instantiate(playerBullet, weaponLauncher.position, transform.rotation);

            fireCounter = fireRate;
        }
    }


    #region SCREEN WRAP
    private void ScreenWrap()
    {
        bool objectIsVisible = WithinScreenBoundary();

        if (objectIsVisible)
        {
            screenWrappingX = false;

            screenWrappingY = false;

            return;
        }


        if (screenWrappingX && screenWrappingY)
        {
            return;
        }


        Vector2 objectPosition = transform.position;


        if (objectPosition.y > screenBoundary.topScreenBoundary.position.y)
        {
            objectPosition.y = screenBoundary.bottomScreenBoundary.position.y;

            screenWrappingY = true;
        }

        if (objectPosition.y < screenBoundary.bottomScreenBoundary.position.y)
        {
            objectPosition.y = screenBoundary.topScreenBoundary.position.y;

            screenWrappingY = true;
        }


        if (objectPosition.x > screenBoundary.rightScreenBoundary.position.x)
        {
            objectPosition.x = screenBoundary.leftScreenBoundary.position.x;

            screenWrappingX = true;
        }

        if (objectPosition.x < screenBoundary.leftScreenBoundary.position.x)
        {
            objectPosition.x = screenBoundary.rightScreenBoundary.position.x;

            screenWrappingX = true;
        }


        transform.position = objectPosition;
    }


    private bool WithinScreenBoundary()
    {
        if (transform.position.y < screenBoundary.topScreenBoundary.position.y &&
            transform.position.y > screenBoundary.bottomScreenBoundary.position.y &&
            transform.position.x > screenBoundary.leftScreenBoundary.position.x &&
            transform.position.x < screenBoundary.rightScreenBoundary.position.x)
        {
            return true;
        }

        return false;
    }
    #endregion


    public void DestroyPlayer()
    {
        gameObject.SetActive(false);

        GameController.gameController.playerDestroyed = true;
    }


} // end of class
