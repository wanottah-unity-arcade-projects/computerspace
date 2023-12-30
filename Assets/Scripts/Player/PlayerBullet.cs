
using UnityEngine;

//
// Computer Space 1971 v2020.10.26
//
// created 2020.10.16
//


public class PlayerBullet : MonoBehaviour
{
    private ScreenBoundaryController screenBoundary;

    // reference to audio controller script
    private AudioController audioController;

    private Rigidbody2D bulletRigidbody;

    public GameObject bulletEcho;

    private float bulletSpeed;
    private float timeBetweenSpawns;
    private float startTimeBetweenSpawns;

    private float bulletLifeSpan;
    private float soundLifeSpan;

    private bool playingFireBulletSound;

    private bool screenWrappingX;
    private bool screenWrappingY;




    private void Awake()
    {
        screenBoundary = ScreenBoundaryController.screenBoundaries;

        audioController = AudioController.instance;
    }


    private void Start()
    {
        bulletRigidbody = GetComponent<Rigidbody2D>();

        Initialise();
    }


    private void FixedUpdate()
    {
        MoveBullet();
    }


    private void Initialise()
    {
        bulletSpeed = 2.5f;

        bulletLifeSpan = 2f;

        soundLifeSpan = bulletLifeSpan;

        playingFireBulletSound = false;

        timeBetweenSpawns = 0f;

        startTimeBetweenSpawns = 0.001f;
    }


    private void MoveBullet()
    {
        if (!playingFireBulletSound)
        {
            audioController.PlayAudioClip("Fire Player Bullet");

            playingFireBulletSound = true;
        }

        soundLifeSpan -= Time.deltaTime;

        bulletRigidbody.velocity = transform.up * bulletSpeed;

        bulletRigidbody.rotation += -PlayerController.playerController.rotationInput * PlayerController.playerController.rotationSpeed;

        ScreenWrap();

        SpawnBulletEcho();

        Destroy(gameObject, bulletLifeSpan);

        if (soundLifeSpan <= 0f)
        {
            audioController.StopAudioClip("Fire Player Bullet");

            playingFireBulletSound = false;
        }
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

        if (collision.gameObject.CompareTag("Enemy0") || collision.gameObject.CompareTag("Enemy1"))
        {
            string collidingObject = collision.gameObject.tag;

            collision.transform.parent.GetComponent<EnemyController>().DestroyEnemy(collidingObject);

            if (!GameController.gameController.gameOver)
            {
                GameController.gameController.UpdatePlayerScore();
            }
        }
    }


} // end of class
