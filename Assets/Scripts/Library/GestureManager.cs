/**
 *Brief: gesture manager, 
 *Detail: muliti-touch control, 
 *Author: GooKu
 *Revsion:
	*20151108:Increase hit parameter on FingerDownEvent.
	*20151107:Increase main camera get accessor.
	*20151106:Increase listener:TouchNumberChangeEven
	*20151105:Increase zoom listener with muliti-touch.
	*20151104:Increase muliti-touch listener with each touch state, including long press.
	*20151006:Frist version.
*/

using UnityEngine;
using System;
using System.Collections.Generic;

public class GestureManager : Singleton<GestureManager>
{
//gooku: implement by each view control/-s
	public Action<Vector3> fingerMove = delegate (Vector3 _deltaPos) { };
	public Action<Vector2> fingerMove_screenCoordinate = delegate (Vector2 _pos) { };

	public Action<float> fingerZoom_2Points = delegate (float _disOffset) { };
	public Action<float> fingerRateZoom_2Points = delegate (float _rate) {
//gooku:not update at there		m_mainCamera.camera.orthographicSize = m_mainCamera.currentOrthographicSize / _rate;
	};

	public Action<Touch, RaycastHit2D> FingerDownEvent = delegate (Touch _touch, RaycastHit2D _hit) { };
	public Action<Touch> FingerUpEvent = delegate (Touch _touch) { };
	public Action<float> FingerZoomEvent = delegate (float _disOffset) { };

	public Action<Touch> FingerMovetEvent = delegate (Touch _touch) { };
	public Action<Touch,PressData> FingerStationaryEvent = delegate (Touch _touch, PressData _pressData) { };

	public Action<Touch> LongPressStartEvent = delegate (Touch _touch) { };
	public Action<Touch> OnLongPressEvent = delegate (Touch _touch) { };
	public Action<Touch> LongPressEndEvent = delegate (Touch _touch) { };

	public Action<Vector2> TouchNumberChangeEven = delegate (Vector2 _pos) { };
	//gooku: implement by each view control/-e

	private PointData m_centerPoint = new PointData();
	private Dictionary<int, PressData> m_fingerList = new Dictionary<int, PressData>(); 
	private GestureData m_gestureData_2Point = new GestureData();
	private List<GestureData> m_gestureDataList = new List<GestureData>();

	private static CameraData m_mainCamera = new CameraData();
	public Camera Camera {	get { return m_mainCamera.camera; }	}

	private int m_lastTouchCount = 0;
	private int m_moveOffset = 1;
	private int m_zoomoOffset = 5;

    private float m_longPressTimeLimit = 0.2f;

	private float m_r = (float)Screen.width / Screen.height;
	public float ScreenRate { get { return m_r; } }

	private bool m_isTouchNumberChange;
	private int m_fingerId;
	private PressData m_fingerData;
	private Touch m_touch;
	private Vector2 m_totalDelta;
	private GestureData m_gestureData;

	private float m_totalDisDelta;
	private float m_tmpDisDelta;
	private int fingerSpacingCount;

	private RaycastHit2D m_hit;

	void Start()
	{
	}

