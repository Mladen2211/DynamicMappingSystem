using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicMappingSystem.Mapping
{
    public interface IMapper
    {
        object Map(object source);
    }
}
