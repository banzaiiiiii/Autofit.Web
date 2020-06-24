using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server;

namespace AutoFit.Web.Controllers
{

    [Authorize]
    public class FileSystemController : Controller
    {
        public IActionResult Index()
        {
            var path = @"C:/Users/robin/desktop/test";
            var model = new FileSystemModel();
            var list = new List<string>();
            
            foreach(var directory in Directory.GetFiles(path,"*.*",SearchOption.AllDirectories)){
                var uri = new Uri(directory).ToString().Remove(0,8);
                list.Add(uri);
            };

            model.FolderList = Directory.GetDirectories(path);
            model.FileList = list;
            
            return View("FileSystem", model);
        }
    }


    public class FileSystemModel
    {
        public IEnumerable<string> FolderList{ get; set; }
        public IEnumerable<string> FileList { get; set; }
    }
}