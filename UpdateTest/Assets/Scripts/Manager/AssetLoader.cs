using System.Collections;
using UnityEngine;

public class AssetLoader : DonDeleteSingleton<AssetLoader> {
    public IEnumerator LoadAssetFromLocalDisk() {
        while (!Caching.ready) {
            yield return null;
        }

        // 저장한 에셋 번들로부터 에셋 불러오기

        AssetBundle myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.assetBundleDirectory + "/asset_0");

        if (myLoadedAssetBundle == null) {
            Debug.Log("Failed to load AssetBundle!");

            yield break;
        }
        else {
            Debug.Log("Successed to load AssetBundle!");

            GameObject prefab = myLoadedAssetBundle.LoadAsset<GameObject>("Horse");
            GameObject obj = Instantiate(prefab, Vector3.zero, Quaternion.identity);

            //분홍색으로 나온느 쉐이더 다시 연결해주는 부분.
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