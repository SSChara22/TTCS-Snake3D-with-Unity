using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerInput : MonoBehaviour
{
    private PlayerController playerController;

    private int horizontal = 0;
    private int vertical = 0;
    private Vector2 touchOrigin = -Vector2.one; 

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        
        //Nghe hàm kết thúc game và hàm reset
        GameManager.Instance.GameOverEvent.AddListener(OnGameOver);
        GameManager.Instance.ResetEvent.AddListener(OnReset);
    }

    private void Update()
    {
        horizontal = 0;    
        vertical = 0;    

#if UNITY_EDITOR || UNITY_STANDALONE
        GetKeyboardInput();
#endif
        SetMovement();      
    }

    private void SetMovement()
    {
        if (vertical != 0)
        {
            playerController.SetInputDirection( (vertical == 1) ? PlayerDirection.Up : PlayerDirection.Down );
        }
        else if (horizontal != 0)
        {
            playerController.SetInputDirection( (horizontal == 1) ? PlayerDirection.Right : PlayerDirection.Left );
        }
    }
    private void GetKeyboardInput()
    {
        //Lấy thông tin trục từ bàn phím
        horizontal = GetAxisRaw(Axis.Horizontal);
        vertical = GetAxisRaw(Axis.Vertical);

        //Kiểm tra di chuyển ngang
        if (horizontal != 0)
        {
            vertical = 0;
        }
    }

    private int GetAxisRaw(Axis axis)
    {
        if (axis == Axis.Horizontal)
        {
            bool left = Input.GetKeyDown(KeyCode.LeftArrow);
            bool right = Input.GetKeyDown(KeyCode.RightArrow);

            if (left)
                return -1;

            if (right)
                return 1;

            return 0;
        }
        else if (axis == Axis.Vertical)
        {
            bool up = Input.GetKeyDown(KeyCode.UpArrow);
            bool down = Input.GetKeyDown(KeyCode.DownArrow);

            if (down)
                return -1;

            if (up)
                return 1;

            return 0;
        }

        return 0;
    }


    private void OnGameOver()
    {
        enabled = false;
    }

    private void OnReset()
    {
        enabled = true;
    }
}

public enum Axis
{
    Vertical,
    Horizontal
};