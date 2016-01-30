using UnityEngine;
using System.Collections;

public class Mob : MonoBehaviour {
	
	public Behavior m_defaultBehavior;

	private Behavior m_currentBehavior;

	private Animator m_anim;

	private void Awake()
	{
		m_anim = GetComponent<Animator>();
		init();
	}

	void Start () {
	}

	public void init () {
		SetBehavior(m_defaultBehavior);
		m_anim.SetBool("alive", true);
	}
	
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
		return m_currentBehavior;
	}
	public void SetBehavior (Behavior b) {
		m_currentBehavior = b;
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
