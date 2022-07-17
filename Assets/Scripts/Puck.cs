using UnityEngine;

public class Puck : MonoBehaviour
{
    #region Constantes
    
    private const float MinSpeed = 5f;

    private static readonly Vector3 BaseRotationSpeed = new Vector3(0f, 25f, 0f);

    #endregion
    
    #region Fields

    [SerializeField] private Sprite _icon;
    [SerializeField] private float _speed;
    [SerializeField] private int _damage;

    private Rigidbody _rigidbody;
    private Renderer _renderer;

    private float _currentSpeed;
    private Vector3 _lastVelocity;
    
    private static int _obstacleLayerMask;

    #endregion

    #region Properties

    public Renderer Renderer => _renderer;

    public Sprite Icon => _icon;

    #endregion

    #region Public Methods

    public void Init(Vector3 direction)
    {
      _rigidbody.AddForce(direction * _speed, ForceMode.Impulse);
    }

    #endregion

    #region Unity Event Functions

    protected void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _renderer = GetComponentInChildren<Renderer>();
        
        _obstacleLayerMask = LayerMask.NameToLayer("Obstacle");
    }

    protected void FixedUpdate()
    {
        var velocity = _rigidbody.velocity;
        
        velocity *= 0.99f;
        _rigidbody.velocity = velocity;

        _lastVelocity = velocity;
        _currentSpeed = _lastVelocity.magnitude;

        if (_currentSpeed >= MinSpeed)
        {
            transform.Rotate(BaseRotationSpeed * (_currentSpeed * Time.fixedDeltaTime));
        }
        else if (_renderer.enabled && Game.Instance.State == GameState.PuckThrown)
        {
            Game.Instance.TryDefeat();
        }
    }

    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == _obstacleLayerMask)
        {
            var direction = Vector3.Reflect(_lastVelocity.normalized, collision.contacts[0].normal);
            _rigidbody.velocity = direction * _currentSpeed; 
            
            if (collision.gameObject.TryGetComponent<Obstacle>(out var obstacle))
            {
                obstacle.TakeDamage(_damage);

                if (!(obstacle is Chest) && !obstacle.IsAlive)
                {
                    // Go through
                    _rigidbody.velocity = _lastVelocity.normalized * _currentSpeed;
                }
            }
        }
    }

    #endregion
}
