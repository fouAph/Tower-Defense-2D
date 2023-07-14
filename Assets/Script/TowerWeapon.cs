using System;
using UnityEngine;

public class TowerWeapon : MonoBehaviour
{
    private Animator animator;
    public event EventHandler OnShoot;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void Shoot()
    {
        animator.SetTrigger("Shoot");
        OnShoot?.Invoke(this, EventArgs.Empty);
    }
}