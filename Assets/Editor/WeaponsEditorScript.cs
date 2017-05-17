using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WeaponInventory))]
[CanEditMultipleObjects]
public class WeaponsEditorScript : Editor
{


    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    }

}