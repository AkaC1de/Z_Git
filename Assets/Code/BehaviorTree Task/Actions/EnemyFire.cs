using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

namespace BehaviorDesigner.Runtime.Tasks.MyPoint
{

    [TaskCategory("My Point")]
    [TaskDescription("获取到目标的位置后，朝目标发射子弹")]
    public class EnemyFire : Action
    {
        [Header("子弹偏移角度")]
        public float bulletAngel;
        [Header("子弹的预制体")]
        public GameObject bulletPrefab;
        [Header("弹壳的预制体")]
        public GameObject shellPrefab;
        [Header("子弹生成的位置")]
        public Transform bulletPos;
        [Header("弹壳生成的位置")]
        public Transform bulletShellPos;
        [Header("目标的位置")]
        public SharedTransform target;

        private Vector2 direction;
        public SharedBool isShoot;

        public override TaskStatus OnUpdate()
        {
            if (target == null)
            {
                return TaskStatus.Failure;
            }
            else
            {
                //方向变换
                if (target.Value.position.x < transform.position.x)
                    transform.localScale = new Vector3(-1, 1, 1);
                else
                    transform.localScale = new Vector3(-1, -1, 1);
                Shoot();
                return TaskStatus.Success;
            }
        }
        public void Shoot()
        {
            direction = (target.Value.position - new Vector3(transform.position.x, transform.position.y,target.Value.position.z)).normalized;
            transform.right = -direction;
            if(isShoot.Value == false)
            {
                isShoot.Value = true;
                Fire();
            }
        }
        public void Fire()
        {
            GameObject bullet = MyObjectPool.Instance.GetObject(bulletPrefab);
            bullet.transform.position = bulletPos.position;
            float angel = Random.Range(-bulletAngel, bulletAngel);
            bullet.GetComponent<BulletEnemy>().SetSpeed(Quaternion.AngleAxis(angel, Vector3.forward) * direction);
            GameObject shell = MyObjectPool.Instance.GetObject(shellPrefab);
            shell.transform.position = bulletShellPos.position;
            shell.transform.rotation = bulletShellPos.rotation;
        }
    }
}