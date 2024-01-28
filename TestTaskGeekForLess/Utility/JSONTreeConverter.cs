using Newtonsoft.Json.Linq;
using System.Text.Json;
using TestTaskGeekForLess.Models;

namespace TestTaskGeekForLess.Utility
{
    //public static class JsonExtensions
    //{
    //    public static object GetValue(this JsonElement jsonElement)
    //    {
    //        switch (jsonElement.ValueKind)
    //        {
    //            case JsonValueKind.String:
    //                return jsonElement.GetString();
    //            case JsonValueKind.Number:
    //                return jsonElement.GetDouble();
    //            case JsonValueKind.True:
    //                return true;
    //            case JsonValueKind.False:
    //                return false;
    //            default:
    //                return null;
    //        }
    //    }
    //}

    public class JsonTreeConverter
    {
        public static int counter = 2;
        public TreeNode ConvertJsonToTree(string jsonString)
        {
            var parentObj = JObject.Parse(jsonString);
            var children = new List<TreeNode>();

            foreach (JProperty prop in parentObj.Properties())
            {
                children.Add(TraverseJsonProperty(prop, 1));
            }

            TreeNode rootNode = new TreeNode() { 
                Id = 1,
                Name = null,
                Value = null,
                Children = children,
            };

            return rootNode;
        }

        private TreeNode TraverseJsonProperty(JProperty jProp, int parentId)
        {
            TreeNode node = new TreeNode()
            {
                Id = counter++,
                Name = jProp.Name,
                Value = jProp.Value.Type == JTokenType.Object ? null : jProp.Value.ToString(),
                ParentId = parentId
            };

            node.Children = GetChildren(jProp, node.Id);

            return node;
        }

        private List<TreeNode> GetChildren(JProperty jProperty, int parentId)
        {
            if (jProperty.Value.Type == JTokenType.Object)
            {
                return jProperty.Value
                    .Select(prop => TraverseJsonProperty((JProperty)prop, parentId))
                    .ToList();
            }
            else
            {
                return new List<TreeNode>();
            }
        }
    }
}