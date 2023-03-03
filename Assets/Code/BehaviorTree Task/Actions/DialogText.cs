using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.UI;
using Unity.VisualScripting;

namespace BehaviorDesigner.Runtime.Tasks.MyPoint
{
    [TaskCategory("My Point")]
    [TaskDescription("在选定的位置显示对话文字，文字显示完全后" +
        "再次按任意按键切换到下一句。")]
    public class DialogText : Action
    {
        [Header("文本的内容")]
        public string text;
        [Header("对话文本显示的位置")]
        public Text textContent;
        [Header("是否让文字立即显示")]
        public bool ifInstant = false;
        [Header("是否开启浮现型文字切换为立即显示的功能")]
        public bool ifSwitchInstant = false;
        [Header("浮现型文字的显示速度(间隔时间)")]
        public float textSpeed;
        [Header("切换下一条文字时的等待时间")]
        public float nextWaitTime;
        [Header("按下哪个按键切换下一句")]
        public KeyCode keyCode;
        [Header("对话者的头像（根据需求设置）")]
        public Texture headPortraitImage;
        [Header("对话者头像的位置（根据需求设置）")]
        public GameObject headPortraitTransform ;


        private RawImage headPortrait;
        private bool canNext = false;
        private bool isPrint = false;
        private bool switchInstant = false;

        public override void OnStart()
        {
            //获取对话者的头像以及位置
            if (headPortraitImage != null && headPortraitTransform != null)
            {
                headPortrait = headPortraitTransform.GetComponent<RawImage>();
                headPortrait.texture = headPortraitImage;
            }
        }
        public override TaskStatus OnUpdate()
        {
            //如果无文本或无文本框则返回失败
            if (text == null || textContent == null)
            {
                return TaskStatus.Failure;
            }

            //进行打字过程中切换显示方式的判断
            SwitchPrintMethod();

            //如果不可以进行下一句且存在文本框和文本
            if (canNext == false & textContent != null && text != null)
            {
                //如果立即显示文本
                if(ifInstant == true)
                {
                    //如果不处于正在打字状态
                    if (isPrint == false)
                    {
                        //立即显示文字
                        StartCoroutine(InstantSetTextContent());
                    }
                }
                else
                {
                    //如果不是立即显示文本且不在打字状态
                    if(isPrint == false)
                    {
                        //进行缓慢显示文字
                        StartCoroutine(SetTextContent());
                    }
                }
            }
            //如果可以进行下一句且按下进入下一句的按键
            if (canNext == true && Input.GetKeyDown(keyCode))
            {
                //正在打字状态和可以进入下一句状态变成false
                canNext = false;
                isPrint= false;
                switchInstant = false;
                //返回成功开始下一句
                return TaskStatus.Success;
            }
            //如果正在打字且不能进行下一句返回运行中
            return TaskStatus.Running;
        }

        IEnumerator SetTextContent()
        {
            textContent.text = null;
            isPrint = true;
            int i = 0;
            while (i < text.Length && switchInstant == false)
            {
                yield return new WaitForSecondsRealtime(textSpeed);
                textContent.text += text[i];
                i++;
            }
            textContent.text = text;
            //延迟一定时间才可以进行下一句
            StartCoroutine(WaitToNext());
        }
        IEnumerator InstantSetTextContent()
        {
            isPrint = true;
            textContent.text = null;
            yield return new WaitForSecondsRealtime(textSpeed);
            textContent.text = text;
            //延迟一定时间才可以进行下一句
            StartCoroutine(WaitToNext());
        }
        IEnumerator WaitToNext()
        {
            yield return new WaitForSecondsRealtime(nextWaitTime);
            canNext = true;
        }
        void SwitchPrintMethod()
        {
            if (ifSwitchInstant == true && isPrint == true && canNext == false && switchInstant == false && Input.GetKeyDown(keyCode))
            {
                switchInstant = true;
            }
        }
    }
}