using DigiMedia.Business.Exceptions;
using DigiMedia.Business.Services.Abstract;
using DigiMedia.Core.Models;
using DigiMedia.Core.RepositoryAbstract;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiMedia.Business.Services.Concretes
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IWebHostEnvironment _env;

        public ProjectService(IProjectRepository projectRepository, IWebHostEnvironment env)
        {
            _projectRepository = projectRepository;
            _env = env;
        }

        public async Task AddProjectAsync(Project project)
        {
            if (project.ImageFile.ContentType != "image/png" && project.ImageFile.ContentType != "image/jpeg")
                throw new ImageContentTypeException("fayl formati duzgun deyil");

            if (project.ImageFile.Length > 2097152) throw new ImageSizeException("seklin olcusu maksimum 2mb ola biler");

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(project.ImageFile.FileName);
            string path = _env.WebRootPath + "\\uploads\\projects\\" + fileName;

            using(FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                project.ImageFile.CopyTo(fileStream);
            }

            project.ImageUrl = fileName;

            await _projectRepository.AddAsync(project);
            await _projectRepository.CommitAsync();
        }

        public void DeleteProject(int id)
        {
            var existProject = _projectRepository.Get(x => x.Id == id);
            if (existProject == null) throw new EntityNotFoundException("bele id movcud deyil");

            string path = _env.WebRootPath + "\\uploads\\projects\\" + existProject.ImageUrl;
            if (!File.Exists(path)) throw new Exceptions.FileNotFoundException("Fayl tapilmadi");

            File.Delete(path);

            _projectRepository.Delete(existProject);
            _projectRepository.Commit();
        }

        public List<Project> GetAllProject(Func<Project, bool>? func = null)
        {
            return _projectRepository.GetAll(func);
        }

        public Project GetProject(Func<Project, bool>? func = null)
        {
            return _projectRepository.Get(func);
        }

        public void UpdateProject(int id, Project newProject)
        {
            var existProject = _projectRepository.Get(x => x.Id == id);
            if (existProject == null) throw new EntityNotFoundException("bele id movcud deyil");

            if (newProject.ImageFile != null)
            {
                if (newProject.ImageFile.ContentType != "image/png" && newProject.ImageFile.ContentType != "image/jpeg")
                    throw new ImageContentTypeException("fayl formati duzgun deyil");

                if (newProject.ImageFile.Length > 2097152) throw new ImageSizeException("seklin olcusu maksimum 2mb ola biler");

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(newProject.ImageFile.FileName);
                string path = _env.WebRootPath + "\\uploads\\projects\\" + fileName;

                using (FileStream fileStream = new FileStream(path, FileMode.Create))
                {
                    newProject.ImageFile.CopyTo(fileStream);
                }

                string oldPath = _env.WebRootPath + "\\uploads\\projects\\" + existProject.ImageUrl;

                if (!File.Exists(oldPath)) throw new Exceptions.FileNotFoundException("Fayl tapilmadi");

                File.Delete(oldPath);

                existProject.ImageUrl = fileName;
            }

            existProject.Title = newProject.Title;
            existProject.Description = newProject.Description;

            _projectRepository.Commit();
        }
    }
}
