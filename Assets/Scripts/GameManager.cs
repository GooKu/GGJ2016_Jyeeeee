using UnityEngine;
using System.Collections;

public partial class GameManager : Singleton<GameManager> {
	TimeBarView gameTimeBar;
	private float passTime = 0;
	private float gameTimechange;
	int timer_i;

	protected override void Awake()
	{
		base.Awake();

	}

	private void Start() { }

	private void Update() {
		GameTimeHandle ();
	}

	public void GetDecide(bool _resullt) {
		//TODO:	pass/fail handle
	}

	private void InitStage(int _index) {

	}

	private void UpdateStage(int _index) {
	}
}
