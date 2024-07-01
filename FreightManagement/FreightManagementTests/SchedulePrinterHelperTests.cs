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

		var expected = $"Flight: {flight.FlightNumber}, departure: {flight.DepartureCity}, arrival: {flight.ArrivalCity}";

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
	public void DaySchedulePrinter_Prints_Day_Details()
	{
		// Arrange
		var flight = new Flight
		{
			DepartureCity = "YYZ",
			ArrivalCity = "YVR",
			FlightNumber = 1
		};

		var day = new Day
		{
			Id = 1,
			Flights = new List<Flight> { flight }
		};

		var expectedFlight = $"Flight: {flight.FlightNumber}, departure: {flight.DepartureCity}, arrival: {flight.ArrivalCity}";

		var expected = new List<string>
		{
			$"{expectedFlight}, day: {day.Id}"
		};

		// Act 
		var printDay = DaySchedulePrinter.PrintScheduleForDay(day);

		// Assert
		Assert.Multiple(() =>
		{
			Assert.That(printDay, Is.Not.Null);
			Assert.That(printDay, Is.EqualTo(expected));
		});
	}
}
