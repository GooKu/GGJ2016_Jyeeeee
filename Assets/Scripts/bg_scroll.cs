using UnityEngine;
using System.Collections;

public class bg_scroll : MonoBehaviour
{

	public static float m_start_x = 0.0f;
	public static float m_start_y = 0.0f;
	public static float m_end_x = 0.0f;
	public static float m_end_y = 0.0f;

	public static bool m_scroll_x = false;
	public static bool m_scroll_y = false;
	public static float m_scroll_speed_x = 0.0f;
	public static float m_scroll_speed_y = 0.0f;
	
	//public Vector3 m_pos_now = Vector3.zero;
	public float m_pos_x = 0.0f; 
	public float m_pos_y = 0.0f; 

	public void Start ()
	{
		Init();

		m_pos_x = transform.position.x;
		m_pos_y = transform.position.y;
	}

	public void Init ()
	{
		m_scroll_x = true;
		m_scroll_speed_x = -0.2f;
		m_start_x = 20.0f;
		m_end_x = -20.0f;
	}

	public void Update ()
	{

		if (m_scroll_x) {
			transform.position = new Vector3(transform.position.x + m_scroll_speed_x, transform.position.y);
			
			if (transform.position.x <= m_end_x) {
				transform.position = new Vector3(20, transform.position.y);
			}
		}

		if (m_scroll_y) {
			transform.position = new Vector3(transform.position.x, transform.position.y + m_scroll_speed_y);
		}

	
	}

#if UNITY_EDITOR
	public void OnGUI ()
	{


	}
#endif
}
