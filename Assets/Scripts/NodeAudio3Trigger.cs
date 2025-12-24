using UnityEngine;

/// <summary>
/// 节点触发Audio3音效脚本
/// 不修改雾效，仅在Player进入节点时播放音效
/// </summary>
public class NodeAudio3Trigger : MonoBehaviour
{
    [Header("Audio3音效设置")]
    public AudioClip audio3Clip; // 拖入你的Audio3音效文件
    [Range(0, 1)] public float audioVolume = 1f; // 音效音量
    public bool playOnlyOnce = true; // 是否只播放一次

    private AudioSource audioSource;
    private bool hasPlayed = false; // 防止重复播放标记

    void Start()
    {
        // 自动添加AudioSource组件，无需手动挂载
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // 配置音效参数（关键：禁用自动播放）
        audioSource.clip = audio3Clip;
        audioSource.volume = audioVolume;
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f; // 3D音效，贴合场景空间感
    }

    /// <summary>
    /// 利用节点已有的Trigger检测玩家进入
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        // 只响应Player的触发
        if (other.CompareTag("Player") && audio3Clip != null)
        {
            // 判断是否需要重复播放
            if (playOnlyOnce && !hasPlayed)
            {
                audioSource.Play();
                hasPlayed = true;
            }
            else if (!playOnlyOnce)
            {
                audioSource.Play();
            }
        }
    }
}