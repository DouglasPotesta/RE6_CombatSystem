using UnityEngine;
using UnityEditor;

public class AssetCreator
{
    [MenuItem("Assets/Create/Weapon Behaviour")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<WeaponBehaviour>();
    }
}
