using UnityEditor;
using UnityEngine;


public class QuitGame : MonoBehaviour
{
    public void CloseGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
    }
}
