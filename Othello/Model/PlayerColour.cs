namespace Model;

public class PlayerColour
{
	private Dictionary<string, Colour> avaliableColours = new()
	{
		{"black", new Colour(0, 0, 0)},
		{"white", new Colour(255, 255, 255)}
	};

	public void AddColour(string name ,int r, int g, int b)
	{
		try
		{
			avaliableColours.Add(name, new Colour(r,g,b));
		}
		catch (Exception e)
		{
			Console.WriteLine("cant add this colour");
		}
	}
	public Colour GetColourByName(string name)
	{
		if (!avaliableColours.ContainsKey(name))
		{
			throw new Exception("no such colour");
		}
		return avaliableColours[name];
	}
}

public class Colour
{
	public  int? r { get; set; }
	public int? g { get; set; }
	public int? b { get; set; }

	public Colour(int r, int g, int b)
	{
		r = r;
		g = g;
		b = b;
	}
}