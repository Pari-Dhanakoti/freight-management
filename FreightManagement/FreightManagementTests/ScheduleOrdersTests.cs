using FreightManagement.Models;

namespace FreightManagementTests;
public class ScheduleOrdersTests
{
	private Schedule _testSchedule;

	[SetUp]
	public void Setup()
	{		
		_testSchedule = new Schedule
		{
			Flights = new List<Flight>
			{
				new Flight { FlightNumber = 1, DepartureCity = "YUL", ArrivalCity = "YYZ", Day = 1, Capacity = 2 },
				new Flight { FlightNumber = 2, DepartureCity = "YUL", ArrivalCity = "YYC", Day = 1, Capacity = 1 },
				new Flight { FlightNumber = 3, DepartureCity = "YUL", ArrivalCity = "YVR", Day = 1, Capacity = 3 }				
			}
		};
	}

	[Test]
	public void ScheduleOrders_Marks_Order_Unscheduled()
	{
		// Arrange
		var orders = new List<Order>
		{
			new Order { OrderNumber = "order-001", Destination = "YYZ", Priority = 1 },
			new Order { OrderNumber = "order-002", Destination = "YYC", Priority = 2 },
			new Order { OrderNumber = "order-003", Destination = "YVR", Priority = 3 },
			new Order { OrderNumber = "order-004", Destination = "YYC", Priority = 4 }
		};

		var expectedUnscheduledOrder = new ScheduledOrder
		{
			OrderNumber = "order-004",
			FlightNumber = Constants.Unscheduled
		};

		// Act
		var scheduledOrders = FreightManagement.Program.ScheduleOrders(orders, _testSchedule);
		Assert.Multiple(() =>
		{

			// Assert
			Assert.That(scheduledOrders.Item1.Where(so => so.FlightNumber != Constants.Unscheduled).Count, Is.EqualTo(3));
			Assert.That(expectedUnscheduledOrder, Is.EqualTo(scheduledOrders.Item1.Single(so => so.OrderNumber == "order-004")));
		});
	}

	[Test]
	public void ScheduleOrders_Marks_Decrements_Flight_Capacity()
	{		
		var orders = new List<Order>
		{
			new Order { OrderNumber = "order-001", Destination = "YYZ", Priority = 1 },
			new Order { OrderNumber = "order-002", Destination = "YYC", Priority = 2 },
			new Order { OrderNumber = "order-003", Destination = "YVR", Priority = 3 },
			new Order { OrderNumber = "order-004", Destination = "YYC", Priority = 4 }
		};

		// Act
		var scheduledOrders = FreightManagement.Program.ScheduleOrders(orders, _testSchedule);

		// Assert
		Assert.Multiple(() =>
		{	
			Assert.That(scheduledOrders.Item2.Flights.Single(f => f.FlightNumber == 1).Capacity, Is.EqualTo(1));
			Assert.That(scheduledOrders.Item2.Flights.Single(f => f.FlightNumber == 2).Capacity, Is.EqualTo(0));
			Assert.That(scheduledOrders.Item2.Flights.Single(f => f.FlightNumber == 3).Capacity, Is.EqualTo(2));
		});
	}

	[Test]
	public void ScheduleOrders_skips_Flights_OverCapacity()
	{
		_testSchedule = new Schedule
		{
			Flights = new List<Flight>
			{
				new Flight { FlightNumber = 1, DepartureCity = "YUL", ArrivalCity = "YYZ", Day = 1, Capacity = 0 }			
			}
		};

		var orders = new List<Order>
		{
			new Order { OrderNumber = "order-001", Destination = "YYZ", Priority = 1 }			
		};

		var expectedUnscheduledOrder = new ScheduledOrder
		{
			OrderNumber = "order-001",
			FlightNumber = Constants.Unscheduled
		};

		// Act
		var scheduledOrders = FreightManagement.Program.ScheduleOrders(orders, _testSchedule);
		Assert.Multiple(() =>
		{

			// Assert
			Assert.That(scheduledOrders.Item1.Where(so => so.FlightNumber == Constants.Unscheduled).Count, Is.EqualTo(1));
			Assert.That(scheduledOrders.Item2.Flights.Single(flight => flight.FlightNumber == 1).Capacity, Is.EqualTo(0));
		});
	}

	[Test]
	public void ScheduleOrders_Optimally_Fills_By_Capacity()
	{
		_testSchedule = new Schedule
		{
			Flights = new List<Flight>
			{
				new Flight { FlightNumber = 1, DepartureCity = "YUL", ArrivalCity = "YYZ", Day = 1, Capacity = 2 },
				new Flight { FlightNumber = 2, DepartureCity = "YUL", ArrivalCity = "YYZ", Day = 1, Capacity = 1 },
				new Flight { FlightNumber = 3, DepartureCity = "YUL", ArrivalCity = "YVZ", Day = 1, Capacity = 10 }
			}
		};

		var orders = new List<Order>
		{
			new Order { OrderNumber = "order-001", Destination = "YYZ", Priority = 1 }			
		};

		var expectedChosenFlight = new Flight { FlightNumber = 2, DepartureCity = "YUL", ArrivalCity = "YYZ", Day = 1, Capacity = 1 };

		// Act
		var scheduledOrders = FreightManagement.Program.ScheduleOrders(orders, _testSchedule);

		// Assert
		Assert.That(scheduledOrders.Item1.Single().FlightNumber, Is.EqualTo(expectedChosenFlight.FlightNumber.ToString()));
		Assert.That(scheduledOrders.Item1.Single().ScheduledDay, Is.EqualTo(expectedChosenFlight.Day));
	}	
}
