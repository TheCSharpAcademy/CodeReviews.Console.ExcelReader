using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelReader.samggannon.Services
{
    internal interface IPlayerService
    {
        public Task<bool> DeletePlayerDataDb();
        public Task<bool> CreatePlayerDataDb();
    }
}
