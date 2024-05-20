using DigiMedia.Business.Exceptions;
using DigiMedia.Business.Services.Abstract;
using DigiMedia.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigiMedia.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "superAdmin")]
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        public IActionResult Index()
        {
            var projects = _projectService.GetAllProject();
            if (projects == null) return NotFound();

            return View(projects);
        }


        public IActionResult Create() 
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Project project) 
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                await _projectService.AddProjectAsync(project);
            }
            catch (ImageContentTypeException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
            catch (ImageSizeException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
            catch (EntityNotFoundException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
            catch (Business.Exceptions.FileNotFoundException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var existProject = _projectService.GetProject(x => x.Id == id);
            if (existProject == null) return NotFound();

            return View(existProject);
        }

        [HttpPost]
        public IActionResult DeletePost(int id)
        {
            try
            {
                  _projectService.DeleteProject(id);
            }
            catch (ImageContentTypeException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
            catch (ImageSizeException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
            catch (EntityNotFoundException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
            catch (Business.Exceptions.FileNotFoundException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Update(int id) 
        {
            var existsProject = _projectService.GetProject(x => x.Id == id);
            if (existsProject == null) return NotFound(); 
            
            return View(existsProject);
        }

        [HttpPost]
        public IActionResult Update(Project project)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                 _projectService.UpdateProject(project.Id, project);
            }
            catch (ImageContentTypeException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
            catch (ImageSizeException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
            catch (EntityNotFoundException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
            catch (Business.Exceptions.FileNotFoundException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return RedirectToAction("Index");
        }

    }
}
