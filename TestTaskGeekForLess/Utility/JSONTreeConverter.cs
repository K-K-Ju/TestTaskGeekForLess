using Newtonsoft.Json.Linq;
using System.Text.Json;
using TestTaskGeekForLess.Models;

namespace TestTaskGeekForLess.Utility
{
    public static class JsonExtensions
    {
        public static object GetValue(this JsonElement jsonElement)
        {
            switch (jsonElement.ValueKind)
            {
                case JsonValueKind.String:
                    return jsonElement.GetString();
                case JsonValueKind.Number:
                    return jsonElement.GetDouble();
                case JsonValueKind.True:
                    return true;
                case JsonValueKind.False:
                    return false;
                default:
                    return null;
            }
        }
    }

    public class JsonTreeConverter
    {
        public TreeNode ConvertJsonToTree(string jsonString)
        {
            var parentObj = JObject.Parse(jsonString);
            var children = new List<TreeNode>();

            foreach (JProperty prop in parentObj.Properties())
            {
                children.Add(TraverseJsonProperty(prop));
            }
            TreeNode parentNode = new TreeNode(null, null, children);
            return parentNode;
        }

        private TreeNode TraverseJsonProperty(JProperty jProp)
        {
            TreeNode node = new TreeNode(
                 jProp.Name,
                 jProp.Value.Type == JTokenType.Object ? null : jProp.Value.ToString(),
                 GetChildren(jProp)
             );

            return node;
        }

        private List<TreeNode> GetChildren(JProperty jProperty)
        {
            if (jProperty.Value.Type == JTokenType.Object)
            {
                // If the element is an object, traverse its properties
                return jProperty.Value
                    .Select(prop => TraverseJsonProperty((JProperty)prop))
                    .ToList();
            }
            else
            {
                // For primitive values or null, no children
                return new List<TreeNode>();
            }
        }
    }
}