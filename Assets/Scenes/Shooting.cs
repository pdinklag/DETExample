using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Shooting : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float bulletForce = 20f;
    public int koBullets=100;
    void Update() {
        if (Input.GetButtonDown("Fire1")) {
            ShootImpfpfeil();
        }
        if(Input.GetButtonDown("Fire2"))
        {
            ShootBetaeubung();
        }
    }

    void ShootImpfpfeil() {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
    }

    void ShootBetaeubung(){
        if(koBullets>0)
        {
            koBullets--;
             GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
            bullet bulletScript=bullet.GetComponent<bullet>();
            bulletScript.typ=1;
        }
        
    }
    public void addKOBullets(int count)
    {
        koBullets+=count;
    }
}
