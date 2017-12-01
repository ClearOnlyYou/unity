/** 
 *Copyright(C) 2017 by zhanghao 
 *All rights reserved. 
 *FileName:     TestBuuble 
 *Version:      2017 
 *UnityVersion：5.6.2
 *Date:         2017-11-17
 *Description:    
 *History: 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;

public class TestBubble : MonoBehaviour
{
    public GameObject[] _bundleObject = new GameObject[5];
    public GameObject _parent;
    public int _maxSpeed=2;
    public int _minSpeed = 0;
    public float _regulateSpeed = 0.01f;
    public List<int > array;
    public GameObject _effect;
    ParticleSystem[] _bubble = new ParticleSystem[5];
    ParticleSystem.Particle[] _arrPar;
    SerialPort _stream;
    public int _value = 0;
    int _count = 0;
    bool _isStop = false;
    float _timer;
    public int _one;
    public int _two;
    bool isTrue = false;

    void Start ()
    {
        _stream = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One);
        _bubble[0] = _bundleObject[0].GetComponent<ParticleSystem>();
        _bubble[1] = _bundleObject[1].GetComponent<ParticleSystem>();
        _bubble[2] = _bundleObject[2].GetComponent<ParticleSystem>();
        _bubble[3] = _bundleObject[3].GetComponent<ParticleSystem>();
        _bubble[4] = _bundleObject[4].GetComponent<ParticleSystem>();
        if (!_stream.IsOpen)
        {
            _stream.Open();
        }
        StartCoroutine(ReadValue());

        ParticleBoon();
        Invoke("BoonTimeOne",7);
        Invoke("BoonTimeTwo",15);
    }

	void Update ()
    {
   
        ChangeParticalSpeed();
        //BubbleShow();
        //ParticalControl();
    }

    void ChangeParticalSpeed()//改变粒子速度
    {
        if (array.Count >=11)
        {
            array.Remove(array[0]);
        }
        if (array.Count == 10)
        {
            if (array[7]- array[0]>=4)
            {
                for (int i = 0; i < _bubble.Length; i++)
                {
                    _bubble[i].startSpeed += _regulateSpeed;
                    if (_bubble[i].startSpeed >= _maxSpeed)
                    {
                        _bubble[i].startSpeed = _maxSpeed;
                    }
                    ParticleSystem.VelocityOverLifetimeModule emission = _bubble[i].velocityOverLifetime;
                    emission.z = new ParticleSystem.MinMaxCurve(_bubble[i].startSpeed);
                }
            }
            else if (array[7] - array[0] <= -4 )
            {
                for (int i = 0; i < _bubble.Length; i++)
                {
                    _bubble[i].startSpeed -= _regulateSpeed;
                    if (_bubble[i].startSpeed<= _minSpeed)
                    {
                        _bubble[i].startSpeed = _minSpeed;
                    }
                    ParticleSystem.VelocityOverLifetimeModule emission = _bubble[i].velocityOverLifetime;
                    emission.z = new ParticleSystem.MinMaxCurve(_bubble[i].startSpeed);
                }
            }
        }

    }

    void ParticleBoon()//随机产生将要爆炸的粒子索引值
    {
        while (true)
        {
            _one = UnityEngine.Random.Range(0, 5);
            _two = UnityEngine.Random.Range(0, 5);
            if (_one != _two)
            {
                break;
            }
        }     
    }

    void BoonTimeOne()//第一个粒子爆炸的方法
    {
         _arrPar= new ParticleSystem.Particle[_bubble[_one].main.maxParticles];
        int length = _bubble[_one].GetParticles(_arrPar);
        _bundleObject[_one].SetActive(false); 
        Instantiate(_effect, _arrPar[0].position, Quaternion.identity);
        Debug.LogError("length:" + length);
    }

    void BoonTimeTwo()//第二个粒子爆炸的方法
    {
        _arrPar = new ParticleSystem.Particle[_bubble[_two].main.maxParticles];
        int length = _bubble[_two].GetParticles(_arrPar);
        _bundleObject[_two].SetActive(false);
        Instantiate(_effect, _arrPar[0].position, Quaternion.identity);
        Debug.LogError("length:" + length);
    }

    //void BubbleShow()
    //{
    //    try
    //    {
    //        if (_value == 1)
    //        {
    //            for (int i = 0; i < _bubble.Length; i++)
    //            {
    //                _bubble[i].startSpeed = _minSpeed;
    //                ParticleSystem.VelocityOverLifetimeModule emission = _bubble[i].velocityOverLifetime;
    //                emission.z = new ParticleSystem.MinMaxCurve(_bubble[i].startSpeed);
    //            }
    //        }
    //        else if (_value > 1)
    //        {
    //            _parent.SetActive(true);
    //        }
    //        else
    //        {
    //            _parent.SetActive(false);
    //        }
    //    }
    //    catch (Exception)
    //    {
    //        Debug.Log("read false");
    //    }
    //}

    //void ParticalControl()
    //{
    //    if (_isStop == true)
    //    {
    //        _timer += Time.deltaTime;
    //        if (_timer >= 1.5f)
    //        {
    //            foreach (var item in _bundleObject)
    //            {
    //                item.SetActive(false);
    //            }
    //            _timer = 0; 
    //            _isStop = false;
    //        }
    //    }
    //}

    IEnumerator ReadValue()//读取arduino的值（协程）
    {
        while (true)
        {
            if (_stream.ReadLine() != null)
            {
                _value = int.Parse(_stream.ReadLine());
                array.Add(_value);
            }
            Debug.Log(_value);
            yield return 0;
        }
    }

    void OnApplicationQuit()//程序退出时关闭串口的方法
    {
        _stream.Close();
    }
}
