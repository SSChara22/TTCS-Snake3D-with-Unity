using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeHead : MonoBehaviour {

	void Start ()
    {
        //Nghe event ăn quả
        GameManager.Instance.FruitAteEvent.AddListener(OnFruitAte);
    }

    private void OnFruitAte(int scoreAddition, Vector3 position)
    {
        iTween.ShakeScale(transform.GetChild(0).gameObject, iTween.Hash(
                "amount", new Vector3(1f, 1f, 1f),
                "time", .25f,
                "delay", 0f,
                "easeType", iTween.EaseType.linear));
    }
}
