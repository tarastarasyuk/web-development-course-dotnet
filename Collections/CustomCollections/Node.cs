namespace CustomCollections;

public class Node<T>
{
    public T Element { get; set; }
    public Node<T>? Left { get; set; }
    public Node<T>? Right { get; set; }

    public Node(T element)
    {
        Element = element;
    }

    public override string ToString()
    {
        var leftElement = Left != null ? Left.Element?.ToString() : "null";
        var rightElement = Right != null ? Right.Element?.ToString() : "null";

        return $"Node: {Element}, Left: {leftElement}, Right: {rightElement}";
    }
}