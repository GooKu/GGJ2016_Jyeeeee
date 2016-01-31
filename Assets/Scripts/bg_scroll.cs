using UnityEngine;
using System.Collections;
using System.Collections.Generic; // for List.

public class bg_scroll : MonoBehaviour
{

	#region _data_field_

	[HideInInspector]
	public delegate void callbackEventWithGameObject (GameObject go);
	[HideInInspector]
	public callbackEventWithGameObject OnScrollEnd = null;

	private List<Mob> MobList = new List<Mob>();

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

	public GameObject m_goBackgroundSprite;
	public Component m_comBackgroundSprite;
	//public Sprite m_spBackgroundSprite;

	public List<GameObject> m_goList_MobInst = new List<GameObject>();
	public List<GameObject> m_goList_HandleInst = new List<GameObject>();
	public List<GameObject> m_goList_MapInst = new List<GameObject>();
	public System.Random rnd;

	#endregion

	public void Start ()
	{
		Init();

		m_pos_x = transform.position.x;
		m_pos_y = transform.position.y;

		m_goBackgroundSprite = gameObject;
		m_comBackgroundSprite = m_goBackgroundSprite.GetComponent<SpriteRenderer>();
		//m_spBackgroundSprite = m_goBackgroundSprite.GetComponent<SpriteRenderer>().sprite;

		rnd = new System.Random(System.DateTime.Now.Millisecond);
	}

	public void Init ()
	{
		if (m_goList_MobInst == null) {
			m_goList_MobInst = new List<GameObject>();
		} else {
			ClearAllMobInst();
			ClearAllHandleInst();
			ClearAllMapInst();
		}

		m_scroll_x = true;
		m_scroll_speed_x = -0.2f;
		m_start_x = scroll_unit;
		m_end_x = -(scroll_unit * 2) + 8; // 16

		foreach (Transform child in transform)
		{
			if (child.GetComponent<Mob>()) {
				MobList.Add(child.GetComponent<Mob>());
            }
		}

		InitMobArray();
		InitEvents();

	}

	public void InitEvents ()
	{
		OnScrollEnd = (GameObject go) => {
			Debug.Log ("Default callback : OnScrollEnd()");
#if SM_DEBUG_SCROLL_BG
			ChangeBackground(rnd.Next(1, 4));//debug
#endif
			InitMobArray();
        };
	}

	#region _prefab_inst_

	public void CreateMobInst (Vector3 mob_pos)
	{
		GameObject inst = Instantiate(Resources.Load("Prefab/Mobs/Mob", typeof(GameObject))) as GameObject;
		
		if (inst != null) {
			inst.transform.parent = this.transform;
			inst.transform.localPosition = mob_pos;
			m_goList_MobInst.Add(inst);
			Debug.Log ("CreateMobInst> Add A New Mob Inst!");
		} else {
			Debug.LogWarning ("CreateMobInst> Cannot Create Mob!");
		}
	}

	public void ClearAllMobInst ()
	{
		if (m_goList_MobInst == null) {
			return;
		}

		foreach (var item in m_goList_MobInst) {
			Destroy(item);
		}
		m_goList_MobInst.Clear();
	}
	
	public void CreateHandleInst (Vector3 handle_pos)
	{
		GameObject inst = Instantiate(Resources.Load("Prefab/Handle1", typeof(GameObject))) as GameObject;
		
		if (inst != null) {
			inst.transform.parent = this.transform;
			inst.transform.localPosition = handle_pos;
			m_goList_HandleInst.Add(inst);
			Debug.Log ("CreateHandleInst> Add A New Handle Inst!");
		} else {
			Debug.LogWarning ("CreateHandleInst> Cannot Create Handle!");
		}
	}
	
	public void ClearAllHandleInst ()
	{
		if (m_goList_HandleInst == null) {
			return;
		}
		
		foreach (var item in m_goList_HandleInst) {
			Destroy(item);
		}
		m_goList_HandleInst.Clear();
	}

	public void CreateMapInst (Vector3 map_pos)
	{
		GameObject inst = Instantiate(Resources.Load("Prefab/Map1", typeof(GameObject))) as GameObject;
		
		if (inst != null) {
			inst.transform.parent = this.transform;
			inst.transform.localPosition = map_pos;
			m_goList_MapInst.Add(inst);
			Debug.Log ("CreateMapInst> Add A New Map Inst!");
		} else {
			Debug.LogWarning ("CreateMapInst> Cannot Create Map!");
		}
	}
	
