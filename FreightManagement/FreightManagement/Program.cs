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
	}

	public static void LoadOrderSchedule()
	{
		Console.WriteLine("Order Schedule");
	}
}