using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

namespace BehaviorDesigner.Runtime.Tasks.MyPoint
{
    [TaskCategory("My Point")]
    [TaskDescription("��ȡһ�����λ�ã���ʹ������õ��ƶ�")]
    public class MoveTowards : Action
    {
        // �����ƶ����ٶ�
        [Header("�ƶ��ٶ�")]
        public float speed;
        // �ƶ����������
        [Header("Ŀ���")]
        public SharedTransform target;

        public override TaskStatus OnUpdate()
        {
            // ������Ŀ��λ�ú��Ҳ��С��0.1����ʼ�ڵ㷵��Success
            if (Vector3.SqrMagnitude(transform.position - target.Value.position) < 0.3f)
            {
                return TaskStatus.Success;
            }
            // û����Ŀ��λ��֮ǰһֱ�����ƶ�
            ChangeDirection();
            transform.position = Vector3.MoveTowards(transform.position, target.Value.position, speed * Time.deltaTime);
            return TaskStatus.Running;
        }
        public void ChangeDirection()
        {
            if (transform.position.x < target.Value.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            if (transform.position.x > target.Value.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
    }
}
