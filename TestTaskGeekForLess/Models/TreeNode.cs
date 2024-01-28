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
    }
}
