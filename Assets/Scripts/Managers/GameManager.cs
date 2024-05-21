using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public enum GameState
{
    Playing,
    MainMenu,
    Revive,
};

public class FruitAteEventBase : UnityEvent<int, Vector3>
{ }

public class CreateBombEventBase : UnityEvent<int>
{ }

public class GameManager : MonoBehaviour {

    public static GameManager Instance;        //Đảm bảo chỉ 1 bản game được chạy tại 1 thời điểm

    [HideInInspector]
    public GameObject player;                  //Lưu dữ liệu người chơi

    [HideInInspector]                          //Event bắt đầu trò chơi khi người chơi nhấn bắt đầu và animation chạy hết
    public UnityEvent GameCommencingEvent = new UnityEvent();

    [HideInInspector]                          //Event va vào bom hoặc tường
    public UnityEvent GameOverEvent = new UnityEvent();

    [HideInInspector]                          //Event reset game
    public UnityEvent ResetEvent = new UnityEvent();

    [HideInInspector]                          //Event tạo bom
    public CreateBombEventBase CreateBombEvent = new CreateBombEventBase();

    [HideInInspector]                          //Event huỷ bom
    public UnityEvent BombDepletedEvent = new UnityEvent();

    [HideInInspector]                          //Event ăn quả
    public FruitAteEventBase FruitAteEvent = new FruitAteEventBase();

    [HideInInspector]
    public GameState gameState;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)   //Ngăn chặn nhiều bản game chạy cùng lúc
        {
            Destroy(gameObject);
            
            throw new UnityException("Duplicate Game Manager!!");
        }
       
        DontDestroyOnLoad(gameObject);

        GameOverEvent.AddListener(OnGameOver);
        ResetEvent.AddListener(OnReset);

        player = GameObject.FindGameObjectWithTag(Tags.player);

        gameState = GameState.MainMenu;
    }

    private void OnGameOver()
    {
        gameState = GameState.Revive;

        //Dừng nhạc khi game kết thúc
        SoundManager.Instance.StopMusic();

        //Phát hiệu ứng âm thanh game kết thúc
        SoundManager.Instance.PlaySoundEffect(SoundEffectName.HIT, 1f);
    }

    private void OnReset()
    {
        gameState = GameState.MainMenu;
    }
}
