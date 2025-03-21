using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : EnemyAttack
{
	public GameObject projectilePrefab;
	public Transform enemy;
	public Transform firePoint;
	public float projectileSpeed = 5f;


	public override void PerformAttack()
	{
		GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
		projectile.transform.localScale = enemy.transform.localScale;
		BulletAlien script = projectile.GetComponent<BulletAlien>();
		if (script != null)
		{
			if (projectile.transform.localScale.x > 0)
			{
				script.SetBulletValue(projectileSpeed);
			}
			else
			{
				script.SetBulletValue(-projectileSpeed);
			}
		}
	}
}
