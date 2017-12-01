using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;
using System.Threading;


public class Test : MonoBehaviour
{

    public GameObject _bundleObject;
    public ParticleSystem _bundlePop;
    ParticleSystem _bundle;
    SerialPort _stream;
    List<int> _portValue;
    bool _isStop=false;
    float _timer;
    int value=0;
    int count = 0;

    void Start ()
    {
        _bundle = _bundleObject.GetComponent<ParticleSystem>();
        _stream = new SerialPort("COM3",9600,Parity.None ,8,StopBits.One );
        _portValue = new List<int>();
        if (!_stream.IsOpen)
        {
            _stream.Open();
        }
        StartCoroutine(ValueChange());
    }

    void Update ()
    {
        ParticalControl();
        ByArduino();
        ChangeParticle();
    }   
    void ByArduino()//通过Arduino读取的值判断是否激活气泡特效
    {
        try
        {
            if (value >=1)
            {
                _bundleObject.SetActive(true);
                _bundle.loop = true;
                
            }
            else
            {
                _bundle.loop = false;
                _isStop = true;
            }
        }
        catch (Exception)
        {
            Debug.Log("read false");
        }
    }

    IEnumerator ValueChange()//读取Arduino的值
    {
        while (true)
        {
            if (_stream.ReadLine() != null)
            {
                value = int.Parse(_stream.ReadLine());
            }
            Debug.Log(value);
            yield return 0;
        }
    }
    void ParticalControl()//等待粒子完全消失再让气泡特效消失
    {
        if (_isStop == true)
        {
            _timer += Time.deltaTime;
            if (_timer >= 1.5f)
            {
                _bundleObject.SetActive(false);
                _timer = 0;
                _isStop = false;
            }
        }
    }

    void ChangeParticle()//通过Arduino读取的值来设置气泡特效的大小，速率，粒子数量
    {
        if (value > 0&& value <=100)
        {
            _bundle.startSize = 1.5f;
            _bundle.startSpeed = 7.5f;
            _bundle.maxParticles = 50;
            _bundlePop.startSize = 0.1f;
        }
        else if (value > 100 && value <= 200)
        {
            _bundle.startSize = 2f;
            _bundle.startSpeed = 8f;
            _bundle.maxParticles =100;
            _bundlePop.startSize = 0.2f;
        }
        else if (value > 200 && value <= 300)
        {
            _bundle.startSize = 2.5f;
            _bundle.startSpeed = 8.5f;
            _bundle.maxParticles = 200;
            _bundlePop.startSize = 0.3f;
        }
        else if (value > 300 && value <= 400)
        {
            _bundle.startSize = 3f;
            _bundle.startSpeed = 9f;
            _bundle.maxParticles = 300;
            _bundlePop.startSize = 0.4f;
        }
        else if (value >400 && value <= 500)
        {
            _bundle.startSize = 3.5f;
            _bundle.startSpeed = 9.5f;
            _bundle.maxParticles = 400;
            _bundlePop.startSize = 0.5f;
        }
        else if (value > 500 && value <= 600)
        {
            _bundle.startSize = 4f;
            _bundle.startSpeed = 10f;
            _bundle.maxParticles = 500;
            _bundlePop.startSize = 0.6f;
        }
        else if (value >600 && value <= 700)
        {
            _bundle.startSize = 4.5f;
            _bundle.startSpeed = 10.5f;
            _bundle.maxParticles = 600;
            _bundlePop.startSize = 0.7f;
        }
        else if (value > 700 && value <= 800)
        {
            _bundle.startSize = 5f;
            _bundle.startSpeed = 11f;
            _bundle.maxParticles = 700;
            _bundlePop.startSize = 0.8f;
        }
        else if (value > 800 && value <= 900)
        {
            _bundle.startSize = 5.5f;
            _bundle.startSpeed = 11.5f;
            _bundle.maxParticles = 800;
            _bundlePop.startSize = 0.9f;
        }
        else if (value > 900 && value <= 1000)
        {
            _bundle.startSize = 6f;
            _bundle.startSpeed = 12f;
            _bundle.maxParticles = 900;
            _bundlePop.startSize = 1;
        }
        else if (value >1000&& value<1023)
        {
            _bundle.startSize = 6.5f;
            _bundle.startSpeed = 12.5f;
            _bundle.maxParticles = 1000;
            _bundlePop.startSize = 1.1f;
        }
        else if (value == 1023)
        {
            _bundle.startSize = 20;
            _bundle.startSpeed = 12.5f;
            _bundle.maxParticles = 1000;
            _bundlePop.startSize = 1.1f;
        }
    }

    void OnApplicationQuit()//程序退出时关闭串口
    {
        _stream.Close();
    }
}


