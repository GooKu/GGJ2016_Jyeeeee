using UnityEngine;
using System.Collections;

public partial class GameManager : Singleton<GameManager> {
	private float passRequestTime = 3;

	protected override void Awake()
	{
		base.Awake();
	}

	private void Start() { }

	private void Update() {
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

		}
	}

	private void Pass() {

	}

	private void GameOver() {

	}

	private void InitStage(int _index) {

	}

	private void UpdateStage(int _index) {
	}
}
