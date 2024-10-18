using System;
using System.IO.Pipes;

namespace Delegates.TreeTraversal;

public static class Traversal
{

	public static IEnumerable<Product> GetProducts(ProductCategory root)
	{
        var travesal = new Traversal<ProductCategory, Product>(
            (productCategory) => productCategory.Products,
            (productsCategory) => productsCategory.Categories,
            (productsCategory) => true
            );
        var ans = new List<Product>();
        travesal.PostOrder(root, ans);
        return ans;
	}

    public static IEnumerable<Job> GetEndJobs(Job root)
    {
        var traversal = new Traversal<Job, Job>(
                job => new List<Job> { job },
                job => job.Subjobs,
                job => job.Subjobs == null || job.Subjobs.Count == 0
            );
        var ans = new List<Job>();
        traversal.PostOrder(root, ans);
        return ans;
    }

	public static IEnumerable<T> GetBinaryTreeValues<T>(BinaryTree<T> root)
	{
        var traversal = new Traversal<BinaryTree<T>, T>(
                tree =>
                new List<T> { tree.Value },
                tree =>
                {
                    var nodes = new List<BinaryTree<T>>();
                    if (tree.Left != null) nodes.Add(tree.Left);
                    if (tree.Right != null) nodes.Add(tree.Right);

                    return nodes;
                },
                tree => tree.Right == null && tree.Left == null
                );
        var ans = new List<T>();
        traversal.PostOrder(root, ans);
        return ans;
    }
}
public class Traversal<TTree, Titem>
{
    Func<TTree, IEnumerable<Titem>> treeNodeHandler;
    Func<TTree, IEnumerable<TTree>> treeParser;
    Func<TTree, bool> isValid;

    public Traversal(Func<TTree, IEnumerable<Titem>> treeNodeHandler,
                Func<TTree, IEnumerable<TTree>> treeParser,
                Func<TTree, bool> isValid)
    {
        this.treeNodeHandler = treeNodeHandler;
        this.treeParser = treeParser;
        this.isValid = isValid;
    }
    public void PostOrder(TTree tree, List<Titem> result)
    {
        if (isValid(tree))
            result.AddRange(treeNodeHandler(tree));

        var next = treeParser(tree);
        foreach (var node in next)
            PostOrder(node, result);
    }
}