using UnityEngine;
using System.Collections;

public class Mob : MonoBehaviour {

	public enum MoveDirection
	{
		right,
		left,
		top,
		bottom
	};

	private float m_original_pos_x = 0.0f; 
	private float m_original_pos_y = 0.0f; 
	private MoveDirection m_move_direction;
	private float m_move_speed_x = 0.0f; 
	private float m_move_speed_y = 0.0f; 
	private float m_movable_distance_x = 0.0f; 
	private float m_movable_distance_y = 0.0f; 

	public Behavior m_behavior;

	// Use this for initialization
	void Start () {
		init();

		m_original_pos_x = transform.position.x;
		m_original_pos_y = transform.position.y;
	}

	void init () {
		m_move_speed_x = 0.05f;
		m_move_speed_y = 0.0f;
		m_movable_distance_x = 4.0f;
		m_movable_distance_y = 0.0f;
		m_behavior = Behavior.noise;
		m_move_direction = MoveDirection.left;
	}
	
	// Update is called once per frame
	void Update () {
//		Move ();
	}

	void Move() {

		switch (m_move_direction) {

		case MoveDirection.right:
			
			transform.position = new Vector3 (transform.position.x + m_move_speed_x, transform.position.y);

			if (transform.position.x > (m_original_pos_x + m_movable_distance_x)) {
				m_move_direction = MoveDirection.left;
			}

			break;
		case MoveDirection.left:
			
			transform.position = new Vector3(transform.position.x - m_move_speed_x, transform.position.y);

			if (transform.position.x < (m_original_pos_x - m_movable_distance_x)) {
				m_move_direction = MoveDirection.right;
			}

			break;
		}


		ShakeBody ();
	}
	void ShakeBody() {
		
	}

	public void OnMouseHit () {
		
		switch (GetBehavior()) {
		case Behavior.none:
			break;
		case Behavior.noise:
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
		SetBehavior(Behavior.none);
	}

}

public enum Behavior
{
	none,
	noise
}
