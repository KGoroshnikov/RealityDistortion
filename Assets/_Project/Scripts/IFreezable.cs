using UnityEngine;

public interface IFreezable // заморозить объекты / мобов (например при паузе)
{
    public void Freeze();
    public void UnFreeze();
}
