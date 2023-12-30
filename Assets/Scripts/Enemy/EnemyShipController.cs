
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
// Computer Space 1971
//
// created 2020.10.16
//
// modified 2020.10.18
//


public class EnemyShipController : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        transform.parent.GetComponent<EnemyController>().CollisionDetected(this);
    }


} // end of class
