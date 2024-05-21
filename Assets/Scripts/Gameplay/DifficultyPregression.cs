using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyPregression : MonoBehaviour
{
    private float minMovementFreq = .02f; //Cường độ di chuyển tối thiểu của rắn

    private float difficultyFactor = .01f;//Giảm cường độ di chuyển

    private int foodCountToIncreaseSpeed = 4;  //Tăng tốc mỗi khi ăn được 4 quả
    private int foodCountToIncreaseBombCount = 7; //Tăng lượng bom xuất hiện mỗi khi ăn được 7 quả
    private int fruitAteCount = 0;        //Bộ đếm lượng quả đã ăn

    private int spawnBombAfter = 2;       //Làm xuất hiện bom sau mỗi lần ăn 2 quả
    private float bombSpawnCounter = 0f;  //Bộ đếm để xuất hiện bom ở các khoảng ngẫu nhiên

    private float minBombSpawnTime = 2f;
    private float maxBombSpawnTime = 11f;
    private float nextBombSpawnTime = 0f;
    private bool canSpawnBombs = true;    //Ngăn khả năng xuất hiện bom khi vẫn có bom tồn tại trên màn hình

    private int bombCount = 1;            //Lượng bom được sinh ra
    private int depletedCounter = 0;

    [SerializeField]
    private PlayerController playerController;
    void Start()
    {
        //Hàm nghe event ăn quả
        GameManager.Instance.FruitAteEvent.AddListener(OnFruitAte);

        //Hàm nghe event reset
        GameManager.Instance.ResetEvent.AddListener(OnReset);

        //Hàm nghe event bom biến mất
        GameManager.Instance.BombDepletedEvent.AddListener(OnBombDepleted);
    }

    private void OnFruitAte(int scoreAdded, Vector3 position)
    {
        fruitAteCount++;

        if (fruitAteCount % foodCountToIncreaseSpeed == 0)
        {
            //Hàm tăng tốc rắn
            playerController.DecreaseMovementFrequency(difficultyFactor, minMovementFreq);
        }

        if (fruitAteCount % foodCountToIncreaseBombCount == 0)
        {
            //Hàm tăng lượng bom xuất hiện
            bombCount++;
        }
    }

    private void Update()
    {
        //Điều kiện sinh ra bom
        if (fruitAteCount >= 2 && canSpawnBombs)
        {
            //Hàm tính toán thời gian sinh ra bom
            if (bombSpawnCounter == 0)
            {
                nextBombSpawnTime = Time.time + Random.Range(minBombSpawnTime, maxBombSpawnTime);
            }

            bombSpawnCounter += Time.deltaTime;

            if (bombSpawnCounter + Time.time >= nextBombSpawnTime)
            {
                // Kích hoạt sinh bom từ file PickupSpawner.cs
                depletedCounter = Random.Range(1, bombCount + 1);
                GameManager.Instance.CreateBombEvent.Invoke(depletedCounter);

                // Reset bộ đếm về 0
                bombSpawnCounter = 0f;

                // Tạm thời ngừng sinh bom
                canSpawnBombs = false;
            }
        }
    }

    private void OnBombDepleted()
    {
        depletedCounter--;

        if (depletedCounter == 0)
            canSpawnBombs = true;
    }

    private void OnReset()
    {
        canSpawnBombs = true;
        fruitAteCount = 0;
        bombSpawnCounter = 0f;
        bombCount = 1;
    }
}
