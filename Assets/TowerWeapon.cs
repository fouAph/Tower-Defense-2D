using UnityEngine;

public class TowerWeapon : MonoBehaviour
{
    private Animator animator;
    private Tower tower;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetShoot()
    {
        animator.SetBool("IsShooting", tower.CheckIsEnemyTargetAvailable());
    }

    public void SetShoot(bool value)
    {
        animator.SetBool("IsShooting",false);
    }

    public void Fire()
    {
        tower.FireBullet();
    }

    public void SetAnimationShootingSpeed(float speed)
    {
        animator.SetFloat("SpeedMultiplier", 1 + speed);
    }

    public void SetTower(Tower _tower)
    {
        tower = _tower;
    }
}