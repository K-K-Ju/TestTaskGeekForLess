using System.ComponentModel.DataAnnotations.Schema;

namespace TestTaskGeekForLess.Models
{
    public class TreeNode
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Value { get; set; }
        public int? ParentId { get; set; }
        [NotMapped]
        public List<TreeNode> Children { get; set; }

        //public TreeNode(string? name, object? value)
        //{
        //    Name = name;
        //    Value = value;
        //    Children = new List<TreeNode>();
        //}

        //public TreeNode(string? name, object? value, List<TreeNode> children)
        //{
        //    Name = name;
        //    Value = value;
        //    Children = children;
        //}

    }
}
