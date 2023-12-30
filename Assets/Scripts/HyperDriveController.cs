
using UnityEngine;

//
// Computer Space 1971 v2020.10.26
//
// created 2020.10.26
//


public class HyperDriveController : MonoBehaviour
{

    public GameObject bulletEcho;

    private float timeBetweenSpawns;
    private float startTimeBetweenSpawns;



    // Start is called before the first frame update
    void Start()
    {
        Initialise();
    }


    // Update is called once per frame
    void Update()
    {
        SpawnBulletEcho();
    }


    private void Initialise()
    {
        timeBetweenSpawns = 0f;

        startTimeBetweenSpawns = 0.001f;
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


} // end of class
