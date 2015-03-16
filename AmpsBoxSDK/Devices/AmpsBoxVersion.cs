namespace AmpsBoxSdk.Devices
{
    using System.ComponentModel.Composition;

    using Infrastructure.Domain.Shared;

    [Export]
    public class AmpsBoxVersion : IValueObject<AmpsBoxVersion>
    {
        public AmpsBoxVersion()
        {
            
        }
        public bool SameValueAs(AmpsBoxVersion other)
        {
            throw new System.NotImplementedException();
        }
    }
}