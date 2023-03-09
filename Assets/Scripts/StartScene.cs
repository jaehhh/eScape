using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    private void Update()
    {
        if(Input.anyKeyDown)
        {
            SceneManager.LoadScene("Tutorial");
        }
    }
}
