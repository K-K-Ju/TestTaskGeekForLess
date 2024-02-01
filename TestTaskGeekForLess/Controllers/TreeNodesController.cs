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

        private static readonly int PARENT_ID = 1;

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
                Id = PARENT_ID,
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
        public async Task<IActionResult> Create(UploadFileWrapper formFile)
        {
            TreeNode root;
            if (formFile.File != null && formFile.File.Length > 0)
            {
                _treeDbManager.DeleteTree();
                TreeNodeConverter converter = null;
                if (formFile.File.ContentType == "application/json")
                {
                    converter = new JsonTreeConverter();
                } else if (formFile.File.ContentType == "text/plain")
                {
                    converter = new TxtTreeConverter();
                } else
                {
                    ViewBag.Message = "This format file is not supported";
                    return View("Message");
                }

                root = await _GetRootTreeNodeFromFile(formFile.File, converter);
                _treeDbManager.SaveTree(root);
                await _context.SaveChangesAsync();
            }
            else
            {
                ViewBag.Message = "Please select a JSON file to upload.";

            }

            return View();
        }        
        private async Task<TreeNode> _GetRootTreeNodeFromFile(IFormFile file, TreeNodeConverter converter)
        {
            using (var streamReader = new StreamReader(file.OpenReadStream()))
            {
                var txtContent = await streamReader.ReadToEndAsync();
                
                TreeNode root = converter.convert(txtContent);
                ViewBag.Message = "TXT file uploaded successfully!";

                return root;
            }
        }
    }
}
