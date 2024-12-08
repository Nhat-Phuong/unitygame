using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eagle : Enemy
{
    public float speed = 2; // Tốc độ di chuyển
    private Transform player; // Tham chiếu đến Player
    public float LineSite; // Khoảng cách phát hiện Player
    [SerializeField] private AudioSource flysound;
    [SerializeField] private AudioSource attacksound;

    private Vector3 startPos; // Vị trí ban đầu của Eagle
    public float verticalMovementDistance = 2f; // Khoảng cách di chuyển lên xuống
    public float verticalSpeed = 1f; // Tốc độ di chuyển lên xuống

    private float attackCooldown = 1f; // Thời gian chờ giữa các lần phát âm thanh tấn công
    private float lastAttackTime = -1f; // Lưu thời điểm phát âm thanh tấn công cuối cùng

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

            // Nếu Player nằm trong khoảng phát hiện
            if (distance < LineSite)
            {
                Attack(); // Phát âm thanh tấn công (có cooldown)
                Fly(); // Phát âm thanh bay khi phát hiện Player
                // Di chuyển đến Player
                transform.position = Vector2.MoveTowards(this.transform.position, player.position, speed * Time.deltaTime);

                // **Quay trái hoặc phải theo vị trí của Player**
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
                // Di chuyển lên xuống khi chưa phát hiện Player
                float newY = startPos.y + Mathf.Sin(Time.time * verticalSpeed) * verticalMovementDistance;
                transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, LineSite); // Vẽ hình cầu với bán kính LineSite
    }

    private void Fly()
    {
        if (!flysound.isPlaying)
        {
            flysound.Play(); // Phát âm thanh bay
        }
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
