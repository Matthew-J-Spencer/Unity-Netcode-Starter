using UnityEngine;

public class Projectile : MonoBehaviour {
    [SerializeField] private AudioClip _destroyClip;
    [SerializeField] private GameObject _particles;

    private Vector3 _dir;

    public void Init(Vector3 dir) {
        GetComponent<Rigidbody>().AddForce(dir);
        Invoke(nameof(DestroyBall), 3);
    }

    private void DestroyBall() {
        AudioSource.PlayClipAtPoint(_destroyClip, transform.position);
        Instantiate(_particles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}