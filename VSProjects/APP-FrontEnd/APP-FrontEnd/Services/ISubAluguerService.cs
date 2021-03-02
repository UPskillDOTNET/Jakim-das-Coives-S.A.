using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APP_FrontEnd.Models;

namespace APP_FrontEnd.Services
{
    public interface ISubAluguerService
    {
        public  Task<IEnumerable<SubAluguerDTO>> GetAllSubAluguerByNIF();
        public  Task<SubAluguerDTO> GetSubAluguerById(int id);
        public  Task PostSubAluguerAsync(SubAluguerDTO subAluguerDTO);
        public  Task DeleteSubAluguerAsync(int id);
    }
}
