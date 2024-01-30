using Microsoft.AspNetCore.Http;
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
        private ConfigTreeDbManager _treeDbManager;

        public TreeNodesController(TestTaskGeekForLessContext context)
        {
            _context = context;
            _treeDbManager = new ConfigTreeDbManager(context);
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            TreeNode? rootFromDb = _treeDbManager.RetrieveTree();

            if (rootFromDb == null)
            {
                return RedirectToAction("Create");
            }

            return View(rootFromDb);
        }

        [Route("branch")]
        [HttpGet("branch/{*path}")]
        public IActionResult Branch([FromRoute] string? path = null)
        {
            if (path == null)
            {
                return NotFound();
            }

            string[] paths = path.Split("/");

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
                }
                catch (InvalidOperationException e)
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

            treeNode.Children = _treeDbManager.GetChildren(treeNode);

            return View("Index", treeNode);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UploadFileWrapper formFile)
        {
            TreeNode root;
            if (formFile.File != null && formFile.File.Length > 0)
            {
                _treeDbManager.DeleteTree();
                if (formFile.File.ContentType == "application/json")
                {
                    root = await _GetRootTreeNodeFromJsonAsync(formFile.File);
                    _treeDbManager.SaveTree(root);
                    await _context.SaveChangesAsync();
                } else if (formFile.File.ContentType == "text/plain")
                {
                    root = await _GetRootTreeNodeFromTxtAsync(formFile.File);
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

        private async Task<TreeNode> _GetRootTreeNodeFromJsonAsync(IFormFile file) 
        {
            using (var streamReader = new StreamReader(file.OpenReadStream()))
            {
                var jsonContent = await streamReader.ReadToEndAsync();
                JsonTreeConverter converter = new JsonTreeConverter();
                TreeNode root = converter.convert(jsonContent);
                ViewBag.Message = "JSON file uploaded successfully!";

                return root;
            }
        }

        private async Task<TreeNode> _GetRootTreeNodeFromTxtAsync(IFormFile file)
        {
            using (var streamReader = new StreamReader(file.OpenReadStream()))
            {
                var txtContent = await streamReader.ReadToEndAsync();
                
                var converter = new TxtTreeConverter();
                TreeNode root = converter.convert(txtContent);
                ViewBag.Message = "TXT file uploaded successfully!";

                return root;
            }
        }
    }
}
