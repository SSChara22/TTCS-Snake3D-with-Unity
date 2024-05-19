using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyDelayed : MonoBehaviour {

    private float duration;
	void Start ()
    {
        //Huỷ bom sau 1 khoảng thời gian nhất định
        duration = GetComponent<ScoreText>().aliveDuration;

        StartCoroutine(DestroyAfter(duration));	
	}
	
	IEnumerator DestroyAfter(float dur)
    {
        yield return new WaitForSeconds(dur);

        Destroy(gameObject);
    }
}
