using UnityEngine;
using System.Collections;

public class bg_scroll : MonoBehaviour
{
	private const float scroll_unit = 16.0f;

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
		m_start_x = scroll_unit;
		m_end_x = -(scroll_unit * 2);
	}

	public void Update ()
	{
		m_scroll_x = GameManager.m_bg_scrolling;
		m_scroll_speed_x = GameManager.m_bg_scroll_speed;
		m_pos_x = transform.position.x;
		m_pos_y = transform.position.y;

		if (m_scroll_x) {

			m_pos_x += m_scroll_speed_x;
			if (m_pos_x < m_end_x) {
				m_pos_x = scroll_unit;
			}

			transform.position = new Vector3(m_pos_x, m_pos_y);

		}

		if (m_scroll_y) {
			transform.position = new Vector3(transform.position.x, transform.position.y + m_scroll_speed_y);
		}

	}

#if UNITY_EDITOR && SM_DEBUG_SCROLL_BG

	public int m_DebugUI_StartX = 0;
	public int m_DebugUI_StartY = 0;
	public float m_Debug_Scroll_Speed = -0.05f;

	public void OnGUI ()
	{

		int xx = m_DebugUI_StartX;
		int yy = m_DebugUI_StartY;
		int ww = 100;
		int hh = 20;

		if (GUI.Button (new Rect (xx, yy, ww, hh), "Speed Down")) {
			GameManager.Instance.m_bg_scroll_speed += m_Debug_Scroll_Speed;
			if (GameManager.Instance.m_bg_scroll_speed > 0.0f) {
				GameManager.Instance.m_bg_scroll_speed = 0.0f;
			}
		}
		yy += hh;
		if (GUI.Button (new Rect (xx, yy, ww, hh), "Speed Up")) {
			GameManager.Instance.m_bg_scroll_speed -= m_Debug_Scroll_Speed;
			if (GameManager.Instance.m_bg_scroll_speed <= -scroll_unit) {
				GameManager.Instance.m_bg_scroll_speed = -scroll_unit;
			}
		}
		yy += hh;

	}

#endif

}
