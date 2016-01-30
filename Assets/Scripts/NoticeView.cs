using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NoticeView : MonoBehaviour {
	//gooku, setting on the game object/-s
	public GameObject PassNoice;
	public GameObject FailNoice;
	//gooku, setting on the game object/-e

	void Start () {
		PassNoice.SetActive(false);
		FailNoice.SetActive(false);
    }

	public void IssueNotice(bool _isPass) {
		if (_isPass) {
			StartCoroutine(PopEffect(PassNoice));
        }
		else {
			StartCoroutine(PopEffect(FailNoice));
        }
    }

	private IEnumerator PopEffect(GameObject _obj) {
		_obj.SetActive(true);
		_obj.transform.localScale = Vector2.zero;

		Image img = _obj.GetComponent<Image>();
//		Text text = _obj.transform.Find("Text").GetComponent<Text>();

		float ft = 0.0f;
		float fV = 0.0f;
		float _time = 0.25f;

		Color orgImgColor = img.color;
//		Color orgTextColor = text.color;

		do
		{
			ft = Mathf.SmoothDamp(ft, 1.0f, ref fV, _time); // 0 ~ 1
			_obj.transform.localScale = Vector2.Lerp(Vector2.zero, Vector2.one, ft);

			Color color = Color.Lerp(Color.clear, orgImgColor, ft);

			img.color = color;
//			text.color = color;
            yield return null;
		} while (ft < 0.95f);

		img.color = orgImgColor;
//		text.color = orgTextColor;
		yield return new WaitForSeconds(0.3f);
        _obj.SetActive(false);
	}


}
