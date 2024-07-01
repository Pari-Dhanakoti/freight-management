using FreightManagement.Models;

namespace FreightManagement.Helpers;
public static class FlightSchedulePrinterHelper
{
	public static string PrintFlight(this Flight flight) =>
		$"flightNumber: {flight.FlightNumber}, departure: {flight.DepartureCity}, arrival: {flight.ArrivalCity}, day: {flight.Day}";
}

public static class OrderSchedulePrinterHelper
{
	public static string PrintOrderSchedule(this ScheduledOrder order) => 
		order.FlightNumber switch
		{
			Constants.Unscheduled => $"order: {order.OrderNumber}, flightNumber: {order.FlightNumber}",
			_ => $"order: {order.OrderNumber}, flightNumber: {order.FlightNumber}, departure: {order.DepartureCity}, arrival: {order.ArrivalCity}, day: {order.ScheduledDay}"
		};
}