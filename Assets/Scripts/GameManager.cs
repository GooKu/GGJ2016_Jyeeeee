#define DEBUG
#if !UNITY_EDITOR
#undef DEBUG
#endif

using UnityEngine;
using System.Collections;

public partial class GameManager : Singleton<GameManager> {
	//gooku, setting at the object/-s
	public NoticeView noticeView;
	public ResultView resultView;
	public GameObject StartMenu;
	public GameObject[] StageArray;
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
		remainTime = passTime;
		passRequestTime = 3;
		gameTimeBar.UpdateBar(remainTimeRate);
		gameTimeBar.UpdateTime(passRequestTime);

		for (int i=0; i < StageArray.Length;i++)
		{
			StageArray[i].SetActive(_index == i);
		}
    }

}

public enum GameStatus { NON, PROCESS, PASS, FAIL }
