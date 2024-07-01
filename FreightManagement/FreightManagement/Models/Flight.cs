namespace FreightManagement.Models;
public record Flight
{
	public int FlightNumber { get; set; }
	public string DepartureCity { get; set; } = default!;
	public string ArrivalCity { get; set; } = default!;	
	public int Day { get; set; }
	public int Capacity { get; set; } = Constants.MAX_CAPACITY;
}
