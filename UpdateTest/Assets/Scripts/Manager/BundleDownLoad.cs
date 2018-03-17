using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class BundleDownLoad : DonDeleteSingleton<BundleDownLoad> {
    int BUNDLE_COUNT = 0;
    [SerializeField]
    string[] bundle_uri;
    [SerializeField]
    string[] bundle_name;

    public override void Init() {
        BUNDLE_COUNT = bundle_name.Length;
    }

    public IEnumerator SaveAssetBundleOnDisk() {
        while (BUNDLE_COUNT > 0) {//번들 개수만큼 다운로드 반복
            //번들의 주소, 주소 + 번들명이 대부분인데 다운로드 url이 따로 있는 파이어베이스는 그대로 사용 
            string uri = bundle_uri[BUNDLE_COUNT - 1];

            // 웹 서버에 요청을 생성한다. 
            UnityWebRequest request = UnityWebRequest.Get(uri);
            yield return request.Send();

            //완료될때까지 기다린다.
            yield return request.isDone;

            //에셋번들 받을 경로를 생성한다. pc는 에셋 폴더 내부에, 안드로이드는 앱 폴더 내에 임의로 생성.
            if (!Directory.Exists(Path.assetBundleDirectory)) {
                Directory.CreateDirectory(Path.assetBundleDirectory);
            }

            // 파일 입출력을 통해 받아온 에셋을 저장하는 과정
            FileStream fs = new FileStream(Path.assetBundleDirectory + bundle_name[BUNDLE_COUNT - 1], System.IO.FileMode.Create);
            fs.Write(request.downloadHandler.data, 0, (int)request.downloadedBytes);
            fs.Close();

            BUNDLE_COUNT--;
        }

        //저장이 끝난 후 메인씬 로드 시작
        StartCoroutine(SceneLoader.Instance.LoadSceneFromAssetBundle("Main",false));
    }
}