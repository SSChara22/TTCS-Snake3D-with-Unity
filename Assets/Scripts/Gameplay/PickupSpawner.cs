// PickupSpawner.cs: Spawns pickups like fruits and any other added pickup type
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    [SerializeField]
    private List<Pickups> pickups;      // List pickups

    private Transform pickupsParent;    // parent của pickups, dùng để sắp xếp scene và được reset mỗi khi bắt đầu trò chơi

    private int bombAmount = 1;         //Lượng bom sinh ra
    void Start()
    {
        //Hàm sinh quả mới sau khi quả cũ đã được ăn
        GameManager.Instance.FruitAteEvent.AddListener(OnFruitAte);

        //Hàm sinh bom sau khi đạt điều kiện
        GameManager.Instance.CreateBombEvent.AddListener(OnCreateBomb);

        //Hàm sinh quả khi bắt đầu game
        GameManager.Instance.GameCommencingEvent.AddListener(OnGameCommence);

        //Hàm reset danh sách pickups khi người chơi thua
        GameManager.Instance.GameOverEvent.AddListener(OnGameOver);

        pickupsParent = GameObject.FindGameObjectWithTag(Tags.pickupsParent).transform;
    }

    private void OnFruitAte(int scoreAdded, Vector3 position)
    {
        StartCoroutine(CreateFruitRandom());
    }

    private void OnCreateBomb(int amount = 1)
    {
        bombAmount = amount;

        StartCoroutine(CreateBombRandom());
    }

    private void OnGameCommence()
    {
        //Đảm bảo toàn bộ list pickups được reset
        OnGameOver();

        StartCoroutine(CreateFruitRandom(false));
    }

    private IEnumerator CreateFruitRandom(bool playEatFruit = true)
    {
        while(true)
        {
            int ranX = Random.Range((int)WorldBorders.LeftBorder, (int)WorldBorders.RightBorder);
            int ranY = Random.Range((int)WorldBorders.BottomBorder, (int)WorldBorders.TopBorder);
            Vector3 newPosition = new Vector3(ranX, ranY, 5.72f);

            Collider[] hit = Physics.OverlapSphere(newPosition, (int)Metrics.FRUIT);

            //Hàm tìm vị trí sinh quả phù hợp
            if (hit.Length > 0)
            {
                yield return new WaitForEndOfFrame();
            }
            else
            {
                //Vị trí sinh quả
                Quaternion rot = Quaternion.Euler(-90f, 0f, 0f);
                GameObject fruit = ObjectsPoolManager.Instance.GetPooledObject(pickups[0].prefab, newPosition, rot);
                fruit.transform.SetParent(pickupsParent);

                //Hàm phát âm thanh khi ăn quả
                if (playEatFruit)
                    SoundManager.Instance.PlaySoundEffect(SoundEffectName.EAT_FRUIT, 1f);
                else
                    SoundManager.Instance.PlaySoundEffect(SoundEffectName.COLLECT_FRUIT, 1f);
                yield break;
            }
        }
    }


    private IEnumerator CreateBombRandom()
    {
        while (true)
        {
            int ranX = Random.Range((int)WorldBorders.LeftBorder, (int)WorldBorders.RightBorder);
            int ranY = Random.Range((int)WorldBorders.BottomBorder, (int)WorldBorders.TopBorder);
            Vector3 newPosition = new Vector3(ranX, ranY, 5.72f);

            Collider[] hit = Physics.OverlapSphere(newPosition, (int)Metrics.FRUIT);

            //Hàm tìm vị trí sinh bom phù hợp
            if (hit.Length > 0)
            {
                yield return new WaitForEndOfFrame();
            }
            else
            {
                //Vị trí tạo bom
                GameObject bomb = ObjectsPoolManager.Instance.GetPooledObject(pickups[1].prefab, newPosition, Quaternion.identity);
                bomb.transform.SetParent(pickupsParent);

                //Phát âm thanh tạo bom
                SoundManager.Instance.PlaySoundEffect(SoundEffectName.SPAWN_BOMB, 1f);

                bombAmount--;
                if (bombAmount != 0)        //Nếu số lượng bom hơn 1 thì tạo thêm
                {               
                    StartCoroutine(CreateBombRandom());
                }
                yield break;
            }
        }
    }

    private void OnGameOver()
    {
        foreach(Transform pickup in pickupsParent)
        {
            ObjectsPoolManager.Instance.DestroyGameObjectWithPooledChildren(pickup.gameObject);
        }
    }
}

[System.Serializable]
class Pickups
{
    public PickupType type = PickupType.Fruit;
    public GameObject prefab;
}