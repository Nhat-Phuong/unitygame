using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eagle1 : MonoBehaviour
{
    public float speed = 2f; // Tốc độ di chuyển
    private Transform player; // Tham chiếu đến Player
    public float LineSite; // Khoảng cách phát hiện Player

    [SerializeField] private AudioSource attacksound;

    private Vector3 startPos; // Vị trí ban đầu của Eagle

    public float verticalSpeed = 1f; // Tốc độ di chuyển lên xuống
  
    private float attackCooldown = 1f; // Thời gian chờ giữa các lần phát âm thanh tấn công
    private float lastAttackTime = -1f; // Lưu thời điểm phát âm thanh tấn công cuối cùng

    private bool isPlayerInRange = false; // Kiểm tra Player có trong phạm vi không

     void Start()
    {
      
        player = GameObject.FindGameObjectWithTag("Player").transform; // Lấy vị trí Player
        startPos = transform.position; // Lưu vị trí ban đầu của Eagle
    }

    private void Update()
    {
        if (player != null)
        {
            float distance = Vector2.Distance(player.position, transform.position); // Tính khoảng cách với Player

            if (distance < LineSite)
            {
                isPlayerInRange = true;
                Attack(); // Phát âm thanh tấn công (có cooldown)

                // Di chuyển đến Player trên cả trục X và Y
                Vector3 targetPosition = new Vector3(player.position.x, player.position.y, transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

                // Quay mặt về hướng Player
                if (player.position.x < transform.position.x)
                {
                    transform.localScale = new Vector3(0.3f, 0.3f, 0.3f); // Quay mặt sang trái
                }
                else
                {
                    transform.localScale = new Vector3(-0.3f, 0.3f, 0.3f); // Quay mặt sang phải
                }

            }

        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, LineSite); // Vẽ hình cầu với bán kính LineSite
    }

    private void Attack()
    {
        // Kiểm tra cooldown trước khi phát âm thanh
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            attacksound.Play(); // Phát âm thanh tấn công
            lastAttackTime = Time.time; // Cập nhật thời điểm phát âm thanh cuối cùng
        }
    }
}
