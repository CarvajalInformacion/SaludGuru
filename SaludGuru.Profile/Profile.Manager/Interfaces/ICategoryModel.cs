using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profile.Manager.Interfaces
{
    public interface ICategoryModel
    {
        int CategoryId { get; set; }
        string Name { get; set; }
        DateTime LastModify { get; set; }
        DateTime CreateDare { get; set; }
    }
}
