using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreText : MonoBehaviour {

    public float aliveDuration = .6f;           

    private SpriteRenderer textSpriteRenderer;  

    void Awake()
    {
        iTween.Init(gameObject);

        textSpriteRenderer = GetComponent<SpriteRenderer>();
	}

    void Start()
    {
        ScoreTextAnimation();
    }

    private void ScoreTextAnimation()
    {
        iTween.MoveAdd(gameObject, iTween.Hash(
                "y", .4f,
                "time", aliveDuration,
                "delay", 0f,
                "easeType", iTween.EaseType.easeOutQuad));

        iTween.ValueTo(gameObject, iTween.Hash(
                "from", textSpriteRenderer.color,
                "to", Color.clear,
                "time", aliveDuration * .5f,
                "delay", aliveDuration * .5f,
                "onupdate", "OnColorUpdated",
                "easeType", iTween.EaseType.linear));

        iTween.ScaleTo(gameObject, iTween.Hash(
                "scale", new Vector3(1f, 1f, 1f),
                "time", aliveDuration,
                "delay", 0f,
                "easeType", iTween.EaseType.easeOutQuad));
    }

    private void OnColorUpdated(Color color)
    {
        textSpriteRenderer.color = color;
    }
}
