#define DEBUG
#if !UNITY_EDITOR
#undef DEBUG
#endif
/**
 *Brief: Mouse event control.
 *Author: GooKu, 
 *Revsion:
	*20151107:Frist version.
*/


using UnityEngine;
using System;

public class MouseManager : Singleton<MouseManager>
{
	//gooku: implement by each view control/-s
	public Action<float> ScrollWheelEvent = delegate (float _disOffset) { };
	public Action<Vector3> MoveDelta = delegate (Vector3 _mouseDelta) { };
	public Action<Vector3> MoveEvent = delegate (Vector3 _mousePos) { };
	//gooku:long press just decided with mouse index:0
	public Action<RaycastHit2D> MouseDownEven_0 = delegate(RaycastHit2D _hit) { };
	public Action<PointData> MouseDragEven_0 = delegate (PointData _pointData) { };
	public Action MouseUpEven_0 = delegate { };

	public Action LongPressStart = delegate { };
	public Action OnLongPress = delegate { };
	public Action LongPressEnd = delegate { };
	//gooku: implement by each view control/-e

	private static CameraData m_mainCamera = new CameraData();
	public Camera Camera {	get { return m_mainCamera.camera; }	}
	private PointData m_mousePoint_0 = new PointData();
//gooku, not release yet	private PointData m_mousePoint_1 = new PointData();

	private int m_moveOffset = 1;
	private int m_zoomoOffset = 5;

	private int m_longPressOffset = 10;
	private float m_longPressTimeLimit = 0.2f;
	private float m_longPressTime = 0;
	private bool m_isFristLongPress = false;
	private bool m_isPressState = false;

	private float m_r = (float)Screen.width / Screen.height;
//	private Ray m_ray;
	private RaycastHit2D m_hit;

#if DEBUG
	void Update()
	{
		if (m_mainCamera == null || m_mainCamera.camera == null)
			return;

		if (Input.GetMouseButtonDown(0)) {
			m_hit = Physics2D.Raycast(m_mainCamera.camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			MouseDownEven_0(m_hit);
            m_mousePoint_0.SetPos(Input.mousePosition);
			m_longPressTime = 0;
			m_isFristLongPress = true;
			m_isPressState = true;
        }
		
		if (Input.GetMouseButton(0)) {

			m_mousePoint_0.SetPos(Input.mousePosition);

			if (m_mousePoint_0.offset > m_moveOffset)
			{
				MoveDelta(m_mousePoint_0.deltaPos);
				MouseDragEven_0(m_mousePoint_0);
				MoveEvent(Input.mousePosition);
				m_isPressState = false;
            }

			if(m_isPressState){
				CheckLongPress();
            }
		}

		if (Input.GetMouseButtonUp(0)) {
			MouseUpEven_0();
			if (m_isPressState)
				LongPressEnd();
        }

		if (Input.GetAxis("Mouse ScrollWheel") != 0) {
			ScrollWheelEvent(Input.GetAxis("Mouse ScrollWheel"));
        }
	}
#endif

	private void CheckLongPress()
	{

		m_longPressTime += Time.deltaTime;

		if (m_longPressTime < m_longPressTimeLimit)
			return;

		if (m_isFristLongPress)
		{
			m_isFristLongPress = false;
			LongPressStart();
		}
		else
		{
			OnLongPress();
		}
	}

	public void InitCamera(Camera _camera)
	{
		//		Debug.Log("InitCamera");
		m_mainCamera.Init(_camera);
	}

	public Camera GetCamera()
	{
		return m_mainCamera.camera;
	}

	public float GetScreenRate()
	{
		return m_r;
	}

	public void SetLongPressTimeLimit(float _time)
	{
		m_longPressTimeLimit = _time;
	}
}
