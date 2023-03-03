using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Unity.Mathematics;
using TMPro;

namespace BehaviorDesigner.Runtime.Tasks.MyPoint 
{
    [TaskCategory("My Point")]
    [TaskDescription("�����Ŀ�꣬��׷��Ŀ�꣬׷���󷵻سɹ������û��Ŀ��򳬳���Զ׷����ķ�Χ�򷵻�ʧ��")]
    public class SeekTarget : Action
    {
        [Header("��ȡ��׷����Ŀ��")]
        public SharedTransform seekTarget; //׷��Ŀ���λ��
        [Header("׷���ٶ�")]
        public float seekSpeed;
        [Header("�ִ�Ŀ��λ�õļ������")]
        public float arriveDistance;

        [Header("��Զ׷���㣨�����������ã�")]
        public Transform objectPointLeftUp;
        public Transform objectPointRightDown;
        [Header("��ԶĿ��㣨�����������ã�")]
        //�õ��ʹ����Ҫ��֤�����Eye������Ŀ��ʱ��Ŀ��ᳬ����Զ�㣬���������Eye����Ŀ��ʱ������Χ�׳���bug
        public Transform targetPointLeftUp;
        public Transform targetPointRightDown;
        [Header("׷��ʱ�ƶ���Щ��")]
        public bool X = false;
        public bool Y = false;
        [Header("�Ƿ���Ҫ�������λ�ñ任���ҷ���")]
        public bool scale = false;
        [Header("������Զ��ʱ����������Ϊfalse�������������ã�")]
        public SharedBool isHurting;
        public SharedBool isSeek;

