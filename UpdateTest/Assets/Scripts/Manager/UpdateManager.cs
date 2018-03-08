using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class UpdateManager : DeleteSingleton<UpdateManager> {//앱 버전 체크(디비) 파일 다운(저장소)
    DatabaseReference reference;

    public override void Init() {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://updatetest-fffd3.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    void VersionCheck() { //간단한 앱 버전 체크
        reference.Child("Version").GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted) {
                // Handle the error...
            }
            else if (task.IsCompleted) {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Value.Equals(Application.version))
                    Debug.Log(snapshot.Value + " " + Application.version);
                else {//버전 다를때 업뎃 유아이 뜨게 할것

                }
            }
        });


    }

    void Patch() {

    }
}
