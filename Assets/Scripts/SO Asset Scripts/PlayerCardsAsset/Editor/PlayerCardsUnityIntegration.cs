using UnityEngine;
using UnityEditor;

static class PlayerCardsUnityIntegration
{ 
    [MenuItem("Assets/Create/PlayerCardsAsset")]
    public static void CreateYourScriptableObject()
    {
        ScriptableObjectUtility2.CreateAsset<PlayerCardsAsset>();
    }

}
