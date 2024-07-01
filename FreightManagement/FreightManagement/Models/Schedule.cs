namespace FreightManagement.Models;
public record Schedule
{	
	public List<Flight> Flights { get; set; } = [];
}
