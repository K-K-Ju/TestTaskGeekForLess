namespace TestTaskGeekForLess.Models
{
    public class TreeNode
    {
        public string? Name { get; private set; }
        public object? Value { get; private set; }
        public List<TreeNode> Children { get; private set; }

        public TreeNode(string? name, object? value)
        {
            Name = name;
            Value = value;
            Children = new List<TreeNode>();
        }

        public TreeNode(string? name, object? value, List<TreeNode> children)
        {
            Name = name;
            Value = value;
            Children = children;
        }

    }
}
