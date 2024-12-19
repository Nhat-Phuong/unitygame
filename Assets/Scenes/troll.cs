using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class troll : MonoBehaviour
{
    public GameOver gameOver; // Đối tượng quản lý UI Game Over
    [SerializeField] GameObject trollScene; // Đối tượng hiển thị cảnh troll

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Time.timeScale = 0f; // Dừng thời gian
            trollScene.SetActive(true); // Hiển thị cảnh troll
            StartCoroutine(changeScene()); // Gọi coroutine để chuyển sang Game Over
        }
    }

    private IEnumerator changeScene()
    {
        yield return new WaitForSeconds(3); // Đợi 3 giây
        trollScene.SetActive(false); // Tắt cảnh troll
        gameOver.Setup(0); // Hiển thị UI Game Over với số điểm 0 (hoặc tùy ý)
    }
}
