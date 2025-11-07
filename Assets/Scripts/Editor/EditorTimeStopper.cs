using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TimeStopper))]
public class EditorTimeStopper : UnityEditor.Editor
{
    private TimeStopper _stopper;

    private void OnEnable()
    {
        _stopper = target as TimeStopper;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Encontrar Volume"))
        {
            _stopper.LookForColor();
        }
    }
}
