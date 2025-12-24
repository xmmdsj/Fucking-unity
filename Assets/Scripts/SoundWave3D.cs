using UnityEngine;

public class SoundWave3D : MonoBehaviour
{
    private float targetScale;
    private float startScale = 0.1f;
    private float duration = 0.5f;
    private float timer;
    private MeshRenderer mr;

    // 改用Awake提前初始化，Awake比Start执行更早
    void Awake()
    {
        mr = GetComponent<MeshRenderer>();
        // 若仍无MeshRenderer，给出提示而非直接报错
        if (mr == null)
        {
            Debug.LogError("当前游戏对象缺少MeshRenderer组件！", this);
        }
    }

    public void Init(float target, bool isStrong)
    {
        // 先判断mr是否为空，避免空引用
        if (mr == null) return;

        targetScale = target;
        mr.material.color = isStrong ? new Color(0, 0, 1, 0.5f) : new Color(0, 1, 1, 0.5f);
    }

    void Update()
    {
        if (mr == null) return; // 同样加空值校验
        timer += Time.deltaTime;
        float progress = timer / duration;
        transform.localScale = Vector3.one * Mathf.Lerp(startScale, targetScale, progress);
    }
}