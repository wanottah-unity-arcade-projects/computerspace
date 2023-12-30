
using UnityEngine;

//
// Computer Space 1971 v2020.10.26
//
// created 2020.10.16
//


public class EnemyController : MonoBehaviour
{
    public static EnemyController enemyController;

    private ScreenBoundaryController screenBoundary;

    private AudioController audioController;

    public Transform[] enemyShip;

    public Rigidbody2D[] enemyRigidbody;

    public Transform weaponLauncher0;
    public Transform weaponLauncher1;
    public GameObject enemyBullet;

    [HideInInspector] public bool bothFire;
    private float fireCounter;
 
    private float movementSpeed;
    //private int movementDirection;
    private int movementDirection0;
    private int movementDirection1;
    [HideInInspector] public bool moveTogether;

    private const float ENEMY_SHIP_SPACING = 2.4f;

    private float latestDirectionChangeTime;
    private float directionChangeTime;

    // enemy start position
    private float randomEnemyPositionX;
    private float randomEnemyPositionY;

    private Vector2 enemySpawnPosition;

    private bool[] screenWrappingX;
    private bool[] screenWrappingY;



    private void Awake()
    {
        enemyController = this;

        screenBoundary = ScreenBoundaryController.screenBoundaries;

        audioController = AudioController.instance;
    }


    private void Start()
    {
        Initialise();

        //movementDirection = GetRandomDirection();
        movementDirection0 = GetRandomDirection();
        movementDirection1 = GetRandomDirection();
    }


    private void FixedUpdate()
    {
        MoveEnemyShips();
    }


    private void Update()
    {
        FireMissile();       
    }


    private void Initialise()
    {
        screenWrappingX = new bool[GameController.NUMBER_OF_ENEMIES];
        screenWrappingY = new bool[GameController.NUMBER_OF_ENEMIES];

        for (int i = 0; i <= 1; i++)
        {
            screenWrappingX[i] = false;

            screenWrappingY[i] = false;
        }

        movementSpeed = 0.8f;

        directionChangeTime = 5f;
        latestDirectionChangeTime = 0f;

        fireCounter = 0f;
    }


    public void SpawnEnemy()
    {
        if (moveTogether)
        {
            gameObject.SetActive(false);

            GetRandomPosition();

            enemySpawnPosition = new Vector2(randomEnemyPositionX, randomEnemyPositionY);

            transform.position = enemySpawnPosition;

            enemyShip[0].position = new Vector2(randomEnemyPositionX, randomEnemyPositionY - ENEMY_SHIP_SPACING);
            enemyShip[1].position = new Vector2(randomEnemyPositionX, randomEnemyPositionY + ENEMY_SHIP_SPACING);

            GameController.gameController.enemyDestroyed[0] = false;
            GameController.gameController.enemyDestroyed[1] = false;

            gameObject.SetActive(true);
        }

        else
        {
            if (GameController.gameController.enemyDestroyed[0] && GameController.gameController.enemyDestroyed[1])
            {
                GetRandomPosition();

                enemyShip[0].position = new Vector2(randomEnemyPositionX, randomEnemyPositionY);

                GameController.gameController.enemyDestroyed[0] = false;

                enemyShip[0].gameObject.SetActive(true);

                GetRandomPosition();

                enemyShip[1].position = new Vector2(randomEnemyPositionX, randomEnemyPositionY);

                GameController.gameController.enemyDestroyed[1] = false;

                enemyShip[1].gameObject.SetActive(true);
            }
        }
    }


    private void GetRandomPosition()
    {
        randomEnemyPositionX = Random.Range(
            EnemySpawnBoundaryController.enemySpawnBoundaries.leftSpawnBoundary.position.x,
            EnemySpawnBoundaryController.enemySpawnBoundaries.rightSpawnBoundary.position.x);

        randomEnemyPositionY = Random.Range(
            EnemySpawnBoundaryController.enemySpawnBoundaries.topSpawnBoundary.position.y,
            EnemySpawnBoundaryController.enemySpawnBoundaries.bottomSpawnBoundary.position.y);
    }


