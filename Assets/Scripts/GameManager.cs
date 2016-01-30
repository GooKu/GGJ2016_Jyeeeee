using UnityEngine;
using System.Collections;

public partial class GameManager : Singleton<GameManager> {
	//gooku, setting at the object/-s
	public GameObject PassObj;
	public GameObject FailObj;
	public NoticeView noticeView;
	public GameObject StartMenu;
	//gooku, setting at the object/-e

	private int passRequestTime = 3;
	private float punishTime = 1;

	private float remainTimeRate { get { return remainTime / passTime; } }

	private GameStatus gamestatus = GameStatus.NON;


	protected override void Awake()
	{
		base.Awake();
	}

	private void Start() {
		m_bg_scrolling = false;
		gamestatus = GameStatus.NON;
	}

	private void Update() {
		if(gamestatus == GameStatus.PROCESS)
			GameTimeHandle();
	}

	public void GameStart(){
		StartMenu.SetActive(false);
		InitStage(0);
		m_bg_scrolling = true;
		gamestatus = GameStatus.PROCESS;
	}

	public void GetDecide(bool _isPass) {

		if (gamestatus != GameStatus.PROCESS)
			return;

		if(noticeView != null)
			noticeView.IssueNotice(_isPass);

		if (_isPass)
		{
			passRequestTime--;
			gameTimeBar.UpdateTime(passRequestTime);
            if (passRequestTime == 0)
				Pass();
		}
		else {
			remainTime -= punishTime;

			if (remainTime <= 0) {
				gameTimeBar.UpdateBar(0);
				GameOver();
			} else {
				gameTimeBar.UpdateBar(remainTimeRate);
			}
		}
	}

	private void Pass() {
		if (PassObj == null)
			return;

		gamestatus = GameStatus.PASS;
		PassObj.SetActive(true);
    }

	private void GameOver() {
		if (FailObj == null)
			return;
		gamestatus = GameStatus.FAIL;
		FailObj.SetActive(true);
    }

	private void InitStage(int _index) {
		remainTime = passTime;
		passRequestTime = 3;
		gameTimeBar.UpdateBar(remainTimeRate);
		gameTimeBar.UpdateTime(passRequestTime);
    }

	private void UpdateStage(int _index) {
	}
}

public enum GameStatus { NON, PROCESS, PASS, FAIL }
