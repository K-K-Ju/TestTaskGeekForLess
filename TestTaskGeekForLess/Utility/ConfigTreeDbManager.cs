using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TestTaskGeekForLess.Data;
using TestTaskGeekForLess.Models;

namespace TestTaskGeekForLess.Utility
{
    public class ConfigTreeDbManager : TreeDbManager
    {
        public TestTaskGeekForLessContext Context { get; set; }
        public ConfigTreeDbManager(TestTaskGeekForLessContext context)
        {
            Context = context;
        }

        public TreeNode? RetrieveTree()
        {
            var root = Context.TreeNode
                .FirstOrDefault(n => n.Id == 1);

            if (root == null)
                return null;

            root.Children = GetChildren(root);
            return root;
        }

        public void AddNode(TreeNode treeNode)
        {
            using (var transaction = Context.Database.BeginTransaction())
            {
                Context.TreeNode
                    .Add(treeNode);
                transaction.Commit();
            }

        }

        public void SaveTree(TreeNode rootNode)
        {
            AddNode(rootNode);

            foreach (var child in rootNode.Children)
            {
                SaveTree(child);
            }
        }

        public List<TreeNode> GetChildren(TreeNode parent)
        {
            var children = Context.TreeNode
                .Where(n => n.ParentId == parent.Id)
                .ToList();

            foreach (var child in children)
            {
                child.Children = GetChildren(child);
            }

            return children;
        }

        public void DeleteTree()
        {
            using (var transaction = Context.Database.BeginTransaction())
            {
                Context.TreeNode.ExecuteDelete();
                transaction.Commit();
            }
        }
    }
}
