using Microsoft.Data.SqlClient;
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

        public TreeNode RetrieveTree()
        {
            var root = Context.TreeNode
                .Where(n => n.Id == 1)
                .ToList()[0];

            root.Children = _GetChildren(root.Id);
            return root;
        }

        public void AddNode(TreeNode treeNode)
        {
            Context.TreeNode
                .Add(treeNode);
        }

        public void SaveTree(TreeNode rootNode)
        {
            AddNode(rootNode);

            foreach (var child in rootNode.Children)
            {
                SaveTree(child);
            }
        }

        private List<TreeNode> _GetChildren(int parentId)
        {
            var children = Context.TreeNode
                .Where(n => n.ParentId == parentId)
                .ToList();

            foreach (var child in children) 
            {
                child.Children = _GetChildren(child.Id);
            }

            return children;
        }
    }
}
