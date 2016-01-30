using UnityEngine;
using System.Collections;

public class MouseController : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		MouseManager.Instance.MouseDownEven_0 += onMouseDown;
		MouseManager.Instance.MouseUpEven_0 += onMouseUp;
		MouseManager.Instance.MouseDragEven_0 += onMouseDrag;
		MouseManager.Instance.InitCamera(Camera.main);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void onMouseDown(RaycastHit2D _hit) {

		if (_hit.collider) {

			switch (_hit.collider.tag) {
			case "behavior_ok":
				break;
			case "behavior_noise":
				//TODO:	minus player's life
				break;
			}
		}

	}

	void onMouseUp() {
	}

	void onMouseDrag(PointData _pointData) {
	}
}
