using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestTaskGeekForLess.Data;
using TestTaskGeekForLess.Models;
using TestTaskGeekForLess.Utility;

namespace TestTaskGeekForLess.Controllers
{
    public class TreeNodesController : Controller
    {
        private readonly TestTaskGeekForLessContext _context;
        private TreeNodeDbManager _treeDbManager;

        public TreeNodesController(TestTaskGeekForLessContext context)
        {
            _context = context;
            _treeDbManager = new TreeNodeDbManager(context);
        }

        // GET: TreeNodes
        public async Task<IActionResult> Index()
        {
            TreeNode? rootFromDb = _treeDbManager.RetrieveTree();
            
            return View(rootFromDb);
        }

        // GET: TreeNodes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var treeNode = await _context.TreeNode
                .FirstOrDefaultAsync(m => m.Id == id);
            if (treeNode == null)
            {
                return NotFound();
            }

            return View(treeNode);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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

        //private bool TreeNodeExists(int id)
        //{
        //    return _context.TreeNode.Any(e => e.Id == id);
        //}
    }
}
