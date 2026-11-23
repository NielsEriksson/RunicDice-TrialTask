using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] public float attackCooldown;
    [SerializeField] public int attackDamage;
    private Animator animator;
    public void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void Attack()
    {
        FindAnyObjectByType<PlayerHealth>().ChangeHealth(-attackDamage);

        animator.SetTrigger("Attack");
    }
}
