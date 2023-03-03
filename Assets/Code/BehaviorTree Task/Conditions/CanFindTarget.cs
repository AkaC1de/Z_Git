using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskCategory("My Point")]
    [TaskDescription("寻找范围内是否有追击的对象，如果有则获取其位置" +
        "（需要给GameObject添加一个空的子物体，并为其添加isTrigger的碰撞体作为追击范围。）")]
    public class CanFindTarget : Conditional
    {
        [Header("要寻找目标的Tag")]
        public SharedString findTag = "";
        [Header("存储寻找到的目标")]
        public SharedTransform canSeekTarget;
        [Header("没有目标后关闭(根据需求设置)")]
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
