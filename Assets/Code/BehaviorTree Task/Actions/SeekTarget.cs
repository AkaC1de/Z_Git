using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Unity.Mathematics;
using TMPro;

namespace BehaviorDesigner.Runtime.Tasks.MyPoint 
{
    [TaskCategory("My Point")]
    [TaskDescription("如果有目标，则追击目标，追到后返回成功；如果没有目标或超出最远追击点的范围则返回失败")]
    public class SeekTarget : Action
    {
        [Header("获取并追击的目标")]
        public SharedTransform seekTarget; //追击目标的位置
        [Header("追击速度")]
        public float seekSpeed;
        [Header("抵达目标位置的间隔距离")]
        public float arriveDistance;

        [Header("最远追击点（根据需求设置）")]
        public Transform objectPointLeftUp;
        public Transform objectPointRightDown;
        [Header("最远目标点（根据需求设置）")]
        //该点的使用需要保证物体的Eye碰不到目标时，目标会超出最远点，否则如果在Eye碰到目标时超出范围易出现bug
        public Transform targetPointLeftUp;
        public Transform targetPointRightDown;
        [Header("追击时移动哪些轴")]
        public bool X = false;
        public bool Y = false;
        [Header("是否需要根据玩家位置变换左右方向")]
        public bool scale = false;
        [Header("超出最远点时将布尔设置为false（根据需求设置）")]
        public SharedBool isHurting;
        public SharedBool isSeek;

