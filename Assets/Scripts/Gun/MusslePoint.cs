using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusslePoint : MonoBehaviour
{
    private GameObject bullet;
    private Player player;

    private Transform shotPoint;

    private void Start()
    {
        bullet = Resources.Load("Raid/Gun/Prefab/BulletHandgun") as GameObject;
        player = this.transform.parent.GetComponent<Player>();
        shotPoint = player.ShotPoint;
    }

    public void Shot()
    {
        StartCoroutine(Shooting());
    }

    private IEnumerator Shooting()
    {
        player.Anim.SetTrigger("Shot");

        GameObject intantBullet = Instantiate(bullet, this.transform.position, this.transform.rotation);
        Rigidbody bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = (shotPoint.position - this.transform.position) * 5; // 총알 진행 방향
        yield return new WaitForSeconds(0.3f);

        player.IsAttack = false;
    }
}
