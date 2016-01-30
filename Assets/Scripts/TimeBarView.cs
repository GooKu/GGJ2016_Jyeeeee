using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeBarView : MonoBehaviour {
	//gooku, setting at the object/-s
	public Image timeBar_img;
	public Text times_txt;
	//gooku, setting at the object/-e

	private void Start() {
		
	}

	public void Init(int _times){
		times_txt.text = _times.ToString();
		timeBar_img.fillAmount = 1;
		timeBar_img.color = Color.green;
    }

	public void UpdateBar(float _present) {
		timeBar_img.fillAmount = _present;
		timeBar_img.color = Color.Lerp(Color.red, Color.green, _present-0.2f);
    }

	public void UpdateTime(int _times) {
		times_txt.text = "Request: "+ _times.ToString() + " rectify";
    }

}
