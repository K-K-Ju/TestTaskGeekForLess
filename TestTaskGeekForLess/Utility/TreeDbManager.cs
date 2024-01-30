using TestTaskGeekForLess.Models;

namespace TestTaskGeekForLess.Utility
{
    public interface TreeDbManager
    {
        TreeNode? RetrieveTree();
        void SaveTree(TreeNode treeNode);
        void DeleteTree();
        List<TreeNode> GetChildren(TreeNode parent);
    }
}
