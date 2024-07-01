namespace FreightManagement.Models;
public record Order
{
	public int Priority { get; set; }
	public string OrderNumber { get; set; } = default!;
	public string Destination { get; set; } = default!;	
}


public record ScheduledOrder
{
	public string OrderNumber { get; set; } = default!;
	public string FlightNumber { get; set; } = default!;
	public string? DepartureCity { get; set; }
	public string? ArrivalCity { get; set; }
	public int ScheduledDay { get; set; }
}
