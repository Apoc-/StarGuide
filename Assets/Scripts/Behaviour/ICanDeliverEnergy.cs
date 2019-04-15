namespace Behaviour
{
    public interface ICanDeliverEnergy
    {
        /// <summary>
        /// Return the delivered Energy per tick.
        /// </summary>
        int DeliveredEnergyPerTick { get; }

        /// <summary>
        /// True, if this device currently can deliver energy.
        /// Use this in case some energy deliverer itself requires energy and may run out of power.
        /// </summary>
        bool CanDeliverEnergy { get; }

        void DeliverEnergy(IRequiresEnergy target);
    }
}