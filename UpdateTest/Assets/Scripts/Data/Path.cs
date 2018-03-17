using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Path {
#if UNITY_ANDROID
    public static string assetBundleDirectory = Application.persistentDataPath + "/Assets";

#else 
    public static string assetBundleDirectory = "Assets/StreamingAssets";
#endif
}
