using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.AnimatedValues;
using UnityEditor;
using UnityEngine.UI;

public class UIParticleSystem : MonoBehaviour
{
    Coroutine spawning;
    List<GameObject> particles;
    ObjectPoolerList pooler;
    [SerializeField]
    RectTransform boundaryRect;
    float boundaryX;
    float boundaryY;
    Vector2 position;

    [HideInInspector]
    public bool isSpawning;

    //[Header("Amount")]
    //[SerializeField]
    //float amount;

    [Header("SpawnDelay")]
    [SerializeField]
    float delay;

    [Header ("Lifetime")]
    [SerializeField]
    float startLifetime;
    [SerializeField]
    bool startLifetimeRandom;
    [SerializeField]
    Vector2 startLifetimeRange;
    [HideInInspector]
    public float _startLifetime;

    [Header("Speed")]
    [SerializeField]
    float startSpeed;
    [SerializeField]
    bool startSpeedRandom;
    [SerializeField]
    Vector2 startSpeedRange;
    [HideInInspector]
    public float _startSpeed;

    [Header("Direction")]
    [SerializeField]
    Vector3 startDirection;
    [SerializeField]
    bool startDirectionRandom;
    [SerializeField]
    Vector3[] startDirectionRange;
    [HideInInspector]
    public Vector3 _startDirection;

    [Header("Size")]
    [SerializeField]
    bool sizeOverTime;
    [SerializeField]
    float startSize;
    [SerializeField]
    float endSizePercentage;
    [SerializeField]
    bool startSizeRandom;
    [SerializeField]
    Vector2 startSizeRange;
    [SerializeField]
    Vector2 endSizeRange;
    [HideInInspector]
    public float _startSize;
    [HideInInspector]
    public float _endSize;

    [Header("Color")]
    [SerializeField]
    bool fadeIn;
    [SerializeField]
    Color fadeInColor;
    [SerializeField]
    float fadeInSpeed;
    [SerializeField]
    Color startColor;
    [SerializeField]
    Color endColor;
    [HideInInspector]
    public Color _startColor;
    [HideInInspector]
    public Color _endColor;
    [HideInInspector]
    public float _colorChangeSpeed;
    [HideInInspector]
    public Color _fadeInColor;
    [HideInInspector]
    public float _fadeInSpeed;

    [Header("Wiggle")]
    [SerializeField]
    bool isWiggleX;
    [SerializeField]
    bool isWiggleY;
    [SerializeField]
    float wiggleX;
    [SerializeField]
    float wiggleXSpeed;
    [SerializeField]
    float wiggleY;
    [SerializeField]
    float wiggleYSpeed;
    [HideInInspector]
    public float _wiggleX;
    [HideInInspector]
    public float _wiggleY;
    [HideInInspector]
    public float _wiggleXSpeed;
    [HideInInspector]
    public float _wiggleYSpeed;


    [Header("Gravity")]
    [SerializeField]
    float gravity;
    [HideInInspector]
    public float _gravity;

    private void Awake()
    {
        particles = new List<GameObject>();
        pooler = new ObjectPoolerList();
    }

    private void OnEnable()
    {
        _startLifetime = startLifetime;
        _startSpeed = startSpeed;
        _startDirection = startDirection;
        _startSize = startSize;
        _endSize = startSize;
        _startColor = startColor;
        _endColor = endColor;
        _fadeInColor = _startColor;
        _fadeInSpeed = fadeInSpeed;
        _wiggleX = 0f;
        _wiggleY = 0f;
        _wiggleXSpeed = 0f;
        _wiggleYSpeed = 0f;
        _gravity = gravity;
        isSpawning = true;

        spawning = StartCoroutine(SpawnParticles());
    }

    private void OnDisable()
    {
        if(spawning != null)
        {
            StopCoroutine(spawning);
        }
    }
    private void Start()
    {
        int i = 0;
        foreach(Transform ch in GetComponentInChildren<Transform>())
        {
            particles.Add(ch.gameObject);
            i++;
        }
    }
    void SetParticleParameters()
    {
        //Position
        boundaryX = Random.Range(boundaryRect.rect.xMin, boundaryRect.rect.xMax);
        boundaryY = Random.Range(boundaryRect.rect.yMin, boundaryRect.rect.yMax);
        //boundaryY = boundaryRect.rect.yMin;
        position = boundaryRect.TransformPoint(new Vector3(boundaryX, boundaryY, 0f));

        //Lifetime
        if (startLifetimeRandom)
        {
            _startLifetime = Random.Range(startLifetimeRange.x, startLifetimeRange.y);
        }

        ///Speed
        if (startSpeedRandom)
        {
            _startSpeed = Random.Range(startSpeedRange.x, startSpeedRange.y);
        }

        ///Direction
        if (startDirectionRandom)
        {
            float tempDirX = Random.Range(startDirectionRange[0].x, startDirectionRange[1].x);
            float tempDirY = Random.Range(startDirectionRange[0].y, startDirectionRange[1].y);
            _startDirection = new Vector3(tempDirX, tempDirY, 0f);
        }

        ///Size
        if (startSizeRandom)
        {
            _startSize = Random.Range(startSizeRange.x, startSizeRange.y);
            _endSize = _startSize;
        }

        if (sizeOverTime)
        {
            _endSize = endSizePercentage * _startSize;
        }

        //Wiggle
        if (isWiggleX)
        {
            _wiggleX = wiggleX;
            _wiggleXSpeed = wiggleXSpeed;
        }
        if (isWiggleY)
        {
            _wiggleY = wiggleY;
            _wiggleYSpeed = wiggleYSpeed;
        }

        if (fadeIn)
        {
            _fadeInColor = fadeInColor;
        }
    }

    IEnumerator SpawnParticles()
    {
        yield return new WaitForSeconds(.02f);
        while(true)
        {
            while(isSpawning)
            {
                SetParticleParameters();
                pooler.PoolObjects(particles, position, Quaternion.identity, Vector3.zero);
                yield return new WaitForSeconds(delay);
            }
            yield return null;
        }
    }

    public void StartSpawning()
    {
        isSpawning = true;
    }
    public void StopSpawning()
    {
        isSpawning = false;
    }
}
