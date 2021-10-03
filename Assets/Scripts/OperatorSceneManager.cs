using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OperatorSceneManager : MonoBehaviour
{
    public void playAgain()
    {
        SceneManager.LoadScene("Play");
    }
}
