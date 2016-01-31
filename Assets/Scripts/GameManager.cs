#define DEBUG
#if !UNITY_EDITOR
#undef DEBUG
#endif
#undef DEBUG

using UnityEngine;
using System.Collections;

public partial class GameManager : Singleton<GameManager> {
	//gooku, setting at the object/-s
	public NoticeView noticeView;
	public ResultView resultView;
	public GameObject StartMenu;
	public GameObject[] StageArray;
	public AudioSource LoopMusic;
	public AudioSource BtnAudio;
	//gooku, setting at the object/-e

	public bool IsPlayable { get { return gamestatus == GameStatus.PROCESS; } }

	private int currentStageIndex = 0;
	private int passRequestCount = 3;
	private float punishTime = 1;

	private float remainTimeRate { get { return remainTime / passTime; } }

	private GameStatus gamestatus = GameStatus.NON;
	private StageTableHandler stageTableHandler = new StageTableHandler();


	protected override void Awake()
	{
		base.Awake();
		stageTableHandler.InitData();
    }

	private void Start() {
#if DEBUG
		GameStart();
#else
		m_bg_scrolling = false;
		gamestatus = GameStatus.NON;
		StartMenu.SetActive(true);
#endif
	}

	private void Update() {
		if(gamestatus == GameStatus.PROCESS)
			GameTimeHandle();
	}

	public void GameStart(){
		StartMenu.SetActive(false);
		currentStageIndex = 0;
		InitStage(currentStageIndex);
		m_bg_scrolling = true;
		gamestatus = GameStatus.PROCESS;
		LoopMusic.clip = Resources.Load<AudioClip>("sound/bg_music_loop");
		LoopMusic.Play();
    }

	public void GetDecide(bool _isPass) {

		if (gamestatus != GameStatus.PROCESS)
			return;

		if(noticeView != null)
			noticeView.IssueNotice(_isPass);

		if (_isPass)
		{
			BtnAudio.clip = Resources.Load<AudioClip>("sound/correct_sound");
			BtnAudio.Play();
			passRequestCount--;
			gameTimeBar.UpdateTime(passRequestCount);
            if (passRequestCount == 0)
				Pass();
		}
		else {
			BtnAudio.clip = Resources.Load<AudioClip>("sound/sound_wrong");
			BtnAudio.Play();

			remainTime -= punishTime;

			if (remainTime <= 0) {
				gameTimeBar.UpdateBar(0, 0);
				GameOver();
			} else {
				gameTimeBar.UpdateBar(remainTimeRate, remainTime);
			}
		}
	}

	private void Pass() {
		if (resultView == null)
			return;

		gamestatus = GameStatus.PASS;
		resultView.IssueResult(true);
    }

	private void GameOver() {
		if (resultView == null)
			return;
		gamestatus = GameStatus.FAIL;
		resultView.IssueResult(false);
	}

	private void InitStage(int _index) {

		StageData stageData = stageTableHandler.GetStageData(_index);

		passTime = stageData.gameTime;
		passRequestCount = stageData.passRequestCount;

		remainTime = passTime;
		gameTimeBar.UpdateBar(remainTimeRate, remainTime);
		gameTimeBar.UpdateTime(passRequestCount);

		for (int i=0; i < StageArray.Length;i++)
		{
			StageArray[i].SetActive(_index == i);
		}
    }

	public void NextStage()
	{
		currentStageIndex++;

		if(m_bg_scroll_speed > -2)
			m_bg_scroll_speed -= 0.05f;

		if (currentStageIndex >= StageArray.Length)
			currentStageIndex = 0;

		BtnAudio.clip = Resources.Load<AudioClip>("sound/change_stage");
		BtnAudio.Play();


		InitStage(currentStageIndex);
		gamestatus = GameStatus.PROCESS;

		resultView.Init();
	}

	public void Restart() {
		if (m_bg_scroll_speed < -0.05f)
			m_bg_scroll_speed += 0.05f;

		InitStage(currentStageIndex);
		gamestatus = GameStatus.PROCESS;
		resultView.Init();
	}

}

public enum GameStatus { NON, PROCESS, PASS, FAIL }