        private float sqrArriveDistance;
        public override void OnStart()
        {
            sqrArriveDistance = arriveDistance * arriveDistance;
        }
        public override TaskStatus OnUpdate()
        {
            //x,y轴都不移动，返回失败
            if (X == false && Y == false)
            {
                return TaskStatus.Failure;
            }
            if (seekTarget == null || seekTarget.Value == null)
            {
                return TaskStatus.Failure;
            }
            //x,y轴均移动
            if (X == true && Y == true)
            {
                //transform.LookAt(seekTarget.Value.position);//直接朝向目标位置（3D）
                if (scale == true)
                {
                    ChangeDirection();//根据位置变换方向(2D_横板)
                }

                transform.position = Vector3.MoveTowards(transform.position, seekTarget.Value.position, seekSpeed * Time.deltaTime);

                if (objectPointLeftUp != null && objectPointRightDown != null)
                {
                    //该是否超出追击点的判断同样适用于俯视角游戏，因此在放置追击点位置时左上要稍微高一点，防止敌人跳跃出追击范围
                    if (transform.position.x < objectPointLeftUp.position.x || transform.position.y > objectPointLeftUp.position.y
                        || transform.position.x > objectPointRightDown.position.x || transform.position.y < objectPointRightDown.position.y)
                    {
                        isHurting.Value = false;
                        isSeek.Value = false;
                        return TaskStatus.Failure;
                    }

                    else
                    {
                        if ((seekTarget.Value.position - transform.position).sqrMagnitude < sqrArriveDistance)
                        {
                            return TaskStatus.Success;//距离目标的位置比较小，认定为到达
                        }
                        else if(targetPointLeftUp != null && targetPointRightDown != null 
                            && (seekTarget.Value.position.x > targetPointRightDown .position.x || seekTarget.Value.position.x < targetPointLeftUp .position.x
                            || seekTarget.Value.position.y > targetPointLeftUp.position.y || seekTarget.Value.position.y < targetPointRightDown .position.y) )
                        {
                            isHurting.Value = false;
                            isSeek.Value = false;
                            return TaskStatus.Failure;//有目标与该物体的最远点，且超过了最远范围，返回失败 
                        }
                        return TaskStatus.Running;
                    }
                }
                else
                {
                    if ((seekTarget.Value.position - transform.position).sqrMagnitude < sqrArriveDistance)
                    {
                        return TaskStatus.Success;//距离目标的位置比较小，认定为到达
                    }
                    else if (targetPointLeftUp != null && targetPointRightDown != null
                            && (seekTarget.Value.position.x > targetPointRightDown.position.x || seekTarget.Value.position.x < targetPointLeftUp.position.x
                            || seekTarget.Value.position.y > targetPointLeftUp.position.y || seekTarget.Value.position.y < targetPointRightDown.position.y))
                    {
                        isHurting.Value = false;
                        isSeek.Value = false;
                        return TaskStatus.Failure;//有目标与该物体的最远点，且超过了最远范围，返回失败 
                    }
                    return TaskStatus.Running;
                }
            }
            else if(X == true && Y == false)
            {
                //transform.LookAt(seekTarget.Value.position);//直接朝向目标位置（3D）
                if (scale == true)
                {
                    ChangeDirection();//根据位置变换方向(2D_横板)
                }

                transform.position = new Vector3(Mathf.MoveTowards(transform.position.x, seekTarget.Value.position.x, seekSpeed * Time.deltaTime), transform.position.y, transform.position.z);

                if (objectPointLeftUp != null && objectPointRightDown != null)
                {
                    //仅某个轴移动时，最远追击点的放置与物体移动的轴持平
                    if (transform.position.x < objectPointLeftUp.position.x || transform.position.y > objectPointLeftUp.position.y
                        || transform.position.x > objectPointRightDown.position.x || transform.position.y < objectPointRightDown.position.y)
                    {
                        isHurting.Value = false;
                        isSeek.Value = false;
                        return TaskStatus.Failure;
                    }

                    else
                    {
                        if (Mathf.Abs(seekTarget.Value.position.x - transform.position.x) < arriveDistance)
                        {
                            return TaskStatus.Success;//距离目标的位置比较小，认定为到达
                        }
                        else if (targetPointLeftUp != null && targetPointRightDown != null
                             && (seekTarget.Value.position.x > targetPointRightDown.position.x || seekTarget.Value.position.x < targetPointLeftUp.position.x
                             || seekTarget.Value.position.y > targetPointLeftUp.position.y || seekTarget.Value.position.y < targetPointRightDown.position.y))
                        {
                            isHurting.Value = false;
                            isSeek.Value = false;
                            return TaskStatus.Failure;//有目标与该物体的最远点，且超过了最远范围，返回失败 
                        }
                        return TaskStatus.Running;
                    }
                }
                else
                {
                    if (Mathf.Abs(seekTarget.Value.position.x - transform.position.x) < arriveDistance)
                    {
                        return TaskStatus.Success;//距离目标的位置比较小，认定为到达
                    }
                    else if (targetPointLeftUp != null && targetPointRightDown != null
                             && (seekTarget.Value.position.x > targetPointRightDown.position.x || seekTarget.Value.position.x < targetPointLeftUp.position.x
                             || seekTarget.Value.position.y > targetPointLeftUp.position.y || seekTarget.Value.position.y < targetPointRightDown.position.y))
                    {
                        isHurting.Value = false;
                        isSeek.Value = false;
                        return TaskStatus.Failure;//有目标与该物体的最远点，且超过了最远范围，返回失败 
                    }
                    return TaskStatus.Running;
                }
            }
            else if(X == false && Y == true)
            {

                //transform.LookAt(seekTarget.Value.position);//直接朝向目标位置（3D）
                if (scale == true)
                {
                    ChangeDirection();//根据位置变换方向(2D_横板)
                }

                transform.position = new Vector3(transform.position.x, Mathf.MoveTowards(transform.position.y, seekTarget.Value.position.y, seekSpeed * Time.deltaTime), transform.position.z);

                if (objectPointLeftUp != null && objectPointRightDown != null)
                {
                    //仅某个轴移动时，最远追击点的放置与物体移动的轴持平
                    if (transform.position.x < objectPointLeftUp.position.x || transform.position.y > objectPointLeftUp.position.y
                        || transform.position.x > objectPointRightDown.position.x || transform.position.y < objectPointRightDown.position.y)
                    {
                        isHurting.Value = false;
                        isSeek.Value = false;
                        return TaskStatus.Failure;
                    }

                    else
                    {
                        if (Mathf.Abs(seekTarget.Value.position.y - transform.position.y) < arriveDistance)
                        {
                            return TaskStatus.Success;//距离目标的位置比较小，认定为到达
                        }
                        else if (targetPointLeftUp != null && targetPointRightDown != null
                             && (seekTarget.Value.position.x > targetPointRightDown.position.x || seekTarget.Value.position.x < targetPointLeftUp.position.x
                             || seekTarget.Value.position.y > targetPointLeftUp.position.y || seekTarget.Value.position.y < targetPointRightDown.position.y))
                        {
                            isHurting.Value = false;
                            isSeek.Value = false;
                            return TaskStatus.Failure;//有目标与该物体的最远点，且超过了最远范围，返回失败 
                        }
                        return TaskStatus.Running;
                    }
                }
                else
                {
                    if (Mathf.Abs(seekTarget.Value.position.y - transform.position.y) < arriveDistance)
                    {
                        return TaskStatus.Success;//距离目标的位置比较小，认定为到达
                    }
                    else if (targetPointLeftUp != null && targetPointRightDown != null
                             && (seekTarget.Value.position.x > targetPointRightDown.position.x || seekTarget.Value.position.x < targetPointLeftUp.position.x
                             || seekTarget.Value.position.y > targetPointLeftUp.position.y || seekTarget.Value.position.y < targetPointRightDown.position.y))
                    {
                        isHurting.Value = false;
                        isSeek.Value = false;
                        return TaskStatus.Failure;//有目标与该物体的最远点，且超过了最远范围，返回失败 
                    }
                    return TaskStatus.Running;
                }
            }
            else
            {
                isHurting.Value = false;
                isSeek.Value = false;
                return TaskStatus.Failure;
            }
        }
          public void ChangeDirection()
           {
            if ( transform.position.x < seekTarget.Value.position.x) 
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            if ( transform.position.x > seekTarget.Value.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
    }
}
