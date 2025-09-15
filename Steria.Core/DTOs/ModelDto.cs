using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steria.Core.DTOs;
public class ModelDto
{
    public int Id { get; set; }
    public int MakeId { get; set; }
    public string Name { get; set; } = null!;
}