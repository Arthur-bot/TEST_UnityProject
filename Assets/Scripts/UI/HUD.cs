using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    #region Fields

    [SerializeField] private UIPuckButton _puckButtonTemplate;
    [SerializeField] private TextMeshProUGUI _puckLeftText;

    private readonly List<UIPuckButton> _puckButtons = new List<UIPuckButton>();

    #endregion

    #region Public Methods

    public void InitializePuckButtons(PuckSpawner spawner, List<Puck> pucks)
    {
        var puckButtonTemplateParent = _puckButtonTemplate.transform.parent;
        _puckButtonTemplate.gameObject.SetActive(false);
        
        foreach (var puck in pucks)
        {
            var puckButton = Instantiate(_puckButtonTemplate, puckButtonTemplateParent, false);
            puckButton.Initialize(spawner, puck);
            puckButton.gameObject.SetActive(true);
            _puckButtons.Add(puckButton);
        }
    }

    public void UpdatePuckText(int puckLeft) => _puckLeftText.text = $"{puckLeft} DISCS LEFT";

    #endregion
}
