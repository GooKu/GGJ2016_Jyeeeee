﻿using UnityEngine;
using System.Collections;

public partial class GameManager : Singleton<GameManager> {
	public TimeBarView gameTimeBar;
	float passTime = 10;
	float remainTime =10;
	float gameTimechange;
	int timer_i;

	private void GameTimeHandle (){
		remainTime -= Time.deltaTime;
		gameTimeBar.UpdateBar (remainTime/passTime, remainTime);
		if (remainTime <= 0) {
			gameTimeBar.UpdateBar(0, 0);
			GameOver();
		}
			
	}

}