    private int GetRandomDirection()
    {
        return Random.Range(0, 9);
    }


    private float GetRandomFireRate()
    {
        return Random.Range(3f, 4f);
    }


    private void MoveEnemyShips()
    {
        if (moveTogether)
        {
            MoveEnemy();
        }

        else
        {
            MoveEnemy0();

            MoveEnemy1();
        }
    }


    private void MoveEnemy()
    {
        if (!GameController.gameController.enemyDestroyed[0] && !GameController.gameController.enemyDestroyed[1])
        {
            if (Time.time - latestDirectionChangeTime > directionChangeTime)
            {
                latestDirectionChangeTime = Time.time;

                movementDirection0 = GetRandomDirection();
            }

            switch (movementDirection0)
            {
                case 0: enemyRigidbody[0].velocity = new Vector2(0f, 0f);
                        enemyRigidbody[1].velocity = new Vector2(0f, 0f); break;

                case 1: enemyRigidbody[0].velocity = new Vector2(-movementSpeed, 0f);
                        enemyRigidbody[1].velocity = new Vector2(-movementSpeed, 0f); break;

                case 2: enemyRigidbody[0].velocity = new Vector2(movementSpeed, 0f);
                        enemyRigidbody[1].velocity = new Vector2(movementSpeed, 0f); break;

                case 3: enemyRigidbody[0].velocity = new Vector2(0f, movementSpeed);
                        enemyRigidbody[1].velocity = new Vector2(0f, movementSpeed); break;

                case 4: enemyRigidbody[0].velocity = new Vector2(0f, -movementSpeed);
                        enemyRigidbody[1].velocity = new Vector2(0f, -movementSpeed); break;

                case 5: enemyRigidbody[0].velocity = new Vector2(-movementSpeed, movementSpeed);
                        enemyRigidbody[1].velocity = new Vector2(-movementSpeed, movementSpeed); break;

                case 6: enemyRigidbody[0].velocity = new Vector2(movementSpeed, movementSpeed);
                        enemyRigidbody[1].velocity = new Vector2(movementSpeed, movementSpeed); break;

                case 7: enemyRigidbody[0].velocity = new Vector2(-movementSpeed, -movementSpeed);
                        enemyRigidbody[1].velocity = new Vector2(-movementSpeed, -movementSpeed); break;

                case 8: enemyRigidbody[0].velocity = new Vector2(movementSpeed, -movementSpeed);
                        enemyRigidbody[1].velocity = new Vector2(movementSpeed, -movementSpeed); break;
            }

            ScreenWrap0();
            ScreenWrap1();
        }
    }


    private void MoveEnemy0()
    {
        if (!GameController.gameController.enemyDestroyed[0])
        {
            if (Time.time - latestDirectionChangeTime > directionChangeTime)
            {
                latestDirectionChangeTime = Time.time;

                movementDirection0 = GetRandomDirection();
            }

            switch (movementDirection0)
            {
                case 0: enemyRigidbody[0].velocity = new Vector2(0f, 0f); break;

                case 1: enemyRigidbody[0].velocity = new Vector2(-movementSpeed, 0f); break;

                case 2: enemyRigidbody[0].velocity = new Vector2(movementSpeed, 0f); break;

                case 3: enemyRigidbody[0].velocity = new Vector2(0f, movementSpeed); break;

                case 4: enemyRigidbody[0].velocity = new Vector2(0f, -movementSpeed); break;

                case 5: enemyRigidbody[0].velocity = new Vector2(-movementSpeed, movementSpeed); break;

                case 6: enemyRigidbody[0].velocity = new Vector2(movementSpeed, movementSpeed); break;

                case 7: enemyRigidbody[0].velocity = new Vector2(-movementSpeed, -movementSpeed); break;

                case 8: enemyRigidbody[0].velocity = new Vector2(movementSpeed, -movementSpeed); break;
            }

            ScreenWrap0();
        }
    }


