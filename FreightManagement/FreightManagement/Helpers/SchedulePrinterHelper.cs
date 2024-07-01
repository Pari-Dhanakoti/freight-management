using FreightManagement.Models;

namespace FreightManagement.Helpers;
public static class FlightSchedulePrinterHelper
{
	public static string PrintFlight(this Flight flight) =>
		$"Flight: {flight.FlightNumber}, departure: {flight.DepartureCity}, arrival: {flight.ArrivalCity}";
}

public static class DaySchedulePrinter
{
	public static List<string> PrintScheduleForDay(this Day day)
	{
		var scheduleForDay = new List<string>();

		if(day.Flights != null)
		{
			foreach (var flight in day.Flights)
			{
				scheduleForDay.Add($"{FlightSchedulePrinterHelper.PrintFlight(flight)}, day: {day.Id}");
			}
		}
				
		return scheduleForDay;
	}
}