	void Update()
	{
		if (m_mainCamera == null || m_mainCamera.camera == null)
			return;

		m_isTouchNumberChange = m_lastTouchCount != Input.touchCount;

		if (Input.touchCount < 1)
		{
			if (m_isTouchNumberChange) {
				m_lastTouchCount = Input.touchCount;
			}
			return;
		}

		m_totalDelta = Vector2.zero;
		m_totalDisDelta = 0;

        for (int i = 0; i < Input.touches.Length; i++)
		{
			m_touch = Input.touches[i];
            m_totalDelta += m_touch.position;
			m_fingerId = m_touch.fingerId;

			switch (Input.touches[i].phase)
			{
				case TouchPhase.Began:
					m_hit = Physics2D.Raycast(m_mainCamera.camera.ScreenToWorldPoint(m_touch.position), Vector2.zero);
					FingerDownEvent(m_touch, m_hit);
                    m_fingerList.Add(m_fingerId, new PressData());
					break;
				case TouchPhase.Ended:
					FingerUpEvent(m_touch);
					m_fingerList.Remove(m_fingerId);
					m_gestureDataList.RemoveAll(x => x.fingerId_1 == m_fingerId || x.fingerId_2 == m_fingerId);
					continue;
				case TouchPhase.Moved:
					m_fingerData = m_fingerList[m_fingerId];
					m_fingerData.stationaryTime = 0;
					FingerMovetEvent(m_touch);
                    if (!m_fingerData.isNeverMoved)
						break;

					m_fingerData.isNeverMoved = false;
					LongPressEndEvent(m_touch);
					break;
				case TouchPhase.Stationary:
					m_fingerData = m_fingerList[m_fingerId];
					m_fingerData.stationaryTime += m_touch.deltaTime;

					FingerStationaryEvent(m_touch, m_fingerData);

					if (!m_fingerData.isNeverMoved) {
						break;
					}

					m_fingerData.longPressTime += m_touch.deltaTime;

					if (m_fingerData.longPressTime < m_longPressTimeLimit)
						break;

					if (m_fingerData.isLongPress)
					{
						OnLongPressEvent(m_touch);
					}
					else {
						LongPressStartEvent(m_touch);
						m_fingerData.isLongPress = true;
                    }
					break;
			}

			fingerSpacingCount = 0;
			m_tmpDisDelta = 0;
            for (int j = i+1; j < Input.touches.Length; j++) {
				if (Input.touches[j].phase == TouchPhase.Ended)
					continue;

				m_gestureData = m_gestureDataList.Find(x=>x.fingerId_1 == m_touch.fingerId && x.fingerId_2 == Input.touches[j].fingerId);

				if (m_gestureData == null)
				{
					m_gestureData = new GestureData(m_touch, Input.touches[j]);
					m_gestureDataList.Add(m_gestureData);
                }
				else {
					m_gestureData.Set(m_touch, Input.touches[j]);
					m_tmpDisDelta += m_gestureData.deltaDis;
                    fingerSpacingCount++;
                }
            }

			if(fingerSpacingCount > 0)
				m_totalDisDelta += m_tmpDisDelta / fingerSpacingCount;
        }

		m_centerPoint.SetPos(m_totalDelta / Input.touchCount);

		if (!m_isTouchNumberChange)
		{
			if (Math.Abs(m_totalDisDelta) > m_zoomoOffset)
				FingerZoomEvent(m_totalDisDelta);

			if (m_centerPoint.offset > m_moveOffset)
			{
				fingerMove(m_centerPoint.deltaPos * m_mainCamera.orthRate);
				fingerMove_screenCoordinate(m_centerPoint.currentPos);
			}

			//gooku, finger zoom, temp of 2 fingers/-s
			if (Input.touchCount > 1)
			{
				m_gestureData_2Point.Set(Input.touches[0], Input.touches[1]);
				//				print(m_gestureData_2Point.disRateOffset);
				if (Math.Abs(m_gestureData_2Point.deltaDis) > m_zoomoOffset)
				{
					fingerZoom_2Points(m_gestureData_2Point.deltaDis);
					//					print(m_gestureData_2Point.disOffsetRate);
					fingerRateZoom_2Points(m_gestureData_2Point.disOffsetRate);
				}
			}
			//gooku, finger zoom, temp of 2 fingers/-e
		}
		else
		{
			TouchNumberChangeEven(m_centerPoint.currentPos);

			//gooku, temp of 2 fingers/-s
			m_gestureData_2Point.Init();
            if (Input.touchCount > 1)
			{
				m_gestureData_2Point.SetBaseDistance(Input.touches[0], Input.touches[1]);
			}
			//gooku, temp of 2 fingers/-e
		}
		m_lastTouchCount = Input.touchCount;
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

	public void SetLongPressTimeLimit(float _time) {
		m_longPressTimeLimit = _time;
    }
}

public class PointData
{
	public int fingerId;
	public Vector2 currentPos;
	public Vector2 lastPos;
	public Vector3 deltaPos { get { return currentPos - lastPos; } }
	public float offset { get { return Vector2.Distance(currentPos, lastPos); } }
	public void Init()
	{
		currentPos = Vector2.zero;
		lastPos = Vector2.zero;
	}
	public void SetPos(Vector2 _pos)
	{
		lastPos = currentPos;
		currentPos = _pos;
	}
	public void SetPos(Touch _touch)
	{
		lastPos = currentPos;
		currentPos = _touch.position;
	}
}

public class PressData {
	public bool isNeverMoved = true;
	public bool isLongPress = false;
	public float longPressTime=0;
	public float stationaryTime=0;
}

public class GestureData
{
	public int fingerId_1;
	public int fingerId_2;
	public float baseDistance;
	public float currentDistance=0;
	public float lastDistance=0;
	public float deltaDis { get {return currentDistance - lastDistance;}}

	public float disOffsetRate { get { return currentDistance / baseDistance; } }

	public GestureData() { }

	public GestureData(Touch touch_1, Touch touch_2)
	{
		Set(touch_1, touch_2);
	}

	public void Set(Touch touch_1, Touch touch_2)
	{
		lastDistance = currentDistance;
		fingerId_1 = touch_1.fingerId;
		fingerId_2 = touch_2.fingerId;
		currentDistance = Vector2.Distance(touch_1.position, touch_2.position);
	}

	public void SetBaseDistance(Touch touch_1, Touch touch_2)
	{
		baseDistance = Vector2.Distance(touch_1.position, touch_2.position);
	}

	public bool IsExist(Touch touch)
	{
		return touch.fingerId == fingerId_1 || touch.fingerId == fingerId_2;
	}

	public void Init()
	{
		currentDistance = 0;
		lastDistance = 0;
	}
}

public class CameraData
{
	public Camera camera;
	public float orgOrthographicSize;
	public float orthographicSize { get { return camera.orthographicSize; } }

	public float orthRate { get { return camera.orthographicSize / orgOrthographicSize; } }

	public void Init(Camera _camera)
	{
		camera = _camera;
		orgOrthographicSize = camera.orthographicSize;
	}
}
