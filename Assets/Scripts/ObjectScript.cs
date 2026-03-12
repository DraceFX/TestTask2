using UnityEngine;

public class ObjectScript : MonoBehaviour, ISelectable
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        SelectableRegistry.Register(this);
    }

    private void OnDestroy()
    {
        SelectableRegistry.Unregister(this);
    }

    public void ClickObject()
    {
        _animator.SetTrigger("Click");
        ActionManager.Instance.RegisterAction(this, SelectActioType.Click);
    }

    public void HoldObject()
    {
        _animator.SetTrigger("Hold");
        ActionManager.Instance.RegisterAction(this, SelectActioType.Hold);
    }
}
