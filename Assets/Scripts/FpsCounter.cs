using TMPro;
using UnityEngine;

public class FpsCounter : MonoBehaviour
{
    int _frames = 0;
    TextMeshProUGUI _fpsText;
    float _timer = 1f;
    private void Start()
    {
        _fpsText = GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        _frames++;
        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            _fpsText.text = _frames + "/" + Application.targetFrameRate;
            _timer = 1;
            _frames = 0;
        }
    }
}
