using UnityEditor;
using UnityEngine;

public class ObstacleEditorWindow : EditorWindow
{
    ObstacleData obstacleData;

    [MenuItem("Tools/Obstacle Editor")]
    static void Open()
    {
        GetWindow<ObstacleEditorWindow>("Obstacle Editor");
    }

    void OnGUI()
    {
        obstacleData = (ObstacleData)EditorGUILayout.ObjectField(
            "Obstacle Data",
            obstacleData,
            typeof(ObstacleData),
            false
        );

        if (obstacleData == null || obstacleData.blocked == null)
            return;

        GUILayout.Space(10);

        for (int y = obstacleData.height - 1; y >= 0; y--)
        {
            GUILayout.BeginHorizontal();
            for (int x = 0; x < obstacleData.width; x++)
            {
                bool current = obstacleData.IsBlocked(x, y);
                bool newValue = GUILayout.Toggle(current, "");

                if (newValue != current)
                {
                    obstacleData.SetBlocked(x, y, newValue);
                    EditorUtility.SetDirty(obstacleData);
                }
            }
            GUILayout.EndHorizontal();
        }
    }
}
