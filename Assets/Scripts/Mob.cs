using UnityEngine;
using System.Collections;

public class Mob : MonoBehaviour {
	
	public Behavior m_behavior;

	Animator m_anim;

	// Use this for initialization
	void Start () {
		init();

		m_anim = GetComponent<Animator> ();
	}

	void init () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void OnMouseHit () {
		
		switch (GetBehavior()) {
		case Behavior.none:
			break;
		case Behavior.collect_behavior:
			GameManager.Instance.GetDecide(false);
			break;
		case Behavior.wrong_behavior_noise:
			//TODO:	minus player's life
			MakeMobAsRegularMob();
			GameManager.Instance.GetDecide(true);
			break;
		}

	}

	public Behavior GetBehavior () {
		return m_behavior;
	}
	public void SetBehavior (Behavior b) {
		m_behavior = b;
	}

	private void MakeMobAsRegularMob() {
		m_anim.SetBool ("alive", false);
		SetBehavior(Behavior.none);
	}

}

public enum Behavior
{
	none,
	collect_behavior,
	wrong_behavior_noise
}
