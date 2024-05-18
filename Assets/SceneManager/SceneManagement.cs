using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public void ChangeScene(string scene)
    {
        AudioManager.instance.SimpleButtonClickSF();
        SceneManager.LoadScene(scene);
    }
}
