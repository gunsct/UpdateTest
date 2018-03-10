using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class AssetLoader : MonoBehaviour {
    string assetBundleDirectory;
    
    private void Start() {
#if UNITY_ANDROID
        assetBundleDirectory = Application.persistentDataPath + "/Assets";
#endif

#if UNITY_EDITOR
        assetBundleDirectory = "Assets/StreamingAssets";
#endif
        //StartCoroutine(LoadAssetFromLocalDisk());
        StartCoroutine(SaveAssetBundleOnDisk());
    }


    // 서버에서 받아오고자 하는 에셋 번들의 이름 목록


    // 지금은 간단한 배열 형태를 사용하고 있지만 이후에는


    // xml이나 json을 사용하여 현재 가지고 있는 에셋 번들의 버전을 함께 넣어주고


    // 서버의 에셋 번들 버전 정보를 비교해서 받아오는 것이 좋다.
    public string[] assetBundleNames;

    IEnumerator SaveAssetBundleOnDisk() {


        // 에셋 번들을 받아오고자하는 서버의 주소


        // 지금은 주소와 에셋 번들 이름을 함께 묶어 두었지만


        // 주소 + 에셋 번들 이름 형태를 띄는 것이 좋다.
        string uri = "https://firebasestorage.googleapis.com/v0/b/updatetest-fffd3.appspot.com/o/asset_0?alt=media&token=345eb63d-61f1-4680-8604-3e778c7931bf";





        // 웹 서버에 요청을 생성한다.
        UnityWebRequest request = UnityWebRequest.Get(uri);
        yield return request.Send();


        yield return request.isDone;





        // 에셋 번들을 저장할 경로


        // 에셋 번들을 저장할 경로의 폴더가 존재하지 않는다면 생성시킨다.
        if (!Directory.Exists(assetBundleDirectory)) {
            Directory.CreateDirectory(assetBundleDirectory);
        }




        // 파일 입출력을 통해 받아온 에셋을 저장하는 과정
        FileStream fs = new FileStream(assetBundleDirectory + "/asset_0", System.IO.FileMode.Create);
        fs.Write(request.downloadHandler.data, 0, (int)request.downloadedBytes);
        fs.Close();
        
        StartCoroutine(LoadAssetFromLocalDisk());
    }

    IEnumerator LoadAssetFromLocalDisk() {
        while (!Caching.ready) {
            yield return null;
        }
        
        // 저장한 에셋 번들로부터 에셋 불러오기

        var myLoadedAssetBundle = AssetBundle.LoadFromFile(assetBundleDirectory + "/asset_0");
        if (myLoadedAssetBundle == null) {
            Debug.Log("Failed to load AssetBundle!");
            yield break;
        }
        else {
            Debug.Log("Successed to load AssetBundle!");
            var prefab = myLoadedAssetBundle.LoadAsset<GameObject>("Horse");
            GameObject obj = Instantiate(prefab, Vector3.zero, Quaternion.identity);

            Renderer[] renderers = obj.transform.GetComponentsInChildren<Renderer>(true);

            foreach (Renderer item in renderers) {
                if (item.materials != null) {
                    foreach (Material mat in item.materials) {
                        Shader sha = mat.shader;
                        sha = Shader.Find(sha.name);
                        // Debuger.Log(item.gameObject.name + " : " + mat.name, item.gameObject);
                    }
                }
            }
        }
    }
}