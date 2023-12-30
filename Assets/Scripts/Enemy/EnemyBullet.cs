
using UnityEngine;

//
// Computer Space 1971 v2020.10.26
//
// created 2020.10.16
//


public class EnemyBullet : MonoBehaviour
{
    private ScreenBoundaryController screenBoundary;

    private AudioController audioController;

    public GameObject bulletEcho;

    private float bulletSpeed;
    private float timeBetweenSpawns;
    private float startTimeBetweenSpawns;

    private float bulletLifeSpan;
    private float soundLifeSpan;

    public bool playingFireBulletSound;

    private Vector3 bulletDirection;

    private bool screenWrappingX;
    private bool screenWrappingY;



    private void Awake()
    {
        screenBoundary = ScreenBoundaryController.screenBoundaries;

        audioController = AudioController.instance;
    }


    void Start()
    {
        Initialise();
    }


    void Update()
    {
        MoveBullet();
    }


    private void Initialise()
    {
        bulletSpeed = 4f;

        bulletLifeSpan = 3f;
        
        soundLifeSpan = bulletLifeSpan;
        
        playingFireBulletSound = false;

        bulletDirection = PlayerController.playerController.transform.position - transform.position;

        bulletDirection.Normalize();

        timeBetweenSpawns = 0f;

        startTimeBetweenSpawns = 0.001f;
    }


    private void MoveBullet()
    {
        transform.position += bulletDirection * bulletSpeed * Time.deltaTime;

        ScreenWrap();

        soundLifeSpan -= Time.deltaTime;       

        PlayFireBulletSound();

        SpawnBulletEcho();

        Destroy(gameObject, bulletLifeSpan);
    }


    private void SpawnBulletEcho()
    {
        if (timeBetweenSpawns <= 0f)
        {
            GameObject echoInstance = Instantiate(bulletEcho, transform.position, Quaternion.identity);

            Destroy(echoInstance, 0.2f);

            timeBetweenSpawns = startTimeBetweenSpawns;
        }

        else
        {
            timeBetweenSpawns -= Time.deltaTime;
        }
    }


    private void PlayFireBulletSound()
    {
        if (soundLifeSpan <= 0f)
        {
            audioController.StopAudioClip("Fire Enemy Bullet");

            playingFireBulletSound = false;

            return;
        }

        if (!playingFireBulletSound)
        {
            audioController.PlayAudioClip("Fire Enemy Bullet");

            playingFireBulletSound = true;
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


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);

        if (collision.gameObject.CompareTag("Player"))
        {
            if (!GameController.gameController.playerDestroyed)
            {
                audioController.StopAudioClip("Fire Enemy Bullet");
                audioController.StopAudioClip("Rotate Ship");
                audioController.StopAudioClip("Thrusters Engaged");

                audioController.PlayAudioClip("Explosion");

                GameController.gameController.playerDestroyed = true;

                if (!GameController.gameController.gameOver)
                {
                    GameController.gameController.UpdateEnemyScore();
                }
            }
        }
    }


} // end of class
