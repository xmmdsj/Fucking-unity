using UnityEngine;

public class NodeTrigger3D : MonoBehaviour
{
    public AudioClip audio3;
    private AudioSource audioSource;
    public bool isActivated = false; // 供NodeFogControl判断
    private MeshRenderer mr;

    void Start()
    {
        mr = GetComponent<MeshRenderer>();
        // 初始透明（仅Alpha=0，颜色保留暖黄色）
        Color initialColor = mr.material.color;
        initialColor.a = 0;
        mr.material.color = initialColor;

        // 初始化音频组件
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        // 给节点添加碰撞体（适配修正后节点尺寸）
        SphereCollider collider = GetComponent<SphereCollider>();
        if (collider == null)
            collider = gameObject.AddComponent<SphereCollider>();
        collider.isTrigger = true;
        collider.radius = 0.8f; // 扩大碰撞范围，适配Player尺寸（0.7）
    }

    // 声波触发节点激活
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SoundWave") && !isActivated)
        {
            ActivateNode();
        }
    }

    // 点亮节点+播放音效3
    void ActivateNode()
    {
        isActivated = true;
        Color activeColor = mr.material.color;
        activeColor.a = 1;
        mr.material.color = activeColor;

        if (audio3 != null)
            audioSource.PlayOneShot(audio3);
    }
}
