// See https://aka.ms/new-console-template for more information

using System.Collections.Specialized;
using CustomCollections;

// Define variable
IBinarySearchTree<int> tree = new RecursiveBinarySearchTree<int>();

// Configure events
tree.CollectionChanged += (sender, eventArgs) =>
{
    if (eventArgs.Action == NotifyCollectionChangedAction.Add)
    {
        Console.WriteLine($"Item added: {eventArgs.NewItems?[0]}");
    }
};

// Insert
tree.Insert(8);
tree.Insert(3);
tree.Insert(10);
tree.Insert(1);
tree.Insert(6);
tree.Insert(14);
tree.Insert(9);

// Find
var foundNode1 = tree.Find(14);
Console.WriteLine($"Found node -  {foundNode1}");

// Contains
var element1 = 100;
var contains1 = tree.Contains(100);
Console.WriteLine($"Tree contains {element1}: {contains1}");

var element2 = 10;
var contains2 = tree.Contains(10);
Console.WriteLine($"Tree contains {element2}: {contains2}");

// Size
var treeSize = tree.Size();
Console.WriteLine($"Tree size: {treeSize}");

// Tree depth
var treeDepth = tree.Depth();
Console.WriteLine($"Tree depth: {treeDepth}");

// InOrderTraversal
Console.WriteLine("InOrderTraversal:");
tree.InOrderTraversal(val => Console.Write(val + " "));
Console.WriteLine();

// PreOrderTraversal
Console.WriteLine("PreOrderTraversal:");
tree.PreOrderTraversal(val => Console.Write(val + " "));
Console.WriteLine();

// PostOrderTraversal
Console.WriteLine("PostOrderTraversal:");
tree.PostOrderTraversal(val => Console.Write(val + " "));
Console.WriteLine();

// Enumerator
Console.WriteLine("Using foreach loop to test enumerator:");
foreach (var element in tree)
{
    Console.Write(element + " ");
}

Console.WriteLine("Using the enumerator directly:");
var enumerator = tree.GetEnumerator();
while (enumerator.MoveNext())
{
    var element = enumerator.Current;
    Console.WriteLine(element);
}

// Exception handling
IBinarySearchTree<string?> stringTree = new RecursiveBinarySearchTree<string?>();
stringTree.Insert("R");
stringTree.Insert("20twenty");
stringTree.Insert("4");
stringTree.PostOrderTraversal(val => Console.Write(val + " "));
stringTree.Insert(null);