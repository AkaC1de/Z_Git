using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskCategory("My Point")]
    [TaskDescription("Ѱ�ҷ�Χ���Ƿ���׷���Ķ�����������ȡ��λ��" +
        "����Ҫ��GameObject���һ���յ������壬��Ϊ�����isTrigger����ײ����Ϊ׷����Χ����")]
    public class CanFindTarget : Conditional
    {
        [Header("ҪѰ��Ŀ���Tag")]
        public SharedString findTag = "";
        [Header("�洢Ѱ�ҵ���Ŀ��")]
        public SharedTransform canSeekTarget;
        [Header("û��Ŀ���ر�(������������)")]
        public GameObject offObject;

        public SharedBool isEnteredTrigger = false;

        public override TaskStatus OnUpdate()
        {
            return isEnteredTrigger.Value ? TaskStatus.Success : TaskStatus.Failure;
        }


        public override void OnTriggerEnter2D(Collider2D other)
        {
            if (string.IsNullOrEmpty(findTag.Value) || other.gameObject.CompareTag(findTag.Value))
            {
                canSeekTarget.Value = other.transform;
                isEnteredTrigger.Value = true;
            }
        }
        public override void OnTriggerExit2D(Collider2D other)
        {
            if (string.IsNullOrEmpty(findTag.Value) || other.gameObject.CompareTag(findTag.Value))
            {
                if(offObject != null)
                {
                    offObject.SetActive(false);
                }
                isEnteredTrigger.Value = false;
                canSeekTarget.Value = null;
            }
        }

    }
}
