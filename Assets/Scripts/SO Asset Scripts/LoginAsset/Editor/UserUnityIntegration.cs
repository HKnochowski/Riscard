using UnityEngine;
using UnityEditor;

static class UserUnityIntegration
{

    [MenuItem("Assets/Create/LoginAsset")]
    public static void CreateYourScriptableObject()
    {
        ScriptableObjectUtility2.CreateAsset<LoginAsset>();
    }

}
