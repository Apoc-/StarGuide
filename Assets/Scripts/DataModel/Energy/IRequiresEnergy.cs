namespace Behaviour
{
    public interface IRequiresEnergy
    {
        /// <summary>
        /// Maximum energy level of this actor.
        /// Make sure to limit the input in IncreaseEnergy!
        /// </summary>
        int MaximumEnergy { get; }
        
        /// <summary>
        /// Return the current energy level.
        /// </summary>
        // 0 <= CurrentEnergy <= MaximumEnergy
        int CurrentEnergy { get; }

        /// <summary>
        /// Returns true if the device has enough energy for the task.
        /// This assumes that a task has a specific duration and an energy
        /// usage per tick. 
        /// </summary>
        bool CanExecuteTask(int requiredEnergyPerTick, int ticksNeeded);

        void DecreaseEnergy(int n);
        void IncreaseEnergy(int n);
    }
}