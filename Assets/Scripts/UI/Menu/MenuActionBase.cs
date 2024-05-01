using UnityEngine;

public abstract class MenuActionBase : MonoBehaviour
{
    protected MenuItem menuItem;

    public virtual void Start()
    {
        menuItem = GetComponent<MenuItem>();
    }

    public abstract void Trigger(Menu sourceMenu);
    public virtual void Select(Menu sourceMenu){}
    public virtual void Deselect(Menu sourceMenu){}
}