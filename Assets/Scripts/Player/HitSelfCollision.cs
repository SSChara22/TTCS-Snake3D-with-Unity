using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSelfCollision : MonoBehaviour
{
    void OnCollisionEnter(Collision other)
    {
        //Hàm event va chạm với bản thân
        if (other.collider.tag == Tags.node && GameManager.Instance.gameState == GameState.Playing)
        {
            GameManager.Instance.GameOverEvent.Invoke();
        }
    }
}
