using TestTaskGeekForLess.Models;

namespace TestTaskGeekForLess.Utility
{
    public class TxtTreeConverter
    {
        public TreeNode ConvertStrToTreeNode(string[] lines)
        {
            var root = new TreeNode(){
                Id = 1,
                Children = new List<TreeNode>()
            };
            int counter = 2;
            foreach (var line in lines)
            {
                string[] path = line.Split(':');
                var currentNode = root;

                for (int i = 0; i < path.Length - 1; i++)
                {
                    var existingNode = currentNode.Children.FirstOrDefault(n => n.Name == path[i]);
                    if (existingNode == null)
                    {
                        var newNode = new TreeNode { 
                            Id = counter++, 
                            Name = path[i] , 
                            Children = new List<TreeNode>(),
                            ParentId = currentNode.Id
                        };
                        currentNode.Children.Add(newNode);
                        currentNode = newNode;
                    }
                    else
                    {
                        currentNode = existingNode;
                    }
                }

                currentNode.Value = path.Last();
            }
            return root;
        }
    }
}
