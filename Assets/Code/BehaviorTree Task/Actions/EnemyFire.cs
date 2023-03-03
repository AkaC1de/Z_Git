using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

namespace BehaviorDesigner.Runtime.Tasks.MyPoint
{

    [TaskCategory("My Point")]
    [TaskDescription("��ȡ��Ŀ���λ�ú󣬳�Ŀ�귢���ӵ�")]
    public class EnemyFire : Action
    {
        [Header("�ӵ�ƫ�ƽǶ�")]
        public float bulletAngel;
        [Header("�ӵ���Ԥ����")]
        public GameObject bulletPrefab;
        [Header("���ǵ�Ԥ����")]
        public GameObject shellPrefab;
        [Header("�ӵ����ɵ�λ��")]
        public Transform bulletPos;
        [Header("�������ɵ�λ��")]
        public Transform bulletShellPos;
        [Header("Ŀ���λ��")]
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
                //����任
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