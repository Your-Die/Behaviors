namespace Chinchillada.Behavior
{
    using System.Collections.Generic;

    /// <summary>
    /// Composite behaviors have children they manage.
    /// </summary>
    public interface IComposite<T>
    {
        List<T> Children { get; }
    }
}