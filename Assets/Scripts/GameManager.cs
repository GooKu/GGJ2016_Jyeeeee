using UnityEngine;
using System.Collections;

public partial class GameManager : Singleton<GameManager> {
	private int passRequestTime = 3;
	private float punishTime = 1;

	private float remainTimeRate { get { return remainTime / passTime; } }

	private GameStatus gamestatus = GameStatus.NON;


	protected override void Awake()
	{
		base.Awake();
	}

	private void Start() {
		InitStage(0);
		gamestatus = GameStatus.PROCESS;//gooku: tmp for test. will update to button control
    }

	private void Update() {

		if(gamestatus == GameStatus.PROCESS)
			GameTimeHandle();
	}

	public void GetDecide(bool _isPass) {
		//TODO:	pass/fail handle
		if (_isPass)
		{
			passRequestTime--;

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

	}

	private void GameOver() {

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
