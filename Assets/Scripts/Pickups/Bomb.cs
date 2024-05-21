using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : PooledObject
{
    [SerializeField]
    private float Minimum = 2f;

    [SerializeField]
    private float Maximum = 11f;

    void Awake()
    {
        iTween.Init(gameObject);     

        StartingAnimation();         //Animation xuất hiện

        //Hàm tự ngắt sinh bom khi game over
        GameManager.Instance.GameOverEvent.AddListener(OnGameOver);
    }

    public override void OnPooledObjectActivated()
    {
        base.OnPooledObjectActivated();

        GameManager.Instance.GameOverEvent.AddListener(OnGameOver);

        StartingAnimation();       

        StartCoroutine(DestroyAfter(Random.Range(Minimum, Maximum)));
    }

    IEnumerator DestroyAfter(float dur)
    {
        yield return new WaitForSeconds(dur);

        ObjectsPoolManager.Instance.DestroyGameObjectWithPooledChildren(gameObject);

        //Sau khi bom bị huỷ, kích hoạt hàm sinh bom mới
        GameManager.Instance.BombDepletedEvent.Invoke();
    }

    void OnTriggerEnter(Collider other)
    {
        //Hàm event va chạm bom
        if (GameManager.Instance.gameState == GameState.Playing)
        {
            GameManager.Instance.GameOverEvent.Invoke();
;
            ObjectsPoolManager.Instance.DestroyGameObjectWithPooledChildren(gameObject);
        }
    }

    private void OnGameOver()
    {
        ObjectsPoolManager.Instance.DestroyGameObjectWithPooledChildren(gameObject);
    }

    private void StartingAnimation()
    {
        iTween.ScaleAdd(transform.GetChild(0).gameObject, iTween.Hash(
                "amount", new Vector3(.1f, .1f, .1f),
                "time", .15f,
                "looptype", iTween.LoopType.pingPong,
                "easeType", iTween.EaseType.linear));
    }

    public override void OnPooledObjectDeactivated()
    {
        base.OnPooledObjectDeactivated();

        GameManager.Instance.GameOverEvent.RemoveListener(OnGameOver);
    }
}