	public void ClearAllMapInst ()
	{
		if (m_goList_MapInst == null) {
			return;
		}
		
		foreach (var item in m_goList_MapInst) {
			Destroy(item);
		}
		m_goList_MapInst.Clear();
	}

	#endregion

	public void ChangeBackground (int bg_index)
	{
		SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
		if (sr == null) {
			Debug.LogWarning ("Cannot Find SpriteRenderer!");
		}

		Debug.Log ("Change Background to [" + bg_index.ToString() + "]");
		switch (bg_index)
		{
		case 1:
			sr.sprite = Resources.Load<Sprite>("bg-01");
			CreateHandleInst(new Vector3(2.0f, 0.3333f, 0));
			CreateHandleInst(new Vector3(-2.0f, 0.3333f, 0));
			break;
		case 2:
			sr.sprite = Resources.Load<Sprite>("bg-02");
			break;
		case 3:
			sr.sprite = Resources.Load<Sprite>("bg-03");
			CreateMapInst(new Vector3(1.3f, 0, 0));
			CreateHandleInst(new Vector3(1.5f, 0.3333f, 0));
			CreateHandleInst(new Vector3(-1.5f, 0.3333f, 0));
			break;
		default:
			sr.sprite = Resources.Load<Sprite>("test");
			break;
		}
	}

	public void FixedUpdate ()
	{
		m_scroll_x = GameManager.m_bg_scrolling;
		m_scroll_speed_x = GameManager.m_bg_scroll_speed;
		m_pos_x = transform.position.x;
		m_pos_y = transform.position.y;

		if (m_scroll_x) {

			m_pos_x += m_scroll_speed_x;
			// end of scroll.
			if (m_pos_x < m_end_x) {
				//Debug.Log ("m_pos_x = " + m_pos_x);
				m_pos_x += (scroll_unit * 3);

				ClearAllMobInst();
				ClearAllHandleInst();
				ClearAllMapInst();

				if (OnScrollEnd != null) {
					OnScrollEnd( gameObject );
				}
			}

			transform.position = new Vector3(m_pos_x, m_pos_y);

		}

		if (m_scroll_y) {
			transform.position = new Vector3(transform.position.x, transform.position.y + m_scroll_speed_y);
		}

	}

	public GameObject AddChildGameObject(string name)
	{
		var go = new GameObject ();
		go.name = name;
		go.transform.parent = this.transform;
		return go;
	}

	private void InitMobArray()
	{
		foreach (Mob mob in MobList)
		{
			mob.init();
			mob.gameObject.SetActive(Random.Range(0f, 1f) > 0.2f);
//			mob.gameObject.SetActive(true);//gooku, for debug
		}
	}

	
#if UNITY_EDITOR

	public int m_DebugUI_StartX = 0;
	public int m_DebugUI_StartY = 0;

#if SM_DEBUG_SCROLL_BG
		
	public float m_Debug_Scroll_Speed = -0.05f;

#endif

	private GameObject goMob;
	private Component comMob;

	public void OnGUI ()
	{

		int xx = m_DebugUI_StartX;
		int yy = m_DebugUI_StartY;
		int ww = 100;
		int hh = 20;

#if SM_DEBUG_ADD_MOB

		if (GUI.Button (new Rect (xx, yy, ww, hh), "Add Mob")) {
			GameObject mob_inst = Instantiate(Resources.Load("Prefab/Mobs/Mob", typeof(GameObject))) as GameObject;
			if (mob_inst != null) {
				mob_inst.transform.parent = this.transform;
				mob_inst.transform.localPosition = new Vector3((int)((rnd.NextDouble() * 16.0f) - 8.0f), 0, 0);
				m_goList_MobInst.Add(mob_inst);
				Debug.Log ("Add A New Mob Inst!");
			} else {
				Debug.LogWarning ("Cannot Create Mob!");
			}
		}
		yy += hh;

#endif

#if SM_DEBUG_SCROLL_BG

		if (GUI.Button (new Rect (xx, yy, ww, hh), "Speed Down")) {
			GameManager.m_bg_scroll_speed -= m_Debug_Scroll_Speed;
			if (GameManager.m_bg_scroll_speed > 0.0f) {
				GameManager.m_bg_scroll_speed = 0.0f;
			}
		}
		yy += hh;
		if (GUI.Button (new Rect (xx, yy, ww, hh), "Speed Up")) {
			GameManager.m_bg_scroll_speed += m_Debug_Scroll_Speed;
			if (GameManager.m_bg_scroll_speed <= -scroll_unit) {
				GameManager.m_bg_scroll_speed = -scroll_unit;
			}
		}
		yy += hh;

#endif

	}

#endif

}
