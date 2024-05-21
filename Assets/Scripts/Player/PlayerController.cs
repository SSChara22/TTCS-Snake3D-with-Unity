// This script is responsible for all player movement
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public PlayerDirection direction;       //Hướng di chuyển

    [HideInInspector]
    public float stepLength = .2f;          //Khoảng cách di chuyển

    [HideInInspector]
    public float movementFrequency = .1f;   //Cường độ di chuyển

    [SerializeField]
    private GameObject nodePrefab;          //Mọc thêm thân khi ăn quả

    private float defaultMovementFreq = .1f;//Reset cường độ di chuyển khi bắt đầu lại game
    private List<Vector3> deltaPosition;    //Ghi lại thông tin vị trí khi đổi hướng
    private List<Rigidbody> nodes;          //Kiểm soát vị trí thân
    private Vector3 fruitNodePosition;      //Lưu lại phần thân mọc thêm

    private Rigidbody headRB;             
    private Transform tr;

    private float counter = 0f;
    private bool move = false;
    private bool createNodeAtTail = false; 

    void Awake()
    {
        tr = transform;   

        InitSnakeNodes();     

        InitPlayer();         

        deltaPosition = new List<Vector3>()
        {
            new Vector3(-stepLength, 0f),  // -dx .. Trái
            new Vector3(0f, stepLength),   // dy  .. Trên
            new Vector3(stepLength, 0f),   // dx  .. Phải
            new Vector3(0f, -stepLength)   // -dy .. Dưới
        };

        // Hàm nghe event ăn quả
        GameManager.Instance.FruitAteEvent.AddListener(OnFruitAte);

        // Hàm nghe event reset
        GameManager.Instance.ResetEvent.AddListener(OnReset);
    }

    void Update()
    {
        if (GameManager.Instance.gameState == GameState.Playing)
        {
            counter += Time.deltaTime;

            if (counter >= movementFrequency)
            {
                counter = 0f;
                move = true;
            }
        }
    }

    void FixedUpdate()
    {
        if (move && GameManager.Instance.gameState == GameState.Playing)
        {
            move = false;

            Move();
        }
    }

    private void Move()
    {
        //Tìm vị trí điểm cắt dựa theo hướng di chuyển
        Vector3 dPosition = deltaPosition[(int)direction];

        Vector3 parentPos = headRB.position;
        Vector3 prevPos;
        //Di chuyển đầu sau đó đến thân
        headRB.position = headRB.position + dPosition;
        
        //Di chuyển toàn bộ thân
        for (int i = 1; i < nodes.Count; i++)
        {
            //Gán vị trí mới cho các node thân
            prevPos = nodes[i].position;

            nodes[i].position = parentPos;

            parentPos = prevPos;
        }

        //Kiểm tra có mọc thêm thân mới không
        if (createNodeAtTail)
        {
            createNodeAtTail = false;

            GameObject newNode = ObjectsPoolManager.Instance.GetPooledObject(nodePrefab, fruitNodePosition, Quaternion.identity);
            newNode.transform.SetParent(transform, true);
            nodes.Add(newNode.GetComponent<Rigidbody>());
        }
    }

    private void ForceMove()
    {
        counter = 0f;

        move = false;

        Move();
    }

  
    private void InitSnakeNodes()
    {
        nodes = new List<Rigidbody>();
        
        nodes.Add(tr.GetChild(0).GetComponent<Rigidbody>());   
        nodes.Add(tr.GetChild(1).GetComponent<Rigidbody>());    
        nodes.Add(tr.GetChild(2).GetComponent<Rigidbody>());   

        headRB = nodes[0];
    }

    private void SetDirectionRandom()
    {
        int ranDirection = Random.Range(0, (int)PlayerDirection.Count);
        direction = (PlayerDirection)ranDirection;
    }

    public void SetInputDirection(PlayerDirection dir)
    {
        // Ngăn chặn di chuyển ngược hướng
        if (dir == PlayerDirection.Up && direction == PlayerDirection.Down ||
            dir == PlayerDirection.Down && direction == PlayerDirection.Up ||
            dir == PlayerDirection.Right && direction == PlayerDirection.Left ||
            dir == PlayerDirection.Left && direction == PlayerDirection.Right)
        {
            return;
        }

        direction = dir;

        ForceMove();
    }


    private void OnFruitAte(int scoreAddition, Vector3 fruitPosition)
    {
        //Xác định vị trí mọc thêm thân
        fruitNodePosition = nodes[nodes.Count - 1].position;

        createNodeAtTail = true;
    }

    private void InitPlayer()
    {
        SetDirectionRandom();  //Xuất phát với hướng bất kì

        switch (direction)
        {
            case PlayerDirection.Right:   
                nodes[1].position = nodes[0].position - new Vector3(Metrics.SNACK_NODE, 0f, 0f);
                nodes[2].position = nodes[0].position - new Vector3(Metrics.SNACK_NODE * 2, 0f, 0f);
                break;

            case PlayerDirection.Left:   
                nodes[1].position = nodes[0].position + new Vector3(Metrics.SNACK_NODE, 0f, 0f);
                nodes[2].position = nodes[0].position + new Vector3(Metrics.SNACK_NODE*2, 0f, 0f);
                break;

            case PlayerDirection.Up:    
                nodes[1].position = nodes[0].position - new Vector3(0f, Metrics.SNACK_NODE, 0f);
                nodes[2].position = nodes[0].position - new Vector3(0f, Metrics.SNACK_NODE*2f, 0f);
                break;

            case PlayerDirection.Down:    
                nodes[1].position = nodes[0].position + new Vector3(0f, Metrics.SNACK_NODE, 0f);
                nodes[2].position = nodes[0].position + new Vector3(0f, Metrics.SNACK_NODE * 2f, 0f);
                break;
        }
    }

 
    private void OnReset()
    {
        createNodeAtTail = false;

        //Reset cường độ di chuyển
        movementFrequency = defaultMovementFreq;

        //Loại toàn bộ phần thân bổ sung
        while (nodes.Count > 3)
        {
            Rigidbody node = nodes[3];

            nodes.Remove(node); 

            ObjectsPoolManager.Instance.DestroyGameObjectWithPooledChildren(node.gameObject);
        }

        //Đưa đầu rắn về vị trí xuất phát
        nodes[0].transform.position = new Vector3(0f, 0f, 5.72f);

        //Đưa thân rắn về theo vị trí xuất phát ngẫu nhiên
        InitPlayer();
    }

    public void DecreaseMovementFrequency(float amount, float minMovementFreq)
    {
        float newFreq = movementFrequency - amount;

        movementFrequency = Mathf.Max(newFreq, minMovementFreq);
    }
}
