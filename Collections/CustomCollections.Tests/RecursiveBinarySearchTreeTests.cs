using System.Collections.Specialized;
using System.Numerics;

namespace CustomCollections.Tests
{
    public class RecursiveBinarySearchTreeTests
    {
        private RecursiveBinarySearchTree<BigInteger> _testInstance;
        private bool _isOnChangeEventTriggered;

        [SetUp]
        public void Setup()
        {
            _testInstance = new RecursiveBinarySearchTree<BigInteger>();
            _testInstance.CollectionChanged += (sender, eventArgs) =>
            {
                if (eventArgs.Action == NotifyCollectionChangedAction.Add)
                {
                    _isOnChangeEventTriggered = true;
                }
            };

            _testInstance.Insert(new BigInteger(8));
            _testInstance.Insert(new BigInteger(3));
        }

        [Test]
        public void TestShouldInsertLessValueToTheLeftSide()
        {
            // arrange
            BigInteger newValue = new BigInteger(1);

            // act
            _testInstance.Insert(newValue);

            // assert
            var parentNode = _testInstance.Find(new BigInteger(3));
            Assert.That(parentNode.Left.Element, Is.EqualTo(newValue));
        }

        [Test]
        public void TestShouldInsertGreaterValueToTheRightSide()
        {
            // arrange
            BigInteger newValue = new BigInteger(6);

            // act
            _testInstance.Insert(newValue);

            // assert
            var parentNode = _testInstance.Find(new BigInteger(3));
            Assert.That(parentNode.Right.Element, Is.EqualTo(newValue));
        }

        [Test]
        public void TestParentNodeShouldBeInRangeBetweenChildNodes()
        {
            // arrange
            BigInteger parentNodeValue = new BigInteger(10);
            BigInteger newValueLeft = new BigInteger(9);
            BigInteger newValueRight = new BigInteger(14);

            // act
            _testInstance.Insert(parentNodeValue);
            _testInstance.Insert(newValueLeft);
            _testInstance.Insert(newValueRight);

            // assert
            var parentNode = _testInstance.Find(parentNodeValue);
            Assert.That(parentNodeValue.CompareTo(parentNode.Left.Element) > 0
                          && parentNodeValue.CompareTo(parentNode.Right.Element) < 0, Is.True);
        }

        [Test]
        public void TestInsertedNodeShouldNotHaveChildNodes()
        {
            // arrange
            BigInteger newValue = new BigInteger(1);

            // act
            _testInstance.Insert(newValue);

            // assert
            var insertedNode = _testInstance.Find(newValue);
            Assert.That(insertedNode.Left, Is.Null);
            Assert.That(insertedNode.Right, Is.Null);
        }

        [Test]
        public void TestShouldThrowExceptionIfNullIsInserted()
        {
            // arrange
            RecursiveBinarySearchTree<string> stringTestInstance = new RecursiveBinarySearchTree<string>();
            // act, assert
            Assert.Throws<ArgumentNullException>(() => stringTestInstance.Insert(null!));
        }

        [Test]
        public void TestShouldReturnNodeIfValueIsFound()
        {
            // arrange
            BigInteger searchValue = new BigInteger(3);

            // act
            var foundNode = _testInstance.Find(searchValue);

            // assert
            Assert.That(foundNode, Is.Not.Null);
            Assert.That(foundNode.Element, Is.EqualTo(searchValue));
        }

        [Test]
        public void TestShouldReturnNullIfValueIsNotFound()
        {
            // arrange
            BigInteger searchValue = new BigInteger(100);

            // act
            var foundNode = _testInstance.Find(searchValue);

            // assert
            Assert.That(foundNode, Is.Null);
        }

        [Test]
        public void TestShouldReturnTrueIfTreeContainsValue()
        {
            // arrange
            BigInteger searchValue = new BigInteger(3);

            // act
            bool contains = _testInstance.Contains(searchValue);

            // assert
            Assert.That(contains, Is.True);
        }

        [Test]
        public void TestShouldReturnFalseIfTreeDoesNotContainValue()
        {
            // arrange
            BigInteger searchValue = new BigInteger(100);

            // act
            bool contains = _testInstance.Contains(searchValue);

            // assert
            Assert.That(contains, Is.False);
        }

        [Test]
        public void TestShouldReturnCorrectTreeSize()
        {
            // arrange
            int expectedSize = 2;
            BigInteger newValue = new BigInteger(1);
            _testInstance.Insert(newValue);

            // act
            int size = _testInstance.Size();

            // assert
            Assert.That(size, Is.EqualTo(expectedSize));
        }

        [Test]
        public void TestShouldReturnCorrectTreeDepth()
        {
            // arrange
            int expectedDepth = 1;

            // act
            int depth = _testInstance.Depth();

            // assert
            Assert.That(depth, Is.EqualTo(expectedDepth));
        }

        [Test]
        public void TestShouldCorrectlyTraversalInOrder()
        {
            // arrange
            _testInstance.Insert(new BigInteger(10));
            BigInteger[] expectedTraversal = { new(3), new(8), new(10) };
            int index = 0;

            // act and assert
            _testInstance.InOrderTraversal(element => Assert.That(element, Is.EqualTo(expectedTraversal[index++])));
        }

        [Test]
        public void TestShouldCorrectlyTraversalPreOrder()
        {
            // arrange
            _testInstance.Insert(new BigInteger(10));
            BigInteger[] expectedTraversal = { new(8), new(3), new(10) };
            int index = 0;

            // act and assert
            _testInstance.PreOrderTraversal(element => Assert.That(element, Is.EqualTo(expectedTraversal[index++])));
        }

        [Test]
        public void TestShouldCorrectlyTraversalPostOrder()
        {
            // arrange
            _testInstance.Insert(new BigInteger(10));
            BigInteger[] expectedTraversal = { new(3), new(10), new(8) };
            int index = 0;

            // act and assert
            _testInstance.PostOrderTraversal(element => Assert.That(element, Is.EqualTo(expectedTraversal[index++])));
        }

        [Test]
        public void TestShouldIterateThroughAllElements()
        {
            // arrange
            _testInstance.Insert(new BigInteger(10));
            BigInteger[] expectedElements = { new BigInteger(3), new BigInteger(8), new BigInteger(10) };
            int index = 0;

            // act and assert
            foreach (var element in _testInstance)
            {
                Assert.That(element, Is.EqualTo(expectedElements[index++]));
            }
        }

        [Test]
        public void TestShouldNotIterateIfTreeIsEmpty()
        {
            // arrange
            var emptyTree = new RecursiveBinarySearchTree<BigInteger>();

            // act and assert
            foreach (var _ in emptyTree)
            {
                Assert.Fail("Should not have any elements to iterate.");
            }
        }

        [Test]
        public void TestShouldTriggerEventWhenNewNodeIsInserted()
        {
            // arrange
            _isOnChangeEventTriggered = false;
            BigInteger newValue = new BigInteger(13);

            // act
            _testInstance.Insert(newValue);

            // assert
            Assert.That(_isOnChangeEventTriggered, Is.True);
        }
        
        [Test]
        public void TestShouldNotTriggerEventWhenDuplicateIsInserted()
        {
            // arrange
            _isOnChangeEventTriggered = false;
            BigInteger newValue = new BigInteger(3);

            // act
            _testInstance.Insert(newValue);

            // assert
            Assert.That(_isOnChangeEventTriggered, Is.False);
        }
        
    }
}