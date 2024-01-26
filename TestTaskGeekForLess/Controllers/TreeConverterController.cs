using Microsoft.AspNetCore.Mvc;
using TestTaskGeekForLess.Models;
using TestTaskGeekForLess.Utility;

namespace TestTaskGeekForLess.Controllers
{
    public class TreeConverterController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(FileUploadModel formFile)
        {
            TreeNode parent;
            if (formFile.File != null && formFile.File.Length > 0)
            {
                using (var streamReader = new StreamReader(formFile.File.OpenReadStream()))
                {
                    var jsonContent = await streamReader.ReadToEndAsync();
                    JsonTreeConverter converter = new JsonTreeConverter();
                    parent = converter.ConvertJsonToTree(jsonContent);
                    PrintTree(parent, 0);
                    ViewBag.Message = "JSON file uploaded successfully!";
                }
            }
            else
            {
                ViewBag.Message = "Please select a JSON file to upload.";
            }

            return View();
        }

        static void PrintTree(TreeNode node, int depth)
        {
            string indentation = new string(' ', depth * 2);
            System.Diagnostics.Debug.Print($"{indentation}{node.Name}: {node.Value}");

            foreach (TreeNode child in node.Children)
            {
                PrintTree(child, depth + 1);
            }
        }
    }
}
