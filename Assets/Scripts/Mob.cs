using UnityEngine;
using System.Collections;

public class Mob : MonoBehaviour {

	public enum Behavior
	{
		none,
		noise
	};

	private Behavior m_behavior;

	// Use this for initialization
	void Start () {
		m_behavior = Behavior.none;
	}

	void init () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnMouseHit () {
		
		switch (GetBehavior()) {
		case Mob.Behavior.none:
			break;
		case Mob.Behavior.noise:
			//TODO:	minus player's life
			break;
		}

	}

	public Behavior GetBehavior () {
		return m_behavior;
	}
	public void SetBehavior (Behavior b) {
		m_behavior = b;
	}

}
