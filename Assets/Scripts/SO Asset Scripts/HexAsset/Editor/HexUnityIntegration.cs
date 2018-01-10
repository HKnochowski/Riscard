using UnityEngine;
using UnityEditor;

static class HexUnityIntegration
{

    [MenuItem("Assets/Create/HexAsset")]
    public static void CreateYourScriptableObject()
    {
        ScriptableObjectUtility2.CreateAsset<HexAsset>();
    }

}
