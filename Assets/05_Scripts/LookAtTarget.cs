using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            // 计算目标方向
            Vector3 direction = target.position - transform.position;
            
            // 仅改变y分量
            direction.y = 0;

            if (direction != Vector3.zero)
            {
                // 计算新的旋转
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                
                // 将x分量限制为-90度
                targetRotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, targetRotation.eulerAngles.z);
                
                // 应用旋转
                transform.rotation = targetRotation;
            }
        }
    }
}
