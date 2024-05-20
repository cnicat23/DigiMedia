using DigiMedia.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiMedia.Business.Services.Abstract
{
    public interface IProjectService
    {
        Task AddProjectAsync(Project project);
        void DeleteProject(int id);
        void UpdateProject(int id, Project newProject);
        Project GetProject(Func<Project, bool>? func = null);
        List<Project> GetAllProject(Func<Project, bool>? func = null);
    }
}
