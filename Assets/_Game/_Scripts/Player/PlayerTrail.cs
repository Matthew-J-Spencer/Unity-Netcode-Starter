using System.Collections;
using UnityEngine;

/// <summary>
/// This should be a shader, but lol
/// </summary>
public class PlayerTrail : MonoBehaviour {
    [SerializeField] private Renderer _spotPrefab;
    [SerializeField] private float _spotDistance = 0.5f;
    [SerializeField] private float _spotFadeTime = 1;
    [SerializeField] private float _groundOffset = 0.01f;

    private Vector3 _lastSpotLaid;
    private readonly Color _goalColor = new(1, 1, 1, 0);

    private void Start() {
        LaySpot();
    }

    private void Update() {
        if (Vector3.Distance(_lastSpotLaid, transform.position) >= _spotDistance) LaySpot();
    }

    private void LaySpot() {
        _lastSpotLaid = transform.position;
        var spot = Instantiate(_spotPrefab, _lastSpotLaid + Vector3.up * _groundOffset, Quaternion.Euler(90, 0, 0));

        StartCoroutine(FadeAndDestroy(spot));
    }

    private IEnumerator FadeAndDestroy(Renderer spot) {
        var material = spot.material;
        var s = material.color;
        for (var t = 0f; t < _spotFadeTime; t += Time.deltaTime) {
            material.color = Color.Lerp(s, _goalColor, t / _spotFadeTime);
            yield return null;
        }

        Destroy(spot.gameObject);
    }
}