    private void MoveEnemy1()
    {
        if (!GameController.gameController.enemyDestroyed[1])
        {
            if (Time.time - latestDirectionChangeTime > directionChangeTime)
            {
                latestDirectionChangeTime = Time.time;

                movementDirection1 = GetRandomDirection();
            }

            switch (movementDirection1)
            {
                case 0: enemyRigidbody[1].velocity = new Vector2(0f, 0f); break;

                case 1: enemyRigidbody[1].velocity = new Vector2(-movementSpeed, 0f); break;

                case 2: enemyRigidbody[1].velocity = new Vector2(movementSpeed, 0f); break;

                case 3: enemyRigidbody[1].velocity = new Vector2(0f, movementSpeed); break;

                case 4: enemyRigidbody[1].velocity = new Vector2(0f, -movementSpeed); break;

                case 5: enemyRigidbody[1].velocity = new Vector2(-movementSpeed, movementSpeed); break;

                case 6: enemyRigidbody[1].velocity = new Vector2(movementSpeed, movementSpeed); break;

                case 7: enemyRigidbody[1].velocity = new Vector2(-movementSpeed, -movementSpeed); break;

                case 8: enemyRigidbody[1].velocity = new Vector2(movementSpeed, -movementSpeed); break;
            }

            ScreenWrap1();
        }
    }


    private void FireMissile()
    {
        if (!GameController.gameController.inDemoMode)
        {
            if (!bothFire)
            {
                FireMissile0();
            }

            else
            {
                if (Random.Range(0, 2) == 0)
                {
                    FireMissile0();
                }

                else
                {
                    FireMissile1();
                }

                //return;
            }


            if (!moveTogether)
            {
                if (Random.Range(0, 2) == 0)
                {
                    FireMissile0();
                }

                else
                {
                    FireMissile1();
                }
            }
        }
    }


    private void FireMissile0()
    {
        if (!GameController.gameController.enemyDestroyed[0] &&
            !GameController.gameController.playerDestroyed)
        {
            if (fireCounter > 0f)
            {
                fireCounter -= Time.deltaTime;

                if (fireCounter <= 0f)
                {
                    fireCounter = GetRandomFireRate();

                    Instantiate(enemyBullet, weaponLauncher0.position, transform.rotation);
                }
            }

            else
            {
                fireCounter = GetRandomFireRate();
            }
        }
    }


    private void FireMissile1()
    {
        if (!GameController.gameController.enemyDestroyed[1] &&
            !GameController.gameController.playerDestroyed)
        {
            if (fireCounter > 0f)
            {
                fireCounter -= Time.deltaTime;

                if (fireCounter <= 0f)
                {
                    fireCounter = GetRandomFireRate();

                    Instantiate(enemyBullet, weaponLauncher1.position, transform.rotation);
                }
            }

            else
            {
                fireCounter = GetRandomFireRate();
            }
        }
    }


    #region SCREEN WRAP
    private void ScreenWrap0()
    {
        bool objectIsVisible = WithinScreenBoundary0();

        if (objectIsVisible)
        {
            screenWrappingX[0] = false;
            screenWrappingY[0] = false;

            return;
        }

        if (screenWrappingX[0] && screenWrappingY[0])
        {
            return;
        }


        Vector2 objectPosition = enemyShip[0].position;


        if (objectPosition.y > screenBoundary.topScreenBoundary.position.y)
        {
            objectPosition.y = screenBoundary.bottomScreenBoundary.position.y;

            screenWrappingY[0] = true;
        }

        if (objectPosition.y < screenBoundary.bottomScreenBoundary.position.y)
        {
            objectPosition.y = screenBoundary.topScreenBoundary.position.y;

            screenWrappingY[0] = true;
        }


        if (objectPosition.x > screenBoundary.rightScreenBoundary.position.x)
        {
            objectPosition.x = screenBoundary.leftScreenBoundary.position.x;

            screenWrappingX[0] = true;
        }

        if (objectPosition.x < screenBoundary.leftScreenBoundary.position.x)
        {
            objectPosition.x = screenBoundary.rightScreenBoundary.position.x;

            screenWrappingX[0] = true;
        }


        enemyShip[0].position = objectPosition;
    }


