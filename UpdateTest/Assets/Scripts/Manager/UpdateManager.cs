using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class UpdateManager : DeleteSingleton<UpdateManager> {//앱 버전 체크(디비) 파일 다운(저장소)
    DatabaseReference reference;
    bool update_check;
    public bool UPCK {
        get { return update_check; }
    }

    public override void Init() {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://updatetest-fffd3.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        VersionCheck();
    }

    void VersionCheck() { //간단한 앱 버전 체크
        reference.Child("Version").GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted) {
                // Handle the error...
            }
            else if (task.IsCompleted) {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Value.Equals(Application.version)) {
                    update_check = false;
                    Debug.Log(snapshot.Value + " " + Application.version);
                }
                else {//버전 다를때 업뎃 유아이 뜨게 할것
                    update_check = true;
                    Debug.Log(snapshot.Value + " " + Application.version);
                    UIManager.Instance.update_button.SetActive(true);
                }
            }
        });
    }
}
