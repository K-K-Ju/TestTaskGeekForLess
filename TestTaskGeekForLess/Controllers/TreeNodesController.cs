using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TestTaskGeekForLess.Data;
using TestTaskGeekForLess.Models;
using TestTaskGeekForLess.Utility;

namespace TestTaskGeekForLess.Controllers
{
    [Route("tree")]
    public class TreeNodesController : Controller
    {
        private readonly TestTaskGeekForLessContext _context;
        private TreeNodeDbManager _treeDbManager;

        public TreeNodesController(TestTaskGeekForLessContext context)
        {
            _context = context;
            _treeDbManager = new TreeNodeDbManager(context);
        }

        
        public async Task<IActionResult> Index()
        {
            TreeNode? rootFromDb = _treeDbManager.RetrieveTree();

            return View(rootFromDb);
        }

        [Route("branch")]
        [HttpGet("branch/{*path}")]
        public async Task<IActionResult> Branch([FromRoute] string? path = null)
        {
            if (path == null)
            {
                return NotFound();
            }

            string [] paths = path.Split("/");

            TreeNode? parent = new TreeNode()
            {
                Id = 1,
            };
            TreeNode? treeNode = new TreeNode();
            for (int i = 0; i < paths.Length; i++)
            {
                try
                {
                    treeNode = _context.TreeNode
                                    .Where(n => n.Name == paths[i] && n.ParentId == parent.Id)
                                    .ToList()
                                    .Single();
                } catch (InvalidOperationException e)
                {
                    Debug.WriteLine(e.StackTrace);
                    return NotFound();
                }
                if (treeNode == null)
                {
                    return NotFound();
                }
                parent = treeNode;
            }

            treeNode.Children = _treeDbManager.GetChildren(treeNode.Id);

            return View("Index", treeNode);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("create")]
        public async Task<IActionResult> Create(UploadFileWrapper formFile)
        {
            TreeNode root;
            if (formFile.File != null && formFile.File.Length > 0)
            {
                using (var streamReader = new StreamReader(formFile.File.OpenReadStream()))
                {
                    var jsonContent = await streamReader.ReadToEndAsync();
                    JsonTreeConverter converter = new JsonTreeConverter();
                    root = converter.ConvertJsonToTree(jsonContent);
                    ViewBag.Message = "JSON file uploaded successfully!";

                    _treeDbManager.DeleteDbData();
                    _treeDbManager.SaveTree(root);
                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                ViewBag.Message = "Please select a JSON file to upload.";

            }

            return View();
        }
    }
}
