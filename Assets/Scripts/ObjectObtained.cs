using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectObtained : MonoBehaviour
{
    
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Pacman"))
    //    {

    //        Debug.Log(countleft);
    //        Destroy(gameObject);
    //    }

    //}
        private void OnTriggerEnter2D(Collider2D other)
        {            
            Debug.Log("Contacted");
            Destroy(gameObject);
        }
}
