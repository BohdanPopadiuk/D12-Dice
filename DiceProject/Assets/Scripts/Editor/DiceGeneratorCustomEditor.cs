using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(DiceGenerator))]
    public class DiceGeneratorCustomEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DiceGenerator diceGenerator = (DiceGenerator)target;
            DrawDefaultInspector();
            DrawLine();

            if (GUILayout.Button("GENERATE DICE FACES", ButtonTextStyle(Color.white)))
            {
                diceGenerator.GenerateDiceFaces();
            }

            if (diceGenerator.transform.childCount > 0)
            {
                if (GUILayout.Button("UPDATE DICE FACES", ButtonTextStyle(Color.white)))
                {
                    diceGenerator.UpdateFaces();
                }
            }
        }

        #region Tools

        private void DrawLine()
        {
            EditorGUILayout.Space();
            var rect = EditorGUILayout.BeginHorizontal();
            Handles.color = Color.gray;
            Handles.DrawLine(new Vector2(rect.x - 15, rect.y), new Vector2(rect.width + 15, rect.y));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
        }

        private GUIStyle ButtonTextStyle(Color color)
        {
            GUIStyle labelStyle = new GUIStyle(GUI.skin.button);
            labelStyle.fontStyle = FontStyle.Bold;
            labelStyle.normal.textColor = color;
            return labelStyle;
        }

        #endregion
    }
}