namespace Delegates.TreeTraversal;

public static class Traversal
{
	public static IEnumerable<Product> GetProducts(ProductCategory root)
	{
		return root.Products.ToList();
	}

	public static IEnumerable<Job> GetEndJobs(Job root)
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<T> GetBinaryTreeValues<T>(BinaryTree<T> root)
	{
		throw new NotImplementedException();
	}
}