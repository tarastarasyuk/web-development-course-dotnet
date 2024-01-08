using System.Collections.Specialized;

namespace CustomCollections

{
    public interface IBinarySearchTree<T> : IEnumerable<T>, INotifyCollectionChanged where T : IComparable<T>
    {
        /// <summary>
        /// Inserts an element.
        /// </summary>
        /// <returns>True if the element did not exist in the tree and was inserted successfully.</returns>
        bool Insert(T element);

        /// <summary>
        /// Find the node that holds specified element.
        /// </summary>
        /// <returns>The node that holds specified element, otherwise - null</returns>
        Node<T>? Find(T element);

        /// <summary>
        /// Checks if the tree contains the specified element.
        /// </summary>
        /// <returns>True if the tree contains the element.</returns>
        bool Contains(T element);

        /// <summary>
        /// Gets the number of elements in the tree.
        /// </summary>
        /// <returns>The number of elements in the tree.</returns>
        int Size();

        /// <summary>
        /// Gets the maximum number of transitions between the root node and any other node.
        /// </summary>
        /// <returns>The maximum number of transitions; 0 if the tree is empty or contains 1 element.</returns>
        int Depth();

        /// <summary>
        /// Traverses the tree in the element's natural order, from left to right
        /// </summary>
        /// <param name="action">An action to be performed on each node during traversal.</param>
        void InOrderTraversal(Action<T> action);

        /// <summary>
        /// Traverses the tree from the root, then left, then right recursively.
        /// </summary>
        /// <param name="action">An action to be performed on each node during traversal.</param>
        void PreOrderTraversal(Action<T> action);

        /// <summary>
        /// Traverses the tree from the left, then right, then root recursively.
        /// </summary>
        /// <param name="action">An action to be performed on each node during traversal.</param>
        void PostOrderTraversal(Action<T> action);

        /// <summary>
        /// Returns enumerator.
        /// </summary>
        IEnumerator<T> GetEnumerator();
    }
}