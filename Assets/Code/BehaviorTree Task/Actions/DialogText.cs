using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.UI;
using Unity.VisualScripting;

namespace BehaviorDesigner.Runtime.Tasks.MyPoint
{
    [TaskCategory("My Point")]
    [TaskDescription("��ѡ����λ����ʾ�Ի����֣�������ʾ��ȫ��" +
        "�ٴΰ����ⰴ���л�����һ�䡣")]
    public class DialogText : Action
    {
        [Header("�ı�������")]
        public string text;
        [Header("�Ի��ı���ʾ��λ��")]
        public Text textContent;
        [Header("�Ƿ�������������ʾ")]
        public bool ifInstant = false;
        [Header("�Ƿ��������������л�Ϊ������ʾ�Ĺ���")]
        public bool ifSwitchInstant = false;
        [Header("���������ֵ���ʾ�ٶ�(���ʱ��)")]
        public float textSpeed;
        [Header("�л���һ������ʱ�ĵȴ�ʱ��")]
        public float nextWaitTime;
        [Header("�����ĸ������л���һ��")]
        public KeyCode keyCode;
        [Header("�Ի��ߵ�ͷ�񣨸����������ã�")]
        public Texture headPortraitImage;
        [Header("�Ի���ͷ���λ�ã������������ã�")]
        public GameObject headPortraitTransform ;


        private RawImage headPortrait;
        private bool canNext = false;
        private bool isPrint = false;
        private bool switchInstant = false;

        public override void OnStart()
        {
            //��ȡ�Ի��ߵ�ͷ���Լ�λ��
            if (headPortraitImage != null && headPortraitTransform != null)
            {
                headPortrait = headPortraitTransform.GetComponent<RawImage>();
                headPortrait.texture = headPortraitImage;
            }
        }
        public override TaskStatus OnUpdate()
        {
            //������ı������ı����򷵻�ʧ��
            if (text == null || textContent == null)
            {
                return TaskStatus.Failure;
            }

            //���д��ֹ������л���ʾ��ʽ���ж�
            SwitchPrintMethod();

            //��������Խ�����һ���Ҵ����ı�����ı�
            if (canNext == false & textContent != null && text != null)
            {
                //���������ʾ�ı�
                if(ifInstant == true)
                {
                    //������������ڴ���״̬
                    if (isPrint == false)
                    {
                        //������ʾ����
                        StartCoroutine(InstantSetTextContent());
                    }
                }
                else
                {
                    //�������������ʾ�ı��Ҳ��ڴ���״̬
                    if(isPrint == false)
                    {
                        //���л�����ʾ����
                        StartCoroutine(SetTextContent());
                    }
                }
            }
            //������Խ�����һ���Ұ��½�����һ��İ���
            if (canNext == true && Input.GetKeyDown(keyCode))
            {
                //���ڴ���״̬�Ϳ��Խ�����һ��״̬���false
                canNext = false;
                isPrint= false;
                switchInstant = false;
                //���سɹ���ʼ��һ��
                return TaskStatus.Success;
            }
            //������ڴ����Ҳ��ܽ�����һ�䷵��������
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
            //�ӳ�һ��ʱ��ſ��Խ�����һ��
            StartCoroutine(WaitToNext());
        }
        IEnumerator InstantSetTextContent()
        {
            isPrint = true;
            textContent.text = null;
            yield return new WaitForSecondsRealtime(textSpeed);
            textContent.text = text;
            //�ӳ�һ��ʱ��ſ��Խ�����һ��
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