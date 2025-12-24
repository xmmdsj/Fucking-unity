using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 移动参数
    public float moveSpeed = 8f;
    private Rigidbody rb;

    // 声波参数
    public GameObject soundWavePrefab;
    public float weakWaveScale = 6f;
    public float strongWaveScale = 12f;
    public float waveDuration = 0.5f;
    public float strongWaveCD = 3f;
    private float currentCD;
    private bool isStrongWaveReady = true;

    // 音效参数
    public AudioClip audio1;
    public AudioClip audio2;
    private AudioSource audioSource;

    // 距离检测参数
    public float nearDistanceThreshold = 4f;
    private GameObject[] allPathNodes;

    void Start()
    {
        // 初始化刚体
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Player缺少Rigidbody组件！");
            enabled = false;
            return;
        }

        // 初始化音频组件
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }

        // 初始化节点
        try
        {
            allPathNodes = GameObject.FindGameObjectsWithTag("PathNode");
        }
        catch (UnityException e)
        {
            Debug.LogWarning("未创建PathNode标签！" + e.Message);
            allPathNodes = new GameObject[0];
        }

        currentCD = 0;
    }

    // 物理移动（FixedUpdate更稳定）
    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontal, 0, vertical).normalized * moveSpeed;
        rb.velocity = movement;
    }

    void Update()
    {
        // 冷却计时
        if (!isStrongWaveReady)
        {
            currentCD += Time.deltaTime;
            if (currentCD >= strongWaveCD)
            {
                isStrongWaveReady = true;
                currentCD = 0;
            }
        }

        // 声波触发
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayAudioByDistance();
            CreateSoundWave(false);
        }
        if (Input.GetKey(KeyCode.Space) && isStrongWaveReady && !Input.GetKeyDown(KeyCode.Space))
        {
            PlayAudioByDistance();
            CreateSoundWave(true);
            isStrongWaveReady = false;
        }
    }

    // 生成声波
    void CreateSoundWave(bool isStrong)
    {
        if (soundWavePrefab == null) return;
        GameObject wave = Instantiate(soundWavePrefab, transform.position, Quaternion.identity);
        SoundWave3D waveScript = wave.GetComponent<SoundWave3D>();
        waveScript?.Init(isStrong ? strongWaveScale : weakWaveScale, isStrong);
        Destroy(wave, waveDuration);
    }

    // 按距离播放音效1/2
    void PlayAudioByDistance()
    {
        if (audioSource == null || (audio1 == null && audio2 == null) || allPathNodes.Length == 0) return;

        float closestDistance = Mathf.Infinity;
        foreach (GameObject node in allPathNodes)
        {
            float distance = Vector3.Distance(transform.position, node.transform.position);
            if (distance < closestDistance) closestDistance = distance;
        }

        audioSource.clip = closestDistance <= nearDistanceThreshold ? audio2 : audio1;
        audioSource.Play();
    }
}