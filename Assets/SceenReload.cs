using UnityEngine;
using UnityEngine.SceneManagement;

public class SceenReload : MonoBehaviour
{
    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
