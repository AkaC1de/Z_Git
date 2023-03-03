using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

namespace BehaviorDesigner.Runtime.Tasks.MyPoint
{
    [TaskCategory("My Point")]
    [TaskDescription("获取一个点的位置，并使物体向该点移动")]
    public class MoveTowards : Action
    {
        // 物体移动的速度
        [Header("移动速度")]
        public float speed;
        // 移动朝向的物体
        [Header("目标点")]
        public SharedTransform target;

        public override TaskStatus OnUpdate()
        {
            // 当到达目标位置后，且差距小于0.1则向开始节点返回Success
            if (Vector3.SqrMagnitude(transform.position - target.Value.position) < 0.3f)
            {
                return TaskStatus.Success;
            }
            // 没到达目标位置之前一直保持移动
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
