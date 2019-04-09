namespace DataModel
{
    public interface IStartable
    {
        void Start();
        void Stop();

        bool IsRunning();
    }
}