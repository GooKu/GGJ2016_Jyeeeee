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
		print("onMouseDown");
		if (_hit.collider) {

			if (_hit.collider.gameObject.GetComponent<Mob> ()) {
				handleMobReaction (_hit.collider.gameObject.GetComponent<Mob> ());
			}

		}

	}

	void onMouseUp() {
	}

	void onMouseDrag(PointData _pointData) {
	}

	void handleMobReaction(Mob mob) {
		mob.OnMouseHit ();
	}

}
