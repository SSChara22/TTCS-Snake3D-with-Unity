using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour {

    [HideInInspector]
    public UnityEvent CurrentScoreUpdatedEvent; //Hàm cập nhật điểm mỗi khi tăng

    [HideInInspector]
    public UnityEvent BestScoreUpdatedEvent;    //Hàm cập nhật điểm tốt nhất sau khi có kỷ lục mới

    public static ScoreManager Instance;        //Đảm bảo chỉ có một loại hàm chạy trong 1 thời điểm

    [SerializeField]
    private GameObject[] scorePrefabs;          //Tạo một dòng thông tin hiện điểm

    private bool displayNewHighScore = false;   //Hiển thị điểm cao nhất

    public int CurrentScore
    {
        get
        {
            return currentScore;
        }
        set
        {
            currentScore = value;

            CurrentScoreUpdatedEvent.Invoke();      //Cập nhật thông tin điểm mới
        }
    }

    public int BestScore
    {
        set
        {
            bestScore = value;

            BestScoreUpdatedEvent.Invoke();         //Cập nhật thông tin điểm cao nhất mới

            PlayerSettings.SetBestScore(bestScore); //Lưu lại thông tin điểm cao nhất
        }
        get
        {
            return bestScore;
        }
    }

    private int currentScore;                //Điểm hiện tại
    private int bestScore;                   //Lấy điểm cao nhất từ PlayerPrefs nếu có

    void Awake()
    {
        if (Instance == null)
            Instance = this;

        else if (Instance != this)
            Destroy(gameObject);
                                            
        DontDestroyOnLoad(gameObject);       //Chuyển scene khi có nhiều scene tồn tại

        CurrentScoreUpdatedEvent = new UnityEvent();
        BestScoreUpdatedEvent = new UnityEvent();

        InitScoreManager();
    }

    void InitScoreManager()
    {
        //Nhập điểm hiện tại
        currentScore = 0;

        //Lấy điểm cao nhất từ dữ liệu đã lưu
        bestScore = PlayerSettings.GetBestScore();

        //Nghe hàm kết thúc game để cập nhật lại điểm cao nhất
        GameManager.Instance.GameOverEvent.AddListener(OnGameOver);

        //Cập nhật điểm mỗi khi ăn quả
        GameManager.Instance.FruitAteEvent.AddListener(OnFruitAte);

        //Reset điểm mỗi khi bắt đầu lượt chơi mới
        GameManager.Instance.ResetEvent.AddListener(OnReset);
    }

    private void OnFruitAte(int scoreAddition, Vector3 position)
    {
        //Tăng điểm hiện tại
        CurrentScore += scoreAddition;

        //Hiển thị thông báo có điểm cao nhất mới
        if (CurrentScore > BestScore && !displayNewHighScore && BestScore != 0)
        {
            displayNewHighScore = true;

            Instantiate(ScoringTextResolution(0), new Vector3(position.x, position.y, 5f), Quaternion.identity);

            //Phát hiệu ứng âm thanh
            SoundManager.Instance.PlaySoundEffect(SoundEffectName.NEW_HIGH_SCORE, 1f);
        }
        //Hiển thị điểm tăng lên mỗi khi ăn quả
        else
        {
            Instantiate(ScoringTextResolution(scoreAddition), new Vector3(position.x, position.y, 5f), Quaternion.identity);
        }
    }

    private void OnGameOver()
    {
        if (currentScore > bestScore)
        {
            //Cập nhật lại điểm cao nhất
            BestScore = CurrentScore;
        }
    }

    private void OnReset()
    {
        CurrentScore = 0;
    }

        private GameObject ScoringTextResolution(int score)
    {
        GameObject scorePrefab = null;

        switch(score)
        {
            case 5:
                scorePrefab = scorePrefabs[0];
                break;
            case 10:
                scorePrefab = scorePrefabs[1];
                break;
            case 20:
                scorePrefab = scorePrefabs[2];
                break;
            case 50:
                scorePrefab = scorePrefabs[3];
                break;
            case 0:
                scorePrefab = scorePrefabs[4];
                break;
        }

        return scorePrefab;
    }
}
