using FreightManagement.Models;

namespace FreightManagementTests;

public class LoadFlightScheduleTests
{
	[SetUp]
	public void Setup()
	{
		
	}

	[Test]
	public void TestLoadFlightSchedule()
	{
		// Arrange
		Schedule schedule = FreightManagement.Program.LoadFlightSchedule();

		// Act
		List<Flight> flights = schedule.Flights;

		// Assert
		Assert.That(flights, Is.Not.Null);
		Assert.That(flights, Has.Count.EqualTo(6)); // Ensure all flights are loaded
	}

	[Test]
	public void TestUpdateFlightByFlightNumber()
	{
		// Arrange
		Schedule schedule = FreightManagement.Program.LoadFlightSchedule();
		int flightNumberToUpdate = 3;
		string newDepartureCity = "YVR";

		// Act
		Flight? flightToUpdate = schedule.Flights.FirstOrDefault(f => f.FlightNumber == flightNumberToUpdate);
		if (flightToUpdate != null)
		{
			flightToUpdate.DepartureCity = newDepartureCity;
		}

		// Assert
		Assert.That(flightToUpdate, Is.Not.Null);
		Assert.That(flightToUpdate.DepartureCity, Is.EqualTo(newDepartureCity));
	}

	[Test]
	public void TestFlightInitializesCapacity()
	{
		// Arrange
		Schedule schedule = FreightManagement.Program.LoadFlightSchedule();

		// Act
		List<Flight> flights = schedule.Flights;

		// Assert
		Assert.That(flights, Is.Not.Null);
		Assert.That(flights.First().Capacity, Is.EqualTo(Constants.MAX_CAPACITY));
	}

}
