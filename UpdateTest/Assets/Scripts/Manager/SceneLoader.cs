using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : DonDeleteSingleton<SceneLoader> {
    string scene_name;

    public override void Init() {
        Caching.CleanCache();
        scene_name = SceneManager.GetActiveScene().name;
    }

    void SceneInit() {
        scene_name = SceneManager.GetActiveScene().name;
        Debug.Log(scene_name);

        switch (scene_name) {
            case "Main":
                if(UpdateManager.Instance.UPCK)
                    StartCoroutine(AssetLoader.Instance.LoadAssetFromLocalDisk());
                break;
        }
    }

    public IEnumerator LoadSceneFromAssetBundle(string sceneName, bool isAdditive) {
        AssetBundle assetBundle = AssetBundle.LoadFromFile(Path.assetBundleDirectory + "/gamescene");

        // 에셋 번들 내에 존재하는 씬의 경로를 모두 가져오기
        string[] scenes = assetBundle.GetAllScenePaths();
        string loadScenePath = null;

        foreach (string sname in scenes) {
            if (sname.Contains(sceneName)) {
                loadScenePath = sname;
            }
        }

        if (loadScenePath == null)
            yield return null;

        LoadSceneMode loadMode;

        if (isAdditive)
            loadMode = LoadSceneMode.Additive;
        else
            loadMode = LoadSceneMode.Single;
        
        AsyncOperation async = SceneManager.LoadSceneAsync(loadScenePath, loadMode);

        UIManager.Instance.loading_progress.SetActive(true);

        while (!async.isDone) {
            UIManager.Instance.loading_progress.GetComponent<Text>().text = (async.progress * 100f).ToString();
            Debug.Log("loading" + async.progress * 100f);
            async.allowSceneActivation = async.progress > 0.8;
            yield return null;
        }
        UIManager.Instance.loading_progress.SetActive(false);

        SceneInit();
    }
}
