using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour
{
    public void Scene0()
    {
        SceneManager.LoadScene("Scene1");
    }
    public void Scene1()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void Scene2()
    {
        SceneManager.LoadScene("Playground");
    }
}