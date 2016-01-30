using UnityEngine;
using System.Collections;

public partial class GameManager : Singleton<GameManager> {
	public TimeBarView gameTimeBar;
	float passTime = 0;
	float remainTime;
	float gameTimechange;
	int timer_i;

	private void GameTimeHandle (){


		//TODO:reduce time
		passTime=Time.time;
		timer_i = Mathf.FloorToInt (passTime);
		Debug.Log(timer_i);
		remainTime = 1 - passTime;
		gameTimeBar.UpdateBar (passTime);


	}

}
