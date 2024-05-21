using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVFX : MonoBehaviour {

    [SerializeField]
    private GameObject hitEffect;        //Sinh ra khi va vào tường hoặc bom

    private Transform headPosition;      //Tạo hiệu ứng nổ ở đầu rắn

    void Start()
    {
        headPosition = transform.GetChild(0);

        GameManager.Instance.GameOverEvent.AddListener(OnGameOver);
    }

    private void OnGameOver()
    {
        //Tạo hiệu ứng
        Instantiate(hitEffect, headPosition.position, Quaternion.identity);
    }
}
