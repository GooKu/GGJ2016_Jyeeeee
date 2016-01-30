using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeBarView : MonoBehaviour {
	//gooku, setting at the object/-s
	public Image timeBar_img;
	public Text times_txt;
	//gooku, setting at the object/-e

	public void Init(int _times){
		times_txt.text = _times.ToString();
		timeBar_img.fillAmount = 1;
	}

	public void UpdateBar(float _present) {
		timeBar_img.fillAmount = _present;
    }

	public void UpdateTime(int _times) {
		times_txt.text = _times.ToString();
    }

}
