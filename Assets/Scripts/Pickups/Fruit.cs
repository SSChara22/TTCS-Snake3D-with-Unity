using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : PooledObject {

    [HideInInspector]
    public int scoreAddition = 10;

    void Awake()
    {
        iTween.Init(gameObject); 

        StartingAnimation();         //Animation xuất hiện
    }

    public override void OnPooledObjectActivated()
    {
        base.OnPooledObjectActivated();

        StartingAnimation();
    }

    void OnTriggerEnter(Collider other)
    {
        //Hàm event ăn quả
        if (GameManager.Instance.gameState == GameState.Playing)
        {
            ObjectsPoolManager.Instance.DestroyPooledGameObject(gameObject);

            GameManager.Instance.FruitAteEvent.Invoke(scoreAddition, transform.position);
        }
    }

    private void StartingAnimation()
    {
        iTween.ShakePosition(gameObject, iTween.Hash(
                "x", .035f,
                "y", .035f,
                "time", .2f,
                "delay", .1f,
                "easeType", iTween.EaseType.easeOutCirc));
    }
}
