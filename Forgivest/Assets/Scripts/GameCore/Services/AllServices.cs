namespace GameCore.Services
{
    public class AllServices
    {
        private static  AllServices _instance;
        public static AllServices Container => _instance ??= new AllServices();
    }
}