using FreightManagement.Helpers;
using FreightManagement.Models;

namespace FreightManagement;
public class Program
{
	private static void Main(string[] args)
	{
		PrintSeperator();
		LoadFlightSchedule();
		PrintSeperator();
		LoadOrderSchedule();
		PrintSeperator();
	}

	static void PrintSeperator() => Console.WriteLine("#################################################");

	public static void LoadFlightSchedule()
	{
		Console.WriteLine("Flight Schedule");		
		var schedule = new Schedule
		{
			Days =
			[
				new Day
				{
					Id = 1,
					Flights =
					[
						new Flight
						{
							FlightNumber = 1,
							DepartureCity = "YUL",
							ArrivalCity = "YYZ"
						},
						new Flight
						{
							FlightNumber = 2,
							DepartureCity = "YUL",
							ArrivalCity = "YYC"
						},
						new Flight
						{
							FlightNumber = 3,
							DepartureCity = "YUL",
							ArrivalCity = "YVR"
						}
					],
				},
				new Day
				{
					Id = 2,
					Flights =
					[
						new Flight
						{
							FlightNumber = 4,
							DepartureCity = "YUL",
							ArrivalCity = "YYZ"
						},
						new Flight
						{
							FlightNumber = 5,
							DepartureCity = "YUL",
							ArrivalCity = "YYC"
						},
						new Flight
						{
							FlightNumber = 6,
							DepartureCity = "YUL",
							ArrivalCity = "YVR"
						}
					],
				},
			]
		};
				
		schedule
			.Days
			.SelectMany(DaySchedulePrinter.PrintScheduleForDay)
			.ToList()
			.ForEach(Console.WriteLine);
	}

	public static void LoadOrderSchedule()
	{
		Console.WriteLine("Order Schedule");
	}
}