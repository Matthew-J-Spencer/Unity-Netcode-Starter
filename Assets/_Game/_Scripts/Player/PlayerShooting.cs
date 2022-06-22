using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerShooting : NetworkBehaviour {
    [SerializeField] private Projectile _projectile;
    [SerializeField] private AudioClip _spawnClip;
    [SerializeField] private float _projectileSpeed = 700;
    [SerializeField] private float _cooldown = 0.5f;
    [SerializeField] private Transform _spawner;

    private float _lastFired = float.MinValue;
    private bool _fired;

    private void Update() {
        if (!IsOwner) return;

        if (Input.GetMouseButton(0) && _lastFired + _cooldown < Time.time) {
            _lastFired = Time.time;
            var dir = transform.forward;

            // Send off the request to be executed on all clients
            RequestFireServerRpc(dir);

            // Fire locally immediately
            ExecuteShoot(dir);
            StartCoroutine(ToggleLagIndicator());
        }
    }

    [ServerRpc]
    private void RequestFireServerRpc(Vector3 dir) {
        FireClientRpc(dir);
    }

    [ClientRpc]
    private void FireClientRpc(Vector3 dir) {
        if (!IsOwner) ExecuteShoot(dir);
    }

    private void ExecuteShoot(Vector3 dir) {
        var projectile = Instantiate(_projectile, _spawner.position, Quaternion.identity);
        projectile.Init(dir * _projectileSpeed);
        AudioSource.PlayClipAtPoint(_spawnClip, transform.position);
    }

    private void OnGUI() {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
        if (_fired) GUILayout.Label("FIRED LOCALLY");
        
        GUILayout.EndArea();
    }

    /// <summary>
    /// If you want to test lag locally, go into the "NetworkButtons" script and uncomment the artificial lag
    /// </summary>
    /// <returns></returns>
    private IEnumerator ToggleLagIndicator() {
        _fired = true;
        yield return new WaitForSeconds(0.2f);
        _fired = false;
    }
}