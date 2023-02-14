using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{
    void OnTriggerEnter(Collider player)
    {

        if (player.gameObject.tag == "Player")
        {
 
            SceneManager.LoadScene(2);
            //SceneManager.MoveGameObjectToScene(player.gameObject);
            //DontDestroyOnLoad(this.gameObject);
        }
    }
}