    private void ScreenWrap1()
    {
        bool objectIsVisible = WithinScreenBoundary1();

        if (objectIsVisible)
        {
            screenWrappingX[1] = false;
            screenWrappingY[1] = false;

            return;
        }

        if (screenWrappingX[1] && screenWrappingY[1])
        {
            return;
        }


        Vector2 objectPosition = enemyShip[1].position;


        if (objectPosition.y > screenBoundary.topScreenBoundary.position.y)
        {
            objectPosition.y = screenBoundary.bottomScreenBoundary.position.y;

            screenWrappingY[1] = true;
        }

        if (objectPosition.y < screenBoundary.bottomScreenBoundary.position.y)
        {
            objectPosition.y = screenBoundary.topScreenBoundary.position.y;

            screenWrappingY[1] = true;
        }


        if (objectPosition.x > screenBoundary.rightScreenBoundary.position.x)
        {
            objectPosition.x = screenBoundary.leftScreenBoundary.position.x;

            screenWrappingX[1] = true;
        }

        if (objectPosition.x < screenBoundary.leftScreenBoundary.position.x)
        {
            objectPosition.x = screenBoundary.rightScreenBoundary.position.x;

            screenWrappingX[1] = true;
        }


        enemyShip[1].position = objectPosition;
    }


    private bool WithinScreenBoundary0()
    {
        if (enemyShip[0].position.y < screenBoundary.topScreenBoundary.position.y &&
            enemyShip[0].position.y > screenBoundary.bottomScreenBoundary.position.y &&
            enemyShip[0].position.x > screenBoundary.leftScreenBoundary.position.x &&
            enemyShip[0].position.x < screenBoundary.rightScreenBoundary.position.x)
        {
            return true;
        }

        return false;
    }


    private bool WithinScreenBoundary1()
    {
        if (enemyShip[1].position.y < screenBoundary.topScreenBoundary.position.y &&
            enemyShip[1].position.y > screenBoundary.bottomScreenBoundary.position.y &&
            enemyShip[1].position.x > screenBoundary.leftScreenBoundary.position.x &&
            enemyShip[1].position.x < screenBoundary.rightScreenBoundary.position.x)
        {
            return true;
        }

        return false;
    }
    #endregion


    public void CollisionDetected(EnemyShipController enemy)
    {
        if (!GameController.gameController.playerDestroyed)
        {
            if (enemy.gameObject.CompareTag("Enemy0") || enemy.gameObject.CompareTag("Enemy1"))
            {
                string collidingObject = enemy.gameObject.tag;

                GameController.gameController.playerDestroyed = true;

                DestroyEnemy(collidingObject);

                if (!GameController.gameController.gameOver)
                {
                    GameController.gameController.UpdatePlayerScore();
                    GameController.gameController.UpdateEnemyScore();
                }
            }
        }
    }


    public void DestroyEnemy(string collidingObject)
    {
        if (moveTogether)
        {
            gameObject.SetActive(false);
        }

        else
        {
            if (collidingObject == "Enemy0")
            {
                enemyShip[0].gameObject.SetActive(false);
            }

            if (collidingObject == "Enemy1")
            {
                enemyShip[1].gameObject.SetActive(false);
            }
        }

        audioController.StopAudioClip("Thrusters Engaged");
        audioController.StopAudioClip("Rotate Ship");
        audioController.StopAudioClip("Fire Player Bullet");
        audioController.StopAudioClip("Fire Enemy Bullet");

        audioController.PlayAudioClip("Explosion");

        if (moveTogether)
        {
            GameController.gameController.enemyDestroyed[0] = true;
            GameController.gameController.enemyDestroyed[1] = true;
        }

        else
        {
            if (collidingObject == "Enemy0")
            {
                GameController.gameController.enemyDestroyed[0] = true;
            }

            if (collidingObject == "Enemy1")
            {
                GameController.gameController.enemyDestroyed[1] = true;
            }
        }
    }


} // end of class
