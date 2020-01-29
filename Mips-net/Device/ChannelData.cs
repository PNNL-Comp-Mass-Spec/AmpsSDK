namespace Mips.Device
{
    public class ChannelData
    {
	    public ChannelData(double minimum, double maximum, double actual, double setPoint)
	    {
		    Minimum = minimum;
		    Maximum = maximum;
		    this.Actual = actual;
		    this.Setpoint = setPoint;
	    }

	   
	    public double Actual { get; }

	    
	    public double Maximum { get; }

	    
	    public double Minimum { get; }

	   
	    public double Setpoint { get; }

	    public static ChannelData Generate(int minimum, int maximum, int actual, int setPoint)
	    {
		    return new ChannelData(minimum, maximum, actual, setPoint);
	    }
	}
}
