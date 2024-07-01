namespace FreightManagement.Models;

public record Day
{
	public int Id { get; set; }
	public List<Flight>? Flights { get; set; }
}
