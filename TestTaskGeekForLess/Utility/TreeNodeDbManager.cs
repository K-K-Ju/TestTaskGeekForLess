using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TestTaskGeekForLess.Data;
using TestTaskGeekForLess.Models;

namespace TestTaskGeekForLess.Utility
{
    public class TreeNodeDbManager
    {
        public TestTaskGeekForLessContext Context { get; set; }
        public TreeNodeDbManager(TestTaskGeekForLessContext context)
        {
            Context = context;
        }

        public TreeNode? RetrieveTree()
        {
            var root = Context.TreeNode
                .FirstOrDefault(n => n.Id == 1);

            if (root == null)
                return null;

            root.Children = GetChildren(root.Id);
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

        public List<TreeNode> GetChildren(int parentId)
        {
            var children = Context.TreeNode
                .Where(n => n.ParentId == parentId)
                .ToList();

            foreach (var child in children)
            {
                child.Children = GetChildren(child.Id);
            }

            return children;
        }

        public void DeleteDbData()
        {
            using (var transaction = Context.Database.BeginTransaction())
            {
                Context.TreeNode.ExecuteDelete();
                transaction.Commit();
            }
        }
    }
}
