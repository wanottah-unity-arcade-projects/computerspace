
using UnityEngine;

//
// Computer Space 1971 v2020.10.20
//
// created 2020.10.16
//


public class ScreenBoundaryController : MonoBehaviour
{
    public static ScreenBoundaryController screenBoundaries;


    public Transform topScreenBoundary;
    public Transform bottomScreenBoundary;
    public Transform leftScreenBoundary;
    public Transform rightScreenBoundary;



    private void Awake()
    {
        screenBoundaries = this;
    }


} // end of class
