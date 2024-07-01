using FreightManagement.Helpers;
using FreightManagement.Models;
using Newtonsoft.Json;

namespace FreightManagement;
public class Program
{
	private static void Main()
	{
		PrintSeperator();
		
		// load & print flight schedule
		var schedule = LoadFlightSchedule();
		PrintFlightSchedule(schedule);
		PrintSeperator();

		// read, load orders
		var orders = LoadOrders("Files/coding-assigment-orders.json");
		PrintSeperator();

		// schedule orders in flights
		var orderSchedule = ScheduleOrders(orders, schedule);

		// print scheduled orders
		PrintScheduledOrders(orderSchedule.Item1);
		PrintSeperator();
	}

	private static void PrintScheduledOrders(List<ScheduledOrder> scheduledOrders) =>
		scheduledOrders.ForEach(so => Console.WriteLine(so.PrintOrderSchedule()));

	static void PrintSeperator() => Console.WriteLine("#################################################");

	public static Schedule LoadFlightSchedule()
	{
		Console.WriteLine("Loading Flight Schedule .....");
		var schedule = new Schedule
		{
			Flights = new List<Flight>
			{
				new Flight
				{
					FlightNumber = 1,
					DepartureCity = "YUL",
					ArrivalCity = "YYZ",
					Day = 1,
				},
				new Flight
				{
					FlightNumber = 2,
					DepartureCity = "YUL",
					ArrivalCity = "YYC",
					Day = 1,
				},
				new Flight
				{
					FlightNumber = 3,
					DepartureCity = "YUL",
					ArrivalCity = "YVR",
					Day = 1,
				},
				new Flight
				{
					FlightNumber = 4,
					DepartureCity = "YUL",
					ArrivalCity = "YYZ",
					Day = 2,
				},
				new Flight
				{
					FlightNumber = 5,
					DepartureCity = "YUL",
					ArrivalCity = "YYC",
					Day = 2,
				},
				new Flight
				{
					FlightNumber = 6,
					DepartureCity = "YUL",
					ArrivalCity = "YVR",
					Day = 2
				}
			}
		};

		return schedule;
	}

	public static void PrintFlightSchedule(Schedule schedule) => 
		schedule.Flights.ForEach(f => Console.WriteLine(f.PrintFlight()));


	public static  List<Order> LoadOrders(string filePath)
	{
		Console.WriteLine("Loading Orders .....");		

		// Create a list to hold the Order objects
		List<Order> orders = ReadOrdersData(filePath);
		
		// Print the list of orders
		foreach (var order in orders)
		{
			Console.WriteLine($"Order: {order.OrderNumber}, Destination: {order.Destination}, Priority: {order.Priority}");
		}

		return orders;
	}

	public static Tuple<List<ScheduledOrder>, Schedule> ScheduleOrders(List<Order> orders, Schedule schedule)
	{
		Console.WriteLine("Scheduling Orders in Flights.....");
		var scheduledOrders = new List<ScheduledOrder>();
		
		var ordersByDestination = orders
									.OrderBy(o => o.Priority)
									.GroupBy(x => x.Destination)
									.SelectMany(grp => grp.ToList());

		var flightsByDestination = new Dictionary<string, List<Flight>>();

        foreach (var flight in schedule.Flights)
        {
            if(flightsByDestination.TryGetValue(flight.ArrivalCity, out List<Flight>? availableFlightsForDestination))
			{
				availableFlightsForDestination.Add(flight);
			}
			else
			{
				flightsByDestination.Add(flight.ArrivalCity, [flight]);
			}
        }

		foreach(var order in ordersByDestination)
		{
			flightsByDestination.TryGetValue(order.Destination, out List<Flight>? flights);

			// if there are no flights flying to same destination as in the order, consider the order unscheduled
			if(flights == null)
			{
				scheduledOrders.Add(new ScheduledOrder
				{
					OrderNumber = order.OrderNumber,
					FlightNumber = Constants.Unscheduled
				});
				continue;
			}

			// optimize by day and capacity - choose the earliest flight that still has capacity
			// fill the ones closer to getting full before moving to ones that have higher capacity
			var orderedFlights = flights
									.OrderBy(f => f.Day)
									.ThenBy(f => f.Capacity);

			// no flights to that destination have any capacity left, mark as unscheduled.
			if(!orderedFlights.Any(f => f.Capacity > 0))
			{
				scheduledOrders.Add(new ScheduledOrder
				{
					OrderNumber = order.OrderNumber,
					FlightNumber = Constants.Unscheduled
				});
			}
			else
			{
				// fill in the flight that is close to capacity first before moving to ones that have more capacity
				var chosenFlight = orderedFlights.FirstOrDefault(f => f.Capacity > 0);

				if(chosenFlight == null) 
				{
					scheduledOrders.Add(new ScheduledOrder
					{
						OrderNumber = order.OrderNumber,
						FlightNumber = Constants.Unscheduled
					});
					continue;
				}

				scheduledOrders.Add(new ScheduledOrder
				{
					OrderNumber = order.OrderNumber,
					FlightNumber = chosenFlight.FlightNumber.ToString(),
					DepartureCity = order.Destination,
					ArrivalCity = chosenFlight.ArrivalCity,
					ScheduledDay = chosenFlight.Day					
				});

				// update the capacity
				chosenFlight.Capacity--;
				flights[flights.IndexOf(chosenFlight)] = chosenFlight;
				flightsByDestination[order.Destination] = flights;

				schedule.Flights = flightsByDestination.Values.SelectMany(f => f).ToList();
			}
		}

		return new Tuple<List<ScheduledOrder>, Schedule>(scheduledOrders, schedule);
	}


	private static List<Order> ReadOrdersData(string filePath)
	{
		Console.WriteLine("Reading Orders .....");
		var orders = new List<Order>();

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

			return orders;
		}

		catch (FileNotFoundException)
		{
			Console.WriteLine($"File {filePath} not found.");
			throw;
		}
		catch (Exception e)
		{
			Console.WriteLine($"Error reading JSON file: {e.Message}");
			throw;
		}
	}
}