        private float sqrArriveDistance;
        public override void OnStart()
        {
            sqrArriveDistance = arriveDistance * arriveDistance;
        }
        public override TaskStatus OnUpdate()
        {
            //x,y�ᶼ���ƶ�������ʧ��
            if (X == false && Y == false)
            {
                return TaskStatus.Failure;
            }
            if (seekTarget == null || seekTarget.Value == null)
            {
                return TaskStatus.Failure;
            }
            //x,y����ƶ�
            if (X == true && Y == true)
            {
                //transform.LookAt(seekTarget.Value.position);//ֱ�ӳ���Ŀ��λ�ã�3D��
                if (scale == true)
                {
                    ChangeDirection();//����λ�ñ任����(2D_���)
                }

                transform.position = Vector3.MoveTowards(transform.position, seekTarget.Value.position, seekSpeed * Time.deltaTime);

                if (objectPointLeftUp != null && objectPointRightDown != null)
                {
                    //���Ƿ񳬳�׷������ж�ͬ�������ڸ��ӽ���Ϸ������ڷ���׷����λ��ʱ����Ҫ��΢��һ�㣬��ֹ������Ծ��׷����Χ
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
                            return TaskStatus.Success;//����Ŀ���λ�ñȽ�С���϶�Ϊ����
                        }
                        else if(targetPointLeftUp != null && targetPointRightDown != null 
                            && (seekTarget.Value.position.x > targetPointRightDown .position.x || seekTarget.Value.position.x < targetPointLeftUp .position.x
                            || seekTarget.Value.position.y > targetPointLeftUp.position.y || seekTarget.Value.position.y < targetPointRightDown .position.y) )
                        {
                            isHurting.Value = false;
                            isSeek.Value = false;
                            return TaskStatus.Failure;//��Ŀ������������Զ�㣬�ҳ�������Զ��Χ������ʧ�� 
                        }
                        return TaskStatus.Running;
                    }
                }
                else
                {
                    if ((seekTarget.Value.position - transform.position).sqrMagnitude < sqrArriveDistance)
                    {
                        return TaskStatus.Success;//����Ŀ���λ�ñȽ�С���϶�Ϊ����
                    }
                    else if (targetPointLeftUp != null && targetPointRightDown != null
                            && (seekTarget.Value.position.x > targetPointRightDown.position.x || seekTarget.Value.position.x < targetPointLeftUp.position.x
                            || seekTarget.Value.position.y > targetPointLeftUp.position.y || seekTarget.Value.position.y < targetPointRightDown.position.y))
                    {
                        isHurting.Value = false;
                        isSeek.Value = false;
                        return TaskStatus.Failure;//��Ŀ������������Զ�㣬�ҳ�������Զ��Χ������ʧ�� 
                    }
                    return TaskStatus.Running;
                }
            }
            else if(X == true && Y == false)
            {
                //transform.LookAt(seekTarget.Value.position);//ֱ�ӳ���Ŀ��λ�ã�3D��
                if (scale == true)
                {
                    ChangeDirection();//����λ�ñ任����(2D_���)
                }

                transform.position = new Vector3(Mathf.MoveTowards(transform.position.x, seekTarget.Value.position.x, seekSpeed * Time.deltaTime), transform.position.y, transform.position.z);

                if (objectPointLeftUp != null && objectPointRightDown != null)
                {
                    //��ĳ�����ƶ�ʱ����Զ׷����ķ����������ƶ������ƽ
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
                            return TaskStatus.Success;//����Ŀ���λ�ñȽ�С���϶�Ϊ����
                        }
                        else if (targetPointLeftUp != null && targetPointRightDown != null
                             && (seekTarget.Value.position.x > targetPointRightDown.position.x || seekTarget.Value.position.x < targetPointLeftUp.position.x
                             || seekTarget.Value.position.y > targetPointLeftUp.position.y || seekTarget.Value.position.y < targetPointRightDown.position.y))
                        {
                            isHurting.Value = false;
                            isSeek.Value = false;
                            return TaskStatus.Failure;//��Ŀ������������Զ�㣬�ҳ�������Զ��Χ������ʧ�� 
                        }
                        return TaskStatus.Running;
                    }
                }
                else
                {
                    if (Mathf.Abs(seekTarget.Value.position.x - transform.position.x) < arriveDistance)
                    {
                        return TaskStatus.Success;//����Ŀ���λ�ñȽ�С���϶�Ϊ����
                    }
                    else if (targetPointLeftUp != null && targetPointRightDown != null
                             && (seekTarget.Value.position.x > targetPointRightDown.position.x || seekTarget.Value.position.x < targetPointLeftUp.position.x
                             || seekTarget.Value.position.y > targetPointLeftUp.position.y || seekTarget.Value.position.y < targetPointRightDown.position.y))
                    {
                        isHurting.Value = false;
                        isSeek.Value = false;
                        return TaskStatus.Failure;//��Ŀ������������Զ�㣬�ҳ�������Զ��Χ������ʧ�� 
                    }
                    return TaskStatus.Running;
                }
            }
            else if(X == false && Y == true)
            {

                //transform.LookAt(seekTarget.Value.position);//ֱ�ӳ���Ŀ��λ�ã�3D��
                if (scale == true)
                {
                    ChangeDirection();//����λ�ñ任����(2D_���)
                }

                transform.position = new Vector3(transform.position.x, Mathf.MoveTowards(transform.position.y, seekTarget.Value.position.y, seekSpeed * Time.deltaTime), transform.position.z);

                if (objectPointLeftUp != null && objectPointRightDown != null)
                {
                    //��ĳ�����ƶ�ʱ����Զ׷����ķ����������ƶ������ƽ
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
                            return TaskStatus.Success;//����Ŀ���λ�ñȽ�С���϶�Ϊ����
                        }
                        else if (targetPointLeftUp != null && targetPointRightDown != null
                             && (seekTarget.Value.position.x > targetPointRightDown.position.x || seekTarget.Value.position.x < targetPointLeftUp.position.x
                             || seekTarget.Value.position.y > targetPointLeftUp.position.y || seekTarget.Value.position.y < targetPointRightDown.position.y))
                        {
                            isHurting.Value = false;
                            isSeek.Value = false;
                            return TaskStatus.Failure;//��Ŀ������������Զ�㣬�ҳ�������Զ��Χ������ʧ�� 
                        }
                        return TaskStatus.Running;
                    }
                }
                else
                {
                    if (Mathf.Abs(seekTarget.Value.position.y - transform.position.y) < arriveDistance)
                    {
                        return TaskStatus.Success;//����Ŀ���λ�ñȽ�С���϶�Ϊ����
                    }
                    else if (targetPointLeftUp != null && targetPointRightDown != null
                             && (seekTarget.Value.position.x > targetPointRightDown.position.x || seekTarget.Value.position.x < targetPointLeftUp.position.x
                             || seekTarget.Value.position.y > targetPointLeftUp.position.y || seekTarget.Value.position.y < targetPointRightDown.position.y))
                    {
                        isHurting.Value = false;
                        isSeek.Value = false;
                        return TaskStatus.Failure;//��Ŀ������������Զ�㣬�ҳ�������Զ��Χ������ʧ�� 
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
