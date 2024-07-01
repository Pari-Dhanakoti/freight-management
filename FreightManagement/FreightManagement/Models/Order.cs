using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreightManagement.Models;
public record Order
{
	public int Priority { get; set; }
	public string OrderNumber { get; set; } = default!;
	public string Destination { get; set; } = default!;	
}
