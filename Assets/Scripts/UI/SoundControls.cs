using UnityEngine;
using System.Collections;

public class SoundControls : MonoBehaviour {

	public void ToggleMusic()
    {
        if (Time.time > .3f)     
		    SoundManager.Instance.ToggleMuteMusic();
	}

	public void ToggleSoundEffect()
    {
        if (Time.time > .3f)
		    SoundManager.Instance.ToggleMuteSoundEffects();
	}
}
