using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour {

    public Button pauseBtn;
    public Button playBtn;
    public Button FFBtn;

	// Use this for initialization
	void Start () {

        pauseBtn.onClick.AddListener(Pause);
        playBtn.onClick.AddListener(Play);
        FFBtn.onClick.AddListener(FastForward);

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Pause()
    {
        Time.timeScale = 0;
    }

    void Play()
    {
        Time.timeScale = 1;
    }

    void FastForward()
    {
        Time.timeScale = 2;
    }
}
