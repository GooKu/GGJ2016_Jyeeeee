using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeBarView : MonoBehaviour {
	//gooku, setting at the object/-s
	public Image timeBar_img;
	public Text passRequestCount_txt;
	public Text remainTime_txt;
	//gooku, setting at the object/-e

	private void Start() {
		
	}

	public void Init(int _times){
		passRequestCount_txt.text = _times.ToString();
		timeBar_img.fillAmount = 1;
		timeBar_img.color = Color.green;
    }

	public void UpdateBar(float _present, float _remainTime) {
		timeBar_img.fillAmount = _present;
		timeBar_img.color = Color.Lerp(Color.red, Color.green, _present-0.2f);

		remainTime_txt.text = string.Format("{0:0.##}", _remainTime);
    }

	public void UpdateTime(int _times) {
		passRequestCount_txt.text = "Request: "+ _times.ToString() + " rectify";
    }

}
