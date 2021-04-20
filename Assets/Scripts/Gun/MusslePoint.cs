using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class MusslePoint : MonoBehaviour
{
    private GameObject bulletPrefab;
    private Player player;
    private Bullet bullet;

    private Transform shotPoint;

    private void Start()
    {
        bulletPrefab = Resources.Load("Raid/Gun/Prefab/BulletHandgun") as GameObject;
        player = this.transform.parent.GetComponent<Player>();
        shotPoint = player.ShotPoint;
        
        
        
    }

    public void Shot()
    {
        StartCoroutine(Shooting(Config.shotSpeed));
    }

    private IEnumerator Shooting(float speed)
    {
        player.Anim.SetTrigger("Shot");

        GameObject intantBullet = Instantiate(bulletPrefab, this.transform.position, this.transform.rotation);
        
        // 탄환 생성 후 Bullet의 ID 와 Damage 설정
        Bullet bullet = intantBullet.transform.GetComponent<Bullet>();
        bullet.BulletID = "아닌데요, 전 뚱~인데요";
        bullet.Damage = 4;

        Rigidbody bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = (shotPoint.position - this.transform.position) * 5; // 총알 진행 방향
        yield return new WaitForSeconds(speed);

        player.IsAttack = false;
    }
}
