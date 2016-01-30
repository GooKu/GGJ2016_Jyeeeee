using UnityEngine;
using System.Collections;

public class GameManager : Singleton<GameManager> {

	private int passTime = 0;
	private float gameTime;

	protected override void Awake()
	{
		base.Awake();

	}

	private void Start() { }

	private void Update() {
		//TODO:reduce time
	}

	public void GetDecide(bool _resullt) {
		//TODO:	pass/fail handle
	}

	private void InitStage(int _index) {

	}

	private void UpdateStage(int _index) {
	}
}
