using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private UpdateType updateType;

    private void Update()
    {
        if (updateType == UpdateType.Update)
        {
            SetPos();
        }
    }

    private void FixedUpdate()
    {
        if(updateType == UpdateType.FixedUpdate)
        {
            SetPos();
        }
    }

    private void SetPos()
    {
        transform.position = new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z);
    }
}
