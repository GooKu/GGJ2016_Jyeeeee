using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ResultView : MonoBehaviour {
	//gooku, setting at the object/-s
	public GameObject PassObj;
	public GameObject FailObj;
	public GameObject Restart_btn;
	public GameObject Next_btn;
    public Image Fade_img;
	//gooku, setting at the object/-e

	void Start () {
		Init();
    }

	public void Init() {
		PassObj.SetActive(false);
		FailObj.SetActive(false);
		Restart_btn.SetActive(false);
		Next_btn.SetActive(false);
        Fade_img.gameObject.SetActive(false);
	}

	public void IssueResult(bool _isPass)
	{
		Fade_img.gameObject.SetActive(true);
		PassObj.SetActive(_isPass);
		Next_btn.SetActive(_isPass);
		FailObj.SetActive(!_isPass);
		Restart_btn.SetActive(!_isPass);

	}
}
