using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeNode : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        //Khi va chạm đầu với thân
        if (GameManager.Instance.gameState == GameState.Playing)
        {
            GameManager.Instance.GameOverEvent.Invoke();
        }
    }
}
