using UnityEditor;
using UnityEngine;

// Custom editor tool used to visually edit obstacle data inside the Unity Editor
public class ObstacleEditorWindow : EditorWindow
{
    ObstacleData obstacleData;   // ScriptableObject that stores blocked tile information

    // Adds the obstacle editor window to the Unity Tools menu
    [MenuItem("Tools/Obstacle Editor")]
    static void Open()
    {
        GetWindow<ObstacleEditorWindow>("Obstacle Editor");
    }

    // Draws the custom editor UI for toggling obstacle tiles
    void OnGUI()
    {
        obstacleData = (ObstacleData)EditorGUILayout.ObjectField(
            "Obstacle Data",
            obstacleData,
            typeof(ObstacleData),
            false
        );

        // Stops rendering if no obstacle data is assigned
        if (obstacleData == null || obstacleData.blocked == null)
            return;

        GUILayout.Space(10);

        // Displays a grid of toggle buttons representing the obstacle layout
        for (int y = obstacleData.height - 1; y >= 0; y--)
        {
            GUILayout.BeginHorizontal();
            for (int x = 0; x < obstacleData.width; x++)
            {
                bool current = obstacleData.IsBlocked(x, y);
                bool newValue = GUILayout.Toggle(current, "");

                // Updates ScriptableObject data when a toggle state changes
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
