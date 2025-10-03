using DG.Tweening;
using UnityEngine;


public class PlayerMove : MonoBehaviour, IUpdate, IFixedUpdate
{
    [SerializeField] private Entity entity;
    [SerializeField] private Animator animator;
    [SerializeField] private float speed = 50;
    private IUpdater updater;
    private IPlayerStateManager stateManager;

    private Tween angleTween;
    private Vector2 dir;

	private void Start()
	{
        updater = Locator.Resolve<IUpdater>(UpdaterID.ENTITY);
        stateManager = Locator.Resolve<IPlayerStateManager>();

        updater.AddUpdate(this);
        updater.AddFixedUpdate(this);
    }

    void IUpdate.OnUpdate()
    {
        dir = Vector2.zero;
        if (Input.GetKey(KeyCode.W))
        {
            dir += Vector2.up;
        }
        if (Input.GetKey(KeyCode.A))
        {
            dir += Vector2.left;
        }
        if (Input.GetKey(KeyCode.S))
        {
            dir += Vector2.down;
        }
        if (Input.GetKey(KeyCode.D))
        {
            dir += Vector2.right;
        }

        if (dir.magnitude == 0)
        {
            stateManager.EndState(PlayerStateType.Run);
            StopAngle();
            StopMoveAnimation();
            return;
        }
        dir = dir.normalized;
    }

    void IFixedUpdate.OnFixedUpdate()
    {
        SetMoveAnimation();
        Move(dir);
        SetAngle(dir);
    }

    private void Move(Vector2 dir)
    {
        stateManager.StartState(PlayerStateType.Run);
        entity.Move(dir, speed);
    }

    private void SetAngle(Vector2 dir)
    {
        if (stateManager.ContainsState(PlayerStateType.Drill))
        {
            StopAngle();
            return;
        }
        float angleZ = dir.GetAim();
        angleTween?.Kill();
        angleTween = transform.DORotate(new Vector3(0, 0, angleZ), 0.1f, RotateMode.Fast);
    }

    private void StopAngle()
    {
        angleTween?.Kill();
    }

    private void SetMoveAnimation()
    {
        if (stateManager.ContainsState(PlayerStateType.Drill))
        {
            StopMoveAnimation();
            return;
        }
        if (!animator.GetBool("IsMove"))
        {
            animator.SetBool("IsMove", true);
        }
    }

    private void StopMoveAnimation()
    {
        if (animator.GetBool("IsMove"))
        {
            animator.SetBool("IsMove", false);
        }
    }

    private void OnDestroy()
    {
        StopAngle();
        updater.RemoveUpdate(this);
        updater.RemoveFixedUpdate(this);
    }
}
