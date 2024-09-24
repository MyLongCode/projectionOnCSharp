using System.Collections;

namespace Generics.BinaryTrees;

public class BinaryTree<T> : IEnumerable<T>
    where T : IComparable
{
    bool IsInit = false;
    public T Value;
    public BinaryTree<T> Left;
    public BinaryTree<T> Right;

    public BinaryTree()
    {
    }

    public BinaryTree(T value)
    {
        Value = value;
        IsInit = true;
    }
    
    public void Add(T item)
    {
        if (!IsInit) 
        {
            Value = item;
            IsInit = true;
        }
        else if(Value.CompareTo(item) >= 0)
        {
            if(Left != null) Left.Add(item);
            else Left = new BinaryTree<T>(item);
        }
        else
        {
            if (Right != null) Right.Add(item);
            else Right = new BinaryTree<T>(item);
        }
    }

    public IEnumerator<T> GetEnumerator() => GetEnumeratorForTree(this);

    IEnumerator<T> GetEnumeratorForTree(BinaryTree<T> tree)
    {
        if (tree == null || !tree.IsInit) yield break;
        
        var enumForLeft = GetEnumeratorForTree(tree.Left);
        while(enumForLeft.MoveNext())
            yield return enumForLeft.Current;
        
        yield return tree.Value;

        var enumForRight = GetEnumeratorForTree(tree.Right);
        while (enumForRight.MoveNext())
            yield return enumForRight.Current;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public static class BinaryTree
{
    public static BinaryTree<int> Create(params int[] items)
    {
        var tree = new BinaryTree<int>();
        foreach (var item in items)
            tree.Add(item);
        return tree;
    }
}
