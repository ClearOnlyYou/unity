using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
//泡泡粒子 ,张茂， 2017-11-17
public class NewBubble : MonoBehaviour
{

    SerialPort _stream;
    public GameObject[] _bubble;//泡泡粒子
    float _index = -0.5f;
    float time;
    bool _isbubble = false;
    bool _isMaxValue = false;
    public GameObject _SubEmitterDeath;//爆炸效果
    ParticleSystem.Particle[] arrPar;
    bool _isstart = true;
    bool _isend = true;
    int IO;
    void Start()
    {
        _stream = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One);//连接Arduion和unity
        if (!_stream.IsOpen)
        {
            _stream.Open();
        }
        StartCoroutine(StartBundle());
    }

    void Update()
    {
        if (_isstart == true)
        {
            CreatBubble();
        }
        if (_isend == true) 
        {
            BombBubble();
        }
    }
    IEnumerator StartBundle()//读取Arduion的值
    {
        while (true)
        {
            if (_stream.ReadLine() != null)
            {
                string value = _stream.ReadLine();
                IO = int.Parse(value);
                if (IO > 200 && IO < 1000)//数值达到200进入生成逻辑
                {
                    _isbubble = true;
                    BubbleSpeed();//生成泡泡过程中可改变其速度
                }
                else if (IO > 1000)//数值达到1000+进入爆炸逻辑
                {
                    _isMaxValue = true;
                }
                else if(IO < 200)//数值<200让剩下的都爆炸
                {
                    for (int i = 0; i < 8; i++)
                    {
                        ParticleSystem P_bubble = _bubble[i].GetComponent<ParticleSystem>();
                        arrPar = new ParticleSystem.Particle[P_bubble.main.maxParticles];
                        int arrCount = P_bubble.GetParticles(arrPar);
                        if (arrCount!=0)//遍历所有泡泡，如果没爆炸，则让它爆炸
                        {
                            _bubble[i].SetActive(false);
                            Instantiate(_SubEmitterDeath, arrPar[0].position, Quaternion.identity);
                        }
                    }
                    _isbubble = false;
                    _isMaxValue = false;
                    _isend = true;
                    _isstart = true;
                }
                Debug.Log(IO);
                yield return 0;
            }
        }
    }
    void BubbleSpeed()//泡泡速度的改变，随Arduion的值的变化而改变
    {
        for (int i = 0; i < 8; i++)
        {
            ParticleSystem P_bubble = _bubble[i].GetComponent<ParticleSystem>();
            ParticleSystem.VelocityOverLifetimeModule speed = P_bubble.velocityOverLifetime;
            speed.z = IO / 150;
        }
    }
    void CreatBubble()//泡泡的生成
    {
        if (_isbubble == true)
        {
            time += Time.deltaTime;//每0.5S出一个泡泡
            if (time - _index > 0)
            {
                _index = _index + 0.5f;
                _bubble[(int)_index].SetActive(true);
                _index = _index + 0.5f;

            }
            if (_index == 7.5)
            {
                _isbubble = false;
                time = 0;
                _index = -0.5f;
                _isstart = false;
            }
        }
    }
    void BombBubble()//泡泡的爆炸。
    {
        if (_isMaxValue == true)
        {
            for (int i = 0; i < 5; i++)  //随机5个爆炸
            {
                int index = Random.Range(0, 8);
                ParticleSystem P_bubble = _bubble[index].GetComponent<ParticleSystem>();
                arrPar = new ParticleSystem.Particle[P_bubble.main.maxParticles];
                _bubble[index].SetActive(false);
                Instantiate(_SubEmitterDeath, arrPar[0].position, Quaternion.identity);
            }
            _isMaxValue = false;
            _isend = false;
        }
    }
    void OnApplicationQuit()
    {
        _stream.Close();
    }

}
