using UnityEngine;

public class RigidbodyStablePoint : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private Transform _targetPoint;
    [SerializeField] private float _springStrength;
    [SerializeField] private float _springDumper;

    private void FixedUpdate()
    {
        var delta = (Vector2)_targetPoint.position - _rigidbody2D.position;

        if (delta.sqrMagnitude > 0)
        {
            var deltaVelocity = Vector3.Dot(delta.normalized, _rigidbody2D.linearVelocity);

            float springForce = delta.magnitude * _springStrength - deltaVelocity * _springDumper;
            _rigidbody2D.AddForce(delta.normalized * springForce);
        }
    }
}
