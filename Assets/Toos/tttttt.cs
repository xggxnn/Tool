using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class tttttt : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent nav;
    // Start is called before the first frame update
    void Start()
    {
        nav = this.gameObject.GetComponent<NavMeshAgent>();
        if (target != null)
        {
            nav.destination = target.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            //从摄像机到单击处发出射线
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                nav.destination = hitInfo.point;
                //画出射线, 只有在Scene视图中才能看到
                Debug.DrawLine(ray.origin, hitInfo.point);
                GameObject gameObj = hitInfo.collider.gameObject;
                Debug.Log("click object name is" + gameObj.name);
                //当射线碰撞目标的标签是Pickup时, 执行拾取操作
                if (gameObj.tag == "Pickup")
                {
                    Debug.Log("pick up!");
                }
            }
        }
    }
}
