using UnityEngine;

public abstract class DonDeleteSingleton<T> : MonoBehaviour where T : DonDeleteSingleton<T> {
    private static T m_Instance = null;
    private static bool b_ReMake = true;

    public static T Instance {
        get {
            if (m_Instance == null && b_ReMake) {
                m_Instance = GameObject.FindObjectOfType(typeof(T)) as T;

                if (m_Instance == null) {
                    GameObject obj = new GameObject(typeof(T).ToString());
                    m_Instance = obj.AddComponent(typeof(T)) as T;
                }

                m_Instance.Init();
            }
            return m_Instance;
        }
    }

    private void Awake() {
        if (m_Instance == null) {
            m_Instance = this as T;
            m_Instance.Init();
        }
        //아래는 씬 왔다갔다 해도 중복 안되게 해줌
        else {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public virtual void Init() { } // 초기화를 상속을 통해 구현    

    private void OnApplicationQuit() {
        if (m_Instance != null) {
            m_Instance = null;
            b_ReMake = false;
        }
    }

    public virtual bool IsValid() {
        if (m_Instance != null)
            return true;
        else
            return false;
    }
}