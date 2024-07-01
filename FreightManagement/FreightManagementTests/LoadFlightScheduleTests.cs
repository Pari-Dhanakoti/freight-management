using FreightManagement.Helpers;
using FreightManagement.Models;

namespace FreightManagementTests;

public class LoadFlightScheduleTests
{
	private Schedule schedule;

	[SetUp]
	public void Setup()
	{
		schedule = new Schedule
		{
			Days = new List<Day>
			{
				new Day
				{
					Id = 1,
					Flights = new List<Flight>
					{
						new Flight { FlightNumber = 1, DepartureCity = "YUL", ArrivalCity = "YYZ" },
						new Flight { FlightNumber = 2, DepartureCity = "YUL", ArrivalCity = "YYC" },
						new Flight { FlightNumber = 3, DepartureCity = "YUL", ArrivalCity = "YVR" }
					}
				},
				new Day
				{
					Id = 2,
					Flights = new List<Flight>
					{
						new Flight { FlightNumber = 4, DepartureCity = "YUL", ArrivalCity = "YYZ" },
						new Flight { FlightNumber = 5, DepartureCity = "YUL", ArrivalCity = "YYC" },
						new Flight { FlightNumber = 6, DepartureCity = "YUL", ArrivalCity = "YVR" }
					}
				}
			}
		};
	}

	[Test]
	public void PrintScheduleForDay_HandlesEmptySchedule()
	{
		var scheduleEmpty = new Schedule();

		List<string>? actualResult = scheduleEmpty.Days?
											.SelectMany(DaySchedulePrinter.PrintScheduleForDay)
											.ToList();

		Assert.That(actualResult, Is.Null);
	}

	[Test]
	public void PrintScheduleForDay_HandlesEmptyFlightsInADay()
	{
		var scheduleEmpty = new Schedule
		{
			Days = new List<Day> 
			{ 
				new Day 
				{ 
					Id = 1 
				} 
			}
		};

		var expectedResult = new List<string>();		

		List<string>? actualResult = scheduleEmpty.Days
											.SelectMany(DaySchedulePrinter.PrintScheduleForDay)
											.ToList();

		Assert.That(actualResult, Is.EqualTo(expectedResult));
	}

	[Test]
	public void PrintScheduleForDay_PrintsCorrectSchedule()
	{
		var expectedResult = new List<string>
		{
			"Flight: 1, departure: YUL, arrival: YYZ, day: 1",
			"Flight: 2, departure: YUL, arrival: YYC, day: 1",
			"Flight: 3, departure: YUL, arrival: YVR, day: 1",
			"Flight: 4, departure: YUL, arrival: YYZ, day: 2",
			"Flight: 5, departure: YUL, arrival: YYC, day: 2",
			"Flight: 6, departure: YUL, arrival: YVR, day: 2"
		};

		List<string> actualResult = schedule.Days!
											.SelectMany(DaySchedulePrinter.PrintScheduleForDay)
											.ToList();

		CollectionAssert.AreEqual(expectedResult, actualResult);
	}

	[Test]
	public void PrintScheduleForDay_ContainsAllFlights()
	{
		var allFlights = schedule.Days!
								 .SelectMany(day => day.Flights!)
								 .ToList();

		var printedFlights = schedule.Days!
									.SelectMany(DaySchedulePrinter.PrintScheduleForDay)
									.ToList();

		foreach (var flight in allFlights)
		{
			Assert.IsTrue(printedFlights.Any(s => s.Contains($"Flight: {flight.FlightNumber}, departure: {flight.DepartureCity}, arrival: {flight.ArrivalCity}")));
		}
	}

}
