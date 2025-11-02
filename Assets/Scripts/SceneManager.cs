using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    [Header("Sahne İsimleri (Build Settings'te ekli olmalı)")]
    public string scene1Name = "Scene1";
    public string scene2Name = "Scene2";

    public void GoToScene1()
    {
        LoadSingle(scene1Name);
    }

    public void GoToScene2()
    {
        LoadSingle(scene2Name);
    }

    private void LoadSingle(string sceneName)
    {
        if (!IsInBuildSettings(sceneName))
        {
            Debug.LogError($"[SceneSwitcher] '{sceneName}' Build Settings’e ekli değil!");
            return;
        }
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    private bool IsInBuildSettings(string sceneName)
    {
        int count = SceneManager.sceneCountInBuildSettings;
        for (int i = 0; i < count; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string name = System.IO.Path.GetFileNameWithoutExtension(path);
            if (name == sceneName) return true;
        }
        return false;
    }
}
