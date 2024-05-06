using UnityEngine;

[RequireComponent(typeof(Showable))]
public class InMenu : MonoBehaviour
{
    public Menu parent;

    private void Start()
    {
        parent.AdditionalChildren.Add(GetComponent<Showable>());
    }
}