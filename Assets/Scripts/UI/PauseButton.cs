using UnityEngine.UI;
using UnityEngine;

public class PauseButton : MonoBehaviour {

    [SerializeField]
    private Sprite pauseIcon;
    [SerializeField]
    private Sprite playIcon;

    private Image imageComponent;       //Lấy ảnh icon
    private bool pauseActive = true;    //Icon bật

    void Start()
    {
        imageComponent = GetComponent<Image>();
    }

    public void OnPause()
    {
        if (pauseActive)
        {
            //Chèn ảnh icon phát
            imageComponent.overrideSprite = playIcon;

            pauseActive = false;
        }
        else
        {
            //Chèn ảnh icon ngừng phát
            imageComponent.overrideSprite = pauseIcon;

            pauseActive = true;
        }
    }
}
