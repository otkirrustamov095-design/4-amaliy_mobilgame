using UnityEngine;

// Dushman holatlari
public enum EnemyState
{
    Patrol, // nuqtalar orasida yuradi
    Chase,  // o'yinchini ta'qib qiladi
    Attack  // o'yinchiga hujum qiladi
}

// Oddiy sun'iy intellekt: patrul -> ta'qib -> hujum
public class EnemyCar : MonoBehaviour
{
    [Header("Havolalar")]
    public Transform player;
    public Transform[] patrolPoints; // 2-3 ta patrul nuqtasi
    public Renderer bodyRenderer;

    [Header("Sozlamalar")]
    public float patrolSpeed = 6f;
    public float chaseSpeed = 10f;
    public float seeDistance = 15f;    // shu masofada o'yinchini "ko'radi"
    public float attackDistance = 4f;  // shu masofada hujum qiladi
    public float attackCooldown = 1f;  // hujumlar orasidagi vaqt (soniya)
    public int damage = 1;

    public EnemyState state = EnemyState.Patrol;

    int currentPoint = 0;
    float lastAttackTime;

    void Update()
    {
        if (GameManager.Instance.isGameOver) return;

        // 1) Masofaga qarab holatni tanlaymiz
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackDistance)
            state = EnemyState.Attack;
        else if (distance <= seeDistance)
            state = EnemyState.Chase;
        else
            state = EnemyState.Patrol;

        // 2) Holatga mos ish bajaramiz
        switch (state)
        {
            case EnemyState.Patrol: Patrol(); break;
            case EnemyState.Chase:  Chase();  break;
            case EnemyState.Attack: Attack(); break;
        }
    }

    void Patrol()
    {
        bodyRenderer.material.color = Color.blue;

        Transform target = patrolPoints[currentPoint];
        MoveTo(target.position, patrolSpeed);

        // Nuqtaga yetib kelsak - keyingisiga o'tamiz
        Vector3 a = transform.position; a.y = 0;
        Vector3 b = target.position;    b.y = 0;
        if (Vector3.Distance(a, b) < 1f)
            currentPoint = (currentPoint + 1) % patrolPoints.Length;
    }

    void Chase()
    {
        bodyRenderer.material.color = new Color(1f, 0.5f, 0f); // to'q sariq
        MoveTo(player.position, chaseSpeed);
    }

    void Attack()
    {
        bodyRenderer.material.color = Color.red;
        LookAtTarget(player.position);

        if (Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;
            GameManager.Instance.TakeDamage(damage);
            Debug.Log("Dushman hujum qildi!");
        }
    }

    void MoveTo(Vector3 target, float speed)
    {
        target.y = transform.position.y;
        LookAtTarget(target);
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    void LookAtTarget(Vector3 target)
    {
        Vector3 direction = target - transform.position;
        direction.y = 0;
        if (direction.sqrMagnitude < 0.01f) return;

        Quaternion look = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, look, 200f * Time.deltaTime);
    }

    public string GetStateName()
    {
        switch (state)
        {
            case EnemyState.Patrol: return "Patrul (ko'k)";
            case EnemyState.Chase:  return "Ta'qib (to'q sariq)";
            default:                return "HUJUM (qizil)";
        }
    }

    // Scene oynasida ko'rish va hujum radiuslarini chizib beradi
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, seeDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);

        if (patrolPoints == null) return;
        Gizmos.color = Color.cyan;
        for (int i = 0; i < patrolPoints.Length; i++)
        {
            if (patrolPoints[i] == null) continue;
            Gizmos.DrawSphere(patrolPoints[i].position, 0.5f);
            Transform next = patrolPoints[(i + 1) % patrolPoints.Length];
            if (next != null) Gizmos.DrawLine(patrolPoints[i].position, next.position);
        }
    }
}
