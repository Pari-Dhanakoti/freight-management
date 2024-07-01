using FreightManagement.Helpers;
using FreightManagement.Models;
using Newtonsoft.Json;

namespace FreightManagement;
public class Program
{
	private static void Main(string[] args)
	{
		PrintSeperator();
		LoadFlightSchedule();
		PrintSeperator();
		LoadOrders();
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
			.Days?.SelectMany(DaySchedulePrinter.PrintScheduleForDay)
			.ToList()
			.ForEach(Console.WriteLine);
	}

	public static void LoadOrders()
	{
		var filePath = "Files/coding-assigment-orders.json";

		// Create a list to hold the Order objects
		List<Order> orders = ReadOrdersData(filePath);
		
		// Print the list of orders
		foreach (var order in orders)
		{
			Console.WriteLine($"Order Number: {order.OrderNumber}, Destination: {order.Destination}, Priority: {order.Priority}");
		}
	}

	private static List<Order> ReadOrdersData(string filePath)
	{
		List<Order> orders = new List<Order>();

		try
		{
			using StreamReader r = new StreamReader(filePath);
			string json = r.ReadToEnd();
			var ordersData = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(json);

			// Initialize the priority counter
			int priorityCounter = 1;

			// Iterate through the dictionary to create Order objects
			foreach (var entry in ordersData)
			{
				var order = new Order
				{
					OrderNumber = entry.Key,
					Destination = entry.Value["destination"],
					Priority = priorityCounter++
				};

				orders.Add(order);
			}
		}

		catch (FileNotFoundException)
		{
			Console.WriteLine($"File {filePath} not found.");
		}
		catch (Exception e)
		{
			Console.WriteLine($"Error reading JSON file: {e.Message}");
		}

		return orders;
	}
}