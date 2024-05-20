using DigiMedia.Core.Models;
using DigiMedia.Core.RepositoryAbstract;
using DigiMedia.Data.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiMedia.Data.RepositoryConcretes
{
    public class ProjectRepository : GenericRepository<Project>, IProjectRepository
    {
        public ProjectRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
