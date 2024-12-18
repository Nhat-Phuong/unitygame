using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eagle : Enemy
{
    public float speed = 2f; // Tốc độ di chuyển
    private Transform player; // Tham chiếu đến Player
    public float LineSite; // Khoảng cách phát hiện Player

    [SerializeField] private AudioSource attacksound;

    private Vector3 startPos; // Vị trí ban đầu của Eagle

    public float verticalSpeed = 1f; // Tốc độ di chuyển lên xuống
    [SerializeField] private float top; // Giới hạn trên (trục Y)
    [SerializeField] private float down; // Giới hạn dưới (trục Y)
    private float attackCooldown = 1f; // Thời gian chờ giữa các lần phát âm thanh tấn công
    private float lastAttackTime = -1f; // Lưu thời điểm phát âm thanh tấn công cuối cùng

    private bool isPlayerInRange = false; // Kiểm tra Player có trong phạm vi không

    protected override void Start()
    {
        base.Start(); // Gọi Start() của lớp cha nếu cần
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
                    transform.localScale = new Vector3(1, 1, 1); // Quay mặt sang trái
                }
                else
                {
                    transform.localScale = new Vector3(-1, 1, 1); // Quay mặt sang phải
                }
            }
            else
            {
                isPlayerInRange = false;

                // Di chuyển lên xuống trong phạm vi top-down khi không có Player
                float newY = startPos.y + Mathf.Sin(Time.time * verticalSpeed) * (top - down) / 2f;
                transform.position = new Vector3(transform.position.x, Mathf.Clamp(newY, down, top), transform.position.z);

                // Quay về vị trí ban đầu trên trục X
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(startPos.x, transform.position.y, transform.position.z), speed * Time.deltaTime);
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
