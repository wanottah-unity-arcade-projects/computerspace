
using UnityEngine;

//
// Atari Video Game Data Controller v2020.10.20
//
// created 17-12-2019
//


public class AtariGameDataController : MonoBehaviour
{
    // reference to atari game data script
    private AtariGameData atariGameData;


    private void Awake()
    {
        atariGameData = GetComponent<AtariGameData>();
    }


    public void SelectGame(string GAME_TITLE)
    {
        switch (GAME_TITLE)
        {
            case AtariGameData.COMPUTERSPACE:

                atariGameData.ComputerSpace();

                break;
        }
    }


} // end of class
