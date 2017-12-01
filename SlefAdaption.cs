/* 
 *Copyright(C) 2017 by zhanghao 
 *All rights reserved. 
 *FileName:     SlefAdaption 
 *Version:      2017 
 *UnityVersion：5.6.2
 *Date:         2017-12-1
 *Description:    
 *History: 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using System.Runtime.InteropServices;
using System;

public class SlefAdaption : MonoBehaviour
{
    private ImageTargetBehaviour mImageTargetBehaviour = null;

    int width;

    int height;

    int leftUpPositionX;

    int leftUpPositionY;

    [DllImport("user32.dll")]       

    public static extern int SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int Width, int Height, int flags);
    //参数分别是：x,y分别表示窗口左上角在屏幕的什么位置，width，height为0表示不改变窗口大小。

    [DllImport("user32.dll")]

    public static extern IntPtr GetActiveWindow();
    //获得当前窗口
    void Start()
    {

        mImageTargetBehaviour = GetComponent<ImageTargetBehaviour>();

        if (mImageTargetBehaviour == null)
        {
            Debug.Log("ImageTargetBehaviour not found ");
        }

        StartCoroutine(ForceRes());//此协程改变分辨率及更改程序运行窗口大小
    }

 
    void Update()
    {

        if (mImageTargetBehaviour == null)
        {
            Debug.Log("ImageTargetBehaviour not found");
            return;
        }

        Vector2 targetSize = mImageTargetBehaviour.GetSize();//得到imagetarget的像素大小

        float targetAspect = targetSize.x / targetSize.y;

        Vector3 pointOnTarget = new Vector3(-0.5f, 0, -0.5f / targetAspect);//pointOnTarget=new Vector3(-0.5,0,-0.37)


        Vector3 targetPointInWorldRef = transform.TransformPoint(pointOnTarget);//将imagetarget的本地坐标转化成世界坐标


        Vector3 screenPoint = Camera.main.WorldToScreenPoint(targetPointInWorldRef); //将imagetarget的世界坐标转化成屏幕坐标

        Debug.Log("target point in screen coords: " + screenPoint.x + ", " + screenPoint.y);

        leftUpPositionX = Math.Abs((int)screenPoint.x);//将imagetarget的x轴屏幕坐标转化成绝对值形式赋给变量leftUpPositionX

        leftUpPositionY = Math.Abs((int)screenPoint.y);//将imagetarget的y轴屏幕坐标转化成绝对值形式赋给变量leftUpPositionY
    }

    IEnumerator ForceRes()//此协程改变程序运行窗口分辨率及更改程序运行窗口左上角位置
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);

            Screen.SetResolution(500,500, false);

            yield return new WaitForSeconds(1f);

            SetWindowPos(GetActiveWindow(), 0, leftUpPositionX, leftUpPositionY, 5, 5, 0x001);
        }
    }
}

