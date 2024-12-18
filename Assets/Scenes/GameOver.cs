using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
 public Text pointtext;
    public void Setup(int score)
    {
        Time.timeScale = 0f; // Dừng thời gian
        gameObject.SetActive(true);
        pointtext.text = score.ToString() + " Cherries";
    }
    public void Reset()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void exit()
    {
        SceneManager.LoadScene("Menu");
    }
}
