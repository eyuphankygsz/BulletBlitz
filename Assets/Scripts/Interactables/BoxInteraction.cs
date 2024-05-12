using UnityEngine;

public class BoxInteraction : Interactable
{
    [SerializeField] private bool _isHolding;
    private Rigidbody2D _rb;
    private Animator _animator;
    private SpriteRenderer _renderer;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _renderer = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
    }
    public override void OnEnabled()
    {
        if (!_isHolding)
            Hold();
        else
            LetGo();

        _isHolding = !_isHolding;
    }
    private void Hold()
    {
        _renderer.sortingOrder += 2;
        _rb.constraints = RigidbodyConstraints2D.FreezeAll;
        GameManager.Instance.Player.GetComponent<PlayerController>().HoldObject(this);
    }
    private void LetGo()
    {
        _renderer.sortingOrder -= 2;
        _rb.constraints = RigidbodyConstraints2D.None;
        GameManager.Instance.Player.GetComponent<PlayerController>().HoldObject(null);
    }
    public override bool TriggerEnter(out Interactable interactable)
    {
        interactable = this;
        return false;
    }

    public override void TriggerExit() { }
}
