﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Weapon
{
public class MusslePoint : MonoBehaviour
{
    private GameObject bulletPrefab;
    private PlayerUtil player;
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

        GameObject intantBullet = Instantiate(bulletPrefab, this.transform.position, this.transform.rotation);
        
        // 탄환 생성 후 Bullet의 ID 와 Damage 설정
        Bullet bullet = intantBullet.transform.GetComponent<Bullet>();
        bullet.BulletID = Config.userUuid;
        bullet.Damage = 1;

        Rigidbody bulletRigid = intantBullet.GetComponent<Rigidbody>();
        if(player.IsMyCharacter())
            bulletRigid.velocity = (shotPoint.position - this.transform.position) * 30; // 총알 진행 방향
        else
            bulletRigid.velocity = (new Vector3(player.PInfo.angle.X,player.PInfo.angle.Y,player.PInfo.angle.Z) - this.transform.position) * 5; // 총알 진행 방향
        yield return new WaitForSeconds(speed);

        player.IsAttack = false;
    }
}

}