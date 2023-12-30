
using UnityEngine;

//
// Computer Space 1971 v2020.10.20
//
// created 2020.10.20
//


public class PlayerSpawnBoundaryController : MonoBehaviour
{
    public static PlayerSpawnBoundaryController playerSpawnBoundaries;


    public Transform topSpawnBoundary;
    public Transform bottomSpawnBoundary;
    public Transform leftSpawnBoundary;
    public Transform rightSpawnBoundary;



    private void Awake()
    {
        playerSpawnBoundaries = this;
    }


} // end of class
