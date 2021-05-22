using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneLoad : MonoBehaviour
{
    public int nextScene = 1; //This is the scene for the main game, which is most likely the only scene that this script will use.

    public void StartGame() {
        SceneManager.LoadScene(nextScene);
    }
}
