using UnityEngine;

public class FogDensityControl : MonoBehaviour
{
    [Header("基础参数")]
    public float fogRadius = 5f; // 雾消散半径（与Collider Radius一致）
    public float globalFogDensity = 0.02f; // 全局雾密度（与光照设置一致）
    public float nodeFogDensity = 0f; // 节点中心雾密度（0=完全透明）
    public float transitionSpeed = 3f; // 雾密度过渡速度

    [Header("引用设置")]
    public Transform playerTransform; // 手动拖入Player对象（避免单例错误）
    private bool isPlayerInNode = false;

    void Start()
    {
        // 初始化全局雾密度
        RenderSettings.fogDensity = globalFogDensity;
        // 若未手动赋值Player，自动搜索
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
            else
            {
                Debug.LogError("未找到Player对象！请给Player添加「Player」标签，或手动拖入脚本的playerTransform字段");
            }
        }
    }

    void Update()
    {
        if (playerTransform == null) return;

        // 计算玩家到节点的距离
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        // 根据距离插值调整雾密度（仅玩家在范围内时生效）
        if (distanceToPlayer <= fogRadius || isPlayerInNode)
        {
            float targetDensity = Mathf.Lerp(nodeFogDensity, globalFogDensity, distanceToPlayer / fogRadius);
            RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, targetDensity, Time.deltaTime * transitionSpeed);
        }
        else
        {
            // 玩家远离后，恢复全局雾密度
            RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, globalFogDensity, Time.deltaTime * transitionSpeed);
        }
    }

    // 玩家进入触发区，标记为在节点内
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInNode = true;
        }
    }

    // 玩家离开触发区，取消标记
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInNode = false;
        }
    }

    // Gizmos可视化雾范围（Scene窗口中显示）
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, fogRadius);
    }
}