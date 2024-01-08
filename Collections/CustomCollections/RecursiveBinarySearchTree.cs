using System.Collections;
using System.Collections.Specialized;

namespace CustomCollections;

public class RecursiveBinarySearchTree<T> : IBinarySearchTree<T> where T : IComparable<T>
{
    private Node<T>? _root;
    private int _size;

    #region Common methods implementation

    public bool Insert(T element)
    {
        if (element == null)
        {
            throw new ArgumentNullException();
        }
        
        if (_root == null)
        {
            _root = new Node<T>(element);
            return true;
        }

        var isInserted = InsertRecursive(_root, element);
        if (isInserted)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, element));
        }

        return isInserted;
    }

    private bool InsertRecursive(Node<T> currentNode, T element)
    {
        if (element.CompareTo(currentNode.Element) < 0)
        {
            if (currentNode.Left == null)
            {
                currentNode.Left = new Node<T>(element);
                _size++;
                return true;
            }

            return InsertRecursive(currentNode.Left, element);
        }

        if (element.CompareTo(currentNode.Element) > 0)
        {
            if (currentNode.Right == null)
            {
                currentNode.Right = new Node<T>(element);
                _size++;
                return true;
            }

            return InsertRecursive(currentNode.Right, element);
        }

        return false;
    }

    public bool Contains(T element)
    {
        return Find(element) != null;
    }

    public Node<T> Find(T element)
    {
        return FindRecursive(_root, element);
    }

    private Node<T> FindRecursive(Node<T>? currentNode, T element)
    {
        if (currentNode == null)
        {
            return null!;
        }

        if (element.CompareTo(currentNode.Element) < 0)
        {
            return FindRecursive(currentNode.Left, element);
        }

        if (element.CompareTo(currentNode.Element) > 0)
        {
            return FindRecursive(currentNode.Right, element);
        }

        return currentNode;
    }


    public int Size()
    {
        return _size;
    }

    public int Depth()
    {
        return _root != null ? DepthRecursive(_root) - 1 : 0;
    }

    private int DepthRecursive(Node<T>? currentNode)
    {
        if (currentNode == null)
        {
            return 0;
        }

        return 1 + Math.Max(DepthRecursive(currentNode.Left), DepthRecursive(currentNode.Right));
    }


    public void InOrderTraversal(Action<T> action)
    {
        InOrderTraversalRecursive(_root, action);
    }

    private void InOrderTraversalRecursive(Node<T>? currentNode, Action<T> action)
    {
        if (currentNode != null)
        {
            InOrderTraversalRecursive(currentNode.Left, action);
            performAction(currentNode, action);
            InOrderTraversalRecursive(currentNode.Right, action);
        }
    }


    public void PreOrderTraversal(Action<T> action)
    {
        PreOrderTraversalRecursive(_root, action);
    }

    private void PreOrderTraversalRecursive(Node<T>? currentNode, Action<T> action)
    {
        if (currentNode != null)
        {
            performAction(currentNode, action);
            InOrderTraversalRecursive(currentNode.Left, action);
            InOrderTraversalRecursive(currentNode.Right, action);
        }
    }

    public void PostOrderTraversal(Action<T> action)
    {
        PostOrderTraversalRecursive(_root, action);
    }

    private void PostOrderTraversalRecursive(Node<T>? currentNode, Action<T> action)
    {
        if (currentNode != null)
        {
            InOrderTraversalRecursive(currentNode.Left, action);
            InOrderTraversalRecursive(currentNode.Right, action);
            performAction(currentNode, action);
        }
    }

    private static void performAction(Node<T> currentNode, Action<T> action)
    {
        try
        {
            action(currentNode.Element);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"An exception occurred during traversal action performing: {ex.Message}", ex);
        }
    }

    #endregion

    #region Events handling

    public event NotifyCollectionChangedEventHandler? CollectionChanged;

    private void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
        CollectionChanged?.Invoke(this, e);
    }

    #endregion

    #region Enumerator implementation

    public IEnumerator<T> GetEnumerator() => new Enumerator(this);

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private struct Enumerator : IEnumerator<T>
    {
        private readonly RecursiveBinarySearchTree<T> _tree;
        private Node<T>? _currentNode;
        private readonly Stack<Node<T>> _traversalStack;

        public T Current => _currentNode.Element;

        object IEnumerator.Current => Current;

        public Enumerator(RecursiveBinarySearchTree<T> tree)
        {
            _tree = tree;
            _currentNode = null;
            _traversalStack = new Stack<Node<T>>();

            if (_tree._root != null)
            {
                AddLeftBranchToStack(_tree._root);
            }
        }

        public bool MoveNext()
        {
            if (_traversalStack.Count == 0)
            {
                return false;
            }

            _currentNode = _traversalStack.Pop();

            if (_currentNode.Right != null)
            {
                AddLeftBranchToStack(_currentNode.Right);
            }

            return true;
        }

        public void Reset()
        {
            _traversalStack.Clear();
            if (_tree._root != null)
            {
                AddLeftBranchToStack(_tree._root);
            }
        }

        public void Dispose()
        {
        }

        private void AddLeftBranchToStack(Node<T> node)
        {
            while (node != null)
            {
                _traversalStack.Push(node);
                node = node.Left;
            }
        }
    }

    #endregion
}