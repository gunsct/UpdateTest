using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : DonDeleteSingleton<UIManager> {
    public GameObject update_button, loading_progress;

    public override void Init() {
        update_button.SetActive(false);
        loading_progress.SetActive(false);
    }

    public void UpdateButton() {
        StartCoroutine(BundleDownLoad.Instance.SaveAssetBundleOnDisk());
        update_button.SetActive(false);
    }
}
