using FreightManagement.Helpers;
using FreightManagement.Models;

namespace FreightManagementTests;
public class SchedulePrinterHelperTests
{
	[SetUp]
	public void Setup()
	{
	}

	[Test]
	public void FlightSchedulePrinter_Prints_Flight_Details()
	{
		// Arrange
		var flight = new Flight
		{
			DepartureCity = "YYZ",
			ArrivalCity = "YVR",
			FlightNumber = 1
		};

		var expected = $"flightNumber: {flight.FlightNumber}, departure: {flight.DepartureCity}, arrival: {flight.ArrivalCity}, day: {flight.Day}";

		// Act 
		var printFlight = FlightSchedulePrinterHelper.PrintFlight(flight);

		// Assert
		Assert.Multiple(() =>
		{
			Assert.That(printFlight, Is.Not.Null);
			Assert.That(printFlight, Is.EqualTo(expected));
		});		
	}

	[Test]
	public void PrintOrder_UnscheduledOrder_ReturnsExpectedFormat()
	{
		// Arrange
		var unscheduledOrder = new ScheduledOrder
		{
			OrderNumber = "order-001",
			FlightNumber = Constants.Unscheduled
		};

		// Act
		var result = unscheduledOrder.PrintOrderSchedule();

		// Assert
		Assert.That(result, Is.EqualTo($"order: {unscheduledOrder.OrderNumber}, flightNumber: {Constants.Unscheduled}"));
	}

	[Test]
	public void PrintOrder_ScheduledOrder_ReturnsExpectedFormat()
	{
		// Arrange
		var scheduledOrder = new ScheduledOrder
		{
			OrderNumber = "order-002",
			FlightNumber = "1",
			DepartureCity = "YUL",
			ArrivalCity = "YYZ",
			ScheduledDay = 1
		};

		// Act
		var result = scheduledOrder.PrintOrderSchedule();

		// Assert
		Assert.That(result, Is.EqualTo($"order: {scheduledOrder.OrderNumber}, flightNumber: {scheduledOrder.FlightNumber}, departure: {scheduledOrder.DepartureCity}, arrival: {scheduledOrder.ArrivalCity}, day: {scheduledOrder.ScheduledDay}"));
	}

}
