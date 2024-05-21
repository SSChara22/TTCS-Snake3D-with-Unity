using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitlePanel : MonoBehaviour
{
    private Animator anim;       

    private int slideOutHash;    
    private int slideInHash;     

    [SerializeField]
    private HUD hudScript;     

    [SerializeField]
    private GameObject player;  

    void Awake()
    {
        anim = GetComponent<Animator>();

        slideOutHash = Animator.StringToHash("SlideOut");
        slideInHash = Animator.StringToHash("SlideIn");
    }

    void Start()
    {
        GameManager.Instance.GameOverEvent.AddListener(OnGameOver);
    }

    public void OnPlay()
    {
        if (GameManager.Instance.gameState == GameState.Playing)
            return;

        GameManager.Instance.gameState = GameState.Playing;

        SoundManager.Instance.PlayMenuMusic();

        anim.SetTrigger(slideOutHash);

        StartCoroutine(SlideInHUD(.5f));

        StartCoroutine(ActivatePlayer(.85f));
    }

    IEnumerator SlideInHUD(float delay, bool slideIn = true)
    {
        yield return new WaitForSeconds(delay);

        if (slideIn)
            hudScript.SlideIn();
        else
            hudScript.SlideOut();
    }

    IEnumerator ActivatePlayer(float delay)
    {
        yield return new WaitForSeconds(delay);

        GameManager.Instance.GameCommencingEvent.Invoke();
        player.SetActive(true);
    }


    private void OnGameOver()
    {
        StartCoroutine(ResetGameAfter(1f));
    }

    IEnumerator ResetGameAfter(float dur)
    {
        yield return new WaitForSeconds(dur);

        GameManager.Instance.ResetEvent.Invoke();

        OnReturn();
    }

    private void OnReturn()
    {
        anim.SetTrigger(slideInHash);

        StartCoroutine(SlideInHUD(0f, false));

        player.SetActive(false);
    }
}
