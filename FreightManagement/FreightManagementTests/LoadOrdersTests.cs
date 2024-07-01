using FreightManagement.Models;

namespace FreightManagementTests;

public class LoadOrdersTests
{
	private string _testJsonFilePath;

	[SetUp]
	public void Setup()
	{
		_testJsonFilePath = "Files/test-orders.json";
	}

	[Test]
	public void LoadOrders_WithValidFile_ReturnsOrders()
	{
		// Arrange
		var expectedOrderCount = 2; // Adjust this based on your test JSON file

		var expectedOrder = new Order
		{
			OrderNumber = "order-001",
			Destination = "YYZ",
			Priority = 1
		};

		// Act
		List<Order> orders = FreightManagement.Program.LoadOrders(_testJsonFilePath);

		// Assert
		Assert.That(orders, Has.Count.EqualTo(expectedOrderCount));
		Assert.That(orders[0], Is.EqualTo(expectedOrder));		
	}

	[Test]
	public void LoadOrders_WithInvalidFile_ThrowsFileNotFoundException()
	{
		// Arrange
		var invalidFilePath = "Files/non-existent-file.json";

		// Act & Assert
		Assert.Throws<FileNotFoundException>(() => FreightManagement.Program.LoadOrders(invalidFilePath));
	}
}

