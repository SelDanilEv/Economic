namespace Economic_v2.DataBaseLayer
{
    public class UnitOfWorkSingleton
    {
        private UnitOfWorkSingleton() { }

        private static object locker = new object();

        private static UnitOfWork _unitOfWork;

        public static UnitOfWork GetUnitOfWork
        {
            get
            {
                lock (locker)
                {
                    if (_unitOfWork == null)
                        _unitOfWork = new UnitOfWork();
                    return _unitOfWork;
                }
            }
        }
    }